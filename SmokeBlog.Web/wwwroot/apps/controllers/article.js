var BlogAdmin;
(function (BlogAdmin) {
    var Controllers;
    (function (Controllers) {
        var ArticleList = (function () {
            function ArticleList($scope, $location, $api, $dialog) {
                this.$scope = $scope;
                this.$location = $location;
                this.$api = $api;
                this.$dialog = $dialog;
                $scope.vm = this;
                $scope.$emit('changeMenu', 'article', 'articlelist');
                this.pageSize = 20;
                this.page = $location.search().page || 1;
                this.page = parseInt(this.page.toString());
                this.status = $location.search().status;
                this.keywords = $location.search().keywords;
                this.loadData();
            }
            ArticleList.prototype.loadData = function () {
                var _this = this;
                if (this.loading) {
                    return false;
                }
                this.loading = true;
                var request = {
                    pageIndex: this.page,
                    pageSize: this.pageSize,
                    keywords: this.keywords,
                    status: this.status
                };
                this.$api.getArticleList(request, function (response) {
                    _this.loading = false;
                    _this.total = response.total;
                    _this.total = 500;
                    _this.articleList = response.data;
                });
            };
            ArticleList.prototype.changePage = function (page) {
                var data = {
                    page: page,
                    status: this.status,
                    keywords: this.keywords
                };
                this.$location.search(data);
                this.page = page;
                this.loadData();
            };
            ArticleList.prototype.saveCondition = function () {
            };
            ArticleList.prototype.addArticle = function () {
                var condition = {};
                this.$location.path('modifyarticle');
            };
            return ArticleList;
        })();
        Controllers.ArticleList = ArticleList;
        var ModifyArticle = (function () {
            function ModifyArticle($scope, $api, $dialog) {
                this.$scope = $scope;
                this.$api = $api;
                this.$dialog = $dialog;
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
            ModifyArticle.prototype.init = function () {
                var self = this;
                self.loading = true;
                function loadCategory(callback) {
                    self.$api.getCategoryList(function (response) {
                        if (response.success) {
                            var data = response.data;
                            var list = [];
                            _.each(data, function (parent) {
                                list.push({ id: parent.id, name: parent.name });
                                _.each(parent.children, function (child) {
                                    list.push({ id: child.id, name: child.name, isChild: true });
                                    ;
                                });
                            });
                            self.categoryList = list;
                            callback(null, list);
                        }
                        else {
                            callback(new Error(response.errorMessage), null);
                        }
                    });
                }
                function loadUser(callback) {
                    self.$api.getUserList(function (response) {
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
                    });
                }
                var arr = [
                    loadCategory,
                    loadUser
                ];
                async.parallel(arr, function (err) {
                    self.loading = false;
                    if (err) {
                        self.$dialog.error(err.message);
                    }
                    else {
                    }
                });
            };
            ModifyArticle.prototype.getArticleData = function () {
                var request = {
                    title: this.data.title,
                    content: this.editor.html(),
                    summary: this.data.summary,
                    allowComment: this.data.allowComment,
                    status: this.data.status,
                    userID: this.data.userID,
                    postDate: this.data.postDate
                };
                var categoryIDs = _.map(_.where(this.categoryList, { checked: true }), function (item) {
                    return item.id;
                });
                request.category = categoryIDs.join(',');
                return request;
            };
            ModifyArticle.prototype.save = function (status) {
                var _this = this;
                if (this.loading) {
                    return;
                }
                this.loading = true;
                var data = this.getArticleData();
                data.status = status;
                this.$api.addArticle(data, function (response) {
                    _this.loading = false;
                    if (response.success) {
                        _this.$dialog.success('操作成功');
                    }
                    else {
                        _this.$dialog.error(response.errorMessage);
                    }
                });
            };
            return ModifyArticle;
        })();
        Controllers.ModifyArticle = ModifyArticle;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers')
    .controller('articleListCtrl', BlogAdmin.Controllers.ArticleList)
    .controller('modifyArticleCtrl', BlogAdmin.Controllers.ModifyArticle);
