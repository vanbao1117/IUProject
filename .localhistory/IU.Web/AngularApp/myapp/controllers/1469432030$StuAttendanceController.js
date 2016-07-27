﻿//'use strict';

IUApp.controller('StuAttendanceController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', '$routeParams',
    'AttendanceService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, $routeParams, AttendanceService) {
        $scope.semester = {};
        $scope.attendances = {};
        $scope.currentPage = [];
        $scope.semesterName = $routeParams.semesterName;

        $scope.$watch('attendances', function (newVal, oldVal) {
            console.log('changed');
        }, true);

        $scope.getAttendances = function (page) {
            
      
            AttendanceService.getAttendance(page, $scope.semester.semesterCode, "").then(
              function (attendances) {
                  $scope.attendances = attendances;
                  angular.forEach($scope.attendances, function (item, key) {
                      $scope.currentPage.push({ page: 1, subjectCode: item.subjectCode });
                  });
                  console.log('getAttendance: ', attendances);
              },
              function (error) {
                  console.log('getAttendance error: ' + error);
              });
        };

        
        $scope.getAttendancesFlowSubject = function (page, subjectCode) {
            $scope.currentPage.push({ page: 1, subjectCode: subjectCode });
            $scope.currentPage = page;
            AttendanceService.getAttendance(page, $scope.semester.semesterCode, subjectCode).then(
              function (attendances) {
                  if (subjectCode != "") {
                      var index = 0;
                      angular.forEach($scope.attendances, function (item, key) {
                          if (item.subjectCode == subjectCode) {
                              console.log('$scope.attendances[' + index + ']: ', $scope.attendances[index]);
                              $scope.attendances[index] = attendances[0];
                              
                              return;
                          }
                          index++;
                      });
                  }else{
                      $scope.attendances = attendances;
                  }
                  
                  console.log('getAttendance: ', attendances);
              },
              function (error) {
                  console.log('getAttendance error: ' + error);
              });
        };

        $scope.next = function (subjectCode) {
            angular.forEach($scope.currentPage, function (item, key) {
                if (item.subjectCode == subjectCode) {
                    
                    return;
                }
                index++;
            });
        };

        $scope.previous = function (subjectCode) {

        };

        (function init() {
            $timeout(function () {
                $templateCache.removeAll();
                //set header text
                angular.forEach($scope.semesters, function (item, key) {
                    if (item.semesterCode == $routeParams.semesterName) {
                        $scope.semester = item;
                        $scope.setPageHeader(item.semesterName);
                        return;
                    }
                });
                
                $('#page_title').text('Inteligent | Semester');
                console.log('System controller with timeout fired');
                $scope.getAttendances(1);
                
            }, 500);
        })();
    }]);