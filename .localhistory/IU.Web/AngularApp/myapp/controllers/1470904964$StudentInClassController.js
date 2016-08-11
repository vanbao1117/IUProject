//'use strict';

IUApp.controller('StudentInClassController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', '$routeParams', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'ClassService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, $routeParams, SubjectService, ScheduleServices, AttendanceService, ClassService) {
       
        $scope.getStudentInClass = function (classID) {
            ClassService.getStudentInClass(classID).then(
              function (students) {
                  $scope.students = students;
                  console.log('getStudentInClass: ', students);
              },
              function (error) {
                  console.log('getStudentInClass error: ' + error);
              });
        };

        $scope.studentChangeClass = function (classID) {
            ClassService.studentChangeClass(classID).then(
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
             var classID = $routeParams.classID;
             
             angular.forEach($scope.classes, function (value, key) {
                 if (value.classID == classID) {
                     $scope.setPageHeader("Class " + value.className);
                     return;
                 }
                
             });
             $scope.getStudentInClass(classID);
             console.log('StudentInClassController initial with timeout fired');
         }, 500);
     })();
}]);