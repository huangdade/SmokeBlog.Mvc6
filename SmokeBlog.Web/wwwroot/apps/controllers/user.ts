module BlogAdmin.Controllers {
    export class UserList {
        userList: any[];
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $modal: ng.ui.bootstrap.IModalService) {
            $scope.vm = this;
            this.init();
        }
        private init() {
            this.$api.getUserList(response=> {
                this.userList = response.data;
            });
        }
        addUser() {
            this.$modal.open({
                backdrop: true,
                controller: 'modifyUserCtrl',
                template: 'gagaga'
            });
        }
    }

    export class ModifyUser {
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $modalInstance: ng.ui.bootstrap.IModalServiceInstance) {

        }
    }
}
angular.module('blogAdmin.controllers')
    .controller('userListCtrl', BlogAdmin.Controllers.UserList)
    .controller('modifyUserCtrl', BlogAdmin.Controllers.ModifyUser);