//'use strict';
var IUApp = angular.module('IUApp', ["ngRoute"]).config(function ($routeProvider, $locationProvider) {
    //check browser support
    //if (window.history && window.history.pushState) {
        //$locationProvider.html5Mode(true); will cause an error $location in HTML5 mode requires a  tag to be present! Unless you set baseUrl tag after head tag like so: <head> <base href="/">

        // to know more about setting base URL visit: https://docs.angularjs.org/error/$location/nobase

        // if you don't wish to set base URL then use this

        // check if browser supports history API
        //$locationProvider.html5Mode({
        //    enabled: true,
        //    requireBase: false,
        //    hashPrefix: '!'
        //});
    //}

    $locationProvider.html5Mode(false);

    $routeProvider
        .when('/schedule', {//Dashboard
            templateUrl: '/Home/Dashboard',
            controller: 'DashboardController'
        })
        .when('/schedule/:abbreSubjectName', {//Nhập liệu
            templateUrl: function (params) {
                return 'Home/Schedule'
            },
            controller: 'ScheduleController'
        })
        .when('/lecture/attendance', {//Nhập liệu
            templateUrl: function (params) {
                return 'Home/Lecturer'
            },
            controller: 'LecturerHomeController'
        })
        .when('/lecture/attendance/:abbreSubjectName', {//Nhập liệu
            templateUrl: function (params) {
                return 'Home/Lecturer'
            },
            controller: 'LecturerController'
        })
         .when('/lecture/attendance/take', {//Nhập liệu
             templateUrl: function (params) {
                 return 'Home/Lecturer'
             },
             controller: 'LecturerHomeController'
         })
        .when('/lecture/feedback', {//Nhập liệu
            templateUrl: function (params) {
                return 'Home/Feedback'
            },
            controller: 'FeedbackController'
        })
        .when('/feedback', {//Nhập liệu
            templateUrl: function (params) {
                return 'Home/Feedback'
            },
            controller: 'FeedbackController'
        })
        .when('/bis', {//Nhập liệu
            templateUrl: function (params) {
                return 'Home/Bis'
            },
            controller: 'BisController'
        })
         .when('/attendance', {//Attendance
             templateUrl: '/Home/StuAttendance',
             controller: 'NavAttendanceController'
         })
         .when('/semester/:semesterName', {//Nhập liệu
             templateUrl: function (params) {
                 return 'Home/StuAttendance'
             },
             controller: 'StuAttendanceController'
         })
        .when('/input', {//Nhập liệu
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