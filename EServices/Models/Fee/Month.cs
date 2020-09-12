using EServices.Models.Results;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EServices.Models.Fee
{
    public class Month
    {
        [Key]
        public long MonthId { get; set; }
        public string MonthName { get; set; }
        public bool IsStartMonth { get; set; }
        public List<FeePayment> FeePayment { get; set; }
        public List<TestResult.TestResult> TestResults { get; set; }
        public List<ExamResult> ExamResults { get; set; }
    }
}