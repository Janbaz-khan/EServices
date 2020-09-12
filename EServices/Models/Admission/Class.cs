using EServices.Models.ClassMapping;
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
  
    public class Class
    {
        [Key]
        public long ClassId { get; set; }
        [Required(ErrorMessage ="Class field required...")]
        public string ClassName { get; set; }
        public List<Attendace.Attendance> Attandance { get; set; }

        public List<StudentRegistrationModel> Students { get; set; }
        public List<Subject.Subject_Teacher_Mapping> SubjectMapping { get; set; }
        public List<TestResult.TestResult> TestResults { get; set; }
        public List<ExamResult> ExamResults { get; set; }
        public List<ClassHeadTeacher> HeadTeacher { get; set; }
        public List<HomeWork> HomeWorks { get; set; }
        public List<FeePayment> Feepayment { get; set; }
        //public List<NormalFee> Normal { get; set; }
        //public List<Stationary_Fee> Stationary { get; set; }

    }
}