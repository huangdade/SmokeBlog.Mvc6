angular.module('blogAdmin', ['ngRoute', 'blogAdmin.controllers', 'ui.bootstrap']);

angular.module('blogAdmin').config(['$routeProvider', '$locationProvider', ($routeProvider: ng.route.IRouteProvider, $locationProvider: ng.ILocationProvider) => {
    $routeProvider.when('/', { templateUrl: '/apps/templates/dashboard.html', controller: 'mainCtrl' })
        .when('/userlist', { templateUrl: '/apps/templates/userlist.html', controller: 'userListCtrl' })
        .otherwise({ redirectTo: '/' })

    $locationProvider.html5Mode(true);
}]);

angular.module('blogAdmin.services', []);
angular.module('blogAdmin.controllers', ['blogAdmin.services', 'ui.bootstrap']);