//'use strict';

IUApp.factory('BisServices', ['$http', '$q', function ($http, $q) {

    return {
        submitFeedback: function (feedback) {
            var def = $q.defer();
            $http.post('api/StuAttendance/FeedbackByStudent', feedback)
                .success(function (feedback) {
                    def.resolve(feedback);
                })
                .error(function () {
                    def.reject("Failed to submit feedback");
                });
            return def.promise;
        },
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
        }
    };
}]);