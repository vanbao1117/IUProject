﻿//'use strict';

IUApp.factory('AttendanceService', ['$http', '$q', function ($http, $q) {

    return {
        get: function () {
            var def = $q.defer();
            $http.get("api/StuAttendance/GetSemesterByStudent")
                .success(function (subjects) {
                    def.resolve(subjects);
                })
                .error(function () {
                    def.reject("Failed to get subject");
                });
            return def.promise;
        }
    };
}]);