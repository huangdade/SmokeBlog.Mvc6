module BlogAdmin.Services {
    export class Dialog {
        constructor() {
            Messenger.options = {
                extraClasses: "messenger-fixed messenger-on-top messenger-on-right",
                theme: "future"
            }
        }
        info(message: string) {
            Messenger().post({
                message: message
            });
        }
        error(message: string) {
            Messenger().post({
                message: message,
                type: "error"
            });
        }
        success(message: string) {
            Messenger().post({
                message: message,
                type: "success"
            });
        }
    }
}

angular.module('blogAdmin.services')
    .service('$dialog', BlogAdmin.Services.Dialog);