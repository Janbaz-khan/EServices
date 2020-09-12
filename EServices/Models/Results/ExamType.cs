using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.Results
{
    [Table("ExamType")]
    public class ExamType
    {
        [Key]
        public long ExamTypeId { get; set; }
        public string ExamName { get; set; }
        public List<ExamResult> ExamResults { get; set; }
    }
}