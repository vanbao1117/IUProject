//'use strict';

IUApp.controller('FeedbackController', ['$scope', '$http', '$templateCache', '$timeout', '$routeParams', 'LecturerService',
    function ($scope, $http, $templateCache, $timeout, $routeParams, LecturerService) {
    
   
    (function init() {
        $timeout(function () {
            $templateCache.removeAll();
            //set header text
            $scope.setPageHeader("Feedback");

            console.log('Feedback controller with timeout fired');
        }, 500);
    })();
}]);