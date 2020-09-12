using EServices.Models;
using EServices.Models.Fee;
using EServices.Models.GradeSetting;
using EServices.Models.Logins;
using EServices.Models.Results;
using EServices.Models.Subject;
using EServices_1._0.App_Start;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace EServices
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string conn = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;
        protected void Application_Start()
        {
            using (DB db = new DB())
            {
                if (db.Sessions.Count() == 0)
                {
                    var currentyear = DateTime.Now.Year;
                    var nextyear = DateTime.Now.AddYears(1).Year;
                    Session session = new Session();
                    session.SessionId = 1;
                    session.SessionName = currentyear + "-" + nextyear;
                    db.Sessions.Add(session);


                }
                if (db.Genders.Count() == 0)
                {
                    Gender gender = new Gender();
                    Gender gender2 = new Gender();
                    Gender gender3 = new Gender();

                    gender.GenderId = 1;
                    gender.GenderName = "Male";
                    gender2.GenderId = 2;
                    gender2.GenderName = "Female";
                    gender3.GenderId = 3;
                    gender3.GenderName = "Other";
                    db.Genders.Add(gender);
                    db.Genders.Add(gender2);
                    db.Genders.Add(gender3);
                }
                if (db.Month.Count() == 0)
                {
                    Month month = new Month();
                    Month month2 = new Month();
                    Month month3 = new Month();
                    Month month4 = new Month();
                    Month month5 = new Month();
                    Month month6 = new Month();
                    Month month7 = new Month();
                    Month month8 = new Month();
                    Month month9 = new Month();
                    Month month10 = new Month();
                    Month month11 = new Month();
                    Month month12 = new Month();


                    month.MonthId = 1;
                    month.MonthName = "January";
                    // month.IsStartMonth = false;
                    month2.MonthId = 2;
                    month2.MonthName = "February";
                    // month.IsStartMonth = false;

                    month3.MonthId = 3;
                    month3.MonthName = "March";
                    //month.IsStartMonth = false;

                    month4.MonthId = 4;
                    month4.MonthName = "April";
                    //month.IsStartMonth = false;

                    month5.MonthId = 5;
                    month5.MonthName = "May";
                    // month.IsStartMonth = false;

                    month6.MonthId = 6;
                    month6.MonthName = "June";
                    //month.IsStartMonth = false;

                    month7.MonthId = 7;
                    month7.MonthName = "July";
                    // month.IsStartMonth = false;

                    month8.MonthId = 8;
                    month8.MonthName = "August";
                    // month.IsStartMonth = false;

                    month9.MonthId = 9;
                    month9.MonthName = "September";
                    //month.IsStartMonth = false;

                    month10.MonthId = 10;
                    month10.MonthName = "October";
                    // month.IsStartMonth = false;

                    month11.MonthId = 11;
                    month11.MonthName = "November";
                    //month.IsStartMonth = false;

                    month12.MonthId = 12;
                    month12.MonthName = "December";
                    // month.IsStartMonth = false;

                    db.Month.Add(month);
                    db.Month.Add(month2);
                    db.Month.Add(month3);
                    db.Month.Add(month4);
                    db.Month.Add(month5);
                    db.Month.Add(month6);
                    db.Month.Add(month7);
                    db.Month.Add(month8);
                    db.Month.Add(month9);
                    db.Month.Add(month10);
                    db.Month.Add(month11);
                    db.Month.Add(month12);

                }
                if (db.Religions.Count() == 0)
                {
                    Religion religion1 = new Religion();
                    Religion religion2 = new Religion();
                    Religion religion3 = new Religion();
                    Religion religion4 = new Religion();


                    religion1.ReligionId = 1;
                    religion1.ReligionName = "Islam";
                    religion2.ReligionId = 2;
                    religion2.ReligionName = "Hindo";
                    religion3.ReligionId = 3;
                    religion3.ReligionName = "Cristian";
                    religion4.ReligionId = 4;
                    religion4.ReligionName = "Others";
                    db.Religions.Add(religion1);
                    db.Religions.Add(religion2);
                    db.Religions.Add(religion3);
                    db.Religions.Add(religion4);
                }
                if (db.Classes.Count() == 0)
                {
                    Class cls1 = new Class();
                    Class cls2 = new Class();
                    Class cls3 = new Class();
                    Class cls4 = new Class();


                    cls1.ClassId = 1;
                    cls1.ClassName = "Nursary";
                    cls2.ClassId = 2;
                    cls2.ClassName = "Prep";
                    cls3.ClassId = 3;
                    cls3.ClassName = "One";
                    cls4.ClassId = 4;
                    cls4.ClassName = "Two";
                    db.Classes.Add(cls1);
                    db.Classes.Add(cls2);
                    db.Classes.Add(cls3);
                    db.Classes.Add(cls4);
                }
                if (db.Subjects.Count() == 0)
                {
                    Subject subject1 = new Subject();
                    Subject subject2 = new Subject();
                    Subject subject3 = new Subject();
                    Subject subject4 = new Subject();


                    subject1.SubjectId = 1;
                    subject1.SubjectName = "English";
                    subject2.SubjectId = 2;
                    subject2.SubjectName = "Maths";
                    subject3.SubjectId = 3;
                    subject3.SubjectName = "Science";
                    subject4.SubjectId = 4;
                    subject4.SubjectName = "Urdu";
                    db.Subjects.Add(subject1);
                    db.Subjects.Add(subject2);
                    db.Subjects.Add(subject3);
                    db.Subjects.Add(subject4);
                }
                if (db.Sections.Count() == 0)
                {
                    Section sec1 = new Section();
                    Section sec2 = new Section();
                    sec1.SectionId = 1;
                    sec1.SectionName = "A";
                    sec2.SectionId = 2;
                    sec2.SectionName = "B";
                    db.Sections.Add(sec1);
                    db.Sections.Add(sec2);
                    
                }
               
                if (db.Examtype.Count()==0)
                {
                    ExamType model1 = new ExamType();
                    ExamType model2 = new ExamType();
                    model1.ExamTypeId = 1;
                    model1.ExamName = "Mid Term";
                    model2.ExamTypeId = 2;
                    model2.ExamName = "Final Term";
                    db.Examtype.Add(model1);
                    db.Examtype.Add(model2);

                }
                if (db.HomeWorkType.Count() == 0)
                {
                    HomeWorkType model1 = new HomeWorkType();
                    HomeWorkType model2 = new HomeWorkType();
                    model1.HomeWorkTypeId = 1;
                    model1.HomeWorkName = "Assignment";
                    model2.HomeWorkTypeId = 2;
                    model2.HomeWorkName = "Homework";
                    db.HomeWorkType.Add(model1);
                    db.HomeWorkType.Add(model2);

                }
                if (db.Roles.Count() == 0)
                {
                    Roles model1 = new Roles();
                    Roles model2 = new Roles();
                    Roles model3 = new Roles();
                    model1.RoleId = 1;
                    model1.RoleType = "Admin";
                    model2.RoleId = 2;
                    model2.RoleType = "Teacher";
                    model3.RoleId = 3;
                    model3.RoleType = "Parent";
                    db.Roles.Add(model1);
                    db.Roles.Add(model2);
                    db.Roles.Add(model3);

                }
                if (db.Members.Count()==0)
                {
                    MemberRegistration mem = new MemberRegistration();
                    mem.RegisterId = 1;
                    mem.Name = "Admin";
                    mem.Image = "~/AppFolder/Images/janbaz2.jpg";
                    mem.Email = "jaani2022@gmail.com";
                    mem.CNIC = "1110150393415";
                    mem.Password = "janbazkhan";
                    mem.RoleId = 1;
                    db.Members.Add(mem);
                }
                if (db.SibDiscount.Count()==0)
                {
                    SibDiscount dis = new SibDiscount();
                    dis.Id = 1;
                    dis.Sib2_Discount = 0;
                    dis.Sib3_Discount = 0;
                    dis.Sib4_Discount = 0;
                    dis.Orphan_Discount = 0;
                    db.SibDiscount.Add(dis);
                }
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
                db.SaveChanges();
                if (db.Database.Exists())
                {
                    SqlDependency.Start(conn);

                }
            }
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


        }
        protected void Application_End()
        {
            SqlDependency.Stop(conn);
        }
    }
}
