angular.module('blogAdmin', ['ngRoute']);

angular.module('blogAdmin').config(['$routeProvider', '$locationProvider', ($routeProvider: ng.route.IRouteProvider, $locationProvider: ng.ILocationProvider) => {
    $routeProvider.when('/', { templateUrl: '/apps/templates/dashboard.html' })
        .otherwise({ redirectTo: '/' })

    $locationProvider.html5Mode(true);
}])