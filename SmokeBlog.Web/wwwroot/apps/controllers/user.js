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
                    templateUrl: '/templates/modifyuser.html',
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
                    templateUrl: '/templates/modifyuser.html',
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
                var _this = this;
                if (this.id) {
                    this.title = "编辑用户";
                }
                else {
                    this.title = "新增用户";
                }
                if (this.id) {
                    this.loading = true;
                    this.$api.getUser(this.id, function (response) {
                        _this.loading = false;
                        if (response.success) {
                            _this.data = response.data;
                        }
                        else {
                            _this.close();
                        }
                    });
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
                    this.$api.editUser(this.data, function (response) {
                        _this.requestCallback(response);
                    });
                }
                else {
                    this.$api.addUser(this.data, function (response) {
                        _this.requestCallback(response);
                    });
                }
            };
            return ModifyUser;
        })();
        Controllers.ModifyUser = ModifyUser;
        var My = (function () {
            function My($scope, $api, $modalInstance, $dialog) {
                this.$scope = $scope;
                this.$api = $api;
                this.$modalInstance = $modalInstance;
                this.$dialog = $dialog;
                $scope.vm = this;
                this.init();
            }
            My.prototype.init = function () {
                var _this = this;
                this.loading = true;
                this.$api.getMyInfo(function (response) {
                    _this.loading = false;
                    _this.user = response.data;
                });
            };
            My.prototype.close = function () {
                this.$modalInstance.dismiss();
            };
            My.prototype.submit = function (form) {
                var _this = this;
                form.submitted = true;
                if (this.loading || form.$invalid) {
                    return false;
                }
                var data = {
                    email: this.user.email,
                    nickname: this.user.nickname
                };
                this.loading = true;
                this.$api.updateMyInfo(data, function (response) {
                    if (response.success) {
                        _this.$dialog.success('操作成功');
                        _this.$modalInstance.close(true);
                    }
                    else {
                        _this.$dialog.error(response.errorMessage);
                    }
                });
            };
            return My;
        })();
        Controllers.My = My;
        var ChangePassword = (function () {
            function ChangePassword() {
            }
            return ChangePassword;
        })();
        Controllers.ChangePassword = ChangePassword;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers').controller('userListCtrl', BlogAdmin.Controllers.UserList).controller('modifyUserCtrl', BlogAdmin.Controllers.ModifyUser).controller('myCtrl', BlogAdmin.Controllers.My);
