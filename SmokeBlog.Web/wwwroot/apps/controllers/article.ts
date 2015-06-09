module BlogAdmin.Controllers {
    export class ArticleList {
        page: number;
        pageSize: number;
        keywords: string;
        status: number;
        loading: boolean;
        articleList: any[];
        total: number;
        constructor(private $scope, private $location: ng.ILocationService, private $api: BlogAdmin.Services.Api, private $dialog: BlogAdmin.Services.Dialog) {
            $scope.vm = this;

            $scope.$emit('changeMenu', 'article', 'articlelist');

            this.pageSize = 20;
            this.page = 1;

            this.loadData();
        }
        private loadData() {
            if (this.loading) {
                return false;
            }

            this.loading = true;

            var request: BlogAdmin.Api.IGetArticleListRequest = {
                pageIndex: this.page,
                pageSize: this.pageSize,
                keywords: this.keywords,
                status: this.status
            };

            this.$api.getArticleList(request, response=> {
                this.loading = false;

                this.total = response.total;
                this.total = 500;
                this.articleList = response.data;
            });
        }
        pageChanged() {
            this.loadData();
        }
        addArticle() {
            this.$location.path('modifyarticle');
        }
    }

    export class ModifyArticle {
        editor: KindEditor.KindEditor;
        constructor(private $scope, private $location: ng.ILocationService) {
            $scope.vm = this;

            $scope.$emit('changeMenu', 'article', 'articlelist');

            this.editor = KindEditor.create('#txtContent', {
                resizeType: 0
            });
        }
    }
}

angular.module('blogAdmin.controllers')
    .controller('articleListCtrl', BlogAdmin.Controllers.ArticleList)
    .controller('modifyArticleCtrl', BlogAdmin.Controllers.ModifyArticle)