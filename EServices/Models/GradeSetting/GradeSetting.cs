using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EServices.Models.GradeSetting
{
    public class GradeSetting
    {
        public int id { get; set; }
        [Required(ErrorMessage ="field required")]
        public decimal PassingMarks { get; set; }
        [Required(ErrorMessage = "field required")]
        public decimal Fair_Grade { get; set; }
        [Required(ErrorMessage = "field required")]
        public decimal good_Grade { get; set; }
        [Required(ErrorMessage = "field required")]
        public decimal Excellent_Grade { get; set; }
        [Required(ErrorMessage = "field required")]
        public decimal OutStanding_Grade { get; set; }
       
    }
}