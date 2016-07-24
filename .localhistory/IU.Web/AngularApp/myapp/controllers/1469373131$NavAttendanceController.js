//'use strict';

IUApp.controller('NavAttendanceController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 
    function ($scope, $http, $location, $route, $templateCache, $timeout) {
        console.log(' $scope.semesters',  $scope.semesters);
       

     (function init() {
         $timeout(function () {
             //set header text
             $scope.setPageHeader('Attendance');
             $('#page_title').text('Inteligent | Attendance');
             
             console.log('System controller with timeout fired');
         }, 500);
     })();
}]);