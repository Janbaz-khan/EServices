using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EServices.Models.Logins
{
    
    public class Roles
    {
        [Key]
        public long RoleId { get; set; }
        public string RoleType { get; set; }
        public List<MemberRegistration> Members { get; set; }
    }

}