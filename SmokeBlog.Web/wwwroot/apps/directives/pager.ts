module BlogAdmin.Directives {
    export function Pager(): ng.IDirective {
        function link(scope, element, attrs) {
            scope.change = function (page) {
                scope.pageChanged({ page: page });
            }

            scope.$watch('page', () => {
                console.log(typeof scope.page)

                render();
            })
            scope.$watch('total', ()=> {
                caculatePage();
                render();
            })
            scope.$watch('pageSize', () => {
                caculatePage();
                render();
            })

            scope.items = [];

            function caculatePage() {
                var pageSize = scope.pageSize || 20;
                var total = scope.total || 0;

                var pages = Math.ceil(total / pageSize);

                scope.pages = pages;
            }

            function render() {
                scope.items = [];

                var start = scope.page - 4;
                if (start < 1) {
                    start = 1;
                }
                var end = start + 7;
                if (end > scope.pages) {
                    end = scope.pages;
                }

                for (var i = start; i <= end; i++) {
                    scope.items.push(i);
                }
            }

            scope.disableFirst = function () {
                return scope.page == 1;
            }
            scope.disablePrev = function () {
                return scope.page == 1;
            }
            scope.disableNext = function () {
                return scope.page == scope.pages;
            }
            scope.disableLast = function () {
                return scope.page == scope.pages;
            }

            render();
        }

        var html = '';
        html += '<ul class="pagination">';
        html += '   <li ng-class="{disabled: disableFirst()}"><a href="javascript:void(0)" ng-if="disableFirst()">首页</a><a href="javascript:void(0)" ng-click="change(1)" ng-if="!disableFirst()">首页</a></li>';
        html += '   <li ng-class="{disabled: disablePrev()}"><a href="javascript:void(0)" ng-if="disablePrev()">上一页</a><a href="javascript:void(0)" ng-click="change(page-1)" ng-if="!disablePrev()">上一页</a></li>';
        html += '   <li ng-class="{active: item == page}" ng-repeat="item in items"><a href="javascript:void(0)" ng-click="change(item)">{{item}}</a></li>';
        html += '   <li ng-class="{disabled: disableNext()}"><a href="javascript:void(0)" ng-if="disableNext()">下一页</a><a href="javascript:void(0)" ng-click="change(page+1)" ng-if="!disableNext()">下一页</a></li>';
        html += '   <li ng-class="{disabled: disableLast()}"><a href="javascript:void(0)" ng-if="disableLast()">末页</a><a href="javascript:void(0)" ng-click="change(pages)" ng-if="!disableLast()">末页</a></li>';
        html += '</ul>';

        return {
            restrict: 'E',
            link: link,
            template: html,
            transclude: true,
            scope: {
                total: '=',
                page: '=',
                pageSize: '=',
                pageChanged: '&pageChanged',
            }
        }
    }
}

angular.module('blogAdmin.directives').directive('blogPager', BlogAdmin.Directives.Pager);