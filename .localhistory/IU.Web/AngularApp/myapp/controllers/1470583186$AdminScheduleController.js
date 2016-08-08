//'use strict';

IUApp.controller('AdminScheduleController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService) {
       
        $scope.subjectSelected = {};
        $scope.semesterSelected = {};

        $scope.currentTab = 'activity';
        $scope.activeTab = function (tab, className) {
            $('#' + tab).show();
        
            $scope.currentTab = tab;

            $('.admin-tab-schedule').find('li').each(function () {
                // cache jquery object
                var current = $(this);
                current.removeClass('active');
            });
        
            $('.' + className).addClass('active');

            if (tab == 'activity') {
                $('#timeline').hide();
                $('#settings').hide();
            } else if (tab == 'timeline') {
                $('#activity').hide();
                $('#settings').hide();
            } else if (tab == 'settings') {
                $('#timeline').hide();
                $('#activity').hide();
            }
        };

    $scope.saveChange = function () {
        //Bis
        if ($scope.currentTab == 'activity') {

        } else if ($scope.currentTab == 'timeline') {//lecture 

        } else if ($scope.currentTab == 'settings') {//Class 

        }
    }

    $scope.setPageHeader = function (header) {
        $('.content-header').html('<h1>' + header + '</h1><ol class="breadcrumb"><li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li><li class="active">' + header + '</li></ol>');
    };

    $scope.range = function (n) {
        return new Array(n);
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

    $scope.getAllSemester = function () {
        SubjectService.getAllSemester().then(
           function (semesters) {
               console.log('getAllSemester: ', semesters);
               $scope.semesters = semesters;
           },
           function (error) {
               console.log('getAllSemester error: ' + error);
           });
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

    $scope.getAllLecturer = function () {
        SubjectService.getAllLecturer().then(
           function (lecturers) {
               console.log('getAllLecturer: ', lecturers);
               $scope.lecturers = lecturers;
           },
           function (error) {
               console.log('getAllLecturer error: ' + error);
           });
    };

     (function init() {
         $timeout(function () {
             $scope.setPageHeader('Schedule');

             $scope.getAllSubjects();
             $scope.getAllSemester();

             console.log('AdminScheduleController initial with timeout fired');
         }, 500);
     })();
}]);