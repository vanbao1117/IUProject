//'use strict';

IUApp.factory('SystemCodeService', ['$http', '$q', function ($http, $q) {

    return {
        get: function () {
            var def = $q.defer();
            $http.get("/api/System/GetFormData")
                .success(function (transCodes) {
                    def.resolve(transCodes);
                })
                .error(function () {
                    def.reject("Failed to get trans codes");
                });
            return def.promise;
        },
        create: function (system) {
            var def = $q.defer();
            $http.post('/api/System/CreateSystem', system)
                .success(function (system) {
                    def.resolve(system);
                })
                .error(function () {
                    def.reject("Failed to create system");
                });
            return def.promise;
        },
        delete: function (id) {
            var def = $q.defer();
            $http.delete('/api/TransCode/DeleteSystem/' + id)
                .success(function (transCodes) {
                    def.resolve(transCodes);
                })
                .error(function () {
                    def.reject("Failed to delete system");
                });
            return def.promise;
        }
    };
}]);