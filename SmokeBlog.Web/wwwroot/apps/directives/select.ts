module BlogAdmin.Directives {
    export function Select(): ng.IDirective {
        var link = function (scope, element) {
            $(element).addClass('selectpicker').selectpicker();
        }

        return {
            link: link,
            restrict: 'A'
        }
    }
}

angular.module('blogAdmin.directives')
    .directive('select', BlogAdmin.Directives.Select);