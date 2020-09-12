using EServices.Models;
using EServices.Models.Subject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Controllers
{
    [Authorize(Roles = "Admin")]
    public class TeacherMappingController : Controller
    {
        // GET: TeacherMapping
        public ActionResult Index()
        {
            using (DB db=new DB())
            {
               // var session = db.Sessions.ToList();
                var classes = db.Classes.ToList();
                var section = db.Sections.ToList();
                //ViewBag.session = new SelectList(session, "SessionId", "SessionName");
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                ViewBag.section = new SelectList(section, "SectionId", "SectionName");

                return View();
            }
        }
        [HttpPost]
        public ActionResult Index(int Class,int Section)
        {
            using (DB db = new DB())
            {
               // var session = db.Sessions.ToList();
                var classes = db.Classes.ToList();
                var section = db.Sections.ToList();
               // ViewBag.session = new SelectList(session, "SessionId", "SessionName");
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                ViewBag.section = new SelectList(section, "SectionId", "SectionName");
                CustomClasses.CurrentSession obj = new CustomClasses.CurrentSession();
                var s = obj.Session();
                var session = db.Sessions.Where(a => a.SessionName == s).FirstOrDefault();
                if (session!=null)
                {

                var list = db.SubjectMapping.Include("Subjects").Include("Sections").Include("Teacher").Include("Classes").Include("Sessions").Where(a => a.ClassId == Class && a.SessionId == session.SessionId&&a.SectionId==Section).ToList();
                ViewBag.list=list;
                }
                else
                {
                    return RedirectToAction("Error", "Admission");
                }
                return View();
            }
        }
        public ActionResult AddSubject()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddSubject(Subject model)
        {
            using (DB db = new DB())
            {
                db.Subjects.Add(model);
                db.SaveChanges();
            }
            return RedirectToAction("ViewSubjects","TeacherMapping");
        }
        public ActionResult ViewSubjects()
        {
            using (DB db=new DB())
            {
               
            return View(db.Subjects.ToList());
            }
        }
        public ActionResult AddMapping()
        {
            using (DB db=new DB())
            {
                var teacher = db.Staff.Where(a => a.Status).ToList();
                var section = db.Sections.ToList();
                var classes = db.Classes.ToList();
                var subject = db.Subjects.ToList();
                ViewBag.teacher = new SelectList(teacher, "EmpId", "EmpName");
                ViewBag.section = new SelectList(section, "SectionId", "SectionName");
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                ViewBag.subject = new SelectList(subject, "SubjectId", "SubjectName");
                
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddMapping(Subject_Teacher_Mapping model)
        {
            using (DB db = new DB())
            {
                var nextyear = DateTime.Now.AddYears(1).Year;
                var currentyear = DateTime.Now.Year;
                string ses = currentyear + "-" + nextyear;
                var currentSession = db.Sessions.Where(a => a.SessionName == ses).FirstOrDefault();
                if (currentSession!=null)
                {
                model.SessionId =currentSession.SessionId;
                }
                else
                {
                    return new JsonResult { Data = "sessionnull", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                var teacher = db.Staff.Where(a => a.Status).ToList();
                var section = db.Sections.ToList();
                var classes = db.Classes.ToList();
                var subject = db.Subjects.ToList();
                ViewBag.teacher = new SelectList(teacher, "EmpId", "EmpName");
                ViewBag.section = new SelectList(section, "SessionId", "SessionName");
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                ViewBag.subject = new SelectList(subject, "SubjectId", "SubjectName");
                if (db.SubjectMapping.Where(a=>a.EmpId==model.EmpId&&a.ClassId==model.ClassId&&a.SubjectId==model.SubjectId&&a.SessionId==model.SessionId&&a.SectionId==model.SectionId).Count()>0)
                {
                    return new JsonResult { Data = "exists", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                   
                }
                else if(db.SubjectMapping.Where(a=>a.SubjectId==model.SubjectId&&a.ClassId==model.ClassId&&a.SessionId==model.SessionId && a.SectionId == model.SectionId).Count()>0)
                {
                    return new JsonResult { Data = "alreadyAssign", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else if (db.SubjectMapping.Where(a=>a.EmpId==model.EmpId&&a.SessionId==model.SessionId).Count()>=7)
                {

                     return new JsonResult { Data = "overLoad", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                  
                    db.SubjectMapping.Add(model);
                    db.SaveChanges();
                    return new JsonResult { Data = "success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                   
                }
               
            }
            
        }
        public ActionResult DeleteMapping(int id)
        {
            using (DB db=new  DB())
            {
             var s= db.SubjectMapping.Find(id);
                db.SubjectMapping.Remove(s);
                db.SaveChanges();
                return RedirectToAction("Index","TeacherMapping");
            }
        }
        public ActionResult EditSubject(int id)
        {
            using (DB db = new DB())
            {
                var s = db.Subjects.Find(id);
                return View(s);
            }
        }
        [HttpPost]
        public ActionResult EditSubject(Subject model)
        {
            using (DB db = new DB())
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewSubjects","TeacherMapping");
            }
        }
        public ActionResult Menu()
        {
            return View();
        }
    }
}