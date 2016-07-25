﻿//'use strict';

IUApp.factory('FeedbackServices', ['$http', '$q', function ($http, $q) {

    return {
        submitFeedback: function (feedback) {
            var def = $q.defer();
            $http.post('api/StuAttendance/FeedbackByStudent', feedback)
                .success(function (transCodes) {
                    def.resolve(transCodes);
                })
                .error(function () {
                    def.reject("Failed to get trans codes");
                });
            return def.promise;
        }
    };
}]);