//'use strict';

IUApp.controller('HomeController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    function ($scope, $http, $location, $route, $templateCache, $timeout, SubjectService, ScheduleServices) {
    $scope.Subjects = [];

    $scope.gotoMenu = function (url, header) {
        if (header) {
            $scope.setPageHeader(header);
        }
        
        $templateCache.removeAll();
        $location.path(url);
    };


    $scope.getSubjects = function () {
        SubjectService.get().then(
          function( subjects ) {
              $scope.Subjects = subjects;
              console.log('getSubjects: ', subjects);
          },
          function( error ) {
              console.log('getSubjects error: '+ error);
          });
    }

    $scope.activeMenu = function () {
        $('.sidebar-menu').find('li').each(function (j, li) {
            alert(li.text());
        })
    }

    $scope.setPageHeader = function (header) {
        $('.content-header').html('<h1>' + header + '</h1><ol class="breadcrumb"><li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li><li class="active">' + header + '</li></ol>');
    };

     (function init() {
         $timeout(function () {
             $scope.getSubjects();
             
             console.log('Home controller initial with timeout fired');
         }, 500);
     })();
}]);