module BlogAdmin.Controllers {
    export class ModifyArticle {
        constructor(private $scope) {
            $scope.vm = this;

            $scope.$emit('changeMenu', 'article', 'modifyarticle')
        }
    }
}

angular.module('blogAdmin.controllers')
    .controller('modifyArticleCtrl', BlogAdmin.Controllers.ModifyArticle)