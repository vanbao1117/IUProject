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
            $http.get("api/Subject/GetSubjectByLecturer")
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