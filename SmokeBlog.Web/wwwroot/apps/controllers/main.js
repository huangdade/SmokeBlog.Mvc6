var BlogAdmin;
(function (BlogAdmin) {
    var Controllers;
    (function (Controllers) {
        var Main = (function () {
            function Main($scope) {
                this.$scope = $scope;
                $scope.vm = this;
                this.init();
            }
            Main.prototype.init = function () {
                this.title = "dashboard";
            };
            return Main;
        })();
        Controllers.Main = Main;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers').controller('mainCtrl', BlogAdmin.Controllers.Main);
