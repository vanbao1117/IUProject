//'use strict';

IUApp.controller('EditAttendanceController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', '$routeParams', '$window', 'SubjectService', 'ScheduleServices',
    'AttendanceService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, $routeParams, $window, SubjectService, ScheduleServices, AttendanceService) {
       
        $scope.key = $routeParams.key;
        console.log('$scope.key: ', $scope.key);

        $scope.attendance = JSON.parse($window.localStorage.getItem('attendance'));



    $scope.setPageHeader = function (header) {
        $('.content-header').html('<h1>' + header + '</h1><ol class="breadcrumb"><li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li><li class="active">' + header + '</li></ol>');
    };

    $scope.getClassSchedule = function(item){

    };

    $scope.getAttendance = function (item) {

    };

    $scope.range = function (n) {
        return new Array(n);
    };

     (function init() {
         $timeout(function () {
             $scope.setPageHeader('Edit Attendance');
             if ($scope.attendance.key == $scope.key) {
                 console.log('$scope.attendance:', $scope.attendance);
             }
             console.log('EditAttendanceController initial with timeout fired');
         }, 500);
     })();
}]);