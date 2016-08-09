//'use strict';

IUApp.controller('AdminAccountController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService) {
       
    $scope.student = {};

    $scope.createStudent = function () {
        SubjectService.createStudent($scope.student).then(
          function (classes) {
              console.log('createStudent: ', classes);
              $scope.classes = classes;
          },
          function (error) {
              console.log('createStudent error: ' + error);
          });
    };

    $scope.currentTab = 'activity';
    $scope.activeTab = function (tab, className) {
        $('#' + tab).show();

        $scope.currentTab = tab;

        $('.admin-account').find('li').each(function () {
            // cache jquery object
            var current = $(this);
            current.removeClass('active');
        });

        $('.' + className).addClass('active');

        if (tab == 'activity') {
            $('#timeline').hide();
        } else if (tab == 'timeline') {
            $('#activity').hide();
        }
    };

    $scope.setPageHeader = function (header) {
        $('.content-header').html('<h1>' + header + '</h1><ol class="breadcrumb"><li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li><li class="active">' + header + '</li></ol>');
    };

    $scope.getAllClass = function () {
        SubjectService.getAllClass().then(
           function (classes) {
               console.log('getAllClass: ', classes);
               $scope.classes = classes;
           },
           function (error) {
               console.log('getAllClass error: ' + error);
           });
    };

    $scope.range = function (n) {
        return new Array(n);
    };

     (function init() {
         $timeout(function () {
             $scope.getAllClass();
             console.log('AdminAccountController initial with timeout fired');
         }, 500);
     })();
}]);