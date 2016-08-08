//'use strict';

IUApp.factory('LecturerService', ['$http', '$q', function ($http, $q) {

    return {
        getLecturer: function () {
            var def = $q.defer();
            $http.get("api/Lecturer/GetLecturers")
                .success(function (lecturers) {
                    def.resolve(lecturers);
                })
                .error(function () {
                    def.reject("Failed to get Lecturer");
                });
            return def.promise;
        },
        GetLectureClassSubject: function () {
            var def = $q.defer();
            $http.get("api/Lecturer/GetLectureClassSubject")
                .success(function (ClassSubjects) {
                    def.resolve(ClassSubjects);
                })
                .error(function () {
                    def.reject("Failed to get GetLectureClassSubject");
                });
            return def.promise;
        },
        GetLecturerPreview: function () {
            var def = $q.defer();
            $http.get("api/Lecturer/GetLecturerPreview")
                .success(function (ClassSubjects) {
                    def.resolve(ClassSubjects);
                })
                .error(function () {
                    def.reject("Failed to get GetLecturerPreview");
                });
            return def.promise;
        }
    };
}]);