//'use strict';

IUApp.controller('StudentInClassController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'ClassService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService, ClassService) {
       

        $scope.getStudentInClass = function () {
            ClassService.getStudentInClass().then(
              function (students) {
                  $scope.students = students;
                  console.log('getStudentInClass: ', students);
              },
              function (error) {
                  console.log('getStudentInClass error: ' + error);
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
             console.log('StudentInClassController initial with timeout fired');
         }, 500);
     })();
}]);