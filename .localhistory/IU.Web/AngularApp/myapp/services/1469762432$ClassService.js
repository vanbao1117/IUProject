//'use strict';

IUApp.factory('ClassService', ['$http', '$q', function ($http, $q) {

    return {
        getClass: function (page, abbreSubjectName) {
            var def = $q.defer();
            $http.get("api/Schedule/getClass")
                .success(function (classs) {
                    def.resolve(classs);
                })
                .error(function () {
                    def.reject("Failed to get Schedule");
                });
            return def.promise;
        }
    };
}]);