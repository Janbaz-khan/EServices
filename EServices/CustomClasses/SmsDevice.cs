using EServices.Models;
using EServices.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EServices.CustomClasses
{
    public static class SmsDevice
    {
        public static SMSDevice GetInfo()
        {
            using (DB db=new DB())
            {
                return db.SMSDevices.Where(a => a.status).FirstOrDefault();
            }
        }
    }
}