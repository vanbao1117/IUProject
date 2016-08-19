//'use strict';

IUApp.controller('AdminFeedbackController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'ClassService', 'FeedbackServices',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService, ClassService, FeedbackServices) {
       
        $scope.feedbacks = {};
       
        $scope.$watch('feedbacks', function (newVal, oldVal) {
            console.log('feedbacks', newVal);
        });


        $scope.adminViewFeedback = function () {
            FeedbackServices.adminViewFeedback().then(
              function (feedbacks) {
                  $scope.feedbacks = feedbacks;
                  console.log('adminViewFeedback: ', feedbacks);
              },
              function (error) {
                  console.log('adminViewFeedback error: ' + error);
              });
        };

        

    $scope.setPageHeader = function (header) {
        $('.content-header').html('<h1>' + header + '</h1><ol class="breadcrumb"><li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li><li class="active">' + header + '</li></ol>');
    };

    $scope.range = function (n) {
        return new Array(n);
    };

     (function init() {
         $timeout(function () {
             $scope.adminViewFeedback();
             console.log('AdminFeedbackController initial with timeout fired');
         }, 500);
     })();
}]);