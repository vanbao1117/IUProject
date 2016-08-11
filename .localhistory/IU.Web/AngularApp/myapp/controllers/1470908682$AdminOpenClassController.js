//'use strict';

IUApp.controller('AdminOpenClassController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'ClassService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService, ClassService) {
       

        $scope.getStudentInOpenClass = function () {
            ClassService.getStudentInOpenClass().then(
              function (studentOpenClasses) {
                  $scope.studentOpenClasses = studentOpenClasses;
                  console.log('getStudentInOpenClass: ', studentOpenClasses);
              },
              function (error) {
                  console.log('getStudentInOpenClass error: ' + studentOpenClasses);
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

             $scope.getAllClasses();

             console.log('AdminHomeController initial with timeout fired');
         }, 500);
     })();
}]);