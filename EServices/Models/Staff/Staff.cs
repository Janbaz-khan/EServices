using EServices.Models.ClassMapping;
using EServices.Models.Logins;
using EServices.Models.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.Staff
{
    [Table("Staff")]
    public class Staff
    {
        [Key]
        public long EmpId { get; set; }
        [Required(ErrorMessage ="field required")]
        [Display(Name ="Name")]
        public string EmpName { get; set; }
        [Required(ErrorMessage = "field required")]
        [Display(Name = "Father Name")]
        public string FatherName { get; set; }
        [Required(ErrorMessage = "field required")]
        [MaxLength(13, ErrorMessage = "incorrect CNIC max length exceed")]
        [MinLength(13, ErrorMessage = "incorrect CNIC ")]
        public string CNIC { get; set; }
        [DataType(DataType.Date)]
        public string HiringDate { get; set; }
        [Required(ErrorMessage = "field required")]
        public string Education { get; set; }
        [Required(ErrorMessage = "field required")]
        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Phone number")]
        [MaxLength(12, ErrorMessage = "incorrect phone number max length exceed")]
        [MinLength(12, ErrorMessage = "incorrect phone number ")]
        public string PhoneNo { get; set; }
        [Display(Name = "Gender")]
        public long? GenderId { get; set; }
        [Required(ErrorMessage = "field required")]
        [DataType(DataType.MultilineText)]
        public string Address { get; set; }
        [Required(ErrorMessage = "field required")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "field required")]
       
        [Display(Name = "Basic Salary")]

        public decimal BasicSalary { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }
        [ForeignKey("GenderId")]
        public Gender gender { get; set; }
    



        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }




        public Staff()
        {
            Image = "~/AppFolder/Images/user.png";
        }
        public List<ExamResult> ExamResults { get; set; }
        public List<Subject.Subject_Teacher_Mapping> Mapping { get; set; }
        public List<ClassHeadTeacher> HeadTeacher { get; set; }
        public List<Attendance.TeacherAttendance> TeachersAttendance { get; set; }




    }
}