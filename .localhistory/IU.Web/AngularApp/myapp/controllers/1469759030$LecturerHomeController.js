//'use strict';

IUApp.controller('LecturerHomeController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'ClassService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService, ClassService) {
       
    

    $scope.getClass = function () {
        ClassService.get().then(
            function (subjects) {
                $scope.Subjects = subjects;
                console.log('getSubjects: ', subjects);
            },
            function (error) {
                console.log('getSubjects error: ' + error);
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
            
             console.log('LecturerHomeController initial with timeout fired');
         }, 500);
     })();
}]);