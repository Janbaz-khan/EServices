using EServices.Models.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.Subject
{
    [Table("Subject")]
    public class Subject
    {
        [Key]
        public long SubjectId { get; set; }
        [Required(ErrorMessage ="field required")]
        public string SubjectName { get; set; }
        public List<Subject_Teacher_Mapping> SubjectMapping { get; set; }
        public List<TestResult.TestResult> TestResults { get; set; }
        public List<ExamResult> ExamResults { get; set; }
        public List<HomeWork> HomeWorks { get; set; }

    }
}