module BlogAdmin.Directives {
    interface ISelectAttrs extends ng.IAttributes {
        selectpicker: string;
    }

    export function Select($parse: ng.IParseService): ng.IDirective {
        return {
            restrict: 'A',
            require: '?ngModel',
            priority: 10,
            compile: function (tElement, tAttrs: ISelectAttrs, transclude) {                
                return function (scope, element, attrs, ngModel) {          
                    element.selectpicker($parse(tAttrs.selectpicker)(null));
                    element.selectpicker('refresh');
                              
                    if (!ngModel) return;

                    scope.$watch(attrs.ngModel, function (newVal, oldVal) {
                        scope.$evalAsync(function () {
                            if (!attrs.ngOptions || /track by/.test(attrs.ngOptions)) element.val(newVal);
                            element.selectpicker('refresh');
                        });
                    });

                    ngModel.$render = function () {
                        scope.$evalAsync(function () {
                            element.selectpicker('refresh');
                        });
                    }
                };
            }

        };
    }
}

angular.module('blogAdmin.directives')
    .directive('select', ['$parse', BlogAdmin.Directives.Select]);