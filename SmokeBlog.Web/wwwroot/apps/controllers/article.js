var BlogAdmin;
(function (BlogAdmin) {
    var Controllers;
    (function (Controllers) {
        var ModifyArticle = (function () {
            function ModifyArticle($scope) {
                this.$scope = $scope;
                $scope.vm = this;
                $scope.$emit('changeMenu', 'article', 'modifyarticle');
            }
            return ModifyArticle;
        })();
        Controllers.ModifyArticle = ModifyArticle;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers')
    .controller('modifyArticleCtrl', BlogAdmin.Controllers.ModifyArticle);
