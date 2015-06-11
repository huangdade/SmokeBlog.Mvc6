angular.module('blogAdmin', ['ngRoute', 'ngMessages', 'ui.bootstrap', 'blogAdmin.controllers', 'blogAdmin.directives']);

angular.module('blogAdmin').config(['$routeProvider', '$locationProvider', ($routeProvider: ng.route.IRouteProvider, $locationProvider: ng.ILocationProvider) => {
    $routeProvider.when('/', { templateUrl: '/templates/dashboard.html', controller: 'mainCtrl' })
        .when('/userlist', { templateUrl: '/templates/userlist.html', controller: 'userListCtrl' })
        .when('/categorylist', { templateUrl: '/templates/categorylist.html', controller: 'categoryListCtrl' })
        .when('/articlelist', { templateUrl: '/templates/articlelist.html', controller: 'articleListCtrl', reloadOnSearch: false })
        .when('/modifyarticle/:id?', { templateUrl: '/templates/modifyarticle.html', controller: 'modifyArticleCtrl' })
        .when('/commentlist', { templateUrl: '/templates/commentlist.html', controller: 'commentListCtrl' })
        .otherwise({ redirectTo: '/' })

    $locationProvider.html5Mode(true);
}]);

angular.module('blogAdmin.services', []);
angular.module('blogAdmin.directives', []);
angular.module('blogAdmin.controllers', ['blogAdmin.services', 'ui.bootstrap']);