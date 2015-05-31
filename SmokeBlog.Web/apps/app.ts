﻿angular.module('blogAdmin', ['ngRoute', 'ngMessages', 'ui.bootstrap', 'blogAdmin.controllers', 'blogAdmin.directives']);

angular.module('blogAdmin').config(['$routeProvider', '$locationProvider', ($routeProvider: ng.route.IRouteProvider, $locationProvider: ng.ILocationProvider) => {
    $routeProvider.when('/', { templateUrl: '/apps/templates/dashboard.html', controller: 'mainCtrl' })
        .when('/userlist', { templateUrl: '/apps/templates/userlist.html', controller: 'userListCtrl' })
        .when('/profile', { templateUrl: '/apps/templates/my.html', controller: 'myCtrl' })
        .otherwise({ redirectTo: '/' })

    $locationProvider.html5Mode(true);
}]);

angular.module('blogAdmin.services', []);
angular.module('blogAdmin.directives', []);
angular.module('blogAdmin.controllers', ['blogAdmin.services', 'ui.bootstrap']);