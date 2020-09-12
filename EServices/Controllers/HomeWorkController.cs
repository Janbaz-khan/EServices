using EServices.Models;
using EServices.Models.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EServices.CustomClasses;

namespace EServices.Controllers
{
    [Authorize(Roles = "Teacher,Admin")]
    public class HomeWorkController : Controller
    {
        MyHub hub = new MyHub();
        SMS sms = new SMS();
        // GET: HomeWork

        public ActionResult Index(int id = 0)
        {
            using (DB db = new DB())
            {
                var type = db.HomeWorkType.ToList();
                ViewBag.type = new SelectList(type, "HomeWorkTypeId", "HomeWorkName");
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
                if (id != 0)
                {
                    var existRecord = db.HomeWork.Find(id);
                    return View(existRecord);
                }
            }

            return View();
        }
        [HttpPost]
        public ActionResult Index(HomeWork model, int Section = 0, int Subject = 0)
        {
            CustomClasses.CurrentSession obj = new CustomClasses.CurrentSession();
            var currentSession = obj.Session();
            using (DB db = new DB())
            {
                var type = db.HomeWorkType.ToList();
                ViewBag.type = new SelectList(type, "HomeWorkTypeId", "HomeWorkName");
                if (User.IsInRole("Teacher"))
                {
                    LoadAssignedClass();
                }
                else
                {

                    var sections = db.Sections.ToList();
                    var classes = db.Classes.ToList();
                    var subjects = db.Subjects.ToList();
                    ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                    ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                    ViewBag.subjects = new SelectList(subjects, "SubjectId", "SubjectName");

                }
                try
                {
                    var CurrentSession = db.Sessions.Where(a => a.SessionName == currentSession).FirstOrDefault();
                    model.SessionId = CurrentSession.SessionId;
                    model.Date = LocalTime.Now().ToShortDateString();
                    var Teacher = db.Members.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                    model.TeacherId = Teacher.RegisterId;
                    if (Section != 0)
                    {
                        model.SecionId = Section;
                    }

                    if (Subject != 0)
                    {
                        model.SubjectId = Subject;
                    }
                    if (model.HomeWorkId != 0)
                    {
                        db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                        ViewBag.message = "Updated successfully...";
                    }
                    else
                    {
                        db.HomeWork.Add(model);
                        ViewBag.message = "Submited successfully...";
                    }
                    db.SaveChanges();
                    var std = db.Registration.Where(a => a.ClassId == model.ClassId && a.SessionId == CurrentSession.SessionId && a.Status).FirstOrDefault();
                    var parent = db.Parent.Where(a => a.ParentId == std.ParentId).FirstOrDefault();
                    //send notification to parent
                    hub.Send(parent.CNIC);
                    ModelState.Clear();

                    //sms setting 
                    if (model.SMS_Status)
                    {
                   var stds = db.Registration.Include("Class").Where(a => a.ClassId == model.ClassId && a.SessionId == CurrentSession.SessionId && a.Status).ToList();
                    var homeworkType = db.HomeWorkType.Where(a => a.HomeWorkTypeId == model.HomeWorkTypeId).FirstOrDefault();
                    foreach (var item in stds)
                    {
                        var prnt = db.Parent.Where(a => a.ParentId == item.ParentId).FirstOrDefault();
                        string message = homeworkType.HomeWorkName + " Announced " + "class:" + item.Class.ClassName+" Detail:"+model.Content;
                        string number = prnt.PhoneNo;
                        sms.Send(number, message);

                    }
                    }
                 
                }
                catch
                {
                    return RedirectToAction("Error", "Admission");
                }

            }
            return View();
        }
        public ActionResult HomeWork()
        {
            using (DB db = new DB())
            {

                return View(db.HomeWorkType.ToList());
            }
        }
        public ActionResult Show()
        {
            using (DB db = new DB())
            {
                var classes = db.Classes.ToList();
                var session = db.Sessions.ToList();
                ViewBag.session = new SelectList(session, "SessionId", "SessionName");
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                var teacher = db.Members.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                var Session = new CustomClasses.CurrentSession();
                var CurrentSession = Session.Session();
                if (User.IsInRole("Teacher"))
                {
                    //i mapp home work table with member beacuase member table contain both teacher and admins
                    var list = db.HomeWork.Include("Sessions").Include("Sections").Include("Classes").Include("Subjects").Include("Staff").Include("HomeWorks").Where(a => a.TeacherId == teacher.RegisterId && a.Sessions.SessionName == CurrentSession).ToList();
                    TempData["list"] = list;
                    return View();
                }
                else
                {
                    var list = db.HomeWork.Include("Sessions").Include("Sections").Include("Classes").Include("Subjects").Include("Staff").Include("HomeWorks").Where(a => a.Sessions.SessionName == CurrentSession).ToList();
                    TempData["list"] = list;
                    return View();
                }
            }
        }
        [HttpPost]
        public ActionResult Show(int Session, int Class)
        {
            using (DB db = new DB())
            {
                var classes = db.Classes.ToList();
                var session = db.Sessions.ToList();
                ViewBag.session = new SelectList(session, "SessionId", "SessionName");
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                var list = db.HomeWork.Include("Sessions").Include("Sections").Include("Classes").Include("Subjects").Include("Staff").Include("HomeWorks").Where(a => a.SessionId == Session && a.ClassId == Class).ToList();
                TempData["list"] = list;
                return View();
            }
        }

        public void LoadAssignedClass()
        {
            List<Class> ClassList = new List<Class>();
            List<Section> sectionList = new List<Section>();
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