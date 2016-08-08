﻿//'use strict';

IUApp.factory('SubjectService', ['$http', '$q', function ($http, $q) {

    return {
        createBis: function (bis) {
            var def = $q.defer();
            $http.post('api/Schedule/CreateBis', bis)
                .success(function (bis) {
                    def.resolve(bis);
                })
                .error(function () {
                    def.reject("Failed to submit feedback");
                });
            return def.promise;
        },
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
        },
        getAllLecturer: function () {
            var def = $q.defer();
            $http.get("api/Subject/GetAllLecturer")
                .success(function (lecturers) {
                    def.resolve(lecturers);
                })
                .error(function () {
                    def.reject("Failed to get GetAllLecturer");
                });
            return def.promise;
        },
        GetAllSlots: function () {
            var def = $q.defer();
            $http.get("api/Subject/GetAllSlots")
                .success(function (slots) {
                    def.resolve(slots);
                })
                .error(function () {
                    def.reject("Failed to get GetAllSlots");
                });
            return def.promise;
        },
        GetAllRooms: function () {
            var def = $q.defer();
            $http.get("api/Subject/GetAllRooms")
                .success(function (rooms) {
                    def.resolve(rooms);
                })
                .error(function () {
                    def.reject("Failed to get GetAllRooms");
                });
            return def.promise;
        }
        ,
        GetAllModes: function () {
            var def = $q.defer();
            $http.get("api/Subject/GetAllModes")
                .success(function (modes) {
                    def.resolve(modes);
                })
                .error(function () {
                    def.reject("Failed to get GetAllModes");
                });
            return def.promise;
        }
    };
}]);