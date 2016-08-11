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
                    def.reject("Failed to get Class");
                });
            return def.promise;
        },
        getAllClasses: function () {
            var def = $q.defer();
            $http.get("api/Schedule/GetClasses")
                .success(function (classs) {
                    def.resolve(classs);
                })
                .error(function () {
                    def.reject("Failed to get Class");
                });
            return def.promise;
        },
        getStudentInClass: function (classID) {
            var def = $q.defer();
            $http.get("api/Schedule/GetStudentInClass?classID=" + classID)
                .success(function (students) {
                    def.resolve(students);
                })
                .error(function () {
                    def.reject("Failed to get GetStudentInClass");
                });
            return def.promise;
        },
        studentChangeClass: function (student) {
            var def = $q.defer();
            $http.post('api/Schedule/StudentChangeClass', student)
                .success(function (student) {
                    def.resolve(student);
                })
                .error(function () {
                    def.reject("Failed to submit StudentChangeClass");
                });
            return def.promise;
        },
        getAllClasses: function () {
            var def = $q.defer();
            $http.get("api/Schedule/GetStudentInOpenClass")
                .success(function (classs) {
                    def.resolve(classs);
                })
                .error(function () {
                    def.reject("Failed to get Class");
                });
            return def.promise;
        }
    };
}]);