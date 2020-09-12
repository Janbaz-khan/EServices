using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EServices.CustomClasses
{
    public static class LocalTime
    {
        public static DateTime Now()
        {
            TimeZoneInfo timeZoneInfo;
            DateTime dateTime;
            //Set the time zone information to US Mountain Standard Time 
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
            //Get date and time in US Pakistan Standard Time 
            dateTime = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            return dateTime;
        }
      
    }

}