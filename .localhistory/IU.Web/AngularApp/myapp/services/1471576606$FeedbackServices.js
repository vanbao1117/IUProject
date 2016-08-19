//'use strict';

IUApp.factory('FeedbackServices', ['$http', '$q', function ($http, $q) {

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
        adminViewFeedback: function (feedback) {
            var def = $q.defer();
            $http.get("api/Schedule/GetClasses")
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