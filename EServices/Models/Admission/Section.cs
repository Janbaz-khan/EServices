using EServices.Models.ClassMapping;
using EServices.Models.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EServices.Models
{
    public class Section
    {
        public long SectionId { get; set; }
        [Required(ErrorMessage ="Section required...")]
        public string SectionName { get; set; }
        public List<StudentRegistrationModel> Students { get; set; }
        public List<TestResult.TestResult> TestResults { get; set; }
        public List<Fee.FeePayment> Fee { get; set; }
        public List<Attendace.Attendance> Attendance { get; set; }
        public List<ExamResult> ExamResults { get; set; }

        public List<ClassHeadTeacher> HeadTeacher { get; set; }
        public List<HomeWork> HomeWorks { get; set; }


    }
}