//'use strict';

IUApp.factory('AttendanceService', ['$http', '$q', function ($http, $q) {

    return {
        get: function () {
            var def = $q.defer();
            $http.get("api/StuAttendance/GetSemesterByStudent")
                .success(function (subjects) {
                    def.resolve(subjects);
                })
                .error(function () {
                    def.reject("Failed to get Semester");
                });
            return def.promise;
        },
        getAttendance: function () {
            var def = $q.defer();
            $http.get("api/StuAttendance/GetAttendanceByStudent?pageNumber=" + page + "&pageSize=20" + "&abbreSubjectName=" + semesterName)
                .success(function (attendance) {
                    def.resolve(attendance);
                })
                .error(function () {
                    def.reject("Failed to get attendance");
                });
            return def.promise;
        }
    };
}]);