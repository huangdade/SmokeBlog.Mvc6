module BlogAdmin.Controllers {
    export class ArticleList {
        page: number;
        pageSize: number;
        keywords: string;
        status: number;
        loading: boolean;
        articleList: any[];
        total: number;
        constructor(private $scope, private $location: ng.ILocationService, private $api: BlogAdmin.Services.Api, private $dialog: BlogAdmin.Services.Dialog) {
            $scope.vm = this;

            $scope.$emit('changeMenu', 'article', 'articlelist');

            this.pageSize = 20;
            this.page = 1;

            this.loadData();
        }
        private loadData() {
            if (this.loading) {
                return false;
            }

            this.loading = true;

            var request: BlogAdmin.Api.IGetArticleListRequest = {
                pageIndex: this.page,
                pageSize: this.pageSize,
                keywords: this.keywords,
                status: this.status
            };

            this.$api.getArticleList(request, response=> {
                this.loading = false;

                this.total = response.total;
                this.total = 500;
                this.articleList = response.data;
            });
        }
        pageChanged() {
            this.loadData();
        }
        addArticle() {
            this.$location.path('modifyarticle');
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
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $dialog: BlogAdmin.Services.Dialog) {
            $scope.vm = this;

            $scope.$emit('changeMenu', 'article', 'articlelist');

            this.editor = KindEditor.create('#txtContent', {
                resizeType: 0
            });

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

            var arr = [
                loadCategory,
                loadUser
            ];

            async.parallel(arr,(err) => {
                self.loading = false;

                if (err) {
                    self.$dialog.error(err.message);
                }
                else {

                }
            });
        }
        getArticleData(): BlogAdmin.Api.IAddArticleRequest {
            var request: BlogAdmin.Api.IAddArticleRequest = {
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

            this.$api.addArticle(data, response=> {
                this.loading = false;

                if (response.success) {
                    this.$dialog.success('操作成功');
                }
                else {
                    this.$dialog.error(response.errorMessage);
                }
            });
        }
    }
}

angular.module('blogAdmin.controllers')
    .controller('articleListCtrl', BlogAdmin.Controllers.ArticleList)
    .controller('modifyArticleCtrl', BlogAdmin.Controllers.ModifyArticle)