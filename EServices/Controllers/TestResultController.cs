using EServices.Models;
using EServices.Models.Subject;
using EServices.Models.TestResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EServices.CustomClasses;
using System.IO;
using EServices.Models.Admission;

namespace EServices.Controllers
{
    [Authorize(Roles = "Teacher,Admin")]
    public class TestResultController : Controller
    {
        // GET: TestResult
        SMS sms = new SMS();
        CustomClasses.CurrentSession obj = new CustomClasses.CurrentSession();

        public ActionResult Index()
        {
            using (DB db = new DB())
            {
                if (User.IsInRole("Teacher"))
                {
                    LoadAssignedClass();
                }
                else
                {
                    var sections = db.Sections.ToList();
                    var classes = db.Classes.ToList();
                    var subject = db.Subjects.ToList();
                    ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                    ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                    ViewBag.subjects = new SelectList(subject, "SubjectId", "SubjectName");
                }

            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(int Class, int Section, int Subject)
        {
            CustomClasses.CurrentSession obj = new CustomClasses.CurrentSession();
            var currentSession = obj.Session();
            string date = LocalTime.Now().ToLongDateString();
            using (DB db = new DB())
            {
                var sections = db.Sections.ToList();
                var classes = db.Classes.ToList();
                var subject = db.Subjects.ToList();
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                ViewBag.subjects = new SelectList(subject, "SubjectId", "SubjectName");
                var session = db.Sessions.Where(a => a.SessionName == currentSession).FirstOrDefault();
                var ExistResult = db.TestResults.Where(a => a.ClassId == Class && a.SessionId == session.SessionId && a.SubjectId == Subject && a.SecionId == Section && a.Date == date).FirstOrDefault();
                if (ExistResult != null)
                {
                    ViewBag.exist = "Today Result is already submited...";
                }
                else
                {

                    var SelectClass = db.Registration.Where(a => a.SessionId == session.SessionId && a.ClassId == Class && a.SectionId == Section && a.Status).ToList();

                    ViewBag.list = SelectClass;
                    ViewBag.SubjectId = Subject;
                }

            }
            return View();
        }

        [HttpPost]
        public ActionResult AddTestResult(List<TestResult> model)
        {
            MyHub hub = new MyHub();
            string date = LocalTime.Now().ToLongDateString();
            using (DB db = new DB())
                {
                    foreach (var item in model)
                    {
                        var s = db.TestResults.Include("Students").Where(a => a.ClassId == item.ClassId && a.AddmissionNo == item.AddmissionNo && a.SessionId == item.SessionId && a.SubjectId == item.SubjectId && a.SecionId == item.SecionId && a.Date == date).FirstOrDefault();
                        if (s != null)
                        {
                            TempData["msg"] = "The result is already submited";

                            break;
                        }
                        else
                        {

                            item.Date = LocalTime.Now().ToLongDateString();
                            var total = item.TotalMarks;
                            var obtainedTotal = item.ObtainedMarks;
                            if (obtainedTotal > total)
                            {
                                return Json("Obtained Marks greater than total,please try again");
                            }
                            var percentage = Calculate.CalculcatePercentage(total, obtainedTotal);
                            var Status = Calculate.AssignGrade(percentage);
                            item.ObtainedMarks = obtainedTotal;
                            item.Percentage = percentage.ToString();
                            item.Status = Status;

                            db.TestResults.Add(item);

                            TempData["msg"] = "Result submited successfully";

                            var std = db.Registration.Where(a => a.AddmissionNo == item.AddmissionNo).FirstOrDefault();
                            var parent = db.Parent.Where(a => a.ParentId == std.ParentId).FirstOrDefault();
                            var subject = db.Subjects.Where(a => a.SubjectId == item.SubjectId).FirstOrDefault();
                            //send notification to parent
                            hub.Send(parent.CNIC);

                            //setting sms 
                            if (item.SMS_Status)
                            {
                                string number = parent.PhoneNo;
                                string message = "Test Result announced!" + std.StudentName + " Got " + item.ObtainedMarks + " out of " + item.TotalMarks + " in " + subject.SubjectName;
                                sms.Send(number, message);
                            }
                        }
                    }
                    db.SaveChanges();


                    //  hub.Send();
                }

                return RedirectToAction("Index", "TestResult");
           

        }
        public ActionResult menu()
        {
            return View();
        }
        public ActionResult ViewResult()
        {
            using (DB db = new DB())
            {
                var month = db.Month.ToList();
                var session = db.Sessions.ToList();
                ViewBag.months = new SelectList(month, "MonthId", "MonthName");
                ViewBag.session = new SelectList(session, "SessionId", "SessionName");
                if (User.IsInRole("Teacher"))
                {
                    LoadAssignedClass();
                }
                else
                {
                    var sections = db.Sections.ToList();
                    var classes = db.Classes.ToList();
                    var subject = db.Subjects.ToList();
                    ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                    ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                    ViewBag.subjects = new SelectList(subject, "SubjectId", "SubjectName");
                }

            }
            return View();
        }
        [HttpPost]
        public ActionResult ViewResult(int Class, int Section, int Subject, int Month, int Session,string tests)
        {
            using (DB db = new DB())
            {
                TempData["ClassId"] = Class;
                TempData["SectionId"] = Section;
                TempData["SubjectId"] = Subject;
                TempData["MonthId"] = Month;
                TempData["SessionId"] = Session;
                TempData["tests"] = tests;
                var month = db.Month.ToList();
                var session = db.Sessions.ToList();
                ViewBag.months = new SelectList(month, "MonthId", "MonthName");
                ViewBag.session = new SelectList(session, "SessionId", "SessionName");
                if (User.IsInRole("Teacher"))
                {
                    LoadAssignedClass();
                }
                else
                {
                    var sections = db.Sections.ToList();
                    var classes = db.Classes.ToList();
                    var subject = db.Subjects.ToList();
                    ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                    ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                    ViewBag.subjects = new SelectList(subject, "SubjectId", "SubjectName");
                }
                var result = db.TestResults.Include("Students").Include("Classes").Include("Months").Include("Sections").Where(a => a.ClassId == Class && a.SecionId == Section && a.SessionId == Session && a.SubjectId == Subject && a.MonthId == Month&&a.Date==tests).ToList();
                ViewBag.Result = result;

            }
            return View();
        }
 
        public ActionResult EditResult(int id)
        {
            using (DB db = new DB())
            {
                var s = db.TestResults.Include("Students").Where(a => a.ResultId == id).FirstOrDefault();
                return View(s);
            }
        }
        [HttpPost]
        public ActionResult EditResult(TestResult model)
        {
            MyHub hub = new MyHub();
            using (DB db = new DB())
            {
                var s = db.TestResults.Where(a => a.ResultId == model.ResultId).FirstOrDefault();
                var total = model.TotalMarks;
                var obtainedTotal = model.ObtainedMarks;
                if (obtainedTotal > total)
                {
                    return Json("Obtained Marks greater than total,please try again");
                }
                var percentage = Calculate.CalculcatePercentage(total, obtainedTotal);
                var Status = Calculate.AssignGrade(percentage);
                s.ObtainedMarks = obtainedTotal;
                s.Percentage = percentage.ToString();
                s.Status = Status;
                db.SaveChanges();
                var std = db.Registration.Where(a => a.AddmissionNo == s.AddmissionNo).FirstOrDefault();
                var parent = db.Parent.Where(a => a.ParentId == std.ParentId).FirstOrDefault();
                //send notification to parent
                hub.Send(parent.CNIC);
            }
            return new JsonResult { Data = "Result updated successfully...", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult FindSection(int ClsId)
        {
            CustomClasses.CurrentSession obj = new CustomClasses.CurrentSession();
            var CurrentSession = obj.Session();
            using (DB db = new DB())
            {
                var teacherMapping = db.SubjectMapping.Include("Sections").Include("Sessions").Include("Classes").Include("Teacher").Where(a => a.Teacher.CNIC == User.Identity.Name && a.Sessions.SessionName == CurrentSession && a.ClassId == ClsId).ToList();
                List<Section2> sectionList = new List<Section2>();

                foreach (var item in teacherMapping)
                {
                    Section2 sec = new Section2();
                    sec.SectionId = item.SectionId;
                    sec.SectionName = item.Sections.SectionName;
                    var DuplicateSection = sectionList.Where(a => a.SectionName == sec.SectionName).FirstOrDefault();
                    if (DuplicateSection == null)
                    {
                        sectionList.Add(sec);
                    }
                }

                return Json(sectionList, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult FindSubject(int ClassId, int Section)
        {
            using (DB db = new DB())
            {
                CustomClasses.CurrentSession obj = new CustomClasses.CurrentSession();
                var CurrentSession = obj.Session();
                var teacherMapping = db.SubjectMapping.Include("Subjects").Include("Sections").Include("Sessions").Include("Classes").Include("Teacher").Where(a => a.Teacher.CNIC == User.Identity.Name && a.Sessions.SessionName == CurrentSession && a.ClassId == ClassId && a.SectionId == Section).ToList();
                List<Subject> subjectList = new List<Subject>();

                foreach (var item in teacherMapping)
                {
                    Subject sub = new Subject();
                    sub.SubjectId = item.SubjectId;
                    sub.SubjectName = item.Subjects.SubjectName;
                    //var DuplicateSubject = subjectList.Where(a => a.SubjectName== sub.SubjectName).FirstOrDefault();
                    //if (DuplicateSubject == null)
                    //{
                    subjectList.Add(sub);
                    // }
                }

                return Json(subjectList, JsonRequestBehavior.AllowGet);
            }
        }
        
        public void LoadAssignedClass()
        {
            List<Class> ClassList = new List<Class>();
            List<Section2> sectionList = new List<Section2>();
            CustomClasses.CurrentSession obj = new CustomClasses.CurrentSession();
            var CurrentSession = obj.Session();
            using (DB db = new DB())
            {


                var teacherMapping = db.SubjectMapping.Include("Sections").Include("Sessions").Include("Classes").Include("Teacher").Where(a => a.Teacher.CNIC == User.Identity.Name && a.Sessions.SessionName == CurrentSession).ToList();

                if (teacherMapping != null)
                {

                    foreach (var item in teacherMapping)
                    {
                        Class cls = new Class();
                        // Section sec = new Section();
                        cls.ClassId = item.ClassId;
                        cls.ClassName = item.Classes.ClassName;
                        //sec.SectionId = item.SectionId;
                        //sec.SectionName = item.Sections.SectionName;
                        var DuplicateClass = ClassList.Where(a => a.ClassName == cls.ClassName).FirstOrDefault();
                        //var DuplicateSection = sectionList.Where(a => a.SectionName == sec.SectionName).FirstOrDefault();
                        if (DuplicateClass == null)
                        {
                            ClassList.Add(cls);
                        }
                        //if (DuplicateSection == null)
                        //{
                        //    sectionList.Add(sec);
                        //}
                    }
                }
                var Classes = ClassList.ToList();
                //var sections = sectionList.ToList();

                ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                //ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
            }
        }
     
    }
}