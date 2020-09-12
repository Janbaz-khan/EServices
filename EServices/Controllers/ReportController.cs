using EServices.CustomClasses;
using EServices.Models;
using EServices.Models.Results;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Controllers
{
    public class ReportController : Controller
    {
        [Authorize(Roles ="Admin,Teacher")]
        // GET: Report
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult FullResult()
        {
            using (DB db = new DB())
            {
                var type = db.Examtype.ToList();
                var session = db.Sessions.ToList();
                var section = db.Sections.ToList();
                var classes = db.Classes.ToList();
                ViewBag.type = new SelectList(type, "ExamTypeId", "ExamName");
                ViewBag.sessions = new SelectList(session, "SessionId", "SessionName");
                ViewBag.sections = new SelectList(section, "SectionId", "SectionName");
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
            }
            return View();
        }
        [HttpPost]
        public ActionResult FullResult(int Type, int Class, int Session, int Section)
        {
            TempData["ClassId"] = Class;
            TempData["SectionId"] = Section;
            TempData["SessionId"] = Session;
            TempData["Type"] = Type;
            
            using (DB db = new DB())
            {
                var type = db.Examtype.ToList();
                var session = db.Sessions.ToList();
                var section = db.Sections.ToList();
                var classes = db.Classes.ToList();
                ViewBag.type = new SelectList(type, "ExamTypeId", "ExamName");
                ViewBag.sessions = new SelectList(session, "SessionId", "SessionName");
                ViewBag.sections = new SelectList(section, "SectionId", "SectionName");
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");

                var Result = db.ExamResults.Include("Students").Where(a => a.ExamTypeId == Type && a.SessionId == Session && a.ClassId == Class && a.SectionId == Section).ToList();
                var query = Result.GroupBy(row => new { row.Students.StudentName})
                     .Select(grp => new FullExamVM
                     {
                         TotalMarks = grp.Sum(r => r.TotalMarks),
                         ObtainedMarks = grp.Sum(r => r.ObtainedTotal),
                         Name=grp.Key.StudentName
                     });
                var list2 = query.ToList();
                ViewBag.list = list2;
                return View();
            }
        }
    
    }
}