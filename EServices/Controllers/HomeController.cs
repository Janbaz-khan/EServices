using EServices.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Controllers
{

    public class HomeController : Controller
    {
        [Authorize(Roles = "Admin,Teacher")]
        // GET: Home

        public ActionResult Index()
        {
            using (DB db = new DB())
            {

                ViewBag.std = db.Registration.Where(a => a.Status).Count();
                ViewBag.teacher = db.Staff.Where(a => a.Status).Count();

            }
            return View();
        }
        public ActionResult Default()
        {
            if (User.IsInRole("Admin") || User.IsInRole("Teacher"))
            {
                return RedirectToAction("Index", "Home", new { area = "" });
            }
            else if (User.IsInRole("Parent"))
            {
                return RedirectToAction("Index", "Home", new { area = "Parent" });
            }
            else
            {
                return View();

            }
        }

        public ActionResult Features()
        {
            return View();
        }
        [Authorize(Roles = "Admin,Teacher")]
        public ActionResult profile(string cnic)
        {
            using (DB db = new DB())
            {
                var teacher = db.Staff.Where(a => a.CNIC == cnic).FirstOrDefault();
                if (teacher != null)
                {
                    return View(teacher);
                }
                return RedirectToAction("Index", "Home", new { area = "" });
            }
        }
        [Authorize(Roles = "Admin,Teacher")]

        public ActionResult userName()
        {
            if (User.Identity.IsAuthenticated)
            {

                using (DB db = new DB())
                {
                    var s = db.Members.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                    if (s != null)
                    {
                        TempData["user-name"] = s.Name;
                        TempData["user-img"] = s.Image;
                    }
                    //else
                    //{
                    //    var Parent = db.Parent.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                    //    TempData["user-name"] = Parent.CNIC;
                    //    TempData["user-img"] = "~/AppFolder/Images/std.png";
                    //}

                }
            }
            return View();
        }
        public ActionResult userEmail()
        {
            if (User.Identity.IsAuthenticated)
            {

                using (DB db = new DB())
                {
                    var s = db.Members.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                    if (s != null)
                    {
                        TempData["user-name"] = s.Name;
                        TempData["user-img"] = s.Image;
                        TempData["user-email"] = s.Email;
                    }
                    //else
                    //{
                    //    var Parent = db.Parent.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                    //    TempData["user-name"] = Parent.CNIC;
                    //    TempData["user-img"] = "~/AppFolder/Images/std.png";
                    //}

                }
            }
            return View();
        }
        [Authorize(Roles = "Admin,Teacher")]
        public ActionResult userImage()
        {
            if (User.Identity.IsAuthenticated)
            {

                using (DB db = new DB())
                {
                    var s = db.Members.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                    if (s != null)
                    {
                        TempData["user-img"] = s.Image;
                    }

                }
            }
            return View();
        }
        [Authorize(Roles = "Admin,Teacher")]
        public ActionResult HeadTeacher()
        {

            return View();
        }
        [Authorize(Roles = "Admin,Teacher")]
        public ActionResult EditProfile()
        {
            if (User.Identity.IsAuthenticated)
            {
                using (DB db = new DB())
                {
                    var s = db.Members.Where(a => a.CNIC == User.Identity.Name).FirstOrDefault();
                    Session["img"] = s.Image;
                    return View(s);
                }
            }
            else
            {
                return RedirectToAction("Error", "Admission");
            }


        }
        [Authorize(Roles = "Admin,Teacher")]
        [HttpPost]
        public ActionResult EditProfile(MemberRegistration model)
        {
            using (DB db = new DB())
            {
                var match = db.Members.Where(a => a.RegisterId == model.RegisterId).FirstOrDefault();
                if (model.ImageFile == null)
                {
                    match.Image = Session["img"].ToString();
                }
                else
                {

                    var Filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    var Externtion = Path.GetExtension(model.ImageFile.FileName);
                    Filename = Filename + DateTime.Now.ToString("yymmssfff") + Externtion;
                    model.Image = "~/AppFolder/Images/" + Filename;
                    model.ImageFile.SaveAs(Path.Combine(Server.MapPath("~/AppFolder/Images/"), Filename));
                    match.Image = model.Image;
                }

                match.Name = model.Name;
                match.CNIC = model.CNIC;
                match.Email = model.Email;
                match.RoleId = model.RoleId;
                match.Password = model.Password;
                db.SaveChanges();
                Session["img"] = match.Image;
                return View(match);
            }
        }
    }

}