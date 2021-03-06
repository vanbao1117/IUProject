﻿//'use strict';

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
        //subjectID
        //semesterID
        //classID
        AttendanceService.getAttendances(item).then(
            function (attendances) {
                $scope.listAttendances = attendances;
                console.log('getAttendances: ', $scope.listAttendances);
            },
            function (error) {
                console.log('getAttendances error: ' + error);
            });
    };

    $scope.saveAttendance = function (item) {
        /*
        ClassID
        SemesterID
        StudentListID
        SubjectID
        SlotID
        Attendancer
        DateAttendance
        Attendance
        RomID
        Note
        */
    };

    $scope.range = function (n) {
        return new Array(n);
    };

     (function init() {
         $timeout(function () {
             $scope.setPageHeader('Edit Attendance');
             if ($scope.attendance.key == $scope.key) {
                 console.log('$scope.attendance:', $scope.attendance);
                 if ($scope.attendance.isAttendanced == true) {
                     $scope.getAttendance($scope.attendance.item);
                 } else {
                     $scope.getClassSchedule($scope.attendance.item);
                 }
             }
             console.log('EditAttendanceController initial with timeout fired');
         }, 500);
     })();
}]);