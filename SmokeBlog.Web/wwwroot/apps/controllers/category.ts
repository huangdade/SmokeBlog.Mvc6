module BlogAdmin.Controllers {
    export class CategoryList {
        loading: boolean;
        categoryList: any[];
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $modal: ng.ui.bootstrap.IModalService) {
            $scope.vm = this;

            $scope.$emit('changeMenu', 'article', 'categorylist')
        }
        addCategory() {
            this.$modal.open({
                backdrop: "static",
                templateUrl: "/templates/modifycategory.html",
                controller: 'modifyCategoryCtrl'
            })
        }
    }
    export class ModifyCategory {
        loading: boolean;
        data: any;
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $modalInstance: ng.ui.bootstrap.IModalServiceInstance) {
            $scope.vm = this;
        }
        close() {
            this.$modalInstance.dismiss();
        }
        submit(form) {
            form.submitted = true;

            if (this.loading || form.$invalid) {
                return false;
            }
        }
    }
}

angular.module('blogAdmin.controllers')
    .controller('categoryListCtrl', BlogAdmin.Controllers.CategoryList)
    .controller('modifyCategoryCtrl', BlogAdmin.Controllers.ModifyCategory);