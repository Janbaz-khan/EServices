using EServices.CustomClasses;
using EServices.Models;
using EServices.Models.Admission;
using EServices.Models.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EServices.CustomClasses;
namespace EServices.Controllers
{
    [Authorize(Roles = "Teacher,Admin")]
    public class ResultController : Controller
    {
        MyHub hub = new MyHub();
        SMS sms = new SMS();
        // GET: Regis
        public ActionResult Index()
        {
            ViewBag.q = LocalTime.Now().ToLongDateString();
            using (DB db = new DB())
            {
                var type = db.Examtype.ToList();
                ViewBag.type = new SelectList(type, "ExamTypeId", "ExamName");
                if (User.IsInRole("Teacher"))
                {
                    LoadAssignedClass();
                }
                else
                {
                    var section = db.Sections.ToList();
                    var classes = db.Classes.ToList();
                    var subjects = db.Subjects.ToList();
                    ViewBag.sections = new SelectList(section, "SectionId", "SectionName");
                    ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                    ViewBag.subjects = new SelectList(subjects, "SubjectId", "SubjectName");
                }

            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(int Type, int Class, int Section, int Subject)
        {
            CurrentSession session = new CurrentSession();
            var currentsession = session.Session();
            using (DB db = new DB())
            {
                var type = db.Examtype.ToList();
                ViewBag.type = new SelectList(type, "ExamTypeId", "ExamName");

                var section = db.Sections.ToList();
                var classes = db.Classes.ToList();
                var subjects = db.Subjects.ToList();
                ViewBag.sections = new SelectList(section, "SectionId", "SectionName");
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                ViewBag.subjects = new SelectList(subjects, "SubjectId", "SubjectName");
                var ExistResocrd = db.ExamResults.Include("Sessions").Where(a => a.ExamTypeId == Type && a.SectionId == Section && a.Sessions.SessionName == currentsession && a.ClassId == Class && a.SubjectId == Subject).FirstOrDefault();
                if (ExistResocrd != null)
                {
                    ViewBag.warning = "Result for this exam  is already submited";
                    return View();
                }
                else
                {
                    var selectSession = db.Sessions.Where(a => a.SessionName == currentsession).FirstOrDefault();
                    //var teacher = db.SubjectMapping.Where(a => a.SessionId == selectSession.SessionId && a.ClassId == Class && a.SectionId == Section && a.SubjectId == Subject).FirstOrDefault();
                    var Teacher = db.Members.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                    if (selectSession == null || Teacher == null)
                    {
                        return RedirectToAction("Error", "Admission");
                    }
                    else
                    {
                        var selectClass = db.Registration.Where(a => a.SessionId == selectSession.SessionId && a.ClassId == Class && a.SectionId == Section && a.Status).ToList();
                        ViewBag.subjectId = Subject;
                        ViewBag.ExamTypeId = Type;
                        ViewBag.list = selectClass;
                        ViewBag.teacherid = Teacher.RegisterId;
                        return View();
                    }
                }


            }
        }
        [HttpPost]
        public ActionResult SubmitResult(List<ExamResult> models)
        {
            try
            {
                using (DB db = new DB())
                {
                    foreach (var item in models)
                    {
                        var s = db.ExamResults.Where(a => a.ExamTypeId == item.ExamTypeId && a.StudentId == item.StudentId && a.SectionId == item.SectionId && a.SessionId == item.SessionId && a.ClassId == item.ClassId && a.SubjectId == item.SubjectId).FirstOrDefault();
                        if (s != null)
                        {
                            TempData["info-msg"] = "Result has already submited";
                            break;
                        }
                        else
                        {
                            item.DateTime = LocalTime.Now().ToLongDateString();
                            var total = item.TotalMarks;
                            var obtainedTotal =Calculate.SumMarks(item.WrittenMarks, item.OralMarks, item.ConceptualMarks);
                            if (obtainedTotal>total)
                            {
                                return Json("Obtained Marks greater than total,please try again");
                            }
                            var percentage = Calculate.CalculcatePercentage(total, obtainedTotal);
                            var Status = Calculate.AssignGrade(percentage);
                            item.ObtainedTotal = obtainedTotal;
                            item.Percentage = percentage.ToString();
                            item.Status = Status;
                            db.ExamResults.Add(item);
                            TempData["info-msg"] = "Result submited successfully...";
                        }
                        var std = db.Registration.Where(a => a.AddmissionNo == item.StudentId).FirstOrDefault();
                        var parent = db.Parent.Where(a => a.ParentId == std.ParentId).FirstOrDefault();
                        var subject = db.Subjects.Where(a => a.SubjectId == item.SubjectId).FirstOrDefault();
                        var exam = db.Examtype.Where(a => a.ExamTypeId == item.ExamTypeId).FirstOrDefault();
                        //send notification to parent
                        hub.Send(parent.CNIC);
                        //setting sms 
                        if (item.SMS_Status)
                        {
                            string number = parent.PhoneNo;
                            string message = exam.ExamName+" Exam Result announced! " + std.StudentName + " Got " + item.ObtainedTotal + " out of " + item.TotalMarks + " in " + subject.SubjectName;
                            sms.Send(number, message);
                        }
                    }
                    db.SaveChanges();

                    return RedirectToAction("Index", "Result");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Admission");
            }
        }

        public ActionResult ViewResult()
        {
            using (DB db = new DB())
            {

                var type = db.Examtype.ToList();
                var session = db.Sessions.ToList();
                ViewBag.type = new SelectList(type, "ExamTypeId", "ExamName");
                ViewBag.sessions = new SelectList(session, "SessionId", "SessionName");
                if (User.IsInRole("Teacher"))
                {
                    LoadAssignedClass();
                }
                else
                {
                    var section = db.Sections.ToList();
                    var classes = db.Classes.ToList();
                    var subjects = db.Subjects.ToList();
                    ViewBag.sections = new SelectList(section, "SectionId", "SectionName");
                    ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                    ViewBag.subjects = new SelectList(subjects, "SubjectId", "SubjectName");
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult ViewResult(int Type, int Class, int Session, int Section, int Subject)
        {
            TempData["ClassId"] = Class;
            TempData["SectionId"] = Section;
            TempData["SubjectId"] = Subject;
            TempData["SessionId"] = Session;
            TempData["Type"] = Type;
            using (DB db = new DB())
            {
                var type = db.Examtype.ToList();
                var session = db.Sessions.ToList();
                ViewBag.type = new SelectList(type, "ExamTypeId", "ExamName");
                ViewBag.sessions = new SelectList(session, "SessionId", "SessionName");
                if (User.IsInRole("Teacher"))
                {
                    LoadAssignedClass();
                }
                else
                {
                    var section = db.Sections.ToList();
                    var classes = db.Classes.ToList();
                    var subjects = db.Subjects.ToList();
                    ViewBag.sections = new SelectList(section, "SectionId", "SectionName");
                    ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                    ViewBag.subjects = new SelectList(subjects, "SubjectId", "SubjectName");
                }
                var Result = db.ExamResults.Include("Students").Where(a => a.ExamTypeId == Type && a.SessionId == Session && a.ClassId == Class && a.SectionId == Section && a.SubjectId == Subject).ToList();
                ViewBag.Result = Result;
            }
            return View();
        }
      

        public ActionResult EditResult(int id)
        {
            using (DB db = new DB())
            {
                var exam = db.Examtype.ToList();
                ViewBag.type = new SelectList(exam, "ExamTypeId", "ExamName");
                var s = db.ExamResults.Find(id);
                return View(s);
            }
        }
        [HttpPost]
        public ActionResult EditResult(ExamResult model)
        {
            using (DB db = new DB())
            {

                var exam = db.Examtype.ToList();
                ViewBag.type = new SelectList(exam, "ExamTypeId", "ExamName");
                var s = db.ExamResults.Where(a => a.ExamResultId == model.ExamResultId).FirstOrDefault();
                var total = s.TotalMarks;
                var obtainedTotal = Calculate.SumMarks(model.WrittenMarks, model.OralMarks, model.ConceptualMarks);
                if (obtainedTotal > total)
                {
                    return Json("Obtained Marks greater than total,please try again");
                }
                s.WrittenMarks = model.WrittenMarks;
                s.OralMarks = model.OralMarks;
                s.ConceptualMarks = model.ConceptualMarks;
                var percentage = Calculate.CalculcatePercentage(total, obtainedTotal);
                var Status = Calculate.AssignGrade(percentage);
                s.ObtainedTotal = obtainedTotal;
                s.TotalMarks = s.TotalMarks;
                s.Percentage = percentage.ToString();
                s.ExamTypeId = model.ExamTypeId;

                s.Status = Status;
                db.SaveChanges();

            }
            return RedirectToAction("ViewResult", "Result");
        }
        public ActionResult ViewExamType()
        {
            using (DB db = new DB())
            {
                return View(db.Examtype.ToList());
            }
        }

        public ActionResult AddEdit(int id = 0)
        {
            if (id == 0)
            {
                return View();
            }
            else
            {
                using (DB db = new DB())
                {
                    var s = db.Examtype.Where(a => a.ExamTypeId == id).FirstOrDefault();
                    return View(s);
                }
            }
        }
        [HttpPost]
        public ActionResult AddEdit(ExamType model)
        {
            using (DB db = new DB())
            {
                if (model.ExamTypeId == 0)
                {
                    db.Examtype.Add(model);
                }
                else
                {
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
            }

            return RedirectToAction("ViewExamType", "Result");
        }
        public ActionResult Delete(int id)
        {
            using (DB db = new DB())
            {
                var s = db.Examtype.Find(id);
                db.Examtype.Remove(s);
                db.SaveChanges();
            }
            return RedirectToAction("ViewExamType", "Result");
        }
        public ActionResult GetGrade()
        {
            using (DB db=new DB())
            {
                var s = db.GradeSetting.FirstOrDefault();
                return Json(s, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult resultmenu()
        {
            return View();
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