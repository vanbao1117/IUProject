//'use strict';

IUApp.controller('BisController', ['$scope', '$http', '$templateCache', '$timeout', '$routeParams', 'LecturerService', 'FeedbackServices', 'SweetAlert',
    function ($scope, $http, $templateCache, $timeout, $routeParams, LecturerService, FeedbackServices, SweetAlert) {
       

    (function init() {
        $timeout(function () {
            $templateCache.removeAll();
            //set header text
            $scope.setPageHeader("Register BIS-Class");
            $scope.getLecturers();
            console.log('Feedback controller with timeout fired');
        }, 500);
    })();
}]);