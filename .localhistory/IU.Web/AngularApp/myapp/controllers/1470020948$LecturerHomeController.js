﻿//'use strict';

IUApp.controller('LecturerHomeController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'SubjectService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService, SubjectService) {
       
    
    $scope.getAttendanceTwoDaysBefore = function () {
        AttendanceService.getAttendanceTwoDaysBefore().then(
            function (attendances) {
                $scope.attendances = attendances;
                console.log('getAttendanceTwoDaysBefore: ', attendances);
            },
            function (error) {
                console.log('getAttendanceTwoDaysBefore error: ' + error);
            });
    };

    $scope.getAttendanceToDaysBefore = function () {
        AttendanceService.getAttendanceToDaysBefore().then(
            function (attendances) {
                $scope.attendances = attendances;
                console.log('getAttendanceTwoDaysBefore: ', attendances);
            },
            function (error) {
                console.log('getAttendanceTwoDaysBefore error: ' + error);
            });
    };

    $scope.getSubject = function () {
        SubjectService.getLectureSubject().then(
            function (subjects) {
                $scope.subjects = subjects;
                console.log('getSubject: ', subjects);
            },
            function (error) {
                console.log('getSubject error: ' + error);
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
             $scope.setPageHeader('Attendance');
             $templateCache.removeAll();
             $scope.getSubject();
             $scope.getAttendanceTwoDaysBefore();
             $location.path("/lecture/attendance");
             console.log('LecturerHomeController initial with timeout fired');
         }, 500);
     })();
}]);