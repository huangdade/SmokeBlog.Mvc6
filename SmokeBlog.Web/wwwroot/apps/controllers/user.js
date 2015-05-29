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
            function My($scope, $api) {
                this.$scope = $scope;
                this.$api = $api;
                $scope.vm = this;
                $scope.$emit('changeMenu', 'user', 'profile');
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
            return My;
        })();
        Controllers.My = My;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers').controller('userListCtrl', BlogAdmin.Controllers.UserList).controller('modifyUserCtrl', BlogAdmin.Controllers.ModifyUser).controller('myCtrl', BlogAdmin.Controllers.My);
