//'use strict';

IUApp.controller('StuAttendanceController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', '$routeParams',
    function ($scope, $http, $location, $route, $templateCache, $timeout, $routeParams) {
        $scope.semester = {};
        $scope.semesterName = $routeParams.semesterName;



        console.log(' $scope.semesters', $scope.semesters);

        (function init() {
            $timeout(function () {
                $templateCache.removeAll();
                //set header text
                angular.forEach($scope.semesters, function (item, key) {
                    if (item.semesterCode == $routeParams.semesterName) {
                        $scope.semester = item;
                        $scope.setPageHeader(item.semesterName);
                        return;
                    }
                });
                
                $('#page_title').text('Inteligent | Semester');
                console.log('System controller with timeout fired');
            }, 500);
        })();
    }]);