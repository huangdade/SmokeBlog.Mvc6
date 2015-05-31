module BlogAdmin.Directives {
    export function Spinner(): ng.IDirective {
        return {
            restrict: 'E',
            transclude: true,
            scope: {
                loading: '='
            },
            template: '<span data-ng-show="loading"><i class="fa fa-spin fa-spinner"></i></span>'
        };
    }
}

angular.module('blogAdmin.directives')
    .directive('spinner', BlogAdmin.Directives.Spinner);