using EServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Controllers
{
    [Authorize(Roles ="Admin")]
    public class LeaveSchoolController : Controller
    {
        // GET: LeaveSchool
        public ActionResult Index()
        {
            using (DB db=new DB())
            {
                var list = db.Registration.Include("Class").Include("Sections").Include("Sessions").Include("Parent").ToList();
                return View(list);
            }
        }
        public ActionResult Leave(int id)
        {
            using (DB db=new DB())
            {
                var search = db.Registration.Find(id);
                if (search.Status)
                {
                search.Status = false;
                }
                else
                {
                    search.Status = true;
                }
                db.SaveChanges();
                return RedirectToAction("Index", "LeaveSchool");
            }
        }
        public ActionResult Teachers()
        {
            using (DB db=new DB())
            {
                var s = db.Staff.Include("gender").ToList();
            return View(s);
            }
        }
        public ActionResult LeaveTeacher(int id)
        {
            using (DB db=new DB())
            {
                var teacher = db.Staff.Find(id);
                if (teacher.Status)
                {
                    teacher.Status = false;
                    //remove class mapping //teacher can engage only one class at a time so it should be one entity
                    var classmapping = db.ClassHeadTeacher.Where(a => a.EmpId == teacher.EmpId).FirstOrDefault();
                    if (classmapping!=null)
                    {
                        db.ClassHeadTeacher.Remove(classmapping);
                    }
                    //remove subject mapping //teacher can teach many subjects so it should be list of subjects
                    var subjectmapping = db.SubjectMapping.Where(a => a.EmpId == teacher.EmpId).ToList();
                    if (subjectmapping!=null)
                    {
                        foreach (var item in subjectmapping)
                        {
                            db.SubjectMapping.Remove(item);
                        }
                    }
                }
                else
                {
                    teacher.Status = true;
                }
                db.SaveChanges();
                return RedirectToAction("Teachers", "LeaveSchool");
            }
        }
    }
}