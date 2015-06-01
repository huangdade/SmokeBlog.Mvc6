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
            }
            CategoryList.prototype.addCategory = function () {
                this.$modal.open({
                    backdrop: "static",
                    templateUrl: "/templates/modifycategory.html",
                    controller: 'modifyCategoryCtrl'
                });
            };
            return CategoryList;
        })();
        Controllers.CategoryList = CategoryList;
        var ModifyCategory = (function () {
            function ModifyCategory($scope, $api, $modalInstance) {
                this.$scope = $scope;
                this.$api = $api;
                this.$modalInstance = $modalInstance;
                $scope.vm = this;
            }
            ModifyCategory.prototype.close = function () {
                this.$modalInstance.dismiss();
            };
            ModifyCategory.prototype.submit = function (form) {
                form.submitted = true;
                if (this.loading || form.$invalid) {
                    return false;
                }
            };
            return ModifyCategory;
        })();
        Controllers.ModifyCategory = ModifyCategory;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers').controller('categoryListCtrl', BlogAdmin.Controllers.CategoryList).controller('modifyCategoryCtrl', BlogAdmin.Controllers.ModifyCategory);
