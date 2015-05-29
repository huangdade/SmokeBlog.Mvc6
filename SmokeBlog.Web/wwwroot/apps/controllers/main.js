var BlogAdmin;
(function (BlogAdmin) {
    var Controllers;
    (function (Controllers) {
        var Main = (function () {
            function Main($scope, $api, $modal) {
                var _this = this;
                this.$scope = $scope;
                this.$api = $api;
                this.$modal = $modal;
                $scope.vm = this;
                $scope.$on('changeMenu', function (e, key, subKey) {
                    _this.changeMenu(key, subKey);
                });
                this.init();
            }
            Main.prototype.init = function () {
                this.menus = [
                    { key: "dashboard", name: "控制台", url: "dashboard" },
                    { key: "content", name: "内容", url: "articlelist", submenus: [] },
                    {
                        key: "user",
                        name: "用户",
                        url: "userlist",
                        submenus: [
                            { key: "userlist", name: "管理用户", url: "userlist" }
                        ]
                    },
                    {
                        key: "config",
                        name: "设置",
                        url: "config/basic",
                        submenus: [
                            { key: "basicconfig", name: "基础设置", url: "config/basic" },
                            { key: "advanceconfig", name: "高级设置", url: "config/advance" },
                            { key: "emailconfig", name: "邮件设置", url: "config/email" },
                            { key: "formconfig", name: "表单设置", url: "config/form" },
                            { key: "codeconfig", name: "自定义代码", url: "config/code" }
                        ]
                    }
                ];
            };
            Main.prototype.changeMenu = function (key, subKey) {
                var menu = _.find(this.menus, { key: key });
                this.currentMenu = menu;
                this.submenus = menu.submenus;
                this.currentSubmenu = null;
                if (subKey) {
                    var submenu = _.find(menu.submenus, { key: subKey });
                    this.currentSubmenu = submenu;
                }
            };
            Main.prototype.updateInfo = function () {
                this.$modal.open({
                    //backdrop: 'static',
                    controller: 'myCtrl',
                    templateUrl: '/apps/templates/my.html'
                });
            };
            Main.prototype.changePassword = function () {
            };
            return Main;
        })();
        Controllers.Main = Main;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers').controller('mainCtrl', BlogAdmin.Controllers.Main);
