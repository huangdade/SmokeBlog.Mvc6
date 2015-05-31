module BlogAdmin.Controllers {
    export class Main {
        private title: string;
        user: any;
        menus: any[];
        submenus: any[];
        currentMenu: any;
        currentSubmenu: any;
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $modal: ng.ui.bootstrap.IModalService) {
            $scope.vm = this;

            $scope.$on('changeMenu',(e, key, subKey) => {
                this.changeMenu(key, subKey);
            });

            this.init();
            this.loadUser();
        }
        private init() {
            this.menus = [
                { key: "dashboard", name: "控制台", url: "dashboard" },
                { key: "content", name: "内容", url: "articlelist", submenus: [] },
                {
                    key: "user", name: "用户", url: "userlist", submenus: [
                        { key: "userlist", name: "管理用户", url: "userlist" }
                    ]
                },
                {
                    key: "config", name: "设置", url: "config/basic", submenus: [
                        { key: "basicconfig", name: "基础设置", url: "config/basic" },
                        { key: "advanceconfig", name: "高级设置", url: "config/advance" },
                        { key: "emailconfig", name: "邮件设置", url: "config/email" },
                        { key: "formconfig", name: "表单设置", url: "config/form" },
                        { key: "codeconfig", name: "自定义代码", url: "config/code" }
                    ]
                }
            ];
        }
        private loadUser() {
            this.$api.getMyInfo(response=> {
                this.user = response.data;
            });
        }
        changeMenu(key, subKey) {
            var menu = _.find(this.menus, { key: key });
            this.currentMenu = menu;
            this.submenus = menu.submenus;
            this.currentSubmenu = null;
            if (subKey) {
                var submenu = _.find(menu.submenus, { key: subKey });
                this.currentSubmenu = submenu;
            }
        }
        updateInfo() {
            this.$modal.open({
                //backdrop: 'static',
                controller: 'myCtrl',
                templateUrl: '/apps/templates/my.html'
            });
        }
        changePassword() {

        }
    }
}

angular.module('blogAdmin.controllers').controller('mainCtrl', BlogAdmin.Controllers.Main);