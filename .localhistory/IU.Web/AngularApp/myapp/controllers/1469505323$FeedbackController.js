//'use strict';

IUApp.controller('FeedbackController', ['$scope', '$http', '$templateCache', '$timeout', '$routeParams', 'LecturerService', 'FeedbackServices', 'SweetAlert',
    function ($scope, $http, $templateCache, $timeout, $routeParams, LecturerService, FeedbackServices, SweetAlert) {
        $scope.lecturers = [];
        $scope.qualities = [{ quality: "Best" }, { quality: "Good" }, { quality: "Medium" }, { quality: "Fail" }];
        $scope.attitudes = [{ attitude: "Best" }, { attitude: "Good" }, { attitude: "Medium" }, { attitude: "Fail" }];
        $scope.satisfactions = [{ satisfaction: "Best" }, { satisfaction: "Good" }, { satisfaction: "Medium" }, { satisfaction: "Fail" }];
        $scope.onTimes = [{ onTime: "Best" }, { onTime: "Good" }, { onTime: "Medium" }, { onTime: "Fail" }];
        
        

        $scope.lecturer = {};
        $scope.quality = {};
        $scope.attitude = {};
        $scope.satisfaction = {};
        $scope.onTime = {};
        $scope.comments = '';
        
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
            var feedback = {
                lecturerID: $scope.lecturer.lecturerID, quality: $scope.quality.quality,
                attitude: $scope.attitude.attitude, satisfaction: $scope.satisfaction.satisfaction, onTime: $scope.onTime.onTime, comments: $scope.comments
            };
            console.log('feedback: ', feedback);
            FeedbackServices.submitFeedback(feedback).then(
              function (feedback) {
                  $timeout(function () {
                      $scope.lecturer = {};
                      $scope.quality = {};
                      $scope.attitude = {};
                      $scope.satisfaction = {};
                      $scope.onTime = {};
                      $scope.comments = '';
                      SweetAlert.swal("Feedback!", "Thank's for your feedback!", "success");
                  });
                  console.log('submitFeedback: ', feedback);
              },
              function (error) {
                  console.log('submitFeedback error: ' + error);
              });
            //console.log('$scope.lecturer: ', $scope.lecturer);
            //console.log('$scope.quality: ', $scope.quality);
            //console.log('$scope.attitude: ', $scope.attitude);
            //console.log('$scope.satisfaction: ', $scope.satisfaction);
            //console.log('$scope.onTime: ', $scope.onTime);
            //console.log('$scope.comments: ', $scope.comments);
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