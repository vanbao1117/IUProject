//'use strict';

IUApp.controller('AdminScheduleController', ['$scope', '$http', '$location', '$route', '$filter', '$templateCache', '$timeout', 'SubjectService', 'ScheduleServices',
    'AttendanceService', 'SweetAlert',
    function ($scope, $http, $location, $route, $filter, $templateCache, $timeout, SubjectService, ScheduleServices, AttendanceService, SweetAlert) {
        $scope.mode = 'edit';
        $scope.blogs = [{ blogID: 1, name: '1' }, { blogID: 2, name: '2' }];
        $scope.subjects = [];
        $scope.$watch('subjects', function (newVal, oldVal) {
            console.log('subjects', newVal);
        });

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
            _class.edited = true;
            

            var oldVal = $scope.copyObjToObj(_class);
            _class.oldVal = oldVal;

            console.log('edit row: ', '.allow_' + index);
            console.log('edit row: ', '.remove_' + index);
            if (button.indexOf('allow') >= 0) {
                $scope.editedClass.push(_class);
                console.log('edit row: ', _class);
                $("#" + row).find("select").removeAttr('disabled');
                $("#" + row).find("input").removeAttr('disabled');
                
                $('#allow_' + index).css('display', 'none');
                $('#remove_' + index).css('display', 'block');
            } else {

                angular.forEach($scope.editedClass, function (value, key) {
                    if (angular.equals(value, _class)) {
                        $scope.editedClass.splice(value, 1);
                        console.log('not edit row: ', value);
                        return;
                    }
                });

                delete _class.oldVal;
                _class.edited = false;
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

        $scope.copyObjToObj = function (source, destination) {
            if (!!destination) {
                angular.copy(source, destination);
            } else {
                destination = angular.copy(source);
            }
            return destination;
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

        $scope.saveChange = function (isNewSchedule) {
        //Bis
            if ($scope.currentTab == 'activity') {
                if ($scope.editSlot1Selected === undefined || $scope.editSlot2Selected === undefined) {
                    swal({ title: "warning!", text: "Please choose slot!", type: "warning" });
                    return;
                }
                if ($scope.editSlot1Selected.slotID == $scope.editSlot2Selected.slotID) {
                    swal({ title: "warning!", text: "You choosed same slot!", type: "warning" });
                    return;
                }
                if ($scope.className === undefined || $scope.className == '') {
                    swal({ title: "warning!", text: "Please enter class name!", type: "warning" });
                    return;
                }

                if ($scope.startDay === undefined || $scope.startDay == '') {
                    swal({ title: "warning!", text: "Please enter start Day!", type: "warning" });
                    return;
                }

                if ($scope.editRoomSelected === undefined || $scope.editRoomSelected.roomID == '') {
                    swal({ title: "warning!", text: "Please enter room!", type: "warning" });
                    return;
                }

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
                  console.log('createBis status: ', status);
                  
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
                  
              },
              function (error) {
                  console.log('createBis error: ' + error);
              });

        } else if ($scope.currentTab == 'timeline') {//lecture 

        } else if ($scope.currentTab == 'settings') {//Class 
            angular.forEach($scope.classSchedules, function (value, key) {
                if (value.edited) {
              
                    var oldval = $scope.copyObjToObj(value.oldVal, oldval);

                    delete value.oldVal;

                    var nval = $scope.copyObjToObj(value, nval);

                    var submit = { oldModel: oldval, newModel: nval, isNewSchedule: isNewSchedule };

                    console.log('Update submit: ', submit);

                    SubjectService.updateClassSchedule(submit).then(
                      function (status) {
                          console.log('updateClassSchedule status: ', status);
                          $timeout(function () {
                              if (status == false || status == 'false') {
                                  swal({ title: "warning!", text: "Already exist slot applied for Lecture on same day!", type: "warning" });
                                  return;
                              }
                              $scope.GetClassSchedule();
                              SweetAlert.swal("Update Schedule!", "Update Schedule successfuly!", "success");
                          });
                          console.log('Update Schedule: ', status);
                      },
                      function (error) {
                          console.log('Update Schedule error: ' + error);
                      });
                }
            });
           
        }
    }

 

    $scope.setPageHeader = function (header) {
        $('.content-header').html('<h1>' + header + '</h1><ol class="breadcrumb"><li><a href="#"><i class="fa fa-dashboard"></i> Home</a></li><li class="active">' + header + '</li></ol>');
    };

    $scope.range = function (n) {
        return new Array(n);
    };

    $scope.getAllSubjects = function (lecturerID) {
        console.log('lecturerID: ', lecturerID);
        if (lecturerID === undefined) lecturerID = "";
        SubjectService.getAllSubjects(lecturerID).then(
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



    $scope.SetupClassSchedule = function () {
        $scope.mode = 'setup';
       
    };

    


     (function init() {
         $timeout(function () {
             $scope.setPageHeader('Schedule');

             $scope.getAllSubjects("");
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