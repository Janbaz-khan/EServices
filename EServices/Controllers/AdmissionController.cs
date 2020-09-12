using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EServices.Models;
using EServices.SG_Component;
using System.IO;
using EServices.Models.Fee;
using EServices.CustomClasses;

namespace EServices.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdmissionController : Controller
    {
        // GET: Admission
        public ActionResult Index()
        {
            using (DB db = new DB())
            {
                var list = db.Registration.Include("Class").Include("Sections").Include("Parent").Include("Gender").Include("Religion").Where(a => a.Status).ToList();

                return View(list);
            }
        }

        public JsonResult SearchCNIC(string CNIC)
        {
            using (DB db = new DB())
            {
                var record = db.Parent.Where(a => a.CNIC == CNIC).FirstOrDefault();
                if (record != null)
                {

                    return new JsonResult { Data = record, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }


            }
        }
        public JsonResult SearchClassBasedFee(int Class)
        {
            using (DB db = new DB())
            {
                var record = db.Normalfee.Where(a => a.ClassId == Class&&a.Status).FirstOrDefault();
                if (record != null)
                {

                    return new JsonResult { Data = record, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult { Data = "", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }


            }
        }

        public ActionResult AddStudent()
        {
            using (DB db = new DB())
            {
                var Classes = db.Classes.ToList();
                var Sections = db.Sections.ToList();
                var Sessions = db.Sessions.ToList();
                var Gender = db.Genders.ToList();
                var Religion = db.Religions.ToList();
                var month = db.Month.ToList();
                ViewBag.Sessions = new SelectList(Sessions, "SessionId", "SessionName");
                ViewBag.Sections = new SelectList(Sections, "SectionId", "SectionName");
                ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                ViewBag.Gender = new SelectList(Gender, "GenderId", "GenderName");
                ViewBag.Religion = new SelectList(Religion, "ReligionId", "ReligionName");
                ViewBag.Months = new SelectList(month, "MonthId", "MonthName");


            }
            return View();
        }
        [HttpPost]
        public ActionResult AddStudent(StdParentViewModel model)
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
                #region DropDownMenus
                var Classes = db.Classes.ToList();
                var Sections = db.Sections.ToList();
                var Sessions = db.Sessions.ToList();
                var Gender = db.Genders.ToList();
                var Religion = db.Religions.ToList();
                var month = db.Month.ToList();
                ViewBag.Sessions = new SelectList(Sessions, "SessionId", "SessionName");
                ViewBag.Sections = new SelectList(Sections, "SectionId", "SectionName");
                ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                ViewBag.Gender = new SelectList(Gender, "GenderId", "GenderName");
                ViewBag.Religion = new SelectList(Religion, "ReligionId", "ReligionName");
                ViewBag.Months = new SelectList(month, "MonthId", "MonthName");
                #endregion
                //try
                //{
                    var existStudent = db.Registration.Where(a => a.ParentId == model.ParentId && a.StudentName == model.StudentName).FirstOrDefault();
                    if (existStudent == null)
                    {
                        var ParentExist = db.Parent.Where(a => a.CNIC == model.CNIC).FirstOrDefault();


                        if (ParentExist == null)
                        {
                            Parent prnt = new Parent();
                            prnt.FatherName = model.FatherName;
                            prnt.CNIC = model.CNIC;
                            prnt.EmergancyNo = model.EmergancyNo;
                            prnt.PhoneNo = model.PhoneNo;
                            prnt.PostelAddress = model.PostelAddress;
                            db.Parent.Add(prnt);


                            StudentRegistrationModel std = new StudentRegistrationModel();
                            std.StudentName = model.StudentName;
                            std.ParentId = prnt.ParentId;
                            std.DOB = model.DOB;
                            std.Address = model.Address;
                            std.ClassId = model.ClassId;
                            std.SectionId = model.SectionId;
                            std.SessionId = model.SessionId;
                            std.AddmissionDate = model.AddmissionDate;
                            std.ReligionId = model.ReligionId;
                            std.GenderId = model.GenderId;
                            std.Image = model.Image;
                            std.SibCategory = model.SibCategory;
                            db.Registration.Add(std);
                        }
                        else
                        {
                            var s = db.Parent.Where(a => a.CNIC == model.CNIC).FirstOrDefault();
                            StudentRegistrationModel std = new StudentRegistrationModel();
                            std.StudentName = model.StudentName;
                            std.ParentId = s.ParentId;
                            std.DOB = model.DOB;
                            std.Address = model.Address;
                            std.ClassId = model.ClassId;
                            std.SectionId = model.SectionId;
                            std.SessionId = model.SessionId;
                            std.AddmissionDate = model.AddmissionDate;
                            std.ReligionId = model.ReligionId;
                            std.GenderId = model.GenderId;
                            std.Image = model.Image;
                            std.SibCategory = model.SibCategory;
                            db.Registration.Add(std);
                        }
                    if (!model.SmsStatus)
                    {
                    #region FeePayment
                        FeePayment FeeModel = new FeePayment();
                        Stationary_Fee StationaryModel = new Stationary_Fee();
                        NormalFee NormalFee = new NormalFee();
                        var ClassBasedFee = db.Normalfee.Where(a => a.ClassId == model.ClassId && a.Status).FirstOrDefault();
                    FeeModel.ClassId = model.ClassId;
                    FeeModel.SessionId = model.SessionId;
                    FeeModel.SectionId = model.SectionId;
                    FeeModel.MonthId = model.MonthId;
                    FeeModel.Normal_Fee = ClassBasedFee.NormalFeeID;
                        StationaryModel.Books = model.Books;
                        StationaryModel.Note_Books = model.Note_Books;
                        StationaryModel.Homework_Dairy = model.Homework_Dairy;
                        StationaryModel.SchoolBag = model.SchoolBag;
                        StationaryModel.School_IDCard = model.School_IDCard;
                        StationaryModel.Shoes = model.Shoes;
                        StationaryModel.socks = model.socks;
                        StationaryModel.Scarf = model.Scarf;
                        StationaryModel.Uniform = model.Uniform;
                        StationaryModel.Staionary = model.Staionary;
                        StationaryModel.Cap = model.Cap;
                        model.TotalStationary = StationaryModel.Books + StationaryModel.Note_Books + StationaryModel.Homework_Dairy + StationaryModel.SchoolBag + StationaryModel.School_IDCard + StationaryModel.Shoes + StationaryModel.Scarf + StationaryModel.socks + StationaryModel.Uniform + StationaryModel.Cap + StationaryModel.Staionary;
                    StationaryModel.TotalStationary = model.TotalStationary;
                        db.Stationaryfee.Add(StationaryModel);
                        
                        FeeModel.Stationary_Fee = StationaryModel.StationaryFeeID;
                        FeeModel.Fine = 0;
                    FeeModel.Date = LocalTime.Now().ToShortDateString();
                    FeeModel.OtherCharges = model.OtherCharges;
                        var total = ClassBasedFee.TotalNormal + StationaryModel.TotalStationary+model.OtherCharges+model.Fine;
                        FeeModel.Total = total;
                        FeeModel.OutStand = 0;
                    FeeModel.Discount = model.Discount;
                    FeeModel.Paid = model.Paid;
                        FeeModel.NetTotal = model.Total + model.OutStand;
                    FeeModel.NetTotal = FeeModel.NetTotal - model.Discount;
                        FeeModel.CarryForward = model.NetTotal - model.Paid;
                    FeeModel.AddmissionNo = model.AddmissionNo;
                        db.FeePayment.Add(FeeModel);

                        //FeeModel.AdmissionFee = model.AdmissionFee;
                        //applying sib discount on monthly fee
                        //if (model.SibCategory == "Sib-1")
                        //{
                        //    FeeModel.MonthlyFee = model.MonthlyFee;
                        //}
                        //else
                        //{
                        //    SibDisc discount = new CustomClasses.SibDisc();
                        //    var DiscountPercentage = discount.GetSibDiscount(model.SibCategory);
                        //    var result = model.MonthlyFee - (model.MonthlyFee / 100 * DiscountPercentage);
                        //    FeeModel.MonthlyFee = result;
                        //}

                        //FeeModel.PromotionFee = model.PromotionFee;
                        //FeeModel.StationaryFee = model.StationaryFee;
                        //FeeModel.OtherCharges = model.OtherCharges;
                        //FeeModel.MonthId = model.Month;
                        //FeeModel.AddmissionNo = model.AddmissionNo;
                        //FeeModel.ClassId = model.ClassId;
                        //FeeModel.SessionId = model.SessionId;
                        //FeeModel.SectionId = model.SectionId;
                        //FeeModel.ExamFee = model.ExamFee;
                        //FeeModel.Date = model.AddmissionDate;
                        //FeeModel.OutStand = 0;
                        //FeeModel.Total = Convert.ToDecimal(FeeModel.AdmissionFee + FeeModel.MonthlyFee + FeeModel.PromotionFee + FeeModel.StationaryFee + FeeModel.ExamFee);
                        //FeeModel.NetTotal = FeeModel.OutStand + FeeModel.Total;
                        //FeeModel.Paid = model.Paid;
                        //FeeModel.CarryForward = FeeModel.NetTotal - FeeModel.Paid;


                        //db.FeePayment.Add(FeeModel);
                        #endregion
                    }
                        db.SaveChanges();
                    }
                    else
                    {
                        ModelState.Clear();
                        ViewBag.error = "this student already addmited";
                        return View();
                    }


                    return RedirectToAction("Index", "Admission");
              //  }
                //catch (Exception)
                //{
                //    return RedirectToAction("Error", "Admission");
                //}

            }


        }
        public ActionResult EditStudent(int id)
        {
            using (DB db = new DB())
            {


                var Classes = db.Classes.ToList();
                var Sections = db.Sections.ToList();
                var Sessions = db.Sessions.ToList();
                var Gender = db.Genders.ToList();
                var Religion = db.Religions.ToList();
                ViewBag.Sessions = new SelectList(Sessions, "SessionId", "SessionName");
                ViewBag.Sections = new SelectList(Sections, "SectionId", "SectionName");
                ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                ViewBag.Gender = new SelectList(Gender, "GenderId", "GenderName");
                ViewBag.Religion = new SelectList(Religion, "ReligionId", "ReligionName");

                var s = db.Registration.Find(id);
                Session["img"] = s.Image;
                return View(s);
            }
        }
        [HttpPost]
        public ActionResult EditStudent(StudentRegistrationModel model)
        {
            try
            {
                if (model.ImageFile == null)
                {
                    model.Image = Session["img"].ToString();
                }
                else
                {
                    //image setting
                    var Filename = Path.GetFileNameWithoutExtension(model.ImageFile.FileName);
                    var Extenstion = Path.GetExtension(model.ImageFile.FileName);
                    Filename = Filename + DateTime.Now.ToString("yymmssfff") + Extenstion;
                    model.Image = "~/AppFolder/Images/" + Filename;
                    model.ImageFile.SaveAs(Path.Combine(Server.MapPath("~/AppFolder/Images/"), Filename));


                }

                using (DB db = new DB())
                {

                    var Classes = db.Classes.ToList();
                    var Sections = db.Sections.ToList();
                    var Sessions = db.Sessions.ToList();
                    var Gender = db.Genders.ToList();
                    var Religion = db.Religions.ToList();
                    ViewBag.Sessions = new SelectList(Sessions, "SessionId", "SessionName");
                    ViewBag.Sections = new SelectList(Sections, "SectionId", "SectionName");
                    ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                    ViewBag.Gender = new SelectList(Gender, "GenderId", "GenderName");
                    ViewBag.Religion = new SelectList(Religion, "ReligionId", "ReligionName");
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;

                    db.SaveChanges();
                    return RedirectToAction("Index", "Admission");
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Admission");
            }

        }
        public ActionResult EditParent(int id)
        {
            using (DB db = new DB())
            {
                var s = db.Parent.Find(id);
                return View(s);
            }

        }
        [HttpPost]
        public ActionResult EditParent(Parent model)
        {
            try
            {
                using (DB db = new DB())
                {
                    db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "Admission");
            }
            catch (Exception)
            {
                return RedirectToAction("Error", "Admission");
            }
        }
        public ActionResult DeleteStudent(int AddmissionNo)
        {
            using (DB db = new DB())
            {
                var s = db.Registration.Find(AddmissionNo);
                db.Registration.Remove(s);
                db.SaveChanges();
                return RedirectToAction("Index", "Admission");

            }
        }
        public ActionResult StudentDetails(int id)
        {
            using (DB db = new DB())
            {
                var model = db.Registration.Include("Class").Include("Sessions").Include("Sections").Include("Gender").Include("Parent").Include("Religion").Where(a => a.AddmissionNo == id).FirstOrDefault();
                return View(model);
            }
        }
        public ActionResult ParentDetails(int id)
        {
            using (DB db = new DB())
            {
                var s = db.Registration.Where(a => a.AddmissionNo == id).FirstOrDefault();
                var s1 = db.Parent.Where(a => a.ParentId == s.ParentId).FirstOrDefault();
                return View(s1);
            }
        }

        public ActionResult AddClass()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddClass(Class model)
        {

            using (DB db = new DB())
            {
                db.Classes.Add(model);
                db.SaveChanges();
                ViewBag.message = "Class Added Successfully...";
                ModelState.Clear();
            }

            return RedirectToAction("ViewClasses", "Admission");
        }
        public ActionResult ViewClasses()
        {
            using (DB db = new DB())
            {
                var list = db.Classes.ToList();
                return View(list);
            }
        }

        public ActionResult EditClass(int id)
        {
            using (DB db = new DB())
            {
                var v = db.Classes.Find(id);
                return View(v);
            }
        }
        [HttpPost]
        public ActionResult EditClass(Class model)
        {
            using (DB db = new DB())
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewClasses", "Admission");
            }
        }
        public ActionResult DeleteClass(int classId)
        {
            using (DB db = new DB())
            {
                var cls = db.Classes.Find(classId);
                db.Classes.Remove(cls);
                db.SaveChanges();
                return RedirectToAction("ViewClasses", "Admission");
            }

        }
        public ActionResult AddSession()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddSession(Session model)
        {
            using (DB db = new DB())
            {
                db.Sessions.Add(model);
                db.SaveChanges();
                ViewBag.message = "Session Added Successfully...";
                ModelState.Clear();
            }
            return RedirectToAction("ViewSessions", "Admission");
        }
        public ActionResult DeleteSession(int sessionId)
        {
            using (DB db = new DB())
            {
                var v = db.Sessions.Find(sessionId);
                db.Sessions.Remove(v);
                db.SaveChanges();
            }
            return RedirectToAction("ViewSessions", "Admission");
        }
        public ActionResult EditSession(int id)
        {
            using (DB db = new DB())
            {
                var s = db.Sessions.Find(id);

                return View(s);
            }
        }
        [HttpPost]
        public ActionResult EditSession(Session model)
        {
            using (DB db = new DB())
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewSessions", "Admission");
            }

        }
        public ActionResult ViewSessions()
        {
            using (DB db = new DB())
            {
                var list = db.Sessions.ToList();

                return View(list);
            }
        }
        public ActionResult Section()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Section(Section model)
        {
            using (DB db = new DB())
            {
                db.Sections.Add(model);
                db.SaveChanges();
                ViewBag.message = "Section Added Successfully...";
                ModelState.Clear();
            }
            return RedirectToAction("ViewSections", "Admission");
        }
        public ActionResult ViewSections()
        {
            using (DB db = new DB())
            {
                var list = db.Sections.ToList();
                return View(list);
            }
        }
        public ActionResult DeleteSection(int sectionId)
        {
            using (DB db = new DB())
            {
                var s = db.Sections.Find(sectionId);
                db.Sections.Remove(s);
                db.SaveChanges();
            }
            return RedirectToAction("ViewSections", "Admission");
        }
        public ActionResult EditSection(int id)
        {
            using (DB db = new DB())
            {
                var s = db.Sections.Find(id);

                db.SaveChanges();
                return View(s);
            }
        }
        [HttpPost]
        public ActionResult EditSection(Section model)
        {
            using (DB db = new DB())
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ViewSections", "Admission");
            }
        }
        public ActionResult Error()
        {
            return View();
        }
        public ActionResult Menu()
        {
            return View();
        }
    }

}