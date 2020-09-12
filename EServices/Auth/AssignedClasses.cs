using EServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace EServices.Auth
{
    [Authorize(Roles ="Teacher,Admin")]
    public  class AssignedClasses:Controller
    {
        public void  LoadAssignedClass()
        {
            List<Class> ClassList = new List<Class>();
            List<Section> sectionList = new List<Section>();
            CustomClasses.CurrentSession obj = new CustomClasses.CurrentSession();
            var CurrentSession = obj.Session();
            using (DB db = new DB())
            {


                var teacherMapping = db.SubjectMapping.Include("Sections").Include("Sessions").Include("Classes").Include("Teacher").Where(a => a.Teacher.CNIC == User.Identity.Name && a.Sessions.SessionName == CurrentSession).ToList();

                if (teacherMapping != null)
                {

                    foreach (var item in teacherMapping)
                    {
                        Class cls = new Class();
                        // Section sec = new Section();
                        cls.ClassId = item.ClassId;
                        cls.ClassName = item.Classes.ClassName;
                        //sec.SectionId = item.SectionId;
                        //sec.SectionName = item.Sections.SectionName;
                        var DuplicateClass = ClassList.Where(a => a.ClassName == cls.ClassName).FirstOrDefault();
                        //var DuplicateSection = sectionList.Where(a => a.SectionName == sec.SectionName).FirstOrDefault();
                        if (DuplicateClass == null)
                        {
                            ClassList.Add(cls);
                        }
                        //if (DuplicateSection == null)
                        //{
                        //    sectionList.Add(sec);
                        //}
                    }
                }
                var Classes = ClassList.ToList();
                //var sections = sectionList.ToList();

                ViewBag.Classes = new SelectList(Classes, "ClassId", "ClassName");
                //ViewBag.sections = new SelectList(sections, "SectionId", "SectionName");
            }
        }
    }
}