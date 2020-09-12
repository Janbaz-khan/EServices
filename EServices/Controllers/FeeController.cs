using EServices.CustomClasses;
using EServices.Models;
using EServices.Models.Admission;
using EServices.Models.Fee;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Controllers
{
    [Authorize(Roles = "Teacher,Admin")]
    public class FeeController : Controller
    {
        // GET: Fee
        SMS sms = new SMS();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult FeeDetails()
        {
            try
            {
 using (DB db = new DB())
            {
                if (User.IsInRole("Teacher"))
                {
                    var list = LoadHeadTeacherStudents();
                    if (list != null)
                    {
                        TempData["list"] = list;
                    }
                }
                else
                {
                    var Classes = db.Classes.ToList();
                    var sections = db.Sections.ToList();
                    ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                    ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                }
                TempData["list"] = TempData["list"];
                return View();
            }
            }
            catch (Exception)
            {

                return RedirectToAction("Error","Admission");
            }
           
        }
        [HttpPost]
        public ActionResult FeeDetails(int Class, int Section)
        {
            // var msg = TempData["success"];

            using (DB db = new DB())
            {

                if (User.IsInRole("Teacher"))
                {
                    LoadAssignedClass();
                }
                else
                {
                    var Classes = db.Classes.ToList();
                    var sections = db.Sections.ToList();
                    ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                    ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                }

                CustomClasses.CurrentSession sesion = new CustomClasses.CurrentSession();
                var s = sesion.Session();
                var session = db.Sessions.Where(a => a.SessionName == s).FirstOrDefault();
                if (session != null)
                {

                    var list = db.Registration.Include("Class").Include("Sessions").Include("Parent").Where(a => a.SessionId == session.SessionId && a.ClassId == Class && a.SectionId == Section && a.Status).ToList();
                    TempData["list"] = list;
                }
                else
                {
                    return RedirectToAction("Error", "Admission");
                }

            }


            return View();
        }
        [HttpGet]
        public ActionResult AddFee(int id)
        {
            using (DB db = new DB())
            {
                var s = db.Registration.Where(a => a.AddmissionNo == id).FirstOrDefault();
                var currentmonth = DateTime.Now.Month;
               // var currentfee = db.FeePayment.Where(a => a.SessionId == s.SessionId && a.AddmissionNo == s.AddmissionNo &&a.ClassId==s.ClassId&&a.SectionId==s.SectionId&& a.MonthId == currentmonth).Count();
                var ExistRecord = db.FeePayment.Include("Months").Where(a => a.AddmissionNo == s.AddmissionNo && a.SectionId == s.SectionId && a.MonthId == currentmonth && a.ClassId == s.ClassId && a.SessionId == s.SessionId).Count();

                if (ExistRecord>0)
                {
                    ViewBag.exist = "Fee already submited for this month";
                }
                var month = db.Month.ToList();
                ViewBag.Months = new SelectList(month, "MonthId", "MonthName");
                if (s != null)
                {
                    ViewBag.StudentName = s.StudentName;
                    ViewBag.id = s.AddmissionNo;
                    ViewBag.classid = s.ClassId;
                    ViewBag.sectionid = s.SectionId;
                    ViewBag.sessionid = s.SessionId;
                    ViewBag.img = s.Image;
                    var GetNormalFee = db.Normalfee.Where(a => a.Status && a.ClassId == s.ClassId).FirstOrDefault();
                    ViewBag.TotalNormalFee = GetNormalFee.TotalNormal;
                    ViewBag.monthly = GetNormalFee.MonthlyFee;
                    ViewBag.admission = GetNormalFee.AdmissionFee;
                    ViewBag.promotion = GetNormalFee.PromotionFee;
                    ViewBag.exam = GetNormalFee.ExamFee;
                    var Arrears = db.FeePayment.Include("Months").OrderByDescending(a => a.FeePaymentId).Where(a => a.AddmissionNo == s.AddmissionNo).FirstOrDefault();
                    if (Arrears != null)
                    {
                        ViewBag.Arrears = Arrears.CarryForward;
                        ViewBag.PrevMonth = Arrears.Months.MonthName;
                    }
                    else
                    {
                        ViewBag.Arrears = 0;
                    }
                }


                return View();
            }
        }
        public ActionResult LoadAllStudentInClass()
        {
            FeePayment feemodel = new FeePayment();
            try
            {
                using (DB db = new DB())
                {
                    if (User.IsInRole("Teacher"))
                    {
                        var list = LoadHeadTeacherStudents();
                        if (list != null)
                        {
                            TempData["list"] = list;
                        }
                    }
                    else
                    {
                        var Classes = db.Classes.ToList();
                        var sections = db.Sections.ToList();
                        ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                        ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                    }
                    TempData["list"] = TempData["list"];
                    return View();
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Admission");
            }

        }
        [HttpPost]
        public ActionResult LoadAllStudentInClass(int Class,int Section)
        {
            try
            {
        using (DB db = new DB())
                {

                    if (User.IsInRole("Teacher"))
                    {
                        LoadAssignedClass();
                    }
                    else
                    {
                        var Classes = db.Classes.ToList();
                        var sections = db.Sections.ToList();
                        ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                        ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                    }

                    CustomClasses.CurrentSession sesion = new CustomClasses.CurrentSession();
                    var s = sesion.Session();
                    if (s != null)
                    {

                        var list = db.Registration.Include("Class").Include("Sessions").Include("Parent").Where(a => a.Sessions.SessionName == s && a.ClassId == Class && a.SectionId == Section && a.Status).ToList();
                        TempData["list"] = list;
                        var single = list.FirstOrDefault();
                        var GetNormalFee = db.Normalfee.Where(a => a.Status && a.ClassId == single.ClassId).FirstOrDefault();
                        ViewBag.TotalNormalFee = GetNormalFee.TotalNormal;
                        ViewBag.monthly = GetNormalFee.MonthlyFee;
                        ViewBag.admission = GetNormalFee.AdmissionFee;
                        ViewBag.promotion = GetNormalFee.PromotionFee;
                       ViewBag.exam = GetNormalFee.ExamFee;
                    
                    }
                    else
                    {
                        return RedirectToAction("Error", "Admission");
                    }

                }
                return View();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                if (msg.Contains("Non-static method requires a target"))
                {
                    ViewBag.error = "Record not found";
                }
                else
                {
                    ViewBag.error = "faild to load the record please check out the error: " + ex.Message;
                }
                return View();
            }
     
       

}
        [HttpPost]
        public ActionResult AddFeeAtOnce(List<StdParentViewModel> models)
        {
            var messagelist = new List<FeeMessage>();
            using (DB db=new DB())
            {
                foreach (var item in models)
                {
                   var ExistRecord = db.FeePayment.Include("Months").Where(a => a.AddmissionNo == item.AddmissionNo && a.SectionId == item.SectionId && a.MonthId == item.MonthId && a.ClassId == item.ClassId && a.SessionId == item.SessionId).FirstOrDefault();
                    var student = db.Registration.Where(a => a.AddmissionNo == item.AddmissionNo).FirstOrDefault();
                    if (ExistRecord != null)
                    {
                        var message = new FeeMessage();
                        message.Status = false;
                        message.Message = ExistRecord.Months.MonthName + " record already exist for " + student.StudentName;
                        messagelist.Add(message);
                        
                    }
                    else
                    {
                        var PreviousRecord = db.FeePayment.Include("Months").Where(a => a.MonthId == item.MonthId - 1 && a.SectionId == item.SectionId && a.AddmissionNo == item.AddmissionNo && a.ClassId == item.ClassId && a.SessionId == item.SessionId).FirstOrDefault();
                        var DecemberRecord = db.FeePayment.Where(a => a.MonthId == item.MonthId + 11 && a.SectionId == item.SectionId && a.AddmissionNo == item.AddmissionNo && a.ClassId == item.ClassId && a.SessionId == item.SessionId).FirstOrDefault();
                        if (PreviousRecord != null)
                        {

                            item.OutStand = PreviousRecord.CarryForward;
                        }

                        else if (DecemberRecord != null)
                        {
                            item.OutStand = DecemberRecord.CarryForward;
                        }
                        var stationary_model = new Stationary_Fee();
                        var Feepayment = new FeePayment();
                        var ZeroStationary = db.Stationaryfee.Where(a => a.TotalStationary == 0).FirstOrDefault();
                        stationary_model.Books = item.Books;
                        stationary_model.Note_Books = item.Note_Books;
                        stationary_model.Homework_Dairy = item.Homework_Dairy;
                        stationary_model.Scarf = item.Scarf;
                        stationary_model.SchoolBag = item.SchoolBag;
                        stationary_model.School_IDCard = item.School_IDCard;
                        stationary_model.Shoes = item.Shoes;
                        stationary_model.socks = item.socks;
                        stationary_model.Cap = item.Cap;
                        stationary_model.Uniform = item.Uniform;
                        stationary_model.Staionary = item.Staionary;
                        stationary_model.TotalStationary = item.Staionary + item.Uniform + item.Cap + item.socks + item.Shoes + item.School_IDCard + item.SchoolBag + item.Scarf + item.Homework_Dairy + item.Note_Books + item.Books;
                        if (stationary_model.TotalStationary == 0)
                        {
                            if (ZeroStationary != null)
                            {
                                Feepayment.Stationary_Fee = ZeroStationary.StationaryFeeID;
                            }
                            else
                            {
                                db.Stationaryfee.Add(stationary_model);
                                Feepayment.Stationary_Fee = stationary_model.StationaryFeeID;
                            }
                        }
                        else
                        {
                            db.Stationaryfee.Add(stationary_model);
                            Feepayment.Stationary_Fee = stationary_model.StationaryFeeID;
                        }
                        var normalfee = db.Normalfee.Where(a => a.ClassId == item.ClassId && a.Status).FirstOrDefault();
                        Feepayment.Normal_Fee = normalfee.NormalFeeID;
                        Feepayment.OutStand = item.OutStand;
                        Feepayment.Paid = 0;
                        Feepayment.Discount = 0;
                        Feepayment.OtherCharges = item.OtherCharges;
                        Feepayment.Fine = item.Fine;
                        Feepayment.Total = stationary_model.TotalStationary + normalfee.TotalNormal + Feepayment.OtherCharges + Feepayment.Fine;
                        Feepayment.NetTotal = Feepayment.Total + Feepayment.OutStand;
                        Feepayment.AddmissionNo = item.AddmissionNo;
                        Feepayment.ClassId = item.ClassId;
                        Feepayment.SectionId = item.SectionId;
                        Feepayment.SessionId = item.SessionId;
                        Feepayment.MonthId = item.MonthId;
                        Feepayment.Date = LocalTime.Now().ToShortDateString();
                        Feepayment.CarryForward = Feepayment.NetTotal - Feepayment.Paid;
                        db.FeePayment.Add(Feepayment);
                        db.SaveChanges();
                        var message = new FeeMessage();
                       
                        if (item.SmsStatus)
                        {
                            var std = db.Registration.Where(a => a.AddmissionNo == item.AddmissionNo).FirstOrDefault();
                            var monthname = db.Month.Where(a => a.MonthId == Feepayment.MonthId).FirstOrDefault();
                            var parent = db.Parent.Where(a => a.ParentId == std.ParentId).FirstOrDefault();
                            string msg = " Fee record successfully submited for " + student.StudentName + ". ";
                            msg += sms.Send(parent.PhoneNo, "Dear " + parent.FatherName + ", Your kid " + std.StudentName + " Fee record for the " + monthname.MonthName + " decalared. Total Fee: " + Feepayment.NetTotal + " with previous month arrears");
                            message.Message = msg;
                            message.Status=true;
                            messagelist.Add(message);
                        }
                        else
                        {
                            message.Message = "Fee record successfully submited for "+student.StudentName;
                            message.Status = true;
                            messagelist.Add(message);
                        }

                    }
                    
                }
                 
            }
            TempData["messagelist"] = messagelist;
            return RedirectToAction("LoadAllStudentInClass","Fee");
        }
        [HttpPost]
        public ActionResult AddFee(StdParentViewModel model)
        {

            using (DB db = new DB())
            {
                var currentmonth = DateTime.Now.Month;
                var month = db.Month.ToList();
                ViewBag.Months = new SelectList(month, "MonthId", "MonthName");

                var ExistRecord = db.FeePayment.Include("Months").Where(a => a.AddmissionNo == model.AddmissionNo && a.SectionId == model.SectionId && a.MonthId == currentmonth && a.ClassId == model.ClassId && a.SessionId == model.SessionId).FirstOrDefault();
                var student = db.Registration.Where(a => a.AddmissionNo == model.AddmissionNo).FirstOrDefault();
                if (ExistRecord != null)
                {
                   
                    return Json(ExistRecord.Months.MonthName + " record already exist for " + student.StudentName, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var PreviousRecord = db.FeePayment.Include("Months").Where(a => a.MonthId == currentmonth - 1 && a.SectionId == model.SectionId && a.AddmissionNo == model.AddmissionNo && a.ClassId == model.ClassId && a.SessionId == model.SessionId).FirstOrDefault();
                    var DecemberRecord = db.FeePayment.Where(a => a.MonthId == currentmonth + 11 && a.SectionId == model.SectionId && a.AddmissionNo == model.AddmissionNo && a.ClassId == model.ClassId && a.SessionId == model.SessionId).FirstOrDefault();
                    if (PreviousRecord != null)
                    {

                        model.OutStand = PreviousRecord.CarryForward;
                    }

                    else if (DecemberRecord != null)
                    {
                        model.OutStand = DecemberRecord.CarryForward;
                    }
                    var stationary_model = new Stationary_Fee();
                    var Feepayment = new FeePayment();
                    var ZeroStationary = db.Stationaryfee.Where(a => a.TotalStationary == 0).FirstOrDefault();
                    stationary_model.Books = model.Books;
                    stationary_model.Note_Books = model.Note_Books;
                    stationary_model.Homework_Dairy = model.Homework_Dairy;
                    stationary_model.Scarf = model.Scarf;
                    stationary_model.SchoolBag = model.SchoolBag;
                    stationary_model.School_IDCard = model.School_IDCard;
                    stationary_model.Shoes = model.Shoes;
                    stationary_model.socks = model.socks;
                    stationary_model.Cap = model.Cap;
                    stationary_model.Uniform = model.Uniform;
                    stationary_model.Staionary = model.Staionary;
                    stationary_model.TotalStationary = model.Staionary + model.Uniform + model.Cap + model.socks + model.Shoes + model.School_IDCard + model.SchoolBag + model.Scarf + model.Homework_Dairy + model.Note_Books + model.Books;
                    if (stationary_model.TotalStationary==0)
                    {
                        if (ZeroStationary!=null)
                        {
                        Feepayment.Stationary_Fee = ZeroStationary.StationaryFeeID;
                        }
                        else
                        {
                            db.Stationaryfee.Add(stationary_model);
                            Feepayment.Stationary_Fee = stationary_model.StationaryFeeID;
                        }
                    }
                    else
                    {
                    db.Stationaryfee.Add(stationary_model);
                    Feepayment.Stationary_Fee = stationary_model.StationaryFeeID;
                    }
                    var normalfee = db.Normalfee.Where(a => a.ClassId == model.ClassId && a.Status).FirstOrDefault();
                    Feepayment.Normal_Fee = normalfee.NormalFeeID;
                    Feepayment.OutStand = model.OutStand;
                    Feepayment.Paid = 0;
                    Feepayment.Discount = 0;
                    Feepayment.OtherCharges = model.OtherCharges;
                    Feepayment.Fine = model.Fine;
                    Feepayment.Total = stationary_model.TotalStationary + normalfee.TotalNormal + Feepayment.OtherCharges + Feepayment.Fine;
                    Feepayment.NetTotal = Feepayment.Total + Feepayment.OutStand;
                    Feepayment.AddmissionNo = model.AddmissionNo;
                    Feepayment.ClassId = model.ClassId;
                    Feepayment.SectionId = model.SectionId;
                    Feepayment.SessionId = model.SessionId;
                    Feepayment.MonthId = currentmonth;
                    Feepayment.Date = LocalTime.Now().ToShortDateString();
                    Feepayment.CarryForward = Feepayment.NetTotal - Feepayment.Paid;
                    db.FeePayment.Add(Feepayment);
                    db.SaveChanges();
                    if (model.SmsStatus)
                    {
                        var std = db.Registration.Where(a => a.AddmissionNo == model.AddmissionNo).FirstOrDefault();
                        var monthname = db.Month.Where(a => a.MonthId == Feepayment.MonthId).FirstOrDefault();
                        var parent = db.Parent.Where(a => a.ParentId == std.ParentId).FirstOrDefault();
                        string msg = " Fee record successfully submited for " + student.StudentName+". ";
                        msg += sms.Send(parent.PhoneNo,"Dear " + parent.FatherName + ", Your kid " + std.StudentName + " Fee record for the " + monthname.MonthName + " decalared. Total Fee: " + Feepayment.NetTotal + " with previous month arrears");
                        return Json(msg, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                    return Json("Fee record successfully submited for " + student.StudentName, JsonRequestBehavior.AllowGet);
                    }
             
                }


            }

        }
        public ActionResult PrintDetail(int id)
        {
            using (DB db=new DB())
            {
                var s = db.FeePayment.Include("Students").Include("Classes").Include("Sections").Include("Sessions").Include("Stationary").Include("Normal").Where(a => a.FeePaymentId == id).FirstOrDefault();
            return View(s);
            }
        }
        public ActionResult ViewDetails()
        {
            try
            {
 using (DB db = new DB())
            {
                var month = db.Month.ToList();
                var Sessions = db.Sessions.ToList();
                ViewBag.Sessions = new SelectList(Sessions, "SessionId", "SessionName");
                ViewBag.Month = new SelectList(month, "MonthId", "MonthName");
                if (User.IsInRole("Teacher"))
                {
                    LoadAssignedClass();
                }
                else
                {
                    var Classes = db.Classes.ToList();
                    var sections = db.Sections.ToList();
                    ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                    ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                }

                return View();
            }
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Admission");
            }
           
        }
        [HttpPost]
        public ActionResult ViewDetails(int Session, int Class, int Month, int Section)
        {

            using (DB db = new DB())
            {
                var month = db.Month.ToList();
                var Sessions = db.Sessions.ToList();
                ViewBag.Sessions = new SelectList(Sessions, "SessionId", "SessionName");
                ViewBag.Month = new SelectList(month, "MonthId", "MonthName");
                if (User.IsInRole("Teacher"))
                {
                    LoadAssignedClass();
                }
                else
                {
                    var Classes = db.Classes.ToList();
                    var sections = db.Sections.ToList();
                    ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                    ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                }
                var s = db.FeePayment.Include("Classes").Include("Sessions").Include("Students").Include("Months").Where(a => a.SessionId == Session && a.ClassId == Class && a.MonthId == Month && a.SectionId == Section).ToList();
                ViewBag.list = s;
                return View();
            }
        }
        //[HttpGet]
        //public ActionResult EditFee(int id)
        //{
        //    using (DB db = new DB())
        //    {
        //        var record = db.FeePayment.Where(a => a.FeePaymentId == id).FirstOrDefault();
        //        return View(record);
        //    }
        //}
        //[HttpPost]
        //public ActionResult EditFee(FeePayment model)
        //{
        //    using (DB db = new DB())
        //    {
        //        var record = db.FeePayment.Where(a => a.FeePaymentId == model.FeePaymentId).FirstOrDefault();
        //        record.MonthlyFee = model.MonthlyFee;
        //        record.AdmissionFee = model.AdmissionFee;
        //        record.PromotionFee = model.PromotionFee;
        //        record.ExamFee = model.ExamFee;
        //        record.StationaryFee = model.StationaryFee;
        //        record.OtherCharges = model.OtherCharges;
        //        record.Total = Convert.ToDecimal(model.MonthlyFee + model.AdmissionFee + record.PromotionFee + model.ExamFee + model.StationaryFee + model.OtherCharges);
        //        record.NetTotal = record.Total + record.OutStand;
        //        record.Paid = model.Paid;
        //        if (record.Paid>record.NetTotal)
        //        {
        //            return Json("Paid field must be less than total amount of fee,try again please", JsonRequestBehavior.AllowGet);
        //        }
        //        record.CarryForward = record.NetTotal - record.Paid;
        //        db.SaveChanges();
        //        return Json("Updated successfully,reload the record to see changes",JsonRequestBehavior.AllowGet);
        //    }
        //}
        public ActionResult GetPaymentRecord(int id)
        {
            using (DB db = new DB())
            {
                var s = db.FeePayment.Include("Students").Include("Classes").Include("Sessions").Include("Months").Where(a => a.FeePaymentId == id).FirstOrDefault();

                return View(s);
            }
        }
        [HttpPost]
        public ActionResult GetPaymentRecord(FeePayment model)
        {
            using (DB db = new DB())
            {
                var record = db.FeePayment.Where(a => a.FeePaymentId == model.FeePaymentId).FirstOrDefault();
                if (record != null)
                {
                    if (model.Paid > record.CarryForward)
                    {
                        return new JsonResult { Data = "Error! your payment exceed... it should be less than the total ammount", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    record.Discount = model.Discount;
                    record.NetTotal = record.NetTotal - model.Discount;
                    record.Paid =  model.Paid;
                    record.CarryForward = record.NetTotal - record.Paid;
                    db.SaveChanges();
                }
            }
            return new JsonResult { Data = "Paid successfully... result will updated  after page refresh", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public ActionResult DeleteFee(int id)
        {
            using (DB db=new DB())
            {
                var s = db.FeePayment.Find(id);
                db.FeePayment.Remove(s);
                db.SaveChanges();
            }
            return Json("Deleted successfully,please refresh the page to see updated record", JsonRequestBehavior.AllowGet);
        }
        public ActionResult Details(int id)
        {
            using (DB db = new DB())
            {
                var s = db.FeePayment.Include("Students").Include("Classes").Include("Sessions").Include("Months").Where(a => a.FeePaymentId == id).FirstOrDefault();

                // var w=db.FeePayment.Where(a=>a.AddmissionNo==s.AddmissionNo)

                return View(s);
            }
        }
        public ActionResult SibDis()
        {
            using (DB db = new DB())
            {
                var s = db.SibDiscount.ToList();
                return View(s);
            }
        }
        public ActionResult AddSibDiscount()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddSibDiscount(SibDiscount model)
        {
            using (DB db = new DB())
            {
                var s = db.SibDiscount.Find(1);
                s.Sib2_Discount = model.Sib2_Discount;
                s.Sib3_Discount = model.Sib3_Discount;
                s.Sib4_Discount = model.Sib4_Discount;
                s.Orphan_Discount = model.Orphan_Discount;
                db.SaveChanges();
            }

            return RedirectToAction("SibDis", "Fee");
        }

        public string currentSession()
        {
            CustomClasses.CurrentSession obj = new CustomClasses.CurrentSession();
            var CurrentSession = obj.Session();
            return CurrentSession;
        }
        public List<StudentRegistrationModel> LoadHeadTeacherStudents()
        {

            var CurrentSession = currentSession();
            using (DB db = new DB())
            {
                var s = db.ClassHeadTeacher.Include("Teacher").Include("Classes").Include("Sections").Include("Sessions").Where(a => a.Teacher.CNIC == User.Identity.Name && a.Sessions.SessionName == CurrentSession).FirstOrDefault();

                Class cls = new Class();
                cls.ClassId = s.ClassId;
                cls.ClassName = s.Classes.ClassName;
                Section2 sec = new Section2();
                sec.SectionId = s.SectionId;
                sec.SectionName = s.Sections.SectionName;

                var students = db.Registration.Include("Parent").Include("Class").Where(a => a.ClassId == cls.ClassId && a.SectionId == sec.SectionId && a.Sessions.SessionName == CurrentSession && a.Status).ToList();
                return students;

            }
        }

        public void LoadAssignedClass()
        {
            var CurrentSession = currentSession();
            using (DB db = new DB())
            {
                var s = db.ClassHeadTeacher.Include("Teacher").Include("Classes").Include("Sections").Include("Sessions").Where(a => a.Teacher.CNIC == User.Identity.Name && a.Sessions.SessionName == CurrentSession).FirstOrDefault();
                var classList = new List<Class>();
                var sectionList = new List<Section2>();
                Class cls = new Class();
                cls.ClassId = s.ClassId;
                cls.ClassName = s.Classes.ClassName;
                Section2 sec = new Section2();
                sec.SectionId = s.SectionId;
                sec.SectionName = s.Sections.SectionName;
                classList.Add(cls);
                sectionList.Add(sec);
                ViewBag.Classes = new SelectList(classList, "ClassId", "ClassName");
                ViewBag.sections = new SelectList(sectionList, "SectionId", "SectionName");
            }
        }
        
    }

}