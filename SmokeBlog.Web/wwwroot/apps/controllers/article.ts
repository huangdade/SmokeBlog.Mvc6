module BlogAdmin.Controllers {
    export class ArticleList {
        request: BlogAdmin.Api.IGetArticleListRequest;
        loading: boolean;
        articleList: any[];
        total: number;
        checkAll: boolean;
        constructor(private $scope, private $location: ng.ILocationService, private $api: BlogAdmin.Services.Api, private $dialog: BlogAdmin.Services.Dialog) {
            $scope.vm = this;

            $scope.$emit('changeMenu', 'article', 'articlelist');

            $scope.$watch('vm.checkAll',() => {
                this.toggleCheckStatus();
            })
            $scope.$watch('vm.request.status',() => {
                this.changePage(1);
            })

            var s = $location.search();
            this.request = {
                keywords: s.keywords,
                pageIndex: parseInt(s.page || 1),
                pageSize: 20,
                status: s.status ? parseInt(s.status) : null
            }

            this.loadData();
        }
        private toggleCheckStatus() {
            _.each(this.articleList, item=> {
                item.checked = this.checkAll;
            })
        }        
        private loadData() {
            if (this.loading) {
                return false;
            }

            this.loading = true;

            this.$api.getArticleList(this.request, response=> {
                this.loading = false;

                this.total = response.total;
                this.articleList = response.data;
                this.checkAll = false;
            });
        }
        hasItemChecked() {
            return _.any(this.articleList, { checked: true });
        }
        changePage(page) {
            var data = {
                page: page,
                status: this.request.status,
                keywords: this.request.keywords
            };
            
            this.$location.search(data);

            this.request.pageIndex = page;
            this.loadData();
        }
        search(e) {
            if (e.keyCode == 13) {
                this.changePage(1);
            }
        }
        editArticle(article) {
            this.$location.path('modifyarticle/' + article.id);
        }
        addArticle() {
            this.$location.path('modifyarticle');
        }
        changeStatus(status) {
            var ids = _.map(_.where(this.articleList, { checked: true }), item=> {
                return item.id;
            });

            if (ids.length == 0) {
                this.$dialog.error('请选择文章');
                return false;
            }

            if (this.loading) {
                return;
            }

            this.loading = true;

            var request = {
                ids: ids,
                status: status
            };

            this.$api.changeArticleStatus(request, response=> {
                this.loading = false;

                if (response.success) {
                    this.$dialog.success('操作成功');
                    this.loadData();
                }
                else {
                    this.$dialog.error(response.errorMessage);
                }
            })
        }
    }

    export class ModifyArticle {
        id: number;
        editor: KindEditor.KindEditor;
        categoryList: any[];
        loading: boolean;
        data: any;
        tags: any[];
        userList: any[];
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $dialog: BlogAdmin.Services.Dialog, $routeParams) {
            $scope.vm = this;

            $scope.$emit('changeMenu', 'article', 'articlelist');

            this.editor = KindEditor.create('#txtContent', {
                resizeType: 0
            });

            this.id = $routeParams.id ? parseInt($routeParams.id) : null;
            this.data = {
                title: "",
                status: 1
            };

            //$('#txtTags').tagsinput();
            //this.tags = [];

            this.init();
        }
        private init() {
            var self = this;

            self.loading = true;

            function loadCategory(callback: AsyncResultCallback<any>) {
                self.$api.getCategoryList(response=> {
                    if (response.success) {
                        var data = response.data;
                        var list = [];

                        _.each<any>(data, parent=> {
                            list.push({ id: parent.id, name: parent.name });

                            _.each<any>(parent.children, child=> {
                                list.push({ id: child.id, name: child.name, isChild: true });
;                            });
                        });


                        self.categoryList = list;
                        callback(null, list);
                    }
                    else {
                        callback(new Error(response.errorMessage), null);
                    }
                })
            }
            function loadUser(callback: AsyncResultCallback<any>) {
                self.$api.getUserList(response=> {
                    if (response.success) {
                        self.userList = response.data;

                        if (!self.id) {
                            self.data.userID = self.userList[0].id;
                        }

                        callback(null, response.data);
                    }
                    else {
                        callback(new Error(response.errorMessage), null);
                    }
                })
            }
            function loadArticle(callback: AsyncResultCallback<any>) {
                self.$api.getArticle(self.id, response=> {
                    if (response.success) {
                        self.data = response.data;

                        callback(null, response.data);
                    }
                    else {
                        callback(new Error(response.errorMessage), null);
                    }
                });
            }

            var arr = [
                loadCategory,
                loadUser
            ];

            if (self.id) {
                arr.push(loadArticle);
            }

            async.parallel(arr,(err) => {
                self.loading = false;

                if (err) {
                    self.$dialog.error(err.message);
                }
                else {
                    if (self.id) {
                        self.data.userID = self.data.user.id;
                        self.editor.html(self.data.content);

                        _.each(self.categoryList, cat=> {
                            if (_.any(self.data.categoryList, { id: cat.id })) {
                                cat.checked = true;
                            }
                        });
                    }
                }
            });
        }
        getArticleData(): any {
            var request: any = {
                title: this.data.title,
                content: this.editor.html(),
                summary: this.data.summary,
                allowComment: this.data.allowComment,
                status: this.data.status,
                userID: this.data.userID,
                postDate: this.data.postDate
            }

            var categoryIDs = _.map(_.where(this.categoryList, { checked: true }), item=> {
                return item.id;
            })

            request.category = categoryIDs.join(',');

            return request;
        }        
        save(status) {
            if (this.loading) {
                return;
            }

            this.loading = true;

            var data = this.getArticleData();
            data.status = status;
            if (this.id) {
                data.id = this.id;
            }

            var saveCallback = (response: BlogAdmin.Services.IApiResponse) => {
                this.loading = false;

                if (response.success) {
                    this.$dialog.success('操作成功');

                    if (!this.id) {
                        this.id = response.data;
                    }

                    if (!status) {
                        if (!this.id) {
                            this.data.status = 1;
                        }
                    }
                    else {
                        this.data.status = status;
                    }
                }
                else {
                    this.$dialog.error(response.errorMessage);
                }
            }

            if (this.id) {
                this.$api.editArticle(data, response=> {
                    saveCallback(response);
                });
            }
            else {
                this.$api.addArticle(data, response=> {
                    saveCallback(response);
                });
            }
        }
    }
}

angular.module('blogAdmin.controllers')
    .controller('articleListCtrl', BlogAdmin.Controllers.ArticleList)
    .controller('modifyArticleCtrl', BlogAdmin.Controllers.ModifyArticle)