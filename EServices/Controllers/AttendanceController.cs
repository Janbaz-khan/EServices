using EServices.Auth;
using EServices.Models;
using EServices.Models.Attendace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EServices.CustomClasses;
using System.Web.UI;
using EServices.Models.Attendance;

namespace EServices.Controllers
{
    [Authorize(Roles = "Teacher,Admin")]
    public class AttendanceController : Controller
    {
        SMS sms = new SMS();
        // GET: Attendace
        public ActionResult Index()
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
                            ViewBag.list = list;
                        }
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
        public ActionResult Index(int Class, int Section)
        {
            using (DB db = new DB())
            {
                if (User.IsInRole("Admin"))
                {
                    var Classes = db.Classes.ToList();
                    var sections = db.Sections.ToList();
                    ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                    ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                }


                CustomClasses.CurrentSession obj = new CustomClasses.CurrentSession();
                var s = obj.Session();
                var session = db.Sessions.Where(a => a.SessionName == s).FirstOrDefault();

                var list = db.Registration.Include("Class").Include("Sessions").Include("Parent").Where(a => a.SessionId == session.SessionId && a.ClassId == Class && a.SectionId == Section && a.Status).ToList();
                ViewBag.list = list;

            }


            return View();
        }
        [HttpPost]
        public ActionResult SubmitAttendance(List<Attendance> models)
        {
            bool check = true;
            using (DB db = new DB())
            {
                var Classes = db.Classes.ToList();
                var Sessions = db.Sessions.ToList();

                ViewBag.Sessions = new SelectList(Sessions, "SessionId", "SessionName");
                ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                string date = LocalTime.Now().ToShortDateString();
                foreach (var item in models)
                {

                    var s = db.Attendance.Where(a => a.SessionId == item.SessionId && a.SectionId == item.SectionId && a.AddmissionNo == item.AddmissionNo && a.ClassId == item.ClassId && a.Date == date).FirstOrDefault();
                    var std = db.Registration.Where(a => a.AddmissionNo == item.AddmissionNo).FirstOrDefault();
                    var parnt = db.Parent.Where(a => a.ParentId == std.ParentId).FirstOrDefault();
                    if (s != null)
                    {
                        check = false;
                        break;

                        //return new JsonResult { Data = "Attendance already submited for today...", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        check = true;
                        if (item.Status == "Absent")
                        {

                            //send notification if student is absent
                            MyHub hub = new MyHub();
                            hub.Send(parnt.CNIC);
                            //send sms if student is absent

                        }
                        if (item.SMS_Status)
                        {
                            string number = parnt.PhoneNo;
                            string message;
                            if (item.Status == "Leave")
                            {
                                message = "Dear " + parnt.FatherName + " your kid " + std.StudentName + " is on leave Today. Thanks    (developed by janbaz khan)";
                            }
                            else
                            {
                                message = "Dear " + parnt.FatherName + " your kid " + std.StudentName + " is" + item.Status + " Today. Thanks    (developed by janbaz khan)";
                            }
                            sms.Send(number, message);
                        }
                        item.Date = LocalTime.Now().ToShortDateString();
                        item.Time = LocalTime.Now().ToShortTimeString();
                        db.Attendance.Add(item);
                        db.SaveChanges();
                    }

                }
                if (check)
                {
                    return new JsonResult { Data = "success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                else
                {
                    return new JsonResult { Data = "fail", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }

        }
        public ActionResult Details()
        {
            try
            {
                ViewBag.LocalTime = LocalTime.Now().ToShortDateString();
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
                    return View();
                }
            }
            catch (Exception)
            {

                return RedirectToAction("Error", "Admission");
            }
        }
        [HttpPost]
        public ActionResult Details(int Class, int Section, string Date)
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
                var s = db.Attendance.Include("Students").Include("Classes").Where(a => a.ClassId == Class && a.SectionId == Section && a.Date.Contains(Date)).ToList();
                ViewBag.list = s;
                return View();
            }
        }
        public ActionResult Edit(int id)
        {
            using (DB db = new DB())
            {
                var s = db.Attendance.Include("Students").Where(a => a.Attendace_Id == id).FirstOrDefault();
                return View(s);
            }
        }
        [HttpPost]
        public ActionResult Edit(int id,string status)
        {
            using (DB db = new DB())
            {
                var s = db.Attendance.Where(a => a.Attendace_Id == id).FirstOrDefault();
                s.Status = status;
                db.SaveChanges();
                return RedirectToAction("Details");
            }
        }
        public string currentSession()
        {
            CustomClasses.CurrentSession obj = new CustomClasses.CurrentSession();
            var CurrentSession = obj.Session();
            return CurrentSession;
        }
        public void LoadAssignedClass()
        {
            var CurrentSession = currentSession();
            using (DB db = new DB())
            {
                var s = db.ClassHeadTeacher.Include("Teacher").Include("Classes").Include("Sections").Include("Sessions").Where(a => a.Teacher.CNIC == User.Identity.Name && a.Sessions.SessionName == CurrentSession).FirstOrDefault();
                var classList = new List<Class>();
                var sectionList = new List<Section>();
                Class cls = new Class();
                cls.ClassId = s.ClassId;
                cls.ClassName = s.Classes.ClassName;
                Section sec = new Section();
                sec.SectionId = s.SectionId;
                sec.SectionName = s.Sections.SectionName;
                classList.Add(cls);
                sectionList.Add(sec);
                ViewBag.Classes = new SelectList(classList, "ClassId", "ClassName");
                ViewBag.sections = new SelectList(sectionList, "SectionId", "SectionName");
            }
        }

        public ActionResult TeacherAttendace()
        {
            using (DB db = new DB())
            {
                CustomClasses.CurrentSession cs = new CurrentSession();
                string css = cs.Session();
                var sessionid = db.Sessions.Where(a => a.SessionName == css).FirstOrDefault();
                var list = db.Staff.Where(a => a.Status).ToList();
                ViewBag.sessionId = sessionid.SessionId;
                ViewBag.list = list;
            }
            return View();
        }
        public ActionResult SubmitTeacherAttendance(List<TeacherAttendance> models)
        {
            bool check = true;
            using (DB db = new DB())
            {
                string date = LocalTime.Now().ToShortDateString();
                foreach (var item in models)
                {

                    var s = db.TeacherAttendance.Include("Teachers").Where(a => a.SessionId == item.SessionId &&  a.TeacherId == item.TeacherId  && a.Date == date).FirstOrDefault();
                    if (s != null)
                    {
                        check = false;
                        break;

                        //return new JsonResult { Data = "Attendance already submited for today...", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        check = true;
                       
                        item.Date = LocalTime.Now().ToShortDateString();
                        item.Time = LocalTime.Now().ToShortTimeString();
                        db.TeacherAttendance.Add(item);
                        if (item.SMS_Status)
                        {
                            var teacher = db.Staff.Where(a => a.EmpId == item.TeacherId).FirstOrDefault();
                            string number = teacher.PhoneNo;
                            string message;
                            if (item.Status == "Leave")
                            {
                                message ="Dear "+ teacher.EmpName+" You are  on leave Today.  Regards: (The harvard School System Bannu) ";
                            }
                            else
                            {
                                message = "Dear " +teacher.EmpName+" You are "+ item.Status + " Today. Regards: (The harvard School System Bannu) ";
                            }
                            sms.Send(number, message);
                        }
                        db.SaveChanges();
                    }

                }
                if (check)
                {
                    return new JsonResult { Data = "success", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

                else
                {
                    return new JsonResult { Data = "fail", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
        }
        public ActionResult TeacherDetail()
        {
            ViewBag.LocalTime = LocalTime.Now().ToShortDateString();
            return View();
        }
        [HttpPost]
        public ActionResult TeacherDetail(string Date)
        {
            ViewBag.LocalTime = LocalTime.Now().ToShortDateString();
            using (DB db=new DB())
            {
                var teacherlist = db.TeacherAttendance.Include("Teachers").Where(a => a.Date.Contains(Date)).ToList();
                ViewBag.list = teacherlist;
            }
            return View();
        }
        public ActionResult EditTeacher(int id)
        {
                using (DB db = new DB())
                {
                    var s = db.TeacherAttendance.Include("Teachers").Where(a => a.Attendace_Id == id).FirstOrDefault();
                    return View(s);
                }
        }
        [HttpPost]
        public ActionResult EditTeacher(TeacherAttendance model)
        {
            using (DB db = new DB())
            {
                var s = db.TeacherAttendance.Where(a => a.Attendace_Id == model.Attendace_Id).FirstOrDefault();
                s.Status = model.Status;
                db.SaveChanges();
                return Json("Updated Successfully,reload the page to see changes", JsonRequestBehavior.AllowGet);
            }
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
                Section sec = new Section();
                sec.SectionId = s.SectionId;
                sec.SectionName = s.Sections.SectionName;

                var students = db.Registration.Include("Parent").Include("Class").Where(a => a.ClassId == cls.ClassId && a.SectionId == sec.SectionId && a.Sessions.SessionName == CurrentSession && a.Status).ToList();
                return students;

            }
        }
    }

}