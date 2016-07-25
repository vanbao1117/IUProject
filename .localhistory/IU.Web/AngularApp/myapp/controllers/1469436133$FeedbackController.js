//'use strict';

IUApp.controller('FeedbackController', ['$scope', '$http', '$templateCache', '$timeout', '$routeParams', 'LecturerService',
    function ($scope, $http, $templateCache, $timeout, $routeParams, LecturerService) {
        $scope.lecturers = [];
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

    (function init() {
        $timeout(function () {
            $templateCache.removeAll();
            //set header text
            $scope.setPageHeader("Feedback");

            console.log('Feedback controller with timeout fired');
        }, 500);
    })();
}]);