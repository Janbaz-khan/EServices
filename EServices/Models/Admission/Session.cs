using EServices.Models.ClassMapping;
using EServices.Models.Fee;
using EServices.Models.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EServices.Models
{
    public class Session
    {
        [Key]
        public long SessionId { get; set; }
        [Required(ErrorMessage ="Session field required...")]
        public string SessionName { get; set; }
        public List<StudentRegistrationModel> Students { get; set; }
        public List<FeePayment> Fee { get; set; }
        public List<Attendace.Attendance> Attandance { get; set; }
        public List<Subject.Subject_Teacher_Mapping> SubjectMapping { get; set; }
        public List<TestResult.TestResult> TestResults { get; set; }
        public List<ExamResult> ExamResults { get; set; }
        public List<ClassHeadTeacher> HeadTeacher { get; set; }
        public List<HomeWork> HomeWorks { get; set; }



    }
}