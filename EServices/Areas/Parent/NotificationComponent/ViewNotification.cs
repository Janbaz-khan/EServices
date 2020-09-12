using EServices.Models;
using EServices.Models.TestResult;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Entity.Infrastructure;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace EServices.SG_Component
{
    public class ViewNotification
    {
        string conn = ConfigurationManager.ConnectionStrings["DB"].ConnectionString;

        public List<TestResult> SetLiveRecord(string currentParent)
        {
            string query = @"SELECT [AddmissionNo] [ClassId] FROM [dbo].[TestResult]";
            List<TestResult> Tests = new List<TestResult>();
            using (SqlConnection connection=new SqlConnection(conn))
            {
               // string CommondText = null;
               // DB db = new DB();
               // var parent = db.Parent.Where(a => a.CNIC == currentParent).FirstOrDefault();
               // var record=db.TestResults.Include("Classes").Include("Students").Where(a=>a.Students.Parent.CNIC=="change") as DbQuery<TestResult>;
               // currentParent.Replace("JOIN", "");
               // currentParent.Replace("WHERE", "");
               // CommondText = query.ToString().Replace("change", currentParent);

               //SqlCommand comm = new SqlCommand(CommondText, connection);
               // comm.Notification = null;
               // connection.Open();

               // SqlDependency dep = new SqlDependency(comm);
               // dep.OnChange += Dep_OnChange;
               // SqlDataReader reader = comm.ExecuteReader();


            }
            return Tests;

        }

        private void Dep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
            SqlDependency dependency = sender as SqlDependency;
                dependency.OnChange -= Dep_OnChange;
                MyHub hub = new MyHub();
                //hub.Send();
            }

        }
    }
}