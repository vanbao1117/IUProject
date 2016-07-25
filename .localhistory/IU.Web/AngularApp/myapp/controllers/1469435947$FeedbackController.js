//'use strict';

IUApp.controller('FeedbackController', ['$scope', '$http', '$templateCache', '$timeout', '$routeParams', 'LecturerService',
    function ($scope, $http, $templateCache, $timeout, $routeParams, LecturerService) {
    
        $scope.getLecturers = function () {
            LecturerService.getLecturer().then(
              function (schedules) {
                  $scope.schedules = schedules;
                  console.log('getAllClassSchedule: ', schedules);
              },
              function (error) {
                  console.log('getSubjects error: ' + error);
              });
        };

    (function init() {
        $timeout(function () {
            $templateCache.removeAll();
            //set header text
            $scope.setPageHeader("Feedback");

            console.log('Feedback controller with timeout fired');
        }, 500);
    })();
}]);