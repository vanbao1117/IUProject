﻿//'use strict';

IUApp.controller('AdminAccountController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService) {
       

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


    $scope.range = function (n) {
        return new Array(n);
    };

     (function init() {
         $timeout(function () {
             console.log('AdminAccountController initial with timeout fired');
         }, 500);
     })();
}]);