//'use strict';

IUApp.controller('BisController', ['$scope', '$http', '$templateCache', '$timeout', '$routeParams', 'LecturerService', 'BisServices', 'SweetAlert',
    function ($scope, $http, $templateCache, $timeout, $routeParams, LecturerService, BisServices, SweetAlert) {

        $scope.chooseClass = [];

        $scope.$watch('chooseClass', function (newVal, oldVal) {
            console.log('newVal', newVal);
        }, true);

     $scope.subjects = [
         {
             subjectName: 'Java', credit: 3, cost: 400000, select: true, subjectCode:'JAVA', active : true,
             chooseClass: [
                 { className: 'SE901', room: 207, slot: 4, startDate: '2016-07-22', select: true },
                 { className: 'SE903', room: 202, slot: 1, startDate: '2016-07-30', select: false }
             ]
         },
         {
             subjectName: 'C# .Net', credit: 3, cost: 400000, select: false, subjectCode: 'CNET', active: false,
             chooseClass: [
                 { className: 'SE902', room: 207, slot: 4, startDate: '2016-07-12', select: false },
                 { className: 'SE905', room: 202, slot: 1, startDate: '2016-07-18', select: false }
             ]
         }
     ];

     $scope.chooseClass = $scope.subjects[0].chooseClass;

     $scope.selectRow = function (_item) {
         $('.overlay').css('display', 'block');
         $scope.chooseClass = _item.chooseClass;
         var index = 0;
         angular.forEach($scope.subjects, function (item, key) {
             $scope.subjects[index].active = false;
             index++;
         });
         index = 0;
         angular.forEach($scope.subjects, function (item, key) {
             if (item.subjectCode == _item.subjectCode) {
                 $scope.subjects[index].active = true;
                 return;
             }
             index++;
         });
         $timeout(function () {
             $('.overlay').css('display', 'none');
         }, 200);
     };


     $scope.selectSubject = function (item, isSelected) {
         console.log('Select subject', item);
         console.log('Select isSelected', isSelected);
         if (!isSelected) {
             angular.forEach($scope.subjects, function (item, key) {
             if (item.subjectCode == _item.subjectCode) {
                 $scope.subjects[index].active = true;
                 return;
             }
             index++;
         });
         }
     };

     $scope.confirm = function () {
         SweetAlert.swal("Submited!", "Thank's for your submit!", "success");
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