//'use strict';
var IUApp = angular.module('IUApp', ["ngRoute"]).config(function ($routeProvider, $locationProvider) {
    //check browser support
    if (window.history && window.history.pushState) {
        $locationProvider.html5Mode(true);
        $locationProvider.html5Mode({
            enabled: true,
            requireBase: false,
            hashPrefix: '!'
        });
    }

    $locationProvider.html5Mode(false);

    $routeProvider
        .when('/schedule', {//Dashboard
            templateUrl: '/Home/Dashboard',
            controller: 'DashboardController'
        })
        .when('/schedule/:abbreSubjectName', {
            templateUrl: function (params) {
                return 'Home/Schedule'
            },
            controller: 'ScheduleController'
        })
        .when('/lecture/attendance', {
            templateUrl: function (params) {
                return 'Home/Lecturer'
            },
            controller: 'LecturerHomeController'
        })
        .when('/lecture/attendance/:abbreSubjectName', {
            templateUrl: function (params) {
                return 'Home/Lecturer'
            },
            controller: 'LecturerHomeController'
        })
         .when('/attendance/edit/:key', {
             templateUrl: function (params) {
                 return 'Home/TakeAttendance'
             },
             controller: 'EditAttendanceController'
         })
        .when('/lecture/feedback', {
            templateUrl: function (params) {
                return 'Home/Feedback'
            },
            controller: 'FeedbackController'
        })
        .when('/feedback', {
            templateUrl: function (params) {
                return 'Home/Feedback'
            },
            controller: 'FeedbackController'
        })
        .when('/bis', {
            templateUrl: function (params) {
                return 'Home/Bis'
            },
            controller: 'BisController'
        })
         .when('/attendance', {//Attendance
             templateUrl: '/Home/StuAttendance',
             controller: 'NavAttendanceController'
         })
         .when('/semester/:semesterName', {
             templateUrl: function (params) {
                 return 'Home/StuAttendance'
             },
             controller: 'StuAttendanceController'
         })
        .when('/input', {
            templateUrl: '/Home/Input',
            controller: 'InputController'
        })
        .when('/category', {//Danh mục
            templateUrl: '/Home/Category',
            controller: 'CategoryController'
        })
        .when('/exploit', {//Khai thác
            templateUrl: '/Home/Exploit',
            controller: 'ExploitController'
        }).when('/search', {//Tra cứu
            templateUrl: '/Home/Search',
            controller: 'SearchController'
        });
        //.otherwise({ redirectTo: '/' });
});


IUApp.run(function ($rootScope, $templateCache) {
    $rootScope.$on('$viewContentLoaded', function () {
        $templateCache.removeAll();
    });
});