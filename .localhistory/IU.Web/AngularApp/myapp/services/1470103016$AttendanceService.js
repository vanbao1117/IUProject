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
        getAttendanceTwoDaysBefore: function () {
            var def = $q.defer();
            $http.get("api/Lecturer/GetAttendanceTwoDaysBefore")
                .success(function (attendance) {
                    def.resolve(attendance);
                })
                .error(function () {
                    def.reject("Failed to get getAttendanceTwoDaysBefore");
                });
            return def.promise;
        },
        getAttendanceToDay: function () {
            var def = $q.defer();
            $http.get("api/Lecturer/GetAttendanceToDay")
                .success(function (attendance) {
                    def.resolve(attendance);
                })
                .error(function () {
                    def.reject("Failed to get getAttendanceToDay");
                });
            return def.promise;
        },
        getAttendancesNext: function () {
            var def = $q.defer();
            $http.get("api/Lecturer/GetAttendancesNext")
                .success(function (attendance) {
                    def.resolve(attendance);
                })
                .error(function () {
                    def.reject("Failed to get getAttendancesNext");
                });
            return def.promise;
        }
    };
}]);