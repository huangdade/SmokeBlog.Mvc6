var BlogAdmin;
(function (BlogAdmin) {
    var Controllers;
    (function (Controllers) {
        var ArticleList = (function () {
            function ArticleList($scope, $location, $api, $dialog) {
                this.$scope = $scope;
                this.$location = $location;
                this.$api = $api;
                this.$dialog = $dialog;
                $scope.vm = this;
                $scope.$emit('changeMenu', 'article', 'articlelist');
                this.pageSize = 20;
                this.page = 1;
                this.loadData();
            }
            ArticleList.prototype.loadData = function () {
                var _this = this;
                if (this.loading) {
                    return false;
                }
                this.loading = true;
                var request = {
                    pageIndex: this.page,
                    pageSize: this.pageSize,
                    keywords: this.keywords,
                    status: this.status
                };
                this.$api.getArticleList(request, function (response) {
                    _this.loading = false;
                    _this.total = response.total;
                    _this.total = 500;
                    _this.articleList = response.data;
                });
            };
            ArticleList.prototype.pageChanged = function () {
                this.loadData();
            };
            return ArticleList;
        })();
        Controllers.ArticleList = ArticleList;
        var ModifyArticle = (function () {
            function ModifyArticle($scope, $location) {
                this.$scope = $scope;
                this.$location = $location;
                $scope.vm = this;
                $scope.$emit('changeMenu', 'article', 'articlelist');
            }
            return ModifyArticle;
        })();
        Controllers.ModifyArticle = ModifyArticle;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers').controller('articleListCtrl', BlogAdmin.Controllers.ArticleList).controller('modifyArticleCtrl', BlogAdmin.Controllers.ModifyArticle);
