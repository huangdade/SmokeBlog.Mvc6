module BlogAdmin.Controllers {
    export class CommentList {
        loading: boolean;
        request: BlogAdmin.Api.IQueryCommentRequest;
        total: number;
        commentList: any[];
        checkAll: boolean;
        constructor(private $scope, private $api: BlogAdmin.Services.Api, private $dialog: BlogAdmin.Services.Dialog) {
            $scope.vm = this;

            $scope.$emit('changeMenu', 'article', 'commentlist');

            $scope.$watch('vm.request.status',() => {
                this.request.pageIndex = 1;
                this.loadData();
            })

            this.request = {
                pageIndex: 1,
                pageSize: 20,
                keywords: null,
                status: null
            }

            this.loadData();
        }
        private loadData() {
            this.loading = true;

            this.$api.queryComment(this.request, response=> {
                this.loading = false;
                if (response.success) {
                    this.total = response.total;
                    this.commentList = response.data;
                    this.checkAll = false;
                }
                else {
                    this.$dialog.error(response.errorMessage);
                }
            })
        }
        hasItemChecked() {
            return _.any(this.commentList, { checked: true });
        }
        changePage(page) {
            this.request.pageIndex = page;
            this.loadData();
        }
        search(e) {
            if (e.keyCode == 13) {
                this.request.pageIndex = 1;
                this.loadData();
            }
        }
    }
}

angular.module('blogAdmin.controllers')
    .controller('commentListCtrl', BlogAdmin.Controllers.CommentList);