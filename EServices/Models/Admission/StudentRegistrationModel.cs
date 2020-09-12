using EServices.Models.Fee;
using EServices.Models.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models
{
    [Table("Student")]
    public class StudentRegistrationModel
    {
        [Key]
        public long AddmissionNo { get; set; }

        [Required(ErrorMessage = " student name field required...")]
        [Display(Name = "Student Name")]
        public String StudentName { get; set; }
        [Required(ErrorMessage ="Father CNIC field required ")]
        [Display(Name = "Father Name")]
        public long? ParentId { get; set; }
        [ForeignKey("ParentId")]
        public Parent Parent { get; set; }
        [Required(ErrorMessage = "DOB field required...")]
      
        public string DOB { get; set; }
        [Required(ErrorMessage = " Address field required...")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Class field required...")]
        [Display(Name = "Class")]
        public long? ClassId { get; set; }
        [ForeignKey("ClassId")]
        public Class Class { get; set; }
        [Required(ErrorMessage = "Field required")]
        [Display(Name = "Category")]
        public string SibCategory { get; set; }
        [Required(ErrorMessage = " Section field required...")]
        [Display(Name = "Section")]
        public long? SectionId { get; set; }
        [ForeignKey("SectionId")]
        public Section Sections { get; set; }

        [Required(ErrorMessage = "Session field required")]
        [Display(Name = "Session")]
        public long? SessionId { get; set; }

        [ForeignKey("SessionId")]
        public Session Sessions { get; set; }
        [Required(ErrorMessage = "Date field required...")]
        public string AddmissionDate { get; set; }

       
        [Required(ErrorMessage = " Religion field required...")]
        [Display(Name = "Relegion")]
        public long? ReligionId { get; set; }
        [ForeignKey("ReligionId")]
        public Religion Religion { get; set; }
       
        [Required(ErrorMessage = "Gender field required...")]
        [Display(Name = "Gender")]
        public long? GenderId { get; set; }
        [ForeignKey("GenderId")]
        public Gender Gender { get; set; }

        [Required(ErrorMessage ="Image required")]
        public  string Image { get; set; }
        public bool Status { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public List<FeePayment> Payment { get; set; }
        public List<Attendace.Attendance> Attandance { get; set; }
        public List<TestResult.TestResult> TestResults { get; set; }


        public List<ExamResult> ExamResults { get; set; }
        public StudentRegistrationModel()
        {
            Status = true;
        }
    }
}