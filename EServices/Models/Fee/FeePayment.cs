using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.Fee
{
    public class FeePayment
    {
        [Key]
        public long FeePaymentId { get; set; }
        [Required(ErrorMessage = "Field required")]
        public long?  Normal_Fee { get; set; }
        [ForeignKey("Normal_Fee")]
        public NormalFee Normal { get; set; }
        [Required(ErrorMessage = "Field required")]
        public long? Stationary_Fee { get; set; }
        [ForeignKey("Stationary_Fee")]
        public Stationary_Fee Stationary { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal OtherCharges { get; set; }
        public Decimal Total { get; set; }
        public Decimal OutStand { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Fine { get; set; }
        public Decimal NetTotal { get; set; }
        public decimal Discount { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Paid { get; set; }
        public Decimal CarryForward { get; set; }

        [Required(ErrorMessage = "Field required")]
        public long ClassId { get; set; }
        [ForeignKey("ClassId")]
        public Class Classes { get; set; }
        [Required(ErrorMessage = "Field required")]
        public long? SessionId { get; set; }
        public long? SectionId { get; set; }
        [ForeignKey("SessionId")]
        public Session Sessions { get; set; }
        [ForeignKey("SectionId")]
        public Section Sections { get; set; }
        [Required(ErrorMessage = "Field required")]
        public string Date { get; set; }
        [Required(ErrorMessage = "Field required")]
        public long MonthId { get; set; }
        [ForeignKey("MonthId")]
        public Month Months { get; set; }
        [NotMapped]
        public bool SMS_Status { get; set; }
        public long? AddmissionNo { get; set; }
        [ForeignKey("AddmissionNo")]
        public StudentRegistrationModel Students { get; set; }
        public FeePayment()
        {
            SMS_Status = false;
        }

    }
}