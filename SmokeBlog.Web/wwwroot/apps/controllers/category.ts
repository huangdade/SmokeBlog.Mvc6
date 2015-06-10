module BlogAdmin.Controllers {
    export class CategoryList {
        loading: boolean;
        categoryList: any[];
        data: any;
        checkAll: boolean;
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $modal: ng.ui.bootstrap.IModalService, private $dialog: BlogAdmin.Services.Dialog) {
            $scope.vm = this;

            $scope.$emit('changeMenu', 'article', 'categorylist');

            $scope.$watch('vm.checkAll', () => {
                this.toggleCheckStatus();
            })

            this.init();
            this.data = {
                parentID: 3
            }
        }
        private init() {
            this.loading = true;

            this.$api.getCategoryList(response=> {
                this.loading = false;

                if (response.success) {
                    var data = response.data;
                    var list = [];

                    _.each<any>(data, parent=> {
                        list.push({ id: parent.id, name: parent.name });

                        _.each<any>(parent.children, child=> {
                            list.push({ id: child.id, name: child.name, isChild: true });
                            ;
                        });
                    });


                    this.categoryList = list;

                    this.checkAll = false;
                }
            });
        }
        private toggleCheckStatus() {
            _.each(this.categoryList, item=> {
                item.checked = this.checkAll;
            })
        }
        addCategory() {
            this.$modal.open({
                backdrop: "static",
                templateUrl: "/templates/modifycategory.html",
                controller: 'modifyCategoryCtrl',
                resolve: {
                    id: null
                }
            }).result.then(v=> {
                this.init();
            });
        }       
        hasItemChecked() {
            return _.any(this.categoryList, { checked: true });
        }
        editCategory(category) {
            this.$modal.open({
                backdrop: "static",
                templateUrl: "/templates/modifycategory.html",
                controller: 'modifyCategoryCtrl',
                resolve: {
                    id: function () {
                        return category.id;
                    }
                }
            }).result.then(v=> {
                this.init();
            });
        }
        deleteCategory() {
            var ids = _.map(_.where(this.categoryList, { checked: true }), item=> {
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

            this.$api.deleteCategory(ids, response=> {
                this.loading = false;

                if (response.success) {
                    this.$dialog.success('操作成功');
                    this.init();
                }
                else
                {
                    this.$dialog.error(response.errorMessage);
                }
            });
        }
    }
    export class ModifyCategory {
        title: string;
        loading: boolean;
        data: any;
        id: any;
        parentList: any[] = [];
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $modalInstance: ng.ui.bootstrap.IModalServiceInstance, private $dialog: BlogAdmin.Services.Dialog, id) {
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
        private loadData() {
            var self = this;

            self.loading = true;            

            var arr = [function (callback) {
                self.$api.getCategoryList(response=> {
                    if (response.success) {
                        self.parentList = _.filter<any>(response.data, item=> {                            
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
                    self.$api.getCategory(self.id, response=> {
                        if (response.success) {
                            self.data = {
                                id: response.data.id,
                                name: response.data.name,
                                parentID: response.data.parentID
                            }
                            callback(null, this.data);
                        }
                        else {
                            callback(new Error(response.errorMessage), null);
                        }
                    });
                });
            }

            async.parallel(arr, (err, results) => {
                if (err) {
                    self.$dialog.error(err.message);
                }
                else {
                    self.loading = false;
                }
            });
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
                var editRequest: BlogAdmin.Api.IEditCategoryRequest = {
                    id: this.data.id,
                    name: this.data.name,
                    parentID: this.data.parentID || null
                };

                this.$api.editCategory(editRequest, response=> {
                    this.modifyCallback(response);
                })
            }
            else {
                var addRequest: BlogAdmin.Api.IAddCategoryRequest = {
                    name: this.data.name,
                    parentID: this.data.parentID || null
                };

                this.$api.addCategory(addRequest, response=> {
                    this.modifyCallback(response);
                });
            }
        }
    }
}

angular.module('blogAdmin.controllers')
    .controller('categoryListCtrl', BlogAdmin.Controllers.CategoryList)
    .controller('modifyCategoryCtrl', BlogAdmin.Controllers.ModifyCategory);