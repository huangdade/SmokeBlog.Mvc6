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

            $scope.$watch('vm.checkAll',() => {
                _.each(this.commentList, item=> {
                    item.checked = this.checkAll;
                });
            });

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
        requestCallback(response: BlogAdmin.Services.IApiResponse) {
            this.loading = false;

            if (response.success) {
                this.$dialog.success('操作成功');
                this.loadData();
            }
            else {
                this.$dialog.error(response.errorMessage);
            }
        }
        changeStatus(status) {
            var ids = this.getSelectComments();

            if (ids.length == 0) {
                this.$dialog.error('请选择评论');
                return false;
            }

            if (this.loading) {
                return;
            }

            this.loading = true;

            var request = {
                ids: ids,
                status: status
            };

            this.$api.changeCommentStatus(request, response=> {
                this.requestCallback(response);
            })
        }
        deleteComment() {
            var ids = this.getSelectComments();

            if (ids.length == 0) {
                this.$dialog.error('请选择评论');
                return false;
            }

            if (this.loading) {
                return;
            }

            this.loading = true;

            this.$api.deleteComment(ids, response=> {
                this.requestCallback(response);
            })
        }
        deleteJunk() {
            if (this.loading) {
                return;
            }

            this.loading = true;

            this.$api.deleteJunkComment(response=> {
                this.requestCallback(response);
            })
        }
        private getSelectComments() {
            var ids = _.map(_.where(this.commentList, { checked: true }), item=> {
                return item.id;
            });
            return ids;
        }
    }
}

angular.module('blogAdmin.controllers')
    .controller('commentListCtrl', BlogAdmin.Controllers.CommentList);