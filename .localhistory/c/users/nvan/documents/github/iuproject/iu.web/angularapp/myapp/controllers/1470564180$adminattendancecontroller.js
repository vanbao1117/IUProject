//'use strict';

IUApp.controller('AdminAttendanceController', ['$scope', '$http', '$location', '$route', '$templateCache', '$timeout', '$routeParams', '$window', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'SweetAlert', 'LecturerService',
    function ($scope, $http, $location, $route, $templateCache, $timeout, $routeParams, $window, SubjectService, ScheduleServices, AttendanceService, SweetAlert, LecturerService) {

        $scope.classSelected = {};
        $scope.subjectSelected = {};
        $scope.lectureSelected = {};
        

        $scope.$watch('classSelected', function (newVal, oldVal) {
            console.log('classSelected changed', newVal);
            $scope.GetLecturerPreview();
        });
        $scope.$watch('subjectSelected', function (newVal, oldVal) {
            console.log('subjectSelected changed', newVal);
            $scope.GetLecturerPreview();
        });
        $scope.$watch('lectureSelected', function (newVal, oldVal) {
            console.log('lectureSelected changed', newVal);
            $scope.GetLecturerPreview();
        });


        $scope.setPageHeader = function (header) {
            $('.content-header').html('<h1>' + header + '</h1><ol class="breadcrumb"><li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li><li class="active">' + header + '</li></ol>');
        };

        $scope.GetLectureClassSubject = function () {
            LecturerService.GetLectureClassSubject().then(
                 function (classSubjects) {
                     $scope.classSelected = classSubjects.lectureClass[0];
                     $scope.subjectSelected = classSubjects.lectureSubject[0];
                     $scope.lectureSelected = classSubjects.lectures[0];
                     $scope.classSubjects = classSubjects;
                     

                     $scope.GetLecturerPreview();
                 },
                 function (error) {
                     console.log('GetLectureClassSubject error: ' + error);
                 });
        };

        $scope.GetLecturerPreview = function () {
            if ($scope.classSelected === undefined) return;
            if ($scope.subjectSelected === undefined) return;
            if ($scope.lectureSelected === undefined || $scope.lectureSelected == null) return;
            var classID = $scope.classSelected.classID;
            var subjectID = $scope.subjectSelected.subjectID;
            var lecturerID = undefined;
            if ($scope.lectureSelected !== undefined && $scope.lectureSelected != null) {
                lecturerID = $scope.lectureSelected.lecturerID;
            }
            
            LecturerService.GetLecturerPreview(classID, subjectID, lecturerID).then(
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

                console.log('AdminAttendanceController initial with timeout fired');
            }, 500);
        })();
    }]);