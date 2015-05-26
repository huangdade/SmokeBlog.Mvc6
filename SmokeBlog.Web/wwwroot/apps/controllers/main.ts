module BlogAdmin.Controllers {
    export class Main {
        private title: string;
        constructor(private $scope) {
            $scope.vm = this;

            this.init();
        }
        private init() {
            this.title = "dashboard";
        }
    }
}

angular.module('blogAdmin.controllers').controller('mainCtrl', BlogAdmin.Controllers.Main);