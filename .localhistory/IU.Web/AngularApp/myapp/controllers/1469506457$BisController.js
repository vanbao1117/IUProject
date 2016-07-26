//'use strict';

IUApp.controller('BisController', ['$scope', '$http', '$templateCache', '$timeout', '$routeParams', 'LecturerService', 'BisServices', 'SweetAlert',
    function ($scope, $http, $templateCache, $timeout, $routeParams, LecturerService, BisServices, SweetAlert) {
       
     $scope.subjectSelect = false;


    (function init() {
        $timeout(function () {
            $templateCache.removeAll();
            //set header text
            $scope.setPageHeader("Register BIS-Class");


            console.log('Register BIS-Class controller with timeout fired');
        }, 500);
    })();
}]);