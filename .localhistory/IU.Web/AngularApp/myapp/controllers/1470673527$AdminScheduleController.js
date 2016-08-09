//'use strict';

IUApp.controller('AdminScheduleController', ['$scope', '$http', '$location', '$route', '$filter', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'SweetAlert',
    function ($scope, $http, $location, $route, $filter, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService, SweetAlert) {
        $scope.mode = 'edit';

        //Bis
        $scope.subjectSelected = {};
        $scope.semesterSelected = {};
        $scope.editBisLecturerSelected = {};
        $scope.editRoomSelected = {};
        $scope.editSlotSelected = {};
        $scope.editModeSelected = {};
        $scope.startDay = '';
        $scope.className = '';
        $scope.quantity = '';
        $scope.deadLine = '';
        $scope.editedClass = [];

        //Lecturer
        $scope.lecturerSubjectSelected = {};
        $scope.lecturerSelected = {};

        //class
        $scope.classSubjectSelected = {};
        $scope.classSemesterSelected = {};
        $scope.classSelected = {};
        
        $scope.allowEdit = function (row, button, index, _class) {
            console.log('edit row: ', '.allow_' + index);
            console.log('edit row: ', '.remove_' + index);
            if (button.indexOf('allow') >= 0) {
                $scope.editedClass.push(_class);
                $("#" + row).find("select").removeAttr('disabled');
                $("#" + row).find("input").removeAttr('disabled');
                
                $('#allow_' + index).css('display', 'none');
                $('#remove_' + index).css('display', 'block');
                console.log('remove_ row: ', '.remove_' + index);
            } else {
                console.log('disableEdit row: ', row);
                $("#" + row).find("select").attr('disabled', true);
                $("#" + row).find("input").attr('disabled', true);
                $('#allow_' + index).css('display', 'block');
                $('#remove_' + index).css('display', 'none');
            }
        };


        $scope.currentTab = 'activity';
        $scope.activeTab = function (tab, className) {
            $('#' + tab).show();
        
            $scope.currentTab = tab;

            $('.admin-tab-schedule').find('li').each(function () {
                // cache jquery object
                var current = $(this);
                current.removeClass('active');
            });
        
            $('.' + className).addClass('active');

            if (tab == 'activity') {
                $('#timeline').hide();
                $('#settings').hide();
            } else if (tab == 'timeline') {
                $('#activity').hide();
                $('#settings').hide();
            } else if (tab == 'settings') {
                $('#timeline').hide();
                $('#activity').hide();
            }
        };

    $scope.saveChange = function () {
        //Bis
        if ($scope.currentTab == 'activity') {
            var myDate = new Date(new Date().getTime()+(5*24*60*60*1000));
            var openClass = {
                semesterID: $scope.semesterSelected.semesterID,
                className: $scope.className,
                startDate: $scope.startDay,
                roomID: $scope.editRoomSelected.roomID,
                slotIDs: [$scope.editSlot1Selected.slotID, $scope.editSlot2Selected.slotID],
                modeID: $scope.editModeSelected.modeID,
                limit: $scope.quantity,
                deadline: $scope.deadLine,
                subjectID: $scope.subjectSelected.subjectID,
                lecturerID: $scope.editBisLecturerSelected.lecturerID
            };

            SubjectService.createBis(openClass).then(
              function (status) {
                  $timeout(function () {
                      $scope.subjectSelected = {};
                      $scope.semesterSelected = {};
                      $scope.editBisLecturerSelected = {};
                      $scope.editRoomSelected = {};
                      $scope.editSlotSelected = {};
                      $scope.editModeSelected = {};
                      $scope.startDay = '';
                      $scope.className = '';
                      $scope.quantity = '';
                      $scope.deadLine = '';
                      SweetAlert.swal("Create BIS-Class!", "Create BIS-Class successfuly!", "success");
                  });
                  console.log('createBis: ', status);
              },
              function (error) {
                  console.log('createBis error: ' + error);
              });

        } else if ($scope.currentTab == 'timeline') {//lecture 

        } else if ($scope.currentTab == 'settings') {//Class 

        }
    }

    $scope.setPageHeader = function (header) {
        $('.content-header').html('<h1>' + header + '</h1><ol class="breadcrumb"><li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li><li class="active">' + header + '</li></ol>');
    };

    $scope.range = function (n) {
        return new Array(n);
    };

    $scope.getAllSubjects = function () {
        SubjectService.getAllSubjects().then(
           function (subjects) {
               console.log('getAllSubjects: ', subjects);
               $scope.subjects = subjects;
           },
           function (error) {
               console.log('getAllSubjects error: ' + error);
           });
    };

    $scope.getAllSemester = function () {
        SubjectService.getAllSemester().then(
           function (semesters) {
               console.log('getAllSemester: ', semesters);
               $scope.semesters = semesters;
           },
           function (error) {
               console.log('getAllSemester error: ' + error);
           });
    };

    $scope.getAllClass = function () {
        SubjectService.getAllClass().then(
           function (classes) {
               console.log('getAllClass: ', classes);
               $scope.classes = classes;
           },
           function (error) {
               console.log('getAllClass error: ' + error);
           });
    };

    $scope.getAllLecturer = function () {
        SubjectService.getAllLecturer().then(
           function (lecturers) {
               console.log('getAllLecturer: ', lecturers);
               $scope.lecturers = lecturers;
           },
           function (error) {
               console.log('getAllLecturer error: ' + error);
           });
    };

    $scope.GetAllSlots = function () {
        SubjectService.GetAllSlots().then(
           function (slots) {
               console.log('GetAllSlots: ', slots);
               $scope.slots = slots;
           },
           function (error) {
               console.log('GetAllSlots error: ' + error);
           });
    };

    $scope.GetAllRooms = function () {
        SubjectService.GetAllRooms().then(
           function (rooms) {
               console.log('GetAllRooms: ', rooms);
               $scope.rooms = rooms;
           },
           function (error) {
               console.log('GetAllRooms error: ' + error);
           });
    };

    $scope.GetAllModes = function () {
        SubjectService.GetAllModes().then(
           function (modes) {
               console.log('GetAllModes: ', modes);
               $scope.modes = modes;
           },
           function (error) {
               console.log('GetAllModes error: ' + error);
           });
    };

    $scope.GetClassSchedule = function () {
        $scope.mode = 'edit';
        var classID = $scope.classSelected.classID;
        $scope.className = $scope.classSelected.className;
        console.log('GetClassSchedule: ', $scope.className);
        var semesterID = $scope.classSemesterSelected.semesterID;
        SubjectService.GetClassSchedule(classID, semesterID).then(
           function (classSchedules) {
               console.log('GetClassSchedule: ', classSchedules);
               $scope.classSchedules = classSchedules;
           },
           function (error) {
               console.log('GetClassSchedule error: ' + error);
           });
    };

    $scope.SetupClassSchedule = function () {
        $scope.mode = 'setup';
       
    };

     (function init() {
         $timeout(function () {
             $scope.setPageHeader('Schedule');

             $scope.getAllSubjects();
             $scope.getAllSemester();
             $scope.getAllClass();
             $scope.getAllLecturer();
             $scope.GetAllSlots();
             $scope.GetAllRooms();
             $scope.GetAllModes();
             $scope.GetClassSchedule('d5e7b9bc-2614-4fcb-a1ba-6d6b11702850', 'd5e7b9bc-2614-4fcb-a1ba-6d6b11702852');

             console.log('AdminScheduleController initial with timeout fired');
         }, 500);
     })();
}]);