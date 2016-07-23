//'use strict';

IUApp.factory('TransCodeService', ['$http', '$q', function ($http, $q) {

    return {
        get: function () {
            var def = $q.defer();
            $http.get("/api/TransCode/GetTransCode")
                .success(function (transCodes) {
                    def.resolve(transCodes);
                })
                .error(function () {
                    def.reject("Failed to get trans codes");
                });
            return def.promise;
        },
        create: function (transCode) {
            var def = $q.defer();
            $http.post('/api/TransCode/CreateTransCode', transCode)
                .success(function (transCodes) {
                    def.resolve(transCodes);
                })
                .error(function () {
                    def.reject("Failed to get trans codes");
                });
            return def.promise;
        },
        delete: function (id) {
            var def = $q.defer();
            $http.delete('/api/TransCode/DeleteTransCode/' + id)
                .success(function (transCodes) {
                    def.resolve(transCodes);
                })
                .error(function () {
                    def.reject("Failed to get trans codes");
                });
            return def.promise;
        }
    };
}]);