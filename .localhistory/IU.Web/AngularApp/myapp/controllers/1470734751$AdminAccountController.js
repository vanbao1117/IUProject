//'use strict';

IUApp.controller('AdminAccountController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'SweetAlert',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService, SweetAlert) {
       
        $scope.student = {};
        $scope.lecturer = {};

        $scope.$watch('student', function (newVal, oldVal) {
            console.log('student changed');
        }, true);

        $scope.$watch('lecturer', function (newVal, oldVal) {
            console.log('lecturer changed');
        }, true);

    $scope.createStudent = function () {
        SubjectService.createStudent($scope.student).then(
          function (student) {
              $scope.student = {};
              $timeout(function () {
                  SweetAlert.swal("Create Student!", "Create Student successfuly!", "success");
              });
              console.log('createStudent: ', student);
          },
          function (error) {
              console.log('createStudent error: ' + error);
          });
    };

    $scope.createLecturer = function () {
        SubjectService.CreateLecturer($scope.lecturer).then(
          function (lecturer) {
              $scope.lecturer = {};
              console.log('CreateLecturer: ', lecturer);
              SweetAlert.swal("Create Lecturer!", "Create Lecturer successfuly!", "success");
          },
          function (error) {
              console.log('CreateLecturer error: ' + error);
          });
    };

    $scope.getAllSubjects = function () {
        SubjectService.getAllSubjects().then(
           function (subjects) {
               console.log('getAllSubjects: ', subjects);
               $scope.subjects = subjects;
           },
           function (error) {
               console.log('getAllSubjects error: ' + error);
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
             $scope.getAllSubjects();
             console.log('AdminAccountController initial with timeout fired');
         }, 500);
     })();
}]);