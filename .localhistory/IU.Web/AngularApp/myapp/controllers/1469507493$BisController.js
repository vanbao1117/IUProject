﻿//'use strict';

IUApp.controller('BisController', ['$scope', '$http', '$templateCache', '$timeout', '$routeParams', 'LecturerService', 'BisServices', 'SweetAlert',
    function ($scope, $http, $templateCache, $timeout, $routeParams, LecturerService, BisServices, SweetAlert) {
       
     $scope.subjectSelect = false;

     $scope.$watch('subjectSelect', function (newVal, oldVal) {

         if (newVal) {

         }
         console.log('subjectSelect oldVal', oldVal);
         console.log('subjectSelect newVal', newVal);
     }, true);

    (function init() {
        $timeout(function () {
            $templateCache.removeAll();
            //set header text
            $scope.setPageHeader("Register BIS-Class");


            console.log('Register BIS-Class controller with timeout fired');
        }, 500);
    })();
}]);