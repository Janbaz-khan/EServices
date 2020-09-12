using EServices.Models.Fee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models
{
    public class StdParentViewModel
    {

        public long AddmissionNo { get; set; }

        [Required(ErrorMessage = " student name field required...")]
        public String StudentName { get; set; }

        [Required(ErrorMessage = "DOB field required...")]

        public string DOB { get; set; }
        [Required(ErrorMessage = " Address field required...")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Class field required...")]
        [Display(Name ="Class")]
        public int ClassId { get; set; }
        [ForeignKey("ClassId")]
        public Class Class { get; set; }
        [Required(ErrorMessage = "Field required")]
        [Display(Name = "Category")]
        public string SibCategory { get; set; }
        [Required(ErrorMessage = " Section field required...")]
        [Display(Name = "Section")]
        public int SectionId { get; set; }
        [ForeignKey("SectionId")]
        public Section Sections { get; set; }

        [Required(ErrorMessage = "Session field required")]
        [Display(Name = "Session")]
        public int SessionId { get; set; }

        [ForeignKey("SessionId")]
        public Session Sessions { get; set; }
        [Required(ErrorMessage = "Date field required...")]
        public string AddmissionDate { get; set; }


        [Required(ErrorMessage = " Religion field required...")]
        [Display(Name = "Relegion")]
        public int ReligionId { get; set; }
        [ForeignKey("ReligionId")]
        [Display(Name = "Religion")]
        public Religion Religion { get; set; }

        [Required(ErrorMessage = "Gender field required...")]
        [Display(Name = "Gender")]
        public int GenderId { get; set; }
        [ForeignKey("GenderId")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage = "Image required")]
        public string Image { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }

        //Parent Model
        public long ParentId { get; set; }
        [Display(Name = "Father Name")]
        [Required(ErrorMessage = "Name field required")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "Name field required")]
        [MaxLength(13, ErrorMessage = "incorrect CNIC max length exceed")]
        [MinLength(13, ErrorMessage = "incorrect CNIC ")]
        public string CNIC { get; set; }
        [Required(ErrorMessage = " phone field required...")]
        [DataType(DataType.PhoneNumber)]
        [MaxLength(12,ErrorMessage ="incorrect phone number max length exceed")]
        [MinLength(12,ErrorMessage ="incorrect phone number ")]
        public string PhoneNo { get; set; }
        [Required(ErrorMessage = "Emergancy phone field required...")]

        [DataType(DataType.PhoneNumber)]
        [MaxLength(12, ErrorMessage = "incorrect phone number max length exceed")]
        [MinLength(12, ErrorMessage = "incorrect phone number ")]
        public string EmergancyNo { get; set; }
        [Required(ErrorMessage = " Postel Address field required...")]
        [Display(Name = "Postel Address")]
        public string PostelAddress { get; set; }

        public StdParentViewModel()
        {
            Image = "~/AppFolder/Images/std.png";
        }



        //Fee Payment Model Main Properties
        #region Fee Payment Model Main Properties
        [Key]
        public long FeePaymentId { get; set; }
        [Required(ErrorMessage = "Field required")]
        public long? Normal_Fee { get; set; }
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
        public string Date { get; set; }
        [Required(ErrorMessage = "Field required")]
        [Display(Name ="Month")]
        public long MonthId { get; set; }
        [ForeignKey("MonthId")]
        public Month Months { get; set; }
        [NotMapped]
        #endregion
        // Normal Fee
        #region Normal Fee
        public long NormalFeeID { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal AdmissionFee { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal MonthlyFee { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal PromotionFee { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal ExamFee { get; set; }
        public decimal TotalNormal { get; set; }
        #endregion
        //Stationary Fee
        #region Stationary Fee
        public long StationaryFeeID { get; set; }

        [Required(ErrorMessage = "Field required")]
        public Decimal Books { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Homework_Dairy { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Note_Books { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Uniform { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Cap { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Scarf { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Staionary { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Shoes { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal socks { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal SchoolBag { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal School_IDCard { get; set; }
        public Decimal TotalStationary { get; set; }
        public bool SmsStatus { get; set; }
        #endregion
       

    }

}