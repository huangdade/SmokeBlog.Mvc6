module BlogAdmin.Controllers {
    export class UserList {
        userList: any[];
        loading: boolean;
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $modal: ng.ui.bootstrap.IModalService) {
            $scope.vm = this;
            this.loadUser();

            $scope.$emit("changeMenu", "user", "userlist");
        }
        private loadUser() {
            this.loading = true;
            this.$api.getUserList(response=> {
                this.userList = response.data;
                this.loading = false;
            });
        }
        addUser() {
            this.$modal.open({
                backdrop: 'static',
                controller: 'modifyUserCtrl',
                templateUrl: '/templates/modifyuser.html',
                resolve: {
                    id: null
                }
            }).result.then(r=> {
                this.modifyUserComplete();
            });;
        }
        editUser(user) {
            this.$modal.open({
                backdrop: 'static',
                controller: 'modifyUserCtrl',
                templateUrl: '/templates/modifyuser.html',
                resolve: {
                    id: () => {
                        return user.id;
                    }
                }
            }).result.then(r=> {
                this.modifyUserComplete();
            });
        }
        modifyUserComplete() {
            this.loadUser();
        }
    }

    export class ModifyUser {
        data: any;
        title: string;
        loading: boolean;
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $modalInstance: ng.ui.bootstrap.IModalServiceInstance, private id) {
            $scope.vm = this;
            this.init();
        }
        private init() {
            if (this.id) {
                this.title = "编辑用户";
            }
            else {
                this.title = "新增用户";
            }

            if (this.id) {
                this.loading = true;
                this.$api.getUser(this.id, response=> {
                    this.loading = false;
                    if (response.success) {
                        this.data = response.data;
                    }
                    else {
                        this.close();
                    }
                })
            }
        }
        close() {
            this.$modalInstance.dismiss();
        }
        private requestCallback(response) {
            this.loading = false;
            if (response.success) {
                this.$modalInstance.close(response.data);
            }
        }
        submit() {
            if (this.loading) {
                return;
            }

            this.$scope.frmUser.submitted = true;
            if (this.$scope.frmUser.$invalid) {
                return false;
            }
            this.loading = true;
            if (this.id) {
                this.$api.editUser(this.data, response=> {
                    this.requestCallback(response);
                });
            }
            else {
                this.$api.addUser(this.data, response=> {
                    this.requestCallback(response);
                });
            }
        }
    }
    export class My {
        user: any;
        loading: boolean;
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $modalInstance: ng.ui.bootstrap.IModalServiceInstance, private $dialog: BlogAdmin.Services.Dialog) {
            $scope.vm = this;

            this.init();
        }
        private init() {
            this.loading = true;
            this.$api.getMyInfo(response=> {
                this.loading = false;
                this.user = response.data;
            });
        }
        close() {
            this.$modalInstance.dismiss();
        }
        submit(form) {
            form.submitted = true;

            if (this.loading || form.$invalid) {
                return false;
            }

            var data: BlogAdmin.Api.IUpdateInfoRequest = {
                email: this.user.email,
                nickname: this.user.nickname
            };
            
            this.loading = true;
            this.$api.updateMyInfo(data, response=> {
                if (response.success) {
                    this.$dialog.success('操作成功');
                    this.$modalInstance.close(true);
                }
                else {
                    this.$dialog.error(response.errorMessage);
                }
            });
        }
    }
    export class ChangePassword {

    }
}
angular.module('blogAdmin.controllers')
    .controller('userListCtrl', BlogAdmin.Controllers.UserList)
    .controller('modifyUserCtrl', BlogAdmin.Controllers.ModifyUser)
    .controller('myCtrl', BlogAdmin.Controllers.My);