//'use strict';

IUApp.controller('DashboardController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'ScheduleServices',
    function ($scope, $http, $location, $route, $templateCache, $timeout, ScheduleServices) {
        $scope.currentPage = 1;
        $scope.schedules = {};

        $scope.abbreSubjectName = $routeParams.abbreSubjectName;
        $scope.subjectName = "All Subjects";

        angular.forEach($scope.Subjects, function (item, key) {
            if (item.abbreSubjectName == $routeParams.abbreSubjectName) {
                $scope.subjectName = item.subjectName;
                return;
            }
        });

        $scope.getAllClassSchedule = function (page) {
            $scope.currentPage = page;
            ScheduleServices.get(page, "ALL").then(
              function (schedules) {
                  $scope.schedules = schedules;
                  console.log('getAllClassSchedule: ', schedules);
              },
              function (error) {
                  console.log('getSubjects error: ' + error);
              });
        };

        $scope.range = function (n) {
            return new Array(n);
        };

        $scope.next = function () {
            var page = $scope.currentPage == $scope.schedules.totalPages ? 1 : $scope.currentPage + 1;
            $scope.currentPage = page;
            $scope.getAllClassSchedule(page);
        };

        $scope.previous = function () {
            var page = $scope.currentPage <= 0 ? 1 : $scope.currentPage - 1;
            $scope.currentPage = page;
            $scope.getAllClassSchedule(page);
        };

     (function init() {
         $timeout(function () {
             //set header text
             $scope.setPageHeader($scope.subjectName);
             $scope.getAllClassSchedule(1);
             console.log('System controller with timeout fired');
         }, 500);
     })();
}]);