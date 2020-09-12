using EServices.Models.Logins;
using EServices.Models.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models
{
    [Table("Member_Registration")]
    public class MemberRegistration
    {
        [Key]
        public long RegisterId { get; set; }
        [Required(ErrorMessage ="Field required...")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Field required...")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required(ErrorMessage = "Field required...")]
        [MaxLength(13, ErrorMessage = "incorrect CNIC max length exceed")]
        [MinLength(13, ErrorMessage = "incorrect CNIC ")]
        public string CNIC { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Field required...")]
        [MinLength(6,ErrorMessage ="Password should minimum six charector long")]
        public string Password { get; set; }
     
        [NotMapped]
        public string LoginType { get; set; }
        public long RoleId { get; set; }
        [ForeignKey("RoleId")]
        public Roles Role { get; set; }
        public string Image { get; set; }
        public bool AccountStatus { get; set; }

        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public List<HomeWork> HomeWorks { get; set; }


        public MemberRegistration()
        {
                Image = "~/AppFolder/Images/user.png";
            AccountStatus = true;
        }
    }
}