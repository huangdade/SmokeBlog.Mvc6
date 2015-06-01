var BlogAdmin;
(function (BlogAdmin) {
    var Directives;
    (function (Directives) {
        function Select() {
            var link = function (scope, element) {
                $(element).addClass('selectpicker').selectpicker();
            };
            return {
                link: link,
                restrict: 'A'
            };
        }
        Directives.Select = Select;
    })(Directives = BlogAdmin.Directives || (BlogAdmin.Directives = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.directives').directive('select', BlogAdmin.Directives.Select);
