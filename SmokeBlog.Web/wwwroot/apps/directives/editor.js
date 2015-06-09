var BlogAdmin;
(function (BlogAdmin) {
    var Directives;
    (function (Directives) {
        function EditorWrapper() {
            var link = function (scope, element, attrs) {
                var editor = new Editor({
                    element: element.get(0)
                });
                editor.render();
            };
            return {
                restrict: 'A',
                link: link,
                scope: {
                    ngModel: "="
                }
            };
        }
        Directives.EditorWrapper = EditorWrapper;
    })(Directives = BlogAdmin.Directives || (BlogAdmin.Directives = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.directives').directive('editor', BlogAdmin.Directives.EditorWrapper);
