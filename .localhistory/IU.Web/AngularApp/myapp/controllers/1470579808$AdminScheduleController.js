//'use strict';

IUApp.controller('AdminScheduleController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService) {
       

    $scope.setPageHeader = function (header) {
        $('.content-header').html('<h1>' + header + '</h1><ol class="breadcrumb"><li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li><li class="active">' + header + '</li></ol>');
    };

    $scope.range = function (n) {
        return new Array(n);
    };

     (function init() {
         $timeout(function () {
             $scope.setPageHeader();
             console.log('AdminScheduleController initial with timeout fired');
         }, 500);
     })();
}]);