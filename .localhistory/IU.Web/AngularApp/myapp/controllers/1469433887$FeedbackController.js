﻿//'use strict';

IUApp.controller('FeedbackController', ['$scope', '$http', function ($scope, $http) {
    
   
    (function init() {
        $timeout(function () {
            $templateCache.removeAll();
            //set header text
            $scope.setPageHeader("Feedback");

            console.log('System controller with timeout fired');
        }, 500);
    })();
}]);