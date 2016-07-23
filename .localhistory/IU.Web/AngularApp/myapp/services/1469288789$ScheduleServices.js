//'use strict';

IUApp.factory('ScheduleServices', ['$http', '$q', function ($http, $q) {

    return {
        get: function (page, className) {
            var def = $q.defer();
            $http.get("api/Schedule/GetAllClassScheduleSync?pageNumber=" + page + "&pageSize=20" + "&className=" + className)
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