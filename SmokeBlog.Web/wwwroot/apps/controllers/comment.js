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
            return CommentList;
        })();
        Controllers.CommentList = CommentList;
    })(Controllers = BlogAdmin.Controllers || (BlogAdmin.Controllers = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.controllers').controller('commentListCtrl', BlogAdmin.Controllers.CommentList);
