//'use strict';

IUApp.factory('FeedbackServices', ['$http', '$q', function ($http, $q) {

    return {
        submitFeedback: function (feedback) {
            var def = $q.defer();
            $http.get("api/Schedule/GetAllClassScheduleSync?pageNumber=" + page + "&pageSize=20" + "&abbreSubjectName=" + abbreSubjectName)
                .success(function (subjects) {
                    def.resolve(subjects);
                })
                .error(function () {
                    def.reject("Failed to get Schedule");
                });
            return def.promise;
        }
    };
}]);