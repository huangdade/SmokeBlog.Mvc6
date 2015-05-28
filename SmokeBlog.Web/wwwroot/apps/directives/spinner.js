var BlogAdmin;
(function (BlogAdmin) {
    var Directives;
    (function (Directives) {
        function Spinner() {
            return {
                restrict: 'E',
                transclude: true,
                scope: {
                    loading: '='
                },
                template: '<span data-ng-show="loading"><i class="fa fa-spin fa-spinner"></i></span>'
            };
        }
        Directives.Spinner = Spinner;
    })(Directives = BlogAdmin.Directives || (BlogAdmin.Directives = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.directives').directive('spinner', BlogAdmin.Directives.Spinner);
