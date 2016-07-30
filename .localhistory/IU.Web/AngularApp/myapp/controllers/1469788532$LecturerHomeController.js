//'use strict';

IUApp.controller('LecturerHomeController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'ClassService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService, ClassService) {
       
    

    $scope.getClass = function () {
        ClassService.getClass().then(
            function (classs) {
                $scope.classs = classs;
                console.log('getClass: ', classs);
            },
            function (error) {
                console.log('getClass error: ' + error);
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
             $scope.getClass();
             console.log('LecturerHomeController initial with timeout fired');
         }, 500);
     })();
}]);