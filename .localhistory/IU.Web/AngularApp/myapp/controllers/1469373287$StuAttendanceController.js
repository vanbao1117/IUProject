//'use strict';

IUApp.controller('StuAttendanceController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', '$routeParams',
    function ($scope, $http, $location, $route, $templateCache, $timeout, $routeParams) {

        $scope.semesterName = $routeParams.semesterName;



        console.log(' $scope.semesters', $scope.semesters);

        (function init() {
            $timeout(function () {
                $templateCache.removeAll();
                //set header text
                $scope.setPageHeader($scope.semesterName);
                $('#page_title').text('Inteligent | Semester');
                console.log('System controller with timeout fired');
            }, 500);
        })();
    }]);