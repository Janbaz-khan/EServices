using EServices.Models;
using EServices.Models.Staff;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Controllers
{
    [Authorize(Roles = "Admin")]
    public class StaffController : Controller
    {
        // GET: Staff
        public ActionResult Index()
        {
            using (DB db = new DB())
            {
                var s = db.Staff.Include("gender").Where(a => a.Status == true).ToList();
                return View(s);
            }
        }
        public ActionResult AddEmployee()
        {
            using (DB db = new DB())
            {
                var gender = db.Genders.ToList();

                ViewBag.Gender = new SelectList(gender, "GenderId", "GenderName");
            }
            return View();
        }
        [HttpPost]
        public ActionResult AddEmployee(Staff model)
        {

            if (model.ImageFile != null)
            {
                //saving image in folder and then in db
                var Filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                var Extenstion = Path.GetExtension(model.ImageFile.FileName);
                Filename = Filename + DateTime.Now.ToString("yymmssfff") + Extenstion;
                model.Image = "~/AppFolder/Images/" + Filename;
                model.ImageFile.SaveAs(Path.Combine(Server.MapPath("~/AppFolder/Images/"), Filename));
            }
            using (DB db = new DB())
            {
                var s = db.Staff.Where(a => a.CNIC == model.CNIC).FirstOrDefault();
                if (s!=null)
                {
                    return Json("teacher already exist", JsonRequestBehavior.AllowGet);
                }
                var gender = db.Genders.ToList();
                ViewBag.Gender = new SelectList(gender, "GenderId", "GenderName");
                model.Status = true;
                model.HiringDate =CustomClasses.LocalTime.Now().ToLongDateString();
              
                db.Staff.Add(model);
                db.SaveChanges();
            }
            return RedirectToAction("Index", "Staff");
        }
        public ActionResult EditEmployee(int id)
        {
            using (DB db = new DB())
            {
                var gender = db.Genders.ToList();
                ViewBag.Gender = new SelectList(gender, "GenderId", "GenderName");
                var s = db.Staff.Find(id);
                Session["EmpImage"] = s.Image;
                Session["date"] = s.HiringDate;
                Session["status"] = s.Status;
                return View(s);
            }

        }
        [HttpPost]
        public ActionResult EditEmployee(Staff model)
        {
            if (model.ImageFile == null)
            {
                model.Image = Session["EmpImage"].ToString();

            }
            else
            {
                var Filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                var Extenstion = Path.GetExtension(model.ImageFile.FileName);
                Filename = Filename + DateTime.Now.ToString("yymmssfff") + Extenstion;
                model.Image = "~/AppFolder/Images/" + Filename;
                model.ImageFile.SaveAs(Path.Combine(Server.MapPath("~/AppFolder/Images/"), Filename));
            }
            if (Session["status"].ToString().ToLower() == "true")
            {
                model.Status = true;
            }
            else
            {
                model.Status = false;
            }
            model.HiringDate = Session["date"].ToString();
            using (DB db = new DB())
            {
                var gender = db.Genders.ToList();
                ViewBag.Gender = new SelectList(gender, "GenderId", "GenderName");
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

            }
            return RedirectToAction("Index", "Staff");
        }
        public ActionResult Details(int id)
        {
            using (DB db = new DB())
            {
                var s = db.Staff.Include("gender").Where(a => a.EmpId == id).FirstOrDefault();
                return View(s);
            }
        }
    }
}