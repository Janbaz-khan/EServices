using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.Fee
{
    public class NormalFee
    {
        [Key]
        public long NormalFeeID { get; set; }
        [Display(Name ="Class")]
        public long? ClassId { get; set; }
        [ForeignKey("ClassId")]
        public Class Class{ get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal AdmissionFee { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal MonthlyFee { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal PromotionFee { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal ExamFee { get; set; }
        public decimal TotalNormal { get; set; }
        public bool Status { get; set; }
        public List<FeePayment> Fee { get; set; }
        public NormalFee()
        {
            Status = true;
        }
    }
}