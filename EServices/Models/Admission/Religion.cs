using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EServices.Models
{
    public class Religion
    {
        [Key]
        public long ReligionId { get; set; }
        public string ReligionName { get; set; }
        public List<StudentRegistrationModel> Students { get; set; }
    }
}