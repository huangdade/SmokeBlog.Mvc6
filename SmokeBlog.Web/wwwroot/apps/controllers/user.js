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
                this.loadUser();
                $scope.$emit("changeMenu", "user", "userlist");
            }
            UserList.prototype.loadUser = function () {
                var _this = this;
                this.loading = true;
                this.$api.getUserList(function (response) {
                    _this.userList = response.data;
                    _this.loading = false;
                });
            };
            UserList.prototype.addUser = function () {
                var _this = this;
                this.$modal.open({
                    backdrop: 'static',
                    controller: 'modifyUserCtrl',
                    templateUrl: '/apps/templates/modifyuser.html',
                    resolve: {
                        id: null
                    }
                }).result.then(function (r) {
                    _this.modifyUserComplete();
                });
                ;
            };
            UserList.prototype.editUser = function (user) {
                var _this = this;
                this.$modal.open({
                    backdrop: 'static',
                    controller: 'modifyUserCtrl',
                    templateUrl: '/apps/templates/modifyuser.html',
                    resolve: {
                        id: function () {
                            return user.id;
                        }
                    }
                }).result.then(function (r) {
                    _this.modifyUserComplete();
                });
            };
            UserList.prototype.modifyUserComplete = function () {
                this.loadUser();
            };
            return UserList;
        })();
        Controllers.UserList = UserList;
        var ModifyUser = (function () {
            function ModifyUser($scope, $api, $modalInstance, id) {
                this.$scope = $scope;
                this.$api = $api;
                this.$modalInstance = $modalInstance;
                this.id = id;
                $scope.vm = this;
                this.init();
            }
            ModifyUser.prototype.init = function () {
                if (this.id) {
                    this.title = "编辑用户";
                }
                else {
                    this.title = "新增用户";
                }
            };
            ModifyUser.prototype.close = function () {
                this.$modalInstance.dismiss();
            };
            ModifyUser.prototype.requestCallback = function (response) {
                this.loading = false;
                if (response.success) {
                    this.$modalInstance.close(response.data);
                }
            };
            ModifyUser.prototype.submit = function () {
                var _this = this;
                if (this.loading) {
                    return;
                }
                this.$scope.frmUser.submitted = true;
                if (this.$scope.frmUser.$invalid) {
                    return false;
                }
                this.loading = true;
                if (this.id) {
                }
                else {
                    this.$api.addUser(this.data.userName, this.data.password, this.data.nickname, this.data.email, function (response) {
                        _this.requestCallback(response);
                    });
                }
            };
            return ModifyUser;
        })();
        Controllers.ModifyUser = ModifyUser;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers').controller('userListCtrl', BlogAdmin.Controllers.UserList).controller('modifyUserCtrl', BlogAdmin.Controllers.ModifyUser);
