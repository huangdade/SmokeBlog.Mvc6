var BlogAdmin;
(function (BlogAdmin) {
    var Controllers;
    (function (Controllers) {
        var UserList = (function () {
            function UserList($scope, $api, $modal) {
                this.$scope = $scope;
                this.$api = $api;
                this.$modal = $modal;
                $scope.vm = this;
                this.init();
            }
            UserList.prototype.init = function () {
                var _this = this;
                this.$api.getUserList(function (response) {
                    _this.userList = response.data;
                });
            };
            UserList.prototype.addUser = function () {
                this.$modal.open({
                    backdrop: true,
                    controller: 'modifyUserCtrl',
                    template: 'gagaga'
                });
            };
            return UserList;
        })();
        Controllers.UserList = UserList;
        var ModifyUser = (function () {
            function ModifyUser($scope, $api, $modalInstance) {
                this.$scope = $scope;
                this.$api = $api;
                this.$modalInstance = $modalInstance;
            }
            return ModifyUser;
        })();
        Controllers.ModifyUser = ModifyUser;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers').controller('userListCtrl', BlogAdmin.Controllers.UserList).controller('modifyUserCtrl', BlogAdmin.Controllers.ModifyUser);
