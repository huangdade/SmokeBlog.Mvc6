var BlogAdmin;
(function (BlogAdmin) {
    var Controllers;
    (function (Controllers) {
        var CategoryList = (function () {
            function CategoryList($scope, $api, $modal) {
                this.$scope = $scope;
                this.$api = $api;
                this.$modal = $modal;
                $scope.vm = this;
                $scope.$emit('changeMenu', 'article', 'categorylist');
                this.init();
            }
            CategoryList.prototype.init = function () {
                var _this = this;
                this.loading = true;
                this.$api.getCategoryList(function (response) {
                    _this.loading = false;
                    _this.categoryList = response.data;
                });
            };
            CategoryList.prototype.addCategory = function () {
                this.$modal.open({
                    backdrop: "static",
                    templateUrl: "/templates/modifycategory.html",
                    controller: 'modifyCategoryCtrl',
                    resolve: {
                        id: null
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
                $scope.vm = this;
                this.id = id;
                this.init();
            }
            ModifyCategory.prototype.init = function () {
                if (this.id) {
                    this.title = "修改分类";
                }
                else {
                    this.title = "新增分类";
                }
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
                }
                else {
                    var request = {
                        name: this.data.name,
                        parentID: this.data.parentID || null
                    };
                    this.$api.addCategory(request, function (response) {
                        _this.modifyCallback(response);
                    });
                }
            };
            return ModifyCategory;
        })();
        Controllers.ModifyCategory = ModifyCategory;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers').controller('categoryListCtrl', BlogAdmin.Controllers.CategoryList).controller('modifyCategoryCtrl', BlogAdmin.Controllers.ModifyCategory);
