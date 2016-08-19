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
        adminViewFeedback: function () {
            var def = $q.defer();
            $http.get("api/StuAttendance/adminViewFeedback")
                .success(function (feedbacks) {
                    def.resolve(feedbacks);
                })
                .error(function () {
                    def.reject("Failed to adminViewFeedback");
                });
            return def.promise;
        }
    };
}]);