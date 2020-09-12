using EServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Controllers
{
    public class MemberRegistrationController : Controller
    {
        // GET: MemberRegistration
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult CreateAccount(MemberRegistration model)
        {
            string message="";
            using (DB db = new DB())
            {
                if (model.LoginType == "Parent")
                {
                    var alreadyexist = db.Members.Where(a => a.CNIC == model.CNIC).FirstOrDefault();
                    var Authorized = db.Parent.Where(a => a.CNIC == model.CNIC).FirstOrDefault();
                    if (alreadyexist != null)
                    {
                        message = "you are already registered,click on login to In";
                    }
                    else if (Authorized == null)
                    {
                        message = "you are not authorized , parent record not found,make sure your kids admission proccess completed ,or contact with school addministration";
                    }
                    else
                    {
                        model.Name = Authorized.FatherName;
                        model.RoleId = 3;
                        db.Members.Add(model);
                        db.SaveChanges();
                        TempData["success"] = "Your account is created successfully...Please login here";
                        return RedirectToAction("Login","Login");
                    }
                }
                else
                {
                    var alreadyexist = db.Members.Where(a => a.CNIC == model.CNIC).FirstOrDefault();
                    var Authorized = db.Staff.Where(a => a.CNIC == model.CNIC).FirstOrDefault();
                    if (alreadyexist != null)
                    {
                        message = "you are already registered ,click on login to In";
                    }
                    else if (Authorized == null)
                    {
                        message = "you are not authorized ,your Teacher record not found, please contact with school addministration";
                    }
                    else
                    {
                        model.Name = Authorized.EmpName;
                        model.RoleId = 2;
                        model.Image = Authorized.Image;
                        db.Members.Add(model);
                        db.SaveChanges();
                        TempData["success"] = "Your account is created successfully...Please login here";
                        return RedirectToAction("Login","Login");
                    }
                }

            }
            ViewBag.message = message;
            return View();
        }

       public ActionResult Users()
        {
            using (DB db=new DB())
            {
              
                var users = db.Members.Include("Role").ToList();
                return View(users);

            }
        }
        [HttpPost]
        public ActionResult Users(string List)
        {
            using (DB db=new DB())
            {
                var users = db.Members.Include("Role").Where(a => a.Role.RoleType == List).ToList();
                return View(users);
            }
        }
        public ActionResult BlockAccount(int id)
        {
            using (DB db=new DB())
            {
                var s = db.Members.Find(id);
                if (s.AccountStatus)
                {
                    s.AccountStatus = false;
                }
                else
                {
                    s.AccountStatus = true;
                }
                db.SaveChanges();
                return RedirectToAction("Users", "MemberRegistration");
            }
        }
        public ActionResult MakeAdmin(int id)
        {
            using (DB db = new DB())
            {
                var s = db.Members.Find(id);
                if (s.RoleId==1)
                {
                    s.RoleId = 2;
                }
                else
                {
                    s.RoleId = 1;
                }
                db.SaveChanges();
                return RedirectToAction("Users", "MemberRegistration");
            }
        }

    }
}