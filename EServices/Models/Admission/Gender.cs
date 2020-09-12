using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EServices.Models
{
    public class Gender
    {
        [Key]
        public long GenderId { get; set; }
        public string GenderName { get; set; }
        public List<StudentRegistrationModel> Students { get; set; }
        public List<Staff.Staff> Staff { get; set; }
    }

}