angular.module('blogAdmin', ['ngRoute', 'ngMessages', 'ui.bootstrap', 'blogAdmin.controllers', 'blogAdmin.directives']);
angular.module('blogAdmin').config(['$routeProvider', '$locationProvider', function ($routeProvider, $locationProvider) {
    $routeProvider.when('/', { templateUrl: '/templates/dashboard.html', controller: 'mainCtrl' }).when('/userlist', { templateUrl: '/templates/userlist.html', controller: 'userListCtrl' }).when('/categorylist', { templateUrl: '/templates/categorylist.html', controller: 'categoryListCtrl' }).otherwise({ redirectTo: '/' });
    $locationProvider.html5Mode(true);
}]);
angular.module('blogAdmin.services', []);
angular.module('blogAdmin.directives', []);
angular.module('blogAdmin.controllers', ['blogAdmin.services', 'ui.bootstrap']);
