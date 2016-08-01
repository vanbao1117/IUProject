//'use strict';

IUApp.controller('LecturerHomeController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', '$routeParams', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'SubjectService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, $routeParams, SubjectService, ScheduleServices, AttendanceService, SubjectService) {
       
    $scope.abbreSubjectName = $routeParams.abbreSubjectName;

    $scope.editAttendance = function (_item) {
        console.log('editAttendance item: ', _item);
        var _key = Math.floor((Math.random() * 100) + 1);
        $scope.attendance = { key: _key, item: _item };
        $location.path("/attendance/edit/" + _key);
        
    };

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

    $scope.getAttendanceToDay = function () {
        AttendanceService.getAttendanceToDay().then(
            function (attendancesToday) {
                $scope.attendancesToday = attendancesToday;
                console.log('getAttendanceToDay: ', attendancesToday);
            },
            function (error) {
                console.log('getAttendanceToDay error: ' + error);
            });
    };

    $scope.getAttendancesNext = function () {
        AttendanceService.getAttendancesNext().then(
            function (attendancesNext) {
                $scope.attendancesNext = attendancesNext;
                console.log('getAttendancesNext: ', attendancesNext);
            },
            function (error) {
                console.log('getAttendancesNext error: ' + error);
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
             console.log('$scope.abbreSubjectName: ', $scope.abbreSubjectName);
             var url = $location.url();
             console.log('url: ', url);
             if ($scope.abbreSubjectName === undefined) {
                 $scope.getSubject();
                 $scope.getAttendanceTwoDaysBefore();
                 $scope.getAttendanceToDay();
                 $scope.getAttendancesNext();
                 $location.path("/lecture/attendance");
             }
             
             console.log('LecturerHomeController initial with timeout fired');
         }, 500);
     })();
}]);