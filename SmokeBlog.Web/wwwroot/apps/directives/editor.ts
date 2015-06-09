module BlogAdmin.Directives {
    export function EditorWrapper(): ng.IDirective {
        var link = function (scope, element, attrs) {
                  
            var editor = new Editor({
                element: element.get(0)
            });
            editor.render();
        }

        return {
            restrict: 'A',
            link: link,
            scope: {
                ngModel: "="
            }
        }
    }
}

angular.module('blogAdmin.directives').directive('editor', BlogAdmin.Directives.EditorWrapper);