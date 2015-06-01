module BlogAdmin.Controllers {
    export class CategoryList {
        loading: boolean;
        categoryList: any[];
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $modal: ng.ui.bootstrap.IModalService) {
            $scope.vm = this;

            $scope.$emit('changeMenu', 'article', 'categorylist');

            this.init();
        }
        private init() {
            this.loading = true;

            this.$api.getCategoryList(response=> {
                this.loading = false;

                this.categoryList = response.data;
            });
        }
        addCategory() {
            this.$modal.open({
                backdrop: "static",
                templateUrl: "/templates/modifycategory.html",
                controller: 'modifyCategoryCtrl',
                resolve: {
                    id: null
                }
            })
        }
    }
    export class ModifyCategory {
        title: string;
        loading: boolean;
        data: any;
        id: any;
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $modalInstance: ng.ui.bootstrap.IModalServiceInstance, private $dialog: BlogAdmin.Services.Dialog, id) {
            $scope.vm = this;

            this.id = id;
            this.init();
        }
        private init() {
            if (this.id) {
                this.title = "修改分类";
            }
            else {
                this.title = "新增分类";
            }
        }
        close() {
            this.$modalInstance.dismiss();
        }
        private modifyCallback(response: BlogAdmin.Services.IApiResponse) {
            this.loading = false;

            if (response.success) {
                this.$modalInstance.close(true);
                this.$dialog.success('操作成功');
            }
            else {
                this.$dialog.error(response.errorMessage);
            }
        }
        submit(form) {
            form.submitted = true;

            if (this.loading || form.$invalid) {
                return false;
            }

            this.loading = true;

            if (this.id) {

            }
            else {
                var request: BlogAdmin.Api.IAddCategoryRequest = {
                    name: this.data.name,
                    parentID: this.data.parentID || null
                };

                this.$api.addCategory(request, response=> {
                    this.modifyCallback(response);
                });
            }
        }
    }
}

angular.module('blogAdmin.controllers')
    .controller('categoryListCtrl', BlogAdmin.Controllers.CategoryList)
    .controller('modifyCategoryCtrl', BlogAdmin.Controllers.ModifyCategory);