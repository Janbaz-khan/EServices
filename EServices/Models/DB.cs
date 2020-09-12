using EServices.Models.API;
using EServices.Models.Attendace;
using EServices.Models.ClassMapping;
using EServices.Models.Fee;
using EServices.Models.Logins;
using EServices.Models.Results;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace EServices.Models
{
    public class DB:DbContext
    {
        public DbSet<StudentRegistrationModel> Registration { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Session> Sessions { get; set; }
        public DbSet<Section> Sections { get; set; }
        public DbSet<Gender> Genders { get; set; }
        public DbSet<Religion> Religions { get; set; }
        public DbSet<Parent> Parent { get; set; }
        public DbSet<Month> Month { get; set; }
        public DbSet<FeePayment> FeePayment { get; set; }
        public DbSet<Attendace.Attendance> Attendance { get; set; }
        public DbSet<Attendance.TeacherAttendance> TeacherAttendance { get; set; }
        public DbSet<Staff.Staff> Staff { get; set; }
        public DbSet<Subject.Subject> Subjects { get; set; }
        public DbSet<Subject.Subject_Teacher_Mapping> SubjectMapping { get; set; }
        public DbSet<TestResult.TestResult> TestResults { get; set; }
        public DbSet<Results.ExamType> Examtype { get; set; }
        public DbSet<Results.ExamResult> ExamResults { get; set; }
        public DbSet<ClassHeadTeacher> ClassHeadTeacher { get; set; }
        public DbSet<Roles> Roles { get; set; }
        public DbSet<MemberRegistration> Members { get; set; }
        public DbSet<HomeWorkType> HomeWorkType { get; set; }
        public DbSet<HomeWork> HomeWork { get; set; }
        public DbSet<SibDiscount> SibDiscount { get; set; }
        public DbSet<GradeSetting.GradeSetting> GradeSetting { get; set; }
        public DbSet<NormalFee> Normalfee { get; set; }
        public DbSet<Stationary_Fee> Stationaryfee { get; set; }
        public DbSet<SMSDevice> SMSDevices { get; set; }


        //public DB()
        //{
        //    this.Configuration.ProxyCreationEnabled = false;
        //}
        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<FeePayment>()
        //         .HasOptional<StudentRegistrationModel>(a => a.Student)
        //         .WithMany()
        //         .WillCascadeOnDelete(false);
        //}
    }
}