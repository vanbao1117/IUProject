﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IU.Web.Models
{
    public class BisViewModel
    {
         //semesterID: $scope.semesterSelected.semesterID,
         //       className: $scope.className,
         //       startDate: $scope.startDay,
         //       roomID: $scope.editRoomSelected.roomID,
         //       slotID1: $scope.editSlot1Selected.slotID,
         //       slotID2: $scope.editSlot2Selected.slotID,
         //       modeID: $scope.editModeSelected.modeID,
         //       limit: $scope.quantity,
         //       deadline: $scope.deadLine

        public string SemesterID { get; set; }
        public string ClassID { get; set; }
        public string ClassName { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public string RoomID { get; set; }
        public string RoomName { get; set; }
        public string SlotID1 { get; set; }
        public string SlotID2 { get; set; }
        public string SlotName1 { get; set; }
        public string SlotName2 { get; set; }
        public int ModeID { get; set; }
        public int Limit { get; set; }
        public Nullable<System.DateTime> Deadline { get; set; }
    }

}