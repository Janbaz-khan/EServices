using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EServices.Models.Logins
{
    public class Login
    {
      
        public long LoginId { get; set; }
        [Required(ErrorMessage ="Field required")]
        [MaxLength(13, ErrorMessage = "incorrect CNIC max length exceed")]
        [MinLength(13, ErrorMessage = "incorrect CNIC ")]
        public string CNIC { get; set; }
        [Required(ErrorMessage ="Field required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
    }
}