using EServices.Models;
using EServices.Models.Fee;
using EServices.Models.TestResult;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.TestResult
{
    [Table("TestResult")]
    public class TestResult
    {
        [Key]
        public long ResultId { get; set; }
        [Display(Name ="Name")]
        public long? AddmissionNo { get; set; }
      
        public long ClassId { get; set; }
        public long SessionId { get; set; }
        public long SecionId { get; set; }
        public long MonthId { get; set; }
        public long SubjectId { get; set; }
        public string Date { get; set; }
        [Display(Name = "Total Marks")]
        public decimal TotalMarks { get; set; }
        [Display(Name = "Obtained Marks")]
        [Required(ErrorMessage ="field required...")]
        public decimal ObtainedMarks { get; set; }
        public string Percentage { get; set; }  
        public string Status { get; set; }
        [NotMapped]
        public bool SMS_Status { get; set; }
        public bool ReadStatus { get; set; }
        public TestResult()
        {
            ReadStatus = false;
        }


        [ForeignKey("AddmissionNo")]
        public StudentRegistrationModel Students { get; set; }
   
        [ForeignKey("ClassId")]
        public Class Classes { get; set; }
        [ForeignKey("SessionId")]
        public Session Sessions { get; set; }
        [ForeignKey("MonthId")]
        public Month Months { get; set; }
        [ForeignKey("SubjectId")]
        public Subject.Subject Subjects { get; set; }
        [ForeignKey("SecionId")]
        public Section Sections { get; set; }


    }
}
