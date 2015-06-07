angular.module('blogAdmin', ['ngRoute', 'ngMessages', 'ui.bootstrap', 'blogAdmin.controllers', 'blogAdmin.directives', 'angular-bootstrap-select', 'angular-bootstrap-select.extra']);

angular.module('blogAdmin').config(['$routeProvider', '$locationProvider', ($routeProvider: ng.route.IRouteProvider, $locationProvider: ng.ILocationProvider) => {
    $routeProvider.when('/', { templateUrl: '/templates/dashboard.html', controller: 'mainCtrl' })
        .when('/userlist', { templateUrl: '/templates/userlist.html', controller: 'userListCtrl' })
        .when('/categorylist', { templateUrl: '/templates/categorylist.html', controller: 'categoryListCtrl' })
        .when('/modifyarticle', { templateUrl: '/templates/modifyarticle.html', controller: 'modifyArticleCtrl' })
        .otherwise({ redirectTo: '/' })

    $locationProvider.html5Mode(true);
}]);

angular.module('blogAdmin.services', []);
angular.module('blogAdmin.directives', []);
angular.module('blogAdmin.controllers', ['blogAdmin.services', 'ui.bootstrap']);