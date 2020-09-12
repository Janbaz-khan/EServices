using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EServices.Models.API
{
    public class SMSDevice
    {
        public int id { get; set; }
        public string Device_Name { get; set; }
        public int Device_Code { get; set; }
        public bool status { get; set; }
        public SMSDevice()
        {
            status = false;
        }
    }
}