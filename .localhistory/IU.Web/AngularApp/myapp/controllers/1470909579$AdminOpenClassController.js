//'use strict';

IUApp.controller('AdminOpenClassController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'ClassService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService, ClassService) {
       
        $scope.getOpenClass = function () {
            var classID = "";
            ClassService.getOpenClass().then(
            function (openClasses) {
                $scope.openClasses = openClasses;
                console.log('getOpenClass: ', $scope.openClasses);
            },
            function (error) {
                console.log('getOpenClass error: ' + error);
            });
        };

    $scope.getStudentInOpenClass = function () {
            var classID = "";
            ClassService.getStudentInOpenClass(classID).then(
            function (studentOpenClasses) {
                $scope.studentOpenClasses = studentOpenClasses;
                console.log('getStudentInOpenClass: ', $scope.studentOpenClasses);
            },
            function (error) {
                console.log('getStudentInOpenClass error: ' + error);
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
             $scope.setPageHeader("Open Class");
             $scope.getStudentInOpenClass();
             $scope.getOpenClass();
             console.log('AdminOpenClassController initial with timeout fired');
         }, 500);
     })();
}]);