//'use strict';

IUApp.controller('StuAttendanceController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', '$routeParams',
    function ($scope, $http, $location, $route, $templateCache, $timeout, $routeParams) {

        $scope.semesterName = $routeParams.semesterName;


        (function init() {
            $timeout(function () {
                $templateCache.removeAll();
                //set header text
                $scope.setPageHeader("attendance");
                console.log('System controller with timeout fired');
            }, 500);
        })();
    }]);