var BlogAdmin;
(function (BlogAdmin) {
    var Controllers;
    (function (Controllers) {
        var CategoryList = (function () {
            function CategoryList($scope, $api) {
                this.$scope = $scope;
                this.$api = $api;
                $scope.vm = this;
            }
            return CategoryList;
        })();
        Controllers.CategoryList = CategoryList;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers').controller('categoryListCtrl', BlogAdmin.Controllers.CategoryList);
