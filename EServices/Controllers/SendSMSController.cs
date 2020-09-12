using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading;
using System.IO.Ports;
using System.Net;
using EServices.CustomClasses;
using EServices.Models;

namespace EServices.Controllers
{
    public class SendSMSController : Controller
    {
        SMS sms = new SMS();
        // GET: SendSMS
        public ActionResult Index()
        {
            using (DB db=new DB())
            {
                var Classes = db.Classes.ToList();
                var sections = db.Sections.ToList();
                var teacher = db.Staff.Where(a => a.Status).ToList();
                ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
                ViewBag.teachers = new SelectList(teacher, "EmpId", "EmpName");
            }
            return View();
        }
        public ActionResult GetStudents(long Class,long Section)
        {
            CustomClasses.CurrentSession CurrentSession = new CurrentSession();
            var session = CurrentSession.Session();
            using (DB db = new DB())
            {
                var student = db.Registration.Include("Parent").Include("Sessions").Where(a => a.Status && a.ClassId == Class && a.SectionId == Section && a.Sessions.SessionName == session).ToList();
                if (student!=null)
                {
                    var list = new List<StudentParentNames>();
                    foreach (var item in student)
                    {
                        var md = new StudentParentNames();
                        md.Id = item.AddmissionNo;
                        md.Name = item.StudentName + "/" + item.Parent.FatherName;
                        list.Add(md);
                    }
                return Json(list,JsonRequestBehavior.AllowGet);

                }
                else
                {
                return Json("not found", JsonRequestBehavior.AllowGet);
                }
            }
        }
        public ActionResult ReminserFeeSMS(string Message,long Id)
        {
            using (DB db=new DB())
            {
                var fee = db.FeePayment.Find(Id);
                var std = db.Registration.Include("Parent").Where(a => a.AddmissionNo == fee.AddmissionNo).FirstOrDefault();
                string TextMessage = Message;
                string Phone = std.Parent.PhoneNo;
                string msg=sms.Send(Phone, TextMessage);
            return Json(msg,JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SendToAll(string Message)
        {
            using (DB db = new DB())
            {
                var PhoneList = new List<String>();
                var messages = new List<String>();
                var teachers = db.Staff.Where(a => a.Status).ToList();
                var students = db.Registration.Include("Parent").Where(a => a.Status).ToList();
                foreach (var item in teachers)
                {
                    PhoneList.Add(item.PhoneNo);
                }
                foreach (var item in students)
                {
                    PhoneList.Add(item.Parent.PhoneNo);
                }
                foreach (var item in PhoneList)
                {
                    string msg = sms.Send(item, Message);
                    messages.Add(msg);
                }
                return Json(messages, JsonRequestBehavior.AllowGet);

            }
        }
        public ActionResult SendToTeacher(string Message,int TeacherId = 0)
        {
            using (DB db=new DB())
            {
                if (TeacherId == 0)
                {
                    var messages = new List<String>();
                    var teachers = db.Staff.Where(a => a.Status).ToList();
                   
                    foreach (var item in teachers)
                    {
                        string msg = sms.Send(item.PhoneNo, Message);

                        messages.Add(msg+" to:"+item.EmpName+")");
                    }
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var teacher = db.Staff.Where(a => a.EmpId == TeacherId).FirstOrDefault();
                    if (teacher!=null)
                    {
                        var msg = sms.Send(teacher.PhoneNo, Message);
                        return Json(msg+" to "+teacher.EmpName, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json("please try again", JsonRequestBehavior.AllowGet);
        }
        public ActionResult SendToStudent(string Message,int Class=0,int Section=0,long AdmissionNo=0 )
        {
            CustomClasses.CurrentSession CurrentSession = new CurrentSession();
            var messages = new List<String>();
            using (DB db=new DB())
            {
                if (Class==0&&Section==0&&AdmissionNo==0)
                {
                    var students = db.Registration.Include("Parent").Where(a => a.Status).ToList();
                    foreach (var item in students)
                    {
                        string phone = item.Parent.PhoneNo;
                        string msg = sms.Send(phone, Message);
                        messages.Add(msg + " to " + item.Parent.FatherName);
                    }
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                else if(Class!=0&&Section!=0&&AdmissionNo==0)
                {
                    var session = CurrentSession.Session();
                    var students = db.Registration.Include("Parent").Include("Sessions").Where(a => a.Status&&a.ClassId==Class&&a.SectionId==Section&&a.Sessions.SessionName==session).ToList();
                    foreach (var item in students)
                    {
                        string phone = item.Parent.PhoneNo;
                        string msg = sms.Send(phone, Message);
                        messages.Add(msg + " to " + item.Parent.FatherName);
                    }
                    return Json(messages, JsonRequestBehavior.AllowGet);
                }
                else if (AdmissionNo != 0&&Class!=0&Section!=0)
                {
                    var session = CurrentSession.Session();
                    var student = db.Registration.Include("Parent").Include("Sessions").Where(a => a.Status && a.ClassId == Class && a.SectionId == Section && a.Sessions.SessionName == session&&a.AddmissionNo==AdmissionNo).FirstOrDefault();

                    if (student != null)
                    {
                        var msg = sms.Send(student.Parent.PhoneNo, Message);
                        return Json(msg + " to " + student.Parent.FatherName, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            return Json("please try again", JsonRequestBehavior.AllowGet);
        }


        #region comment
        //public ActionResult Test()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult Connect(string port)
        //{
        //    Session["port"] = port;
        //    return new JsonResult { Data = "port saved", JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
        #endregion
    }
    public class StudentParentNames
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}