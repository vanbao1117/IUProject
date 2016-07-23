﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace IU.Domain
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class IUContext : DbContext
    {
        public IUContext()
            : base("name=IUContext")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AdminTBL> AdminTBLs { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<AspTransport> AspTransports { get; set; }
        public virtual DbSet<AttendanceTBL> AttendanceTBLs { get; set; }
        public virtual DbSet<ClassScheduleTBL> ClassScheduleTBLs { get; set; }
        public virtual DbSet<ClassTBL> ClassTBLs { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<LecturerScheduleTBL> LecturerScheduleTBLs { get; set; }
        public virtual DbSet<LecturerTBL> LecturerTBLs { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public virtual DbSet<SemesterTBL> SemesterTBLs { get; set; }
        public virtual DbSet<SlotTBL> SlotTBLs { get; set; }
        public virtual DbSet<StudentListTBL> StudentListTBLs { get; set; }
        public virtual DbSet<StudentTBL> StudentTBLs { get; set; }
        public virtual DbSet<SubjectTBL> SubjectTBLs { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<TestTable> TestTables { get; set; }
    }
}