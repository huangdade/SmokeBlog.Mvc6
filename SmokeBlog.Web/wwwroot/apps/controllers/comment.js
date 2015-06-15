var BlogAdmin;
(function (BlogAdmin) {
    var Controllers;
    (function (Controllers) {
        var CommentList = (function () {
            function CommentList($scope, $api, $dialog) {
                var _this = this;
                this.$scope = $scope;
                this.$api = $api;
                this.$dialog = $dialog;
                $scope.vm = this;
                $scope.$emit('changeMenu', 'article', 'commentlist');
                $scope.$watch('vm.request.status', function () {
                    _this.request.pageIndex = 1;
                    _this.loadData();
                });
                $scope.$watch('vm.checkAll', function () {
                    _.each(_this.commentList, function (item) {
                        item.checked = _this.checkAll;
                    });
                });
                this.request = {
                    pageIndex: 1,
                    pageSize: 20,
                    keywords: null,
                    status: null
                };
                this.loadData();
            }
            CommentList.prototype.loadData = function () {
                var _this = this;
                this.loading = true;
                this.$api.queryComment(this.request, function (response) {
                    _this.loading = false;
                    if (response.success) {
                        _this.total = response.total;
                        _this.commentList = response.data;
                        _this.checkAll = false;
                    }
                    else {
                        _this.$dialog.error(response.errorMessage);
                    }
                });
            };
            CommentList.prototype.hasItemChecked = function () {
                return _.any(this.commentList, { checked: true });
            };
            CommentList.prototype.changePage = function (page) {
                this.request.pageIndex = page;
                this.loadData();
            };
            CommentList.prototype.search = function (e) {
                if (e.keyCode == 13) {
                    this.request.pageIndex = 1;
                    this.loadData();
                }
            };
            CommentList.prototype.requestCallback = function (response) {
                this.loading = false;
                if (response.success) {
                    this.$dialog.success('操作成功');
                    this.loadData();
                }
                else {
                    this.$dialog.error(response.errorMessage);
                }
            };
            CommentList.prototype.changeStatus = function (status) {
                var _this = this;
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
                this.$api.changeCommentStatus(request, function (response) {
                    _this.requestCallback(response);
                });
            };
            CommentList.prototype.deleteComment = function () {
                var _this = this;
                var ids = this.getSelectComments();
                if (ids.length == 0) {
                    this.$dialog.error('请选择评论');
                    return false;
                }
                if (this.loading) {
                    return;
                }
                this.loading = true;
                this.$api.deleteComment(ids, function (response) {
                    _this.requestCallback(response);
                });
            };
            CommentList.prototype.deleteJunk = function () {
                var _this = this;
                if (this.loading) {
                    return;
                }
                this.loading = true;
                this.$api.deleteJunkComment(function (response) {
                    _this.requestCallback(response);
                });
            };
            CommentList.prototype.getSelectComments = function () {
                var ids = _.map(_.where(this.commentList, { checked: true }), function (item) {
                    return item.id;
                });
                return ids;
            };
            return CommentList;
        })();
        Controllers.CommentList = CommentList;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers').controller('commentListCtrl', BlogAdmin.Controllers.CommentList);
