module BlogAdmin.Services {
    export interface IApiResponse {
        success: boolean;
        errorMessage: string;
        data?: any;
    }
    export interface IPagedApiResponse extends IApiResponse {
        total: number;
    }

    export interface IRequestCallback {
        (response: IApiResponse): void;
    }
    export interface IPagedRequestCallback {
        (response: IPagedApiResponse): void;
    }

    enum Method {
        Get = 0,
        Post = 1
    }

    export class Api {
        constructor(private $http: ng.IHttpService) {

        }        
        private request(url: string, method: Method, opt: any, callback: IRequestCallback): void {
            var config: ng.IRequestConfig = {
                url: url,
                method: Method[method],
                headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                transformRequest: function (obj) {
                    var str = [];
                    for (var p in obj) {
                        if (typeof (obj[p]) == "undefined" || obj[p] == null) {
                            continue;
                        }
                        str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                    }
                    return str.join("&");
                }
            };
            angular.extend(config, opt);

            this.$http(config).success(callback).error((data, code) => {
                if (angular.isObject(data) && data.errorMessage) {

                }
                else {
                    data = {
                        success: false,
                        errorMessage: "未知错误"
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
        getUser(id, callback: IRequestCallback) {
            var url = "/api/user/" + id;

            this.get(url, null, callback);
        }
        addUser(request: BlogAdmin.Api.IAddUserRequest, callback: IRequestCallback) {
            this.post('/api/user/add', request, callback);
        }
        editUser(request: BlogAdmin.Api.IEditUserRequest, callback: IRequestCallback) {
            this.post('/api/user/edit', request, callback);
        }
        getMyInfo(callback: IRequestCallback) {
            this.get('/api/my', null, callback);
        }
        updateMyInfo(request: BlogAdmin.Api.IUpdateInfoRequest, callback: IRequestCallback) {
            this.post('/api/my/update', request, callback);
        }
        changePassword(request: BlogAdmin.Api.IChangePasswordRequest, callback: IRequestCallback) {
            this.post('/api/my/changepassword', request, callback);
        }
        getCategoryList(callback: IRequestCallback) {
            this.get('/api/category/all', null, callback);
        }
        addCategory(request: BlogAdmin.Api.IAddCategoryRequest, callback: IRequestCallback) {
            this.post('/api/category/add', request, callback);
        }
        getCategory(id: number, callback: IRequestCallback) {
            var url = "/api/category/" + id;
            this.get(url, null, callback);
        }
        editCategory(request: BlogAdmin.Api.IEditCategoryRequest, callback: IRequestCallback) {
            this.post('/api/category/edit', request, callback);
        }
        getArticleList(request: BlogAdmin.Api.IGetArticleListRequest, callback: IPagedRequestCallback) {
            this.get('/api/article/query', request, callback);
        }
    }
}

angular.module('blogAdmin.services').service('$api', BlogAdmin.Services.Api);