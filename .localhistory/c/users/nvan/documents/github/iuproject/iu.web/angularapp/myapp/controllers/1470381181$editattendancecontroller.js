//'use strict';

IUApp.controller('EditAttendanceController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', '$routeParams', '$window', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'SweetAlert', 'LecturerService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, $routeParams, $window, SubjectService, ScheduleServices, AttendanceService, SweetAlert, LecturerService) {
       
        $scope.key = $routeParams.key;
        console.log('$scope.key: ', $scope.key);

        $scope.attendance = JSON.parse($window.localStorage.getItem('attendance'));

        $scope.selectedClass = {};
        $scope.selectedSubject = {};
        $scope.$watch('selectedClass', function (newVal, oldVal) {
            console.log('selectedClass changed');
        });
        $scope.$watch('selectedSubject', function (newVal, oldVal) {
            console.log('selectedSubject changed');
        });




    $scope.setPageHeader = function (header) {
        $('.content-header').html('<h1>' + header + '</h1><ol class="breadcrumb"><li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li><li class="active">' + header + '</li></ol>');
    };

    $scope.GetLectureClassSubject = function () {
        LecturerService.GetLectureClassSubject().then(
             function (classSubjects) {
                 $scope.selectedClass = classSubjects.lectureClass[0];
                 $scope.selectedSubject = classSubjects.lectureSubject[0];
                 $scope.classSubjects = classSubjects;
                 console.log('GetLectureClassSubject: ', classSubjects.lectureSubject[0]);
                 console.log('class: ', classSubjects.lectureClass[0]);
             },
             function (error) {
                 console.log('GetLectureClassSubject error: ' + error);
             });
    };

    $scope.GetLecturerPreview = function () {
        var subjectID = 'd5e7b9bc-2614-4fcb-a1ba-6d6b11752858';
        var classID = 'd5e7b9bc-2614-4fcb-a1ba-6d6b11752858';
        LecturerService.GetLecturerPreview(subjectID, classID).then(
             function (lecturerPreviews) {
                 $scope.lecturerPreviews = lecturerPreviews;

                 console.log('GetLecturerPreview: ', $scope.lecturerPreviews);
             },
             function (error) {
                 console.log('GetLecturerPreview error: ' + error);
             });
    };

    $scope.getAttendance = function (item) {
        AttendanceService.getAttendances(item).then(
             function (attendances) {
                 $scope.listAttendances = attendances;
                 console.log('getAttendances: ', $scope.listAttendances);
             },
             function (error) {
                 console.log('getAttendances error: ' + error);
             });
    };

    $scope.saveAttendance = function () {
        console.log('takeAttendances: ', $scope.listAttendances);
        AttendanceService.takeAttendances($scope.listAttendances).then(
             function (attendances) {
                
                 console.log('takeAttendances ok');
                 SweetAlert.swal("Take Attendances!", "Take Attendances successfuly!", "success");
                 $scope.getAttendance($scope.attendance.item);
             },
             function (error) {
                 console.log('getAttendances error: ' + error);
             });
    };

    $scope.range = function (n) {
        return new Array(n);
    };

     (function init() {
         $timeout(function () {
             $scope.GetLectureClassSubject();
             $scope.setPageHeader('Edit Attendance');
             if ($scope.attendance.key == $scope.key) {
                 console.log('$scope.attendance:', $scope.attendance);
                 $scope.getAttendance($scope.attendance.item);
             }
             console.log('EditAttendanceController initial with timeout fired');
         }, 500);
     })();
}]);