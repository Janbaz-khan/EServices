using EServices.CustomClasses;
using EServices.Models;
using EServices.Models.Results;
using EServices.Models.TestResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Controllers
{
    [Authorize(Roles = "Admin,Teacher")]
    public class ChartController : Controller
    {
        CustomClasses.CurrentSession obj = new CustomClasses.CurrentSession();
        // GET: Chart
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult TestResult()
        {
            using (DB db = new DB())
            {
                var sections = db.Sections.ToList();
                var classes = db.Classes.ToList();
                var subject = db.Subjects.ToList();
                var month = db.Month.ToList();
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                ViewBag.subjects = new SelectList(subject, "SubjectId", "SubjectName");
                ViewBag.months = new SelectList(month, "MonthId", "MonthName");
            }
            return View();
        }
        public ActionResult ExamResult()
        {
            using (DB db=new DB())
            {
                var type = db.Examtype.ToList();
                //var session = db.Sessions.ToList();
                var section = db.Sections.ToList();
                var classes = db.Classes.ToList();
                ViewBag.type = new SelectList(type, "ExamTypeId", "ExamName");
                //ViewBag.sessions = new SelectList(session, "SessionId", "SessionName");
                ViewBag.sections = new SelectList(section, "SectionId", "SectionName");
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
            }
            return View();
        }
        public ActionResult ExamChart()
        {
            var currentSession = obj.Session();
            using (DB db=new DB())
            {
                var s = new List<ExamResult>();

                if (TempData["ExamType"] == null || TempData["Class2"] == null || TempData["Section2"] == null )
                {
                s = db.ExamResults.Include("Sessions").Include("Students").Where(a => a.ExamTypeId == 1 && a.ClassId == 1 && a.SectionId == 1 && a.Sessions.SessionName == currentSession).ToList();
                }
                else
                {
                    TempData.Keep("ExamType");
                    TempData.Keep("Class2");
                    TempData.Keep("Section2");
                   var examtype=(int)TempData["ExamType"] ;
                   var cls=     (int)  TempData["Class2"];
                   var section= (int) TempData["Section2"] ;
                    s = db.ExamResults.Include("Sessions").Include("Students").Where(a => a.ExamTypeId == examtype &&a.ClassId==cls&& a.Sessions.SessionName == currentSession && a.SectionId == section ).ToList();

                }
                var query = s.GroupBy(a => new { a.Students.StudentName })
                    .Select(grp => new FullExamVM
                    {
                        ObtainedMarks = grp.Sum(a => a.ObtainedTotal),
                        Name = grp.Key.StudentName
                    });
                var list = query.ToList();
                var list2 = new List<vm>();
                foreach (var item in list)
                {
                    var md = new vm();
                    md.Name = item.Name;
                    md.Marks = (int)item.ObtainedMarks;
                    list2.Add(md);
                }
                return new JsonResult {Data= list2,JsonRequestBehavior= JsonRequestBehavior.AllowGet };
            }
        }
        public ActionResult TestChart()
        {
            var currentSession = obj.Session();
            using (DB db = new DB())
            {
                var s = new List<TestResult>();
                var currentMonth = DateTime.Now.Month;
                if (TempData["Class"] == null || TempData["Subject"] == null || TempData["tests"] == null)
                {
                     s = db.TestResults.Include("Students").Include("Sessions").Where(a => a.ClassId == 1 && a.SubjectId == 1&&a.SecionId==1 && a.Sessions.SessionName == currentSession && a.MonthId == currentMonth).ToList();
 }
                else
                {
                    var tests = TempData["tests"].ToString();
                    var Class = (int)TempData["Class"];
                    var subject = (int)TempData["Subject"];
                    // var month = (int)TempData["month"];
                    s = db.TestResults.Include("Students").Include("Sessions").Where(a => a.ClassId == Class && a.Date == tests && a.SubjectId == subject && a.MonthId == currentMonth && a.Sessions.SessionName == currentSession).ToList();
                }

                  var list = new List<vm>();
                if (s != null)
                {
                    foreach (var item in s)
                    {
                        var md = new vm();
                        md.Name = item.Students.StudentName;
                        md.Marks = item.ObtainedMarks;
                        list.Add(md);
                    }
                }

                //var names = s.Select(a => a.Students.StudentName).Distinct();
                return new JsonResult { Data = list, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        public ActionResult LoadTestChart(int Class, int Subject_Id, string tests)
        {
            TempData["tests"] = tests;
            TempData["Class"] = Class;
            TempData["Subject"] = Subject_Id;
            //TempData["month"] = Month;
            return RedirectToAction("Index", "Home");
            //return RedirectToAction("TestResult", "Chart");
        }
        public ActionResult LoadExamChart(int  Type,int Class, int Section)
        {
            TempData["ExamType"] = Type;
            TempData["Class2"] = Class;
            TempData["Section2"] = Section;
            
            //TempData["month"] = Month;
            return RedirectToAction("Index", "Home");
           //return RedirectToAction("ExamResult","Chart");
        }
        public JsonResult FindTests(int clsId,int section=0, int monthId=0,int session=0)
        {
            var currentMonth = DateTime.Now.Month;
            var currentSession = obj.Session();
            using (DB db = new DB())
            {
                var tests=new List<TestResult>();
                if (monthId==0||section==0||session==0)
                {
                tests = db.TestResults.Include("Sessions").Where(a => a.ClassId == clsId &&a.SecionId==1&& a.MonthId == currentMonth && a.Sessions.SessionName == currentSession).ToList();
                }
                else
                {
                 
                tests = db.TestResults.Include("Sessions").Where(a => a.ClassId == clsId &&a.SecionId==section&& a.MonthId == monthId && a.SessionId == session).ToList();
                }
                var dates = new List<String>();
                foreach (var item in tests)
                {
                    if (!dates.Contains(item.Date))
                    {
                        var date = item.Date;
                        dates.Add(date);
                    }

                }
                return Json(dates, JsonRequestBehavior.AllowGet);
            }
        }
    }
    public class vm
    {
        public string Name { get; set; }
        public decimal Marks { get; set; }
    }
}