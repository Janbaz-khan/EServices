using EServices.CustomClasses;
using EServices.Models;
using EServices.Models.Attendace;
using EServices.Models.Results;
using EServices.Models.TestResult;
using EServices.SG_Component;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Areas.Parent.Controllers
{
    [Authorize(Roles = "Parent")]
    public class HomeController : Controller
    {
        CustomClasses.CurrentSession SessionObj = new CustomClasses.CurrentSession();
        // GET: Parent/Home
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetKids()
        {
            using (DB db = new DB())
            {
                var stds = db.Registration.Include("Parent").Include("Class").Include("Sections").Where(a => a.Parent.CNIC == User.Identity.Name).ToList();
                var list = new List<resultVM>();
                foreach (var item in stds)
                {
                    resultVM model = new resultVM();
                    model.Name = item.StudentName;
                    model.Class = item.Class.ClassName;
                    model.Section = item.Sections.SectionName;
                    list.Add(model);
                }
                return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public ActionResult Notification()
        {
            return View();
        }
        public JsonResult GetTestNotification()
        {
            using (DB db = new DB())
            {
                var parent = db.Parent.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                var std = db.Registration.Where(a => a.ParentId == parent.ParentId).ToList();
                var Record = new List<TestResult>();
                foreach (var item in std)
                {
                    var s = db.TestResults.Include("Students").Include("Subjects").Include("Classes").Where(a => a.AddmissionNo == item.AddmissionNo).Take(7).ToList();
                    Record.AddRange(s);
                }

                var Data = new List<resultVM>();
                foreach (var item in Record)
                {
                    resultVM vm = new resultVM();
                    vm.id = item.ResultId;
                    vm.Name = item.Students.StudentName;
                    vm.Class = item.Classes.ClassName;
                    vm.Marks = item.ObtainedMarks;
                    vm.TMarks = item.TotalMarks;
                    vm.date = item.Date;
                    vm.Read = item.ReadStatus;
                    vm.subject = item.Subjects.SubjectName;
                    Data.Add(vm);
                }
                var step2=Data.OrderByDescending(a=>a.id).ToList();
                return new JsonResult { Data = step2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult GetExamNotification()
        {

            using (DB db = new DB())
            {
                var parent = db.Parent.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                var std = db.Registration.Where(a => a.ParentId == parent.ParentId).ToList();
                var Record = new List<ExamResult>();
                foreach (var item in std)
                {
                    var s = db.ExamResults.Include("Students").Include("Subjects").Include("Examtype").Include("Classes").Where(a => a.StudentId == item.AddmissionNo).Take(7).ToList();
                    Record.AddRange(s);
                }

                var Data = new List<ExamResultVM>();
                foreach (var item in Record)
                {
                    ExamResultVM vm = new ExamResultVM();
                    vm.Name = item.Students.StudentName;
                    vm.Class = item.Classes.ClassName;
                    vm.Marks = item.ObtainedTotal;
                    vm.TMarks = item.TotalMarks;
                    vm.date = item.DateTime;
                    vm.type = item.Examtype.ExamName;
                    vm.subject = item.Subjects.SubjectName;
                    vm.Read = item.ReadStatus;
                    vm.id = item.ExamResultId;
                    Data.Add(vm);
                }
                var step2 = Data.OrderByDescending(a => a.id).ToList();
                return new JsonResult { Data = step2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult GetAttendanceNotification()
        {
            using (DB db = new DB())
            {
                var parent = db.Parent.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                var std = db.Registration.Where(a => a.ParentId == parent.ParentId).ToList();
                var Record = new List<Attendance>();
                foreach (var item in std)
                {
                    var s = db.Attendance.Include("Students").Include("Classes").Where(a => a.AddmissionNo == item.AddmissionNo).OrderByDescending(a => a.Attendace_Id).Take(8).ToList();
                    Record.AddRange(s);
                }

                var Data = new List<AttendanceVM>();
                foreach (var item in Record)
                {
                    AttendanceVM vm = new AttendanceVM();
                    vm.Name = item.Students.StudentName;
                    vm.Class = item.Classes.ClassName;
                    vm.Date = item.Date;
                    vm.Read = item.ReadStatus;
                    vm.id = item.Attendace_Id;
                    Data.Add(vm);
                }
                var step2 = Data.OrderByDescending(a => a.id).ToList();
                return new JsonResult { Data = step2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public JsonResult GetHomeworkNotification()
        {

            using (DB db = new DB())
            {
                var Session = SessionObj.Session();
                var parent = db.Parent.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                var std = db.Registration.Where(a => a.ParentId == parent.ParentId).ToList();
                var record = new List<HomeWork>();

                foreach (var item in std)
                {
                    //var subject = db.SubjectMapping.Include("Sessions").Where(a => a.ClassId == item.ClassId && a.Sessions.SessionName == Session).FirstOrDefault();
                    var hw = db.HomeWork.Include("HomeWorks").Include("Subjects").Include("Sessions").Include("Classes").Where(a => a.ClassId == item.ClassId && a.SecionId == item.SectionId && a.Sessions.SessionName == Session).ToList();

                    record.AddRange(hw);
                }
                var Populaterecord = record.OrderByDescending(s => s.HomeWorkId).Take(10).ToList();
                var Data = new List<HomeWorkVM>();
                foreach (var item in Populaterecord)
                {
                    HomeWorkVM vm = new HomeWorkVM();

                    vm.Class = item.Classes.ClassName;
                    vm.date = item.Date;
                    vm.type = item.HomeWorks.HomeWorkName;
                    vm.subject = item.Subjects.SubjectName;
                    vm.Content = item.Content;
                    vm.Read = item.ReadStatus;
                    Data.Add(vm);
                }
                var step2 = Data.OrderByDescending(a => a.id).ToList();
                return new JsonResult { Data = step2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        #region read
        public JsonResult ReadTest()
        {
            using (DB db = new DB())
            {
                var current_session = SessionObj.Session();
                var s = db.TestResults.Include("Sessions").Where(a => a.Sessions.SessionName == current_session).ToList();
                foreach (var item in s)
                {
                    item.ReadStatus = true;
                }
                db.SaveChanges();
            }
            return new JsonResult { Data = "Read", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ReadExam()
        {
            using (DB db = new DB())
            {
                var current_session = SessionObj.Session();
                var s = db.ExamResults.Include("Sessions").Where(a => a.Sessions.SessionName == current_session).ToList();
                foreach (var item in s)
                {
                    item.ReadStatus = true;
                }
                db.SaveChanges();
            }
            return new JsonResult { Data = "Read", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ReadAtten()
        {
            using (DB db = new DB())
            {
                var current_session = SessionObj.Session();
                var s = db.Attendance.Include("Sessions").Where(a => a.Sessions.SessionName == current_session).ToList();
                foreach (var item in s)
                {
                    item.ReadStatus = true;
                }
                db.SaveChanges();
            }
            return new JsonResult { Data = "Read", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult ReadHomeWork()
        {
            using (DB db = new DB())
            {
                var current_session = SessionObj.Session();
                var s = db.HomeWork.Include("Sessions").Where(a => a.Sessions.SessionName == current_session).ToList();
                foreach (var item in s)
                {
                    item.ReadStatus = true;
                }
                db.SaveChanges();
            }
            return new JsonResult { Data = "Read", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        #endregion


    }
}