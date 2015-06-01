var BlogAdmin;
(function (BlogAdmin) {
    var Services;
    (function (Services) {
        var Dialog = (function () {
            function Dialog() {
                Messenger.options = {
                    extraClasses: "messenger-fixed messenger-on-top messenger-on-right",
                    theme: "future"
                };
            }
            Dialog.prototype.info = function (message) {
                Messenger().post({
                    message: message
                });
            };
            Dialog.prototype.error = function (message) {
                Messenger().post({
                    message: message,
                    type: "error"
                });
            };
            Dialog.prototype.success = function (message) {
                Messenger().post({
                    message: message,
                    type: "success"
                });
            };
            return Dialog;
        })();
        Services.Dialog = Dialog;
    })(Services = BlogAdmin.Services || (BlogAdmin.Services = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.services').service('$dialog', BlogAdmin.Services.Dialog);
