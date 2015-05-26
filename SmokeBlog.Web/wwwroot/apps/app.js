angular.module('blogAdmin', ['ngRoute', 'blogAdmin.controllers']);
angular.module('blogAdmin').config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
    $routeProvider.when('/', { templateUrl: '/apps/templates/dashboard.html', controller: 'mainCtrl' }).otherwise({ redirectTo: '/' });
    $locationProvider.html5Mode(true);
}]);
angular.module('blogAdmin.controllers', []);
