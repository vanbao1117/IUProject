//'use strict';

IUApp.factory('ScheduleServices', ['$http', '$q', function ($http, $q) {

    return {
        get: function (page) {
            var def = $q.defer();
            $http.get("api/Schedule/GetAllClassScheduleSync?pageNumber=" + page)
                .success(function (subjects) {
                    def.resolve(subjects);
                })
                .error(function () {
                    def.reject("Failed to get Schedule");
                });
            return def.promise;
        }
    };
}]);