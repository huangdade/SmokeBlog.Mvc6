var BlogAdmin;
(function (BlogAdmin) {
    var Services;
    (function (Services) {
        var Method;
        (function (Method) {
            Method[Method["Get"] = 0] = "Get";
            Method[Method["Post"] = 1] = "Post";
        })(Method || (Method = {}));
        var Api = (function () {
            function Api($http) {
                this.$http = $http;
            }
            Api.prototype.getDefaultConfig = function (url, method) {
                var config = {
                    url: url,
                    method: method,
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                    transformRequest: function (obj) {
                        var str = [];
                        for (var p in obj)
                            str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                        return str.join("&");
                    }
                };
                return config;
            };
            Api.prototype.request = function (url, method, opt, callback) {
                var config = {
                    url: url,
                    method: Method[method],
                    headers: { 'Content-Type': 'application/x-www-form-urlencoded' },
                    transformRequest: function (obj) {
                        var str = [];
                        for (var p in obj)
                            str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                        return str.join("&");
                    }
                };
                angular.extend(config, opt);
                this.$http(config).success(callback).error(function (data, code) {
                    if (angular.isObject(data) && data.error) {
                    }
                    else {
                        data = {
                            success: false,
                            message: "未知错误"
                        };
                    }
                    callback && callback(data);
                });
            };
            Api.prototype.post = function (url, data, callback) {
                var opt = {
                    data: data
                };
                this.request(url, 1 /* Post */, opt, callback);
            };
            Api.prototype.get = function (url, params, callback) {
                var opt = {
                    params: params
                };
                this.request(url, 0 /* Get */, opt, callback);
            };
            Api.prototype.getUserList = function (callback) {
                this.get('/api/user/all', null, callback);
            };
            Api.prototype.addUser = function (userName, password, nickname, email, callback) {
                var data = {
                    userName: userName,
                    password: password,
                    nickname: nickname,
                    email: email
                };
                this.post('/api/user/add', data, callback);
            };
            return Api;
        })();
        Services.Api = Api;
    })(Services = BlogAdmin.Services || (BlogAdmin.Services = {}));
})(BlogAdmin || (BlogAdmin = {}));
angular.module('blogAdmin.services').service('$api', BlogAdmin.Services.Api);
