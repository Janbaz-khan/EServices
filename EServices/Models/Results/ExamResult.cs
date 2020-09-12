using EServices.Models.Fee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.Results
{
    [Table("ExamResult")]
    public class ExamResult
    {
        [Key]
        public long ExamResultId { get; set; }
        [Display(Name ="Name")]
        public long StudentId { get; set; }
        [Display(Name = "Exam Type")]
        public long ExamTypeId { get; set; }
        [Display(Name ="Class")]
        public long? ClassId { get; set; }
        [Display(Name ="Section")]
        public long? SectionId { get; set; }
        public long? SessionId { get; set; }
        public long? MonthId { get; set; }
        public long TeacherId { get; set; }
        public long? SubjectId { get; set; }
        public string DateTime { get; set; }
        [Display(Name ="Written marks")]
        [Required(ErrorMessage ="Field required")]
        public decimal WrittenMarks { get; set; }
        [Display(Name ="Conceptual marks")]
        [Required(ErrorMessage ="Field required")]
        public decimal ConceptualMarks { get; set; }
        [Display(Name ="Oral marks")]
        [Required(ErrorMessage ="Field required")]
        public decimal OralMarks { get; set; }
        [Required(ErrorMessage ="Field required")]
        [Display(Name ="Total marks")]
        public decimal TotalMarks { get; set; }
        [Required(ErrorMessage ="Field required")]
        public decimal ObtainedTotal { get; set; }
        public string Percentage { get; set; }
        public string Status { get; set; }
        [NotMapped]
        public bool SMS_Status { get; set; }
        public bool  ReadStatus { get; set; }
        public ExamResult()
        {
            ReadStatus = false;
        }




        [ForeignKey("StudentId")]
        public StudentRegistrationModel Students { get; set; }
        [ForeignKey("ExamTypeId")]
        public ExamType Examtype { get; set; }
        [ForeignKey("ClassId")]
        public Class Classes { get; set; }
        [ForeignKey("SessionId")]
        public Session Sessions { get; set; }
        [ForeignKey("SectionId")]
        public Section Sections { get; set; }
        [ForeignKey("MonthId")]
        public Month Months { get; set; }
        [ForeignKey("TeacherId")]
        public MemberRegistration Teachers { get; set; }
        [ForeignKey("SubjectId")]
        public Subject.Subject Subjects { get; set; }

    }
}