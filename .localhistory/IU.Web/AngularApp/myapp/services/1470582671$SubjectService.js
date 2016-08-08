//'use strict';

IUApp.factory('SubjectService', ['$http', '$q', function ($http, $q) {

    return {
        get: function () {
            var def = $q.defer();
            $http.get("api/Subject/GetSubjectByStudent")
                .success(function (subjects) {
                    def.resolve(subjects);
                })
                .error(function () {
                    def.reject("Failed to get subject");
                });
            return def.promise;
        },
        getLectureSubject: function () {
            var def = $q.defer();
            $http.get("api/Subject/GetSubjectByLecturer")
                .success(function (subjects) {
                    def.resolve(subjects);
                })
                .error(function () {
                    def.reject("Failed to get subject");
                });
            return def.promise;
        },
        getAllSubjects: function () {
            var def = $q.defer();
            $http.get("api/Subject/GetSubjects")
                .success(function (subjects) {
                    def.resolve(subjects);
                })
                .error(function () {
                    def.reject("Failed to get GetSubjects");
                });
            return def.promise;
        },
        getAllSemester: function () {
            var def = $q.defer();
            $http.get("api/Subject/GetAllSemester")
                .success(function (semesters) {
                    def.resolve(semesters);
                })
                .error(function () {
                    def.reject("Failed to get GetAllSemester");
                });
            return def.promise;
        },
        getAllClass: function () {
            var def = $q.defer();
            $http.get("api/Subject/GetAllClass")
                .success(function (classes) {
                    def.resolve(classes);
                })
                .error(function () {
                    def.reject("Failed to get GetAllClass");
                });
            return def.promise;
        }
    };
}]);