//'use strict';

IUApp.controller('FeedbackController', ['$scope', '$http', '$templateCache', '$timeout', '$routeParams', 'LecturerService',
    function ($scope, $http, $templateCache, $timeout, $routeParams, LecturerService) {
        $scope.lecturers = [];
        $scope.qualities = ["Best", "Good", "Medium", "Fail"];

        $scope.lecturer = {};
        $scope.qualitie = {};

        $scope.getLecturers = function () {
            LecturerService.getLecturer().then(
              function (lecturers) {
                  $scope.lecturers = lecturers;
                  console.log('getLecturer: ', lecturers);
              },
              function (error) {
                  console.log('getSubjects error: ' + error);
              });
        };

        $scope.sendFeedback = function () {
            console.log('$scope.lecturer: ', $scope.lecturer);
        };

    (function init() {
        $timeout(function () {
            $templateCache.removeAll();
            //set header text
            $scope.setPageHeader("Feedback");
            $scope.getLecturers();
            console.log('Feedback controller with timeout fired');
        }, 500);
    })();
}]);