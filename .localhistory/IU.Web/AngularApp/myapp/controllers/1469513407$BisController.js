//'use strict';

IUApp.controller('BisController', ['$scope', '$http', '$templateCache', '$timeout', '$routeParams', 'LecturerService', 'BisServices', 'SweetAlert',
    function ($scope, $http, $templateCache, $timeout, $routeParams, LecturerService, BisServices, SweetAlert) {

        $scope.chooseClass = [];

        $scope.$watch('class', function (newVal, oldVal) {
            console.log('newVal', newVal);
        }, true);

     $scope.subjects = [
         {
             subjectName: 'Java', credit: 3, cost: 400000, select: true, subjectCode:'JAVA',
             chooseClass: [
                 { className: 'SE901', room: 207, slot: 4, startDate: '2016-07-22', select: true },
                 { className: 'SE903', room: 202, slot: 1, startDate: '2016-07-30', select: false }
             ]
         },
         {
             subjectName: 'C# .Net', credit: 3, cost: 400000, select: false, subjectCode:'CNET',
             chooseClass: [
                 { className: 'SE902', room: 207, slot: 4, startDate: '2016-07-22', select: false },
                 { className: 'SE905', room: 202, slot: 1, startDate: '2016-07-30', select: false }
             ]
         }
     ];

     $scope.chooseClass = $scope.subjects[0].chooseClass;

     $scope.selectRow = function (_item) {
         $scope.class = _item.chooseClass;
     };

    (function init() {
        $timeout(function () {
            $templateCache.removeAll();
            //set header text
            $scope.setPageHeader("Register BIS-Class");


            console.log('Register BIS-Class controller with timeout fired');
        }, 500);
    })();
}]);