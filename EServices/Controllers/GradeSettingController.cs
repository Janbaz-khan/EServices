using EServices.Models;
using EServices.Models.GradeSetting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Controllers
{
    public class GradeSettingController : Controller
    {
        // GET: GradeSetting
        public ActionResult Index()
        {

            try
            {
                       var md = new GradeSetting();
                using (DB db=new DB())
                {
                    var s = db.GradeSetting.Find(1);
                    md.PassingMarks = s.PassingMarks;
                    md.Fair_Grade = s.Fair_Grade;
                    md.good_Grade = s.good_Grade;
                    md.Excellent_Grade = s.Excellent_Grade;
                    md.OutStanding_Grade = s.OutStanding_Grade;
                }
                //    md.PassingMarks =Convert.ToDecimal( Request.Cookies["passing"].Value);
                //    md.Fair_Grade =Convert.ToDecimal( Request.Cookies["fair"].Value);
                //    md.good_Grade = Convert.ToDecimal(Request.Cookies["good"].Value);
                //    md.Excellent_Grade =Convert.ToDecimal( Request.Cookies["excellent"].Value);
                //    md.OutStanding_Grade =Convert.ToDecimal( Request.Cookies["outstanding"].Value);
                  return View(md);
                  }
                   catch (Exception)
                   {

                        return Json("no data found,go back and add new data",JsonRequestBehavior.AllowGet);
                   }

            }
        public ActionResult UpdateGrade()
        {
            using (DB db=new DB())
            {
                var s = db.GradeSetting.Find(1);
                if (s!=null)
                {
                    return View(s);
                }
            }
            return View();
        }
        [HttpPost]
        public ActionResult UpdateGrade(GradeSetting model)
        {
            using (DB db=new DB())
            {

                //Response.Cookies.Add(new HttpCookie("passing", model.PassingMarks.ToString()));
                //Request.Cookies["passing"].Expires = DateTime.Now.AddYears(1);
                //Response.Cookies.Add(new HttpCookie("fair", model.Fair_Grade.ToString()));
                //Request.Cookies["fair"].Expires = DateTime.Now.AddYears(1);
                //Response.Cookies.Add(new HttpCookie("good", model.good_Grade.ToString()));
                //Request.Cookies["good"].Expires = DateTime.Now.AddYears(1);
                //Response.Cookies.Add(new HttpCookie("excellent", model.PassingMarks.ToString()));
                //Request.Cookies["excellent"].Expires = DateTime.Now.AddYears(1);
                //Response.Cookies.Add(new HttpCookie("outstanding", model.PassingMarks.ToString()));
                //Request.Cookies["outstanding"].Expires = DateTime.Now.AddYears(1);

                if (db.GradeSetting.Count() == 0)
                {
                    GradeSetting grade = new GradeSetting();
                    grade.id = 1;
                    grade.PassingMarks = 50;
                    grade.Fair_Grade = 60;
                    grade.good_Grade = 70;
                    grade.Excellent_Grade = 80;
                    grade.OutStanding_Grade = 90;
                    db.GradeSetting.Add(grade);
                }
                else
                {
                    model.id = 1;
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                }
                db.SaveChanges();
                return RedirectToAction("Index");
            }
        }
    }
}