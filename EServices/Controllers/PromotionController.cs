using EServices.CustomClasses;
using EServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Controllers
{
    [Authorize(Roles ="Admin")]
    public class PromotionController : Controller
    {
        CurrentSession SessionObj = new CurrentSession();
        // GET: Promotion
        public ActionResult Index()
        {
            using (DB db = new DB())
            {
                var Classes = db.Classes.ToList();
                var sections = db.Sections.ToList();
                var sessions = db.Sessions.ToList();
                ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                ViewBag.sessions = new SelectList(sessions, "SessionId", "SessionName");
            }
            return View();
        }
        [HttpPost]
        public ActionResult Index(int Class, int Section,int Session, int PClass, int PSection,int PSession)
        {
            using (DB db = new DB())
            {
                var Classes = db.Classes.ToList();
                var sections = db.Sections.ToList();
                var sessions = db.Sessions.ToList();
                ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                ViewBag.sessions = new SelectList(sessions, "SessionId", "SessionName");
                TempData["PClass"] = PClass;
                TempData["PSection"] = PSection;
                TempData["PSession"] = PSession;
                var students = db.Registration.Include("Parent").Include("Class").Include("Sections").Include("Sessions").Where(a => a.SessionId == Session && a.ClassId == Class && a.SectionId == Section&&a.Status).ToList();
                ViewBag.list = students;
            }
            return View();
        }
        [HttpPost]
        public ActionResult SubmitPromotion(List<PromoteVM> list)
        {
            var PClass = TempData["PClass"];
            var PSection = TempData["PSection"];
            var pSession = TempData["PSession"];

            try
            {
                using (DB db = new DB())
                {
                    foreach (var item in list)
                    {
                        var std = db.Registration.Find(item.AdmissionNo);
                        if (item.IsPromoted)
                        {
                            std.ClassId = (int)PClass;
                            std.SectionId = (int)PSection;
                            std.SessionId = (int)pSession;
                        }
                        else
                        {
                            std.SessionId = (int)pSession;
                        }
                    }
                        db.SaveChanges();
                }
                TempData["success"] = "Successfully promotred...";
                return RedirectToAction("Index", "Promotion");
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Admission");
            }

        }

    }
}