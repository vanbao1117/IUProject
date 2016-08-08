//'use strict';

IUApp.controller('AdminScheduleController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService) {
       
    $scope.activeTab = function (tab) {
        $('#' + tab).show();
        $('#' + tab).parent.addClass('active');
        
        $('#admin-tab-schedule').find('li').each(function () {
            // cache jquery object
            var current = $(this);
            current.removeClass('active');
        });
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

    $scope.setPageHeader = function (header) {
        $('.content-header').html('<h1>' + header + '</h1><ol class="breadcrumb"><li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li><li class="active">' + header + '</li></ol>');
    };

    $scope.range = function (n) {
        return new Array(n);
    };

     (function init() {
         $timeout(function () {
             $scope.setPageHeader('Schedule');
             console.log('AdminScheduleController initial with timeout fired');
         }, 500);
     })();
}]);