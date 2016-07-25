//'use strict';

IUApp.controller('FeedbackController', ['$scope', '$http', '$templateCache', '$timeout', '$routeParams', 'LecturerService',
    function ($scope, $http, $templateCache, $timeout, $routeParams, LecturerService) {
        $scope.lecturers = [];
        $scope.qualities = [{ quality: "Best" }, { quality: "Good" }, { quality: "Medium" }, { quality: "Fail" }];
        $scope.attitudes = [{ attitude: "Best" }, { attitude: "Good" }, { attitude: "Medium" }, { attitude: "Fail" }];
        $scope.satisfactions = [{ satisfaction: "Best" }, { satisfaction: "Good" }, { satisfaction: "Medium" }, { satisfaction: "Fail" }];
        $scope.onTime = [{ onTime: "Best" }, { onTime: "Good" }, { onTime: "Medium" }, { onTime: "Fail" }];
        

        $scope.lecturer = {};
        $scope.quality = {};
        $scope.attitudes = {};
        $scope.satisfaction = {};

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
            console.log('$scope.quality: ', $scope.quality);
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