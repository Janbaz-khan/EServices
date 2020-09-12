using EServices.Models;
using EServices.Models.ClassMapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Controllers
{
    [Authorize(Roles = "Admin")]
    public class HeadTeacherMappingController : Controller
    {
        // GET: HeadTeacherMapping

        public ActionResult Index()
        {
            using (DB db = new DB())
            {

                var classes = db.Classes.ToList();
                var section = db.Sections.ToList();
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                ViewBag.section = new SelectList(section, "SectionId", "SectionName");
                var s = db.ClassHeadTeacher.Include("Sections").Include("Teacher").Include("Classes").Include("Sessions").ToList();
                ViewBag.list = s;
                return View();
            }
        }

       
        public ActionResult AddMapping()
        {
            using (DB db = new DB())
            {
                var teacher = db.Staff.Where(a => a.Status).ToList();
                var section = db.Sections.ToList();
                var classes = db.Classes.ToList();
                ViewBag.teacher = new SelectList(teacher, "EmpId", "EmpName");
                ViewBag.section = new SelectList(section, "SectionId", "SectionName");
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                return View();
            }
        }
        [HttpPost]
        public ActionResult AddMapping(ClassHeadTeacher model)
        {
            using (DB db = new DB())
            {
                var nextyear = DateTime.Now.AddYears(1).Year;
                var currentyear = DateTime.Now.Year;
                string ses = currentyear + "-" + nextyear;
                var currentSession = db.Sessions.Where(a => a.SessionName == ses).FirstOrDefault();
                var teacher = db.Staff.Where(a => a.Status).ToList();
                var section = db.Sections.ToList();
                var classes = db.Classes.ToList();
                var subject = db.Subjects.ToList();
                ViewBag.teacher = new SelectList(teacher, "EmpId", "EmpName");
                ViewBag.section = new SelectList(section, "SessionId", "SessionName");
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                ViewBag.subject = new SelectList(subject, "SubjectId", "SubjectName");
                model.SessionId = currentSession.SessionId;
                if (db.ClassHeadTeacher.Where(a => a.EmpId == model.EmpId && a.ClassId == model.ClassId && a.SessionId == model.SessionId && a.SectionId == model.SectionId).Count() > 0)
                {
                    return new JsonResult { Data = "exists", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }
                else if (db.ClassHeadTeacher.Where(a => a.ClassId == model.ClassId && a.SessionId == model.SessionId && a.SectionId == model.SectionId).Count() > 0)
                {
                    return new JsonResult { Data = "alreadyAssign", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else if (db.ClassHeadTeacher.Where(a => a.EmpId == model.EmpId && a.SessionId == model.SessionId).Count() >= 1)
                {

                    return new JsonResult { Data = "overLoad", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {

                    db.ClassHeadTeacher.Add(model);
                    db.SaveChanges();
                    return new JsonResult { Data = "success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };

                }

            }

        }
        public ActionResult DeleteMapping(int id)
        {
            using (DB db = new DB())
            {
                var s = db.ClassHeadTeacher.Find(id);
                db.ClassHeadTeacher.Remove(s);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "HeadTeacherMapping");
        }
    }

}