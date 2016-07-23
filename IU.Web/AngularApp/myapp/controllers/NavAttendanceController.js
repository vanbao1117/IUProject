//'use strict';

IUApp.controller('NavAttendanceController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 
    function ($scope, $http, $location, $route, $templateCache, $timeout) {



     (function init() {
         $timeout(function () {
             //set header text
             $scope.setPageHeader('Attendance');

             $scope.getTransOption();
             console.log('System controller with timeout fired');
         }, 500);
     })();
}]);