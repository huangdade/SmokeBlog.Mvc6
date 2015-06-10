var BlogAdmin;
(function (BlogAdmin) {
    var Controllers;
    (function (Controllers) {
        var CategoryList = (function () {
            function CategoryList($scope, $api, $modal, $dialog) {
                var _this = this;
                this.$scope = $scope;
                this.$api = $api;
                this.$modal = $modal;
                this.$dialog = $dialog;
                $scope.vm = this;
                $scope.$emit('changeMenu', 'article', 'categorylist');
                $scope.$watch('vm.checkAll', function () {
                    _this.toggleCheckStatus();
                });
                this.init();
                this.data = {
                    parentID: 3
                };
            }
            CategoryList.prototype.init = function () {
                var _this = this;
                this.loading = true;
                this.$api.getCategoryList(function (response) {
                    _this.loading = false;
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
                        _this.categoryList = list;
                        _this.checkAll = false;
                    }
                });
            };
            CategoryList.prototype.toggleCheckStatus = function () {
                var _this = this;
                _.each(this.categoryList, function (item) {
                    item.checked = _this.checkAll;
                });
            };
            CategoryList.prototype.addCategory = function () {
                var _this = this;
                this.$modal.open({
                    backdrop: "static",
                    templateUrl: "/templates/modifycategory.html",
                    controller: 'modifyCategoryCtrl',
                    resolve: {
                        id: null
                    }
                }).result.then(function (v) {
                    _this.init();
                });
            };
            CategoryList.prototype.hasItemChecked = function () {
                return _.any(this.categoryList, { checked: true });
            };
            CategoryList.prototype.editCategory = function (category) {
                var _this = this;
                this.$modal.open({
                    backdrop: "static",
                    templateUrl: "/templates/modifycategory.html",
                    controller: 'modifyCategoryCtrl',
                    resolve: {
                        id: function () {
                            return category.id;
                        }
                    }
                }).result.then(function (v) {
                    _this.init();
                });
            };
            CategoryList.prototype.deleteCategory = function () {
                var _this = this;
                var ids = _.map(_.where(this.categoryList, { checked: true }), function (item) {
                    return item.id;
                });
                if (ids.length == 0) {
                    this.$dialog.error('请选择分类');
                    return false;
                }
                if (this.loading) {
                    return;
                }
                this.loading = true;
                this.$api.deleteCategory(ids, function (response) {
                    _this.loading = false;
                    if (response.success) {
                        _this.$dialog.success('操作成功');
                        _this.init();
                    }
                    else {
                        _this.$dialog.error(response.errorMessage);
                    }
                });
            };
            return CategoryList;
        })();
        Controllers.CategoryList = CategoryList;
        var ModifyCategory = (function () {
            function ModifyCategory($scope, $api, $modalInstance, $dialog, id) {
                this.$scope = $scope;
                this.$api = $api;
                this.$modalInstance = $modalInstance;
                this.$dialog = $dialog;
                this.parentList = [];
                $scope.vm = this;
                this.id = id;
                this.data = {
                    parentID: null
                };
                if (this.id) {
                    this.title = "修改分类";
                }
                else {
                    this.title = "新增分类";
                }
                this.loadData();
            }
            ModifyCategory.prototype.loadData = function () {
                var self = this;
                self.loading = true;
                var arr = [function (callback) {
                        self.$api.getCategoryList(function (response) {
                            if (response.success) {
                                self.parentList = _.filter(response.data, function (item) {
                                    return item.id != self.id;
                                });
                                callback(null, response.data);
                            }
                            else {
                                callback(new Error(response.errorMessage), null);
                            }
                        });
                    }];
                if (self.id) {
                    arr.push(function (callback) {
                        var _this = this;
                        self.$api.getCategory(self.id, function (response) {
                            if (response.success) {
                                self.data = {
                                    id: response.data.id,
                                    name: response.data.name,
                                    parentID: response.data.parentID
                                };
                                callback(null, _this.data);
                            }
                            else {
                                callback(new Error(response.errorMessage), null);
                            }
                        });
                    });
                }
                async.parallel(arr, function (err, results) {
                    if (err) {
                        self.$dialog.error(err.message);
                    }
                    else {
                        self.loading = false;
                    }
                });
            };
            ModifyCategory.prototype.close = function () {
                this.$modalInstance.dismiss();
            };
            ModifyCategory.prototype.modifyCallback = function (response) {
                this.loading = false;
                if (response.success) {
                    this.$modalInstance.close(true);
                    this.$dialog.success('操作成功');
                }
                else {
                    this.$dialog.error(response.errorMessage);
                }
            };
            ModifyCategory.prototype.submit = function (form) {
                var _this = this;
                form.submitted = true;
                if (this.loading || form.$invalid) {
                    return false;
                }
                this.loading = true;
                if (this.id) {
                    var editRequest = {
                        id: this.data.id,
                        name: this.data.name,
                        parentID: this.data.parentID || null
                    };
                    this.$api.editCategory(editRequest, function (response) {
                        _this.modifyCallback(response);
                    });
                }
                else {
                    var addRequest = {
                        name: this.data.name,
                        parentID: this.data.parentID || null
                    };
                    this.$api.addCategory(addRequest, function (response) {
                        _this.modifyCallback(response);
                    });
                }
            };
            return ModifyCategory;
        })();
        Controllers.ModifyCategory = ModifyCategory;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers')
    .controller('categoryListCtrl', BlogAdmin.Controllers.CategoryList)
    .controller('modifyCategoryCtrl', BlogAdmin.Controllers.ModifyCategory);
