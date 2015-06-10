var BlogAdmin;
(function (BlogAdmin) {
    var Controllers;
    (function (Controllers) {
        var ArticleList = (function () {
            function ArticleList($scope, $location, $api, $dialog) {
                var _this = this;
                this.$scope = $scope;
                this.$location = $location;
                this.$api = $api;
                this.$dialog = $dialog;
                $scope.vm = this;
                $scope.$emit('changeMenu', 'article', 'articlelist');
                $scope.$watch('vm.checkAll', function () {
                    _this.toggleCheckStatus();
                });
                $scope.$watch('vm.request.status', function () {
                    _this.changePage(1);
                });
                var s = $location.search();
                this.request = {
                    keywords: s.keywords,
                    pageIndex: parseInt(s.page || 1),
                    pageSize: 20,
                    status: s.status ? parseInt(s.status) : null
                };
                this.loadData();
            }
            ArticleList.prototype.toggleCheckStatus = function () {
                var _this = this;
                _.each(this.articleList, function (item) {
                    item.checked = _this.checkAll;
                });
            };
            ArticleList.prototype.loadData = function () {
                var _this = this;
                if (this.loading) {
                    return false;
                }
                this.loading = true;
                this.$api.getArticleList(this.request, function (response) {
                    _this.loading = false;
                    _this.total = response.total;
                    _this.articleList = response.data;
                    _this.checkAll = false;
                });
            };
            ArticleList.prototype.hasItemChecked = function () {
                return _.any(this.articleList, { checked: true });
            };
            ArticleList.prototype.changePage = function (page) {
                var data = {
                    page: page,
                    status: this.request.status,
                    keywords: this.request.keywords
                };
                this.$location.search(data);
                this.request.pageIndex = page;
                this.loadData();
            };
            ArticleList.prototype.search = function (e) {
                if (e.keyCode == 13) {
                    this.changePage(1);
                }
            };
            ArticleList.prototype.editArticle = function (article) {
                this.$location.path('modifyarticle/' + article.id);
            };
            ArticleList.prototype.addArticle = function () {
                this.$location.path('modifyarticle');
            };
            ArticleList.prototype.changeStatus = function (status) {
                var _this = this;
                var ids = _.map(_.where(this.articleList, { checked: true }), function (item) {
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
                this.$api.changeArticleStatus(request, function (response) {
                    _this.loading = false;
                    if (response.success) {
                        _this.$dialog.success('操作成功');
                        _this.loadData();
                    }
                    else {
                        _this.$dialog.error(response.errorMessage);
                    }
                });
            };
            return ArticleList;
        })();
        Controllers.ArticleList = ArticleList;
        var ModifyArticle = (function () {
            function ModifyArticle($scope, $api, $dialog, $routeParams) {
                this.$scope = $scope;
                this.$api = $api;
                this.$dialog = $dialog;
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
                function loadArticle(callback) {
                    self.$api.getArticle(self.id, function (response) {
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
                async.parallel(arr, function (err) {
                    self.loading = false;
                    if (err) {
                        self.$dialog.error(err.message);
                    }
                    else {
                        if (self.id) {
                            self.data.userID = self.data.user.id;
                            self.editor.html(self.data.content);
                            _.each(self.categoryList, function (cat) {
                                if (_.any(self.data.categoryList, { id: cat.id })) {
                                    cat.checked = true;
                                }
                            });
                        }
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
                if (this.id) {
                    data.id = this.id;
                }
                var saveCallback = function (response) {
                    _this.loading = false;
                    if (response.success) {
                        _this.$dialog.success('操作成功');
                        if (!_this.id) {
                            _this.id = response.data;
                        }
                        if (!status) {
                            if (!_this.id) {
                                _this.data.status = 1;
                            }
                        }
                        else {
                            _this.data.status = status;
                        }
                    }
                    else {
                        _this.$dialog.error(response.errorMessage);
                    }
                };
                if (this.id) {
                    this.$api.editArticle(data, function (response) {
                        saveCallback(response);
                    });
                }
                else {
                    this.$api.addArticle(data, function (response) {
                        saveCallback(response);
                    });
                }
            };
            return ModifyArticle;
        })();
        Controllers.ModifyArticle = ModifyArticle;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers')
    .controller('articleListCtrl', BlogAdmin.Controllers.ArticleList)
    .controller('modifyArticleCtrl', BlogAdmin.Controllers.ModifyArticle);
