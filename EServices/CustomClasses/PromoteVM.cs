using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EServices.CustomClasses
{
    public class PromoteVM
    {
        public int AdmissionNo { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public bool IsPromoted { get; set; }
    }
}