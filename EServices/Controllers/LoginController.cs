using EServices.Models;
using EServices.Models.Logins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net;
using System.Net.Mail;
using System.Web.Helpers;

namespace EServices.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Login model, string returnurl)
        {
            if (ModelState.IsValid)
            {
                using (DB db = new DB())
                {
                    var member = db.Members.Where(a => a.CNIC == model.CNIC && a.Password == model.Password).FirstOrDefault();
                    if (member != null)
                    {
                        if (!member.AccountStatus)
                        {
                            ViewBag.message = " your account has been blocked please consult the admin";
                            return View();
                        }
                        var timeOut = model.RememberMe ? 3441 : 1;
                        var Ticket = new FormsAuthenticationTicket(model.CNIC, model.RememberMe, timeOut);
                        string Encrypt = FormsAuthentication.Encrypt(Ticket);
                        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, Encrypt);
                        cookie.Expires = DateTime.Now.AddHours(timeOut);
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        if (member.RoleId == 3)
                        {
                            return RedirectToAction("Index", "Home", new { area = "Parent" });
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }

                    }
                    else
                    {
                        ViewBag.message = "Invalid Credential";
                        return View();
                    }
                }
            }
            return View();
        }
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login", "Login");
        }
        [HttpPost]
        public ActionResult SendPasswordToEmail(string txtEmail)
        {
            try
            {
                using (DB db=new DB())
            {
                var s = db.Members.Where(a => a.Email == txtEmail).FirstOrDefault();
                if (s!=null)
                {
                    var subject = "The Havard School System. Forgot Password";
                    var body = "<h4>Dear "+s.Name+ " your login detail are below,</h4> <p>Your CNIC: "+s.CNIC+ "<br>Your Password: " + s.Password + "</p><p>Please dont share your login detail with other</p>"+
                        "<p>Thanks for using our services.<br>Regards: The Harvard School System</p>";
            WebMail.Send(txtEmail,subject,body, null,
                                         null,
                                         null,
                                         true, 
                                         null,
                                         null,
                                         null,
                                         null,
                                         null,
                                         null);
            TempData["success"] = "we sent your login details to your Email Address,please check your Email Account ,check spam folder if you not see in inbox";
                }
                else
                {
                    TempData["error"] = "your email address dont match with your account";
                }
            }
                return RedirectToAction("Login");
            }
            catch (Exception)
            {

                TempData["error"] = "could not sent an email,please check internet or check your email address carefully";
                return RedirectToAction("Login");
            }
          

            
        }
    }
}