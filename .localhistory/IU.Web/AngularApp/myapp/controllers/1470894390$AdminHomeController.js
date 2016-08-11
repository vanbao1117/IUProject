//'use strict';

IUApp.controller('AdminHomeController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'ClassService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService, ClassService) {
       

        $scope.getAllClasses = function () {
            $scope.currentPage = page;
            ClassService.getAllClasses().then(
              function (classes) {
                  $scope.classes = classes;
                  console.log('getAllClasses: ', classes);
              },
              function (error) {
                  console.log('getAllClasses error: ' + error);
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