using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EServices.Models
{
    public class Parent
    {
        [Key]
        public long ParentId { get; set; }
        [Display(Name ="Father Name")]
        [Required(ErrorMessage ="Name field required")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "Name field required")]
        [MaxLength(13, ErrorMessage = "incorrect CNIC max length exceed")]
        [MinLength(13, ErrorMessage = "incorrect CNIC ")]
        public string CNIC { get; set; }
        [Required(ErrorMessage = " phone field required...")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(12, ErrorMessage = "incorrect phone number max length exceed")]
        [MinLength(12, ErrorMessage = "incorrect phone number ")]
        public string PhoneNo { get; set; }
        [Required(ErrorMessage = "Emergancy phone field required...")]

        [DataType(DataType.PhoneNumber)]
        [MaxLength(12, ErrorMessage = "incorrect phone number max length exceed")]
        [MinLength(12, ErrorMessage = "incorrect phone number ")]
        public string EmergancyNo { get; set; }
        [Required(ErrorMessage = " Postel Address field required...")]
        [Display(Name ="Postel Address")]
        public string PostelAddress { get; set; }


    }
}