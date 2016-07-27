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
                 { className: 'SE901', roomID: 207, slotName: 4, startDate: '2016-07-22', select: true },
                 { className: 'SE903', roomID: 202, slotName: 1, startDate: '2016-07-30', select: false }
             ]
         },
         {
             subjectName: 'C# .Net', credit: 3, cost: 400000, select: false, subjectCode: 'CNET', active: false,
             chooseClass: [
                 { className: 'SE902', roomID: 207, slotName: 4, startDate: '2016-07-12', select: false },
                 { className: 'SE905', roomID: 202, slotName: 1, startDate: '2016-07-18', select: false }
             ]
         }
     ];

     $scope.chooseClass = $scope.subjects[0].chooseClass;

     $scope.GetRegisterData = function () {
       
         BisServices.getRegisterData().then(
           function (subjects) {
               console.log('getRegisterData: ', subjects);
               $scope.subjects = subjects;
               if ($scope.subjects[0] !== undefined)
                   $scope.chooseClass = $scope.subjects[0].chooseClass;
               
           },
           function (error) {
               console.log('getRegisterData error: ' + error);
           });
     };

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
             var index = 0;
             angular.forEach($scope.subjects, function (_item, key) {
                 if (item.subjectCode == _item.subjectCode) {
                     var class_index = 0;
                     angular.forEach($scope.subjects[index].chooseClass, function (_class, key) {
                         $scope.chooseClass[class_index].select = false;
                         class_index++;
                     });

                     return;
                 }
                 index++;
             });

             
         }
         
     };

     
     $scope.unRegister = function (submitItem) {
         BisServices.undoRegister(submitItem).then(
            function (accept) {
                console.log('acceptRegister: ', accept);

                if (accept.error) {
                    SweetAlert.swal("Warning!", "You already register this subject!");
                    return;
                }

                $scope.GetRegisterData();
                SweetAlert.swal("Submited!", "Your subject removed!", "success");
            },
            function (error) {
                console.log('getRegisterData error: ' + error);
            });
     };

     $scope.confirm = function () {

         var submitItem = {};
         var index = 0;
         var choosedSubject = false;
         var choosedClass = false;
         angular.forEach($scope.subjects, function (_item, key) {
             if (_item.select) {
                 choosedSubject = true;
                 var class_index = 0;
                 angular.forEach($scope.subjects[index].chooseClass, function (_class, key) {
                     if ($scope.chooseClass[class_index].select) {
                         choosedClass = true;
                         submitItem = $scope.subjects[index];
                         BisServices.acceptRegister(submitItem).then(
                              function (accept) {
                                  console.log('acceptRegister: ', accept);

                                  if (accept.error) {
                                      SweetAlert.swal("Warning!", "You already register this subject!");
                                      return;
                                  }

                                  $scope.GetRegisterData();
                                  SweetAlert.swal("Submited!", "Thank's for your submit!", "success");
                              },
                              function (error) {
                                  console.log('getRegisterData error: ' + error);
                              });
                         return;
                     }
                         
                     class_index++;
                 });

                 return;
             }
             index++;
         });

         if (!choosedSubject) {
             SweetAlert.swal("Warning!", "You must choose Subject");
         }

         if (!choosedClass) {
             SweetAlert.swal("Warning!", "You must choose Class");
         }

         console.log('Select submitItem', submitItem);
         
     };

    (function init() {
        $timeout(function () {
            $templateCache.removeAll();
            //set header text
            $scope.setPageHeader("Register BIS-Class");

            $scope.GetRegisterData();

            

            console.log('Register BIS-Class controller with timeout fired');
        }, 500);
    })();
}]);