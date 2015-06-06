var BlogAdmin;
(function (BlogAdmin) {
    var Directives;
    (function (Directives) {
        function Select($parse) {
            return {
                restrict: 'A',
                require: '?ngModel',
                priority: 10,
                compile: function (tElement, tAttrs, transclude) {
                    return function (scope, element, attrs, ngModel) {
                        element.selectpicker($parse(tAttrs.selectpicker)(null));
                        element.selectpicker('refresh');
                        if (!ngModel)
                            return;
                        scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                            scope.$evalAsync(function () {
                                if (!attrs.ngOptions || /track by/.test(attrs.ngOptions))
                                    element.val(newVal);
                                element.selectpicker('refresh');
                            });
                        });
                        ngModel.$render = function () {
                            scope.$evalAsync(function () {
                                element.selectpicker('refresh');
                            });
                        };
                    };
                }
            };
        }
        Directives.Select = Select;
    })(Directives = BlogAdmin.Directives || (BlogAdmin.Directives = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.directives')
    .directive('select', ['$parse', BlogAdmin.Directives.Select]);
