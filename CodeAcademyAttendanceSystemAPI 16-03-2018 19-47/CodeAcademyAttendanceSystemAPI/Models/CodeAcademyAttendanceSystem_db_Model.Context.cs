﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CodeAcademyAttendanceSystemAPI.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class CodeAcademyAttendanceSystem_dbEntities : DbContext
    {
        public CodeAcademyAttendanceSystem_dbEntities()
            : base("name=CodeAcademyAttendanceSystem_dbEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Genders> Genders { get; set; }
        public virtual DbSet<Group_Schedule> Group_Schedule { get; set; }
        public virtual DbSet<Group_Types> Group_Types { get; set; }
        public virtual DbSet<Groups> Groups { get; set; }
        public virtual DbSet<Lesson_Times> Lesson_Times { get; set; }
        public virtual DbSet<Qr_Codes> Qr_Codes { get; set; }
        public virtual DbSet<Role_Types> Role_Types { get; set; }
        public virtual DbSet<Students> Students { get; set; }
        public virtual DbSet<Students_Attendance> Students_Attendance { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<System_Settings> System_Settings { get; set; }
        public virtual DbSet<Teacher_Roles> Teacher_Roles { get; set; }
        public virtual DbSet<Teachers> Teachers { get; set; }
    }
}
