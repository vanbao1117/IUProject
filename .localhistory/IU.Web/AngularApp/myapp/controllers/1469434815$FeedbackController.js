//'use strict';

IUApp.controller('FeedbackController', ['$scope', '$http', '$timeout', '$routeParams', function ($scope, $http, $timeout, $routeParams) {
    
   
    (function init() {
        $timeout(function () {
            $templateCache.removeAll();
            //set header text
            $scope.setPageHeader("Feedback");

            console.log('Feedback controller with timeout fired');
        }, 500);
    })();
}]);