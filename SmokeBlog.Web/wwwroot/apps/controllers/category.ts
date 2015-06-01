module BlogAdmin.Controllers {
    export class CategoryList {
        loading: boolean;
        categoryList: any[];
        constructor(private $scope, private $api: BlogAdmin.Services.Api) {
            $scope.vm = this;
        }
    }
}

angular.module('blogAdmin.controllers')
    .controller('categoryListCtrl', BlogAdmin.Controllers.CategoryList);