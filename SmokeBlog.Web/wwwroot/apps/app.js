angular.module('blogAdmin', ['ngRoute']);
angular.module('blogAdmin').config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
    $routeProvider.when('/', { templateUrl: '/apps/templates/dashboard.html' }).otherwise({ redirectTo: '/' });
    $locationProvider.html5Mode(true);
}]);
