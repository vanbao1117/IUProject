//'use strict';

IUApp.controller('HomeController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService) {
    $scope.Subjects = [];

    $scope.activeScheduleItems = [];
    
    $scope.gotoMenu = function (url, header) {
        if (header) {
            $scope.setPageHeader(header);
        }
        
        $templateCache.removeAll();
        $location.path(url);
    };

    $scope.activeMenu = function (item, group) {
        
        if (item !== undefined && group == 'schedule') {

            angular.forEach($scope.activeScheduleItems, function (_item, key) {
                _item.clicked = false;
            });

            
            if ($scope.activeScheduleItems.indexOf(item) === -1) {
                // a is NOT in array
                $scope.activeScheduleItems.push(item);
            }

            item.clicked = true;
        }
    };

    $scope.getSubjects = function () {
        SubjectService.get().then(
          function (subjects) {
              $scope.Subjects = subjects;
              console.log('getSubjects: ', subjects);
          },
          function (error) {
              console.log('getSubjects error: ' + error);
          });
    };

    $scope.getSemesterByStudent = function () {
        AttendanceService.get().then(
          function (semesters) {
              $scope.semesters = semesters;
              console.log('getSemesterByStudent: ', semesters);
          },
          function (error) {
              console.log('getSemesterByStudent error: ' + error);
          });
    };

    $scope.setPageHeader = function (header) {
        $('.content-header').html('<h1>' + header + '</h1><ol class="breadcrumb"><li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li><li class="active">' + header + '</li></ol>');
    };

     (function init() {
         $timeout(function () {
             $scope.activeMenu();
             $scope.getSubjects();
             $scope.getSemesterByStudent();
             console.log('Home controller initial with timeout fired');
         }, 500);
     })();
}]);