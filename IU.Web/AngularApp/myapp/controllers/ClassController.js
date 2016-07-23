//'use strict';

IUApp.controller('ClassController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', '$routeParams', 'ScheduleServices',
    function ($scope, $http, $location, $route, $templateCache, $timeout, $routeParams, ScheduleServices) {

        $scope.className = $routeParams.className;

        


         (function init() {
             $timeout(function () {
                 $templateCache.removeAll();
                 //set header text
                 $scope.setPageHeader($scope.className);
                 console.log('System controller with timeout fired');
             }, 500);
         })();
}]);