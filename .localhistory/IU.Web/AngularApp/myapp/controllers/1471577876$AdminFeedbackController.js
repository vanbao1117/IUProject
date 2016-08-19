﻿//'use strict';

IUApp.controller('AdminFeedbackController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'ClassService', 'FeedbackServices',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService, ClassService, FeedbackServices) {
        $scope.classxx = {};
        $scope.schedules = {};
        $scope.classxx.viewScheduleSelected = {};

        $scope.$watch('schedules', function (newVal, oldVal) {
            console.log('schedules', newVal);
        });

        $scope.$watch('classxx.viewScheduleSelected', function (newVal, oldVal) {
            console.log('newVal', newVal);
        });

        $scope.getAllClasses = function () {
            ClassService.getAllClasses().then(
              function (classes) {
                  $scope.classes = classes;
                  $scope.classes2 = classes;
                  console.log('getAllClasses: ', classes);
              },
              function (error) {
                  console.log('getAllClasses error: ' + error);
              });
        };

        $scope.getAllClasses();

        $scope.getClassScheduleAdmin = function (page) {
            if (page === undefined) page = 1;
            console.log('getClassScheduleAdmin .page: ', page);
            console.log('getClassScheduleAdmin .classID: ', $scope.classxx.viewScheduleSelected.classID);
            SubjectService.getClassScheduleAdmin(page, $scope.classxx.viewScheduleSelected.classID).then(
               function (schedules) {
                   console.log('getClassScheduleAdmin schedules: ', schedules);
                   $scope.schedules = schedules;
               },
               function (error) {
                   console.log('getClassScheduleAdmin error: ' + error);
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
             $location.path('/admin');

             console.log('AdminHomeController initial with timeout fired');
         }, 500);
     })();
}]);