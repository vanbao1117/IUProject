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
                    def.reject("Failed to get subject");
                });
            return def.promise;
        }
    };
}]);