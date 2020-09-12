using EServices.Models;
using EServices.Models.Fee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Controllers
{
    public class FeeSettingController : Controller
    {
        // GET: FeeSetting
        public ActionResult Index()
        {
            using (DB db = new DB())
            {
                var s = db.Normalfee.Include("Class").Where(a => a.Status).ToList();

                return View(s);
            }
        }
        public ActionResult SetNormalFee()
        {
            using (DB db = new DB())
            {
                var classes = db.Classes.ToList();
                ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                return View();
            }
        }
        [HttpPost]
        public ActionResult SetNormalFee(NormalFee model)
        {
            try
            {
                using (DB db = new DB())
                {
                    var classes = db.Classes.ToList();
                    ViewBag.classes = new SelectList(classes, "ClassId", "ClassName");
                    if (model.ClassId == null)
                    {
                        return Json("class field required", JsonRequestBehavior.AllowGet);
                    }
                    if (db.Normalfee.Where(a => a.ClassId == model.ClassId && a.Status).Count() > 0)
                    {
                        return Json("Record already exist", JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        var total = model.MonthlyFee + model.AdmissionFee + model.ExamFee + model.PromotionFee;
                        model.TotalNormal = total;
                        db.Normalfee.Add(model);
                        db.SaveChanges();
                        return Json("Submited Successfully...", JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (Exception)
            {

                return Json("something went wrong ,please try again");
            }

        }
        public ActionResult EditNormalFee(int id)
        {
            using (DB db = new DB())
            {
                var s = db.Normalfee.Find(id);
                return View(s);
            }
        }
        [HttpPost]
        public ActionResult EditNormalFee(NormalFee model)
        {
            try
            {
                using (DB db = new DB())
                {
                    var total = model.MonthlyFee + model.AdmissionFee + model.ExamFee + model.PromotionFee;
                    model.TotalNormal = total;
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    return Json("Updated successfullt,reload the page to see changes",JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {

                return Json("Something went wrong,please try again later",JsonRequestBehavior.AllowGet);
            }

        }
        public ActionResult DisableReocrd(int id)
        {
            using (DB db=new DB())
            {
                var s = db.Normalfee.Find(id);
                s.Status = false;
                db.SaveChanges();
                return Json("success", JsonRequestBehavior.AllowGet);
            }
        }

    }
}