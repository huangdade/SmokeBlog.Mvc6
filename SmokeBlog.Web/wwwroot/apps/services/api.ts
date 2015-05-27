module BlogAdmin.Services {
    export interface ApiResponse {
        success: boolean;
        message: string;
        data?: any;
    }

    export interface IRequestCallback {
        (response: ApiResponse): void;
    }

    enum Method {
        Get = 0,
        Post = 1
    }

    export class Api {
        constructor(private $http: ng.IHttpService) {

        }
        private getDefaultConfig(url: string, method: string): ng.IRequestShortcutConfig {
            var config: ng.IRequestConfig = {
                url: url,
                method: method
            };
            return config;
        }
        private request(url: string, method: Method, opt: any, callback: IRequestCallback): void {
            var config: ng.IRequestConfig = {
                url: url,
                method: Method[method]
            };
            angular.extend(config, opt);

            this.$http(config).success(callback).error((data, code) => {
                if (angular.isObject(data) && data.error) {

                }
                else {
                    data = {
                        success: false,
                        message: "未知错误"
                    };
                }
                callback && callback(data);
            })
        }
        post(url: string, data: any, callback: IRequestCallback) {
            var opt = {
                data: data
            };
            this.request(url, Method.Post, opt, callback);
        }
        get(url: string, params: any, callback: IRequestCallback) {
            var opt = {
                params: params
            }
            this.request(url, Method.Get, opt, callback);
        }
        getUserList(callback: IRequestCallback) {
            this.get('/api/user/all', null, callback);
        }
    }
}

angular.module('blogAdmin.services').service('$api', BlogAdmin.Services.Api);