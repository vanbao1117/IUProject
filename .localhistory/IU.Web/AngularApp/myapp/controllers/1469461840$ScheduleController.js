//'use strict';

IUApp.controller('ScheduleController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', '$routeParams', 'ScheduleServices',
    function ($scope, $http, $location, $route, $templateCache, $timeout, $routeParams, ScheduleServices) {

        angular.forEach($scope.Subjects, function (item, key) {
            if (item.abbreSubjectName == $routeParams.abbreSubjectName) {
                $scope.abbreSubjectName = item.subjectName;
                return;
            }
            index++;
        });

        console.log('$routeParams.abbreSubjectName: ', $routeParams.abbreSubjectName);
        $scope.currentPage = 1;
        $scope.schedules = {};

        $scope.getAllClassSchedule = function (page) {
            $scope.currentPage = page;
            ScheduleServices.get(page, $scope.abbreSubjectName).then(
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
                 $templateCache.removeAll();
                 //set header text
                 $scope.setPageHeader($scope.abbreSubjectName);

                 $scope.getAllClassSchedule($scope.currentPage);

                 console.log('System controller with timeout fired');
             }, 500);
         })();
}]);