using EServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EServices.CustomClasses
{
    public class RandomValue
    {
        public int GeneratePassword()
        {
            Random rndm = new Random();
            int min = 10000;
            int max = 99999;
            return rndm.Next(min, max);
        }
    }
    public class CurrentSession
    {
        public string Session()
        {
            using (DB db=new DB())
            {
                var s = db.Sessions.OrderByDescending(a => a.SessionId).Take(1).FirstOrDefault();
                if (s==null)
                {
                    var currentyear = DateTime.Now.Year;
                    var nextyear = DateTime.Now.AddYears(1).Year;
                    Session session = new Session();
                    session.SessionId = 1;
                    session.SessionName = currentyear + "-" + nextyear;
                    db.Sessions.Add(session);
                    db.SaveChanges();
                    return session.SessionName;
                }
                else
                {
                string ses = s.SessionName;
                  return ses;
                }
            }
        }
    }
    
}