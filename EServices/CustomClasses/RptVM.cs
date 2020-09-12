using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EServices.CustomClasses
{
    public class TestRptVM
    {
        public int ResultId { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Session { get; set; }
        public string Subject { get; set; }
        public string Month { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal ObtainedMarks { get; set; }
        public string Percentage { get; set; }
        public string Date { get; set; }
    }
    public class ExamRptVM
    {
        public int ResultId { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Session { get; set; }
        public string Subject { get; set; }
        public decimal Written { get; set; }
        public decimal Oral { get; set; }
        public decimal Conceptual { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal ObtainedMarks { get; set; }
        public string Percentage { get; set; }
        public string Status { get; set; }
        public string ExamType { get; set; }
        public string TeacherName { get; set; }
    }
    public class FullExamVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FatherName { get; set; }
        public string Class { get; set; }
        public string Section { get; set; }
        public string Session { get; set; }
        public string ExamType { get; set; }
        public decimal TotalMarks { get; set; }
        public decimal ObtainedMarks { get; set; }
    }
    public class FeeRptVM
    {
        public int FeePaymentId { get; set; }
        public string AdmissionFee { get; set; }
        public string MonthlyFee { get; set; }
        public string PromotionFee { get; set; }
        public string ExamFee { get; set; }
        public string StationaryFee { get; set; }
        public string OtherCharges { get; set; }
        public Decimal Total { get; set; }
        public Decimal OutStand { get; set; }
        public Decimal NetTotal { get; set; }
        public Decimal Paid { get; set; }
        public Decimal CarryForward { get; set; }

        public string Class { get; set; }
    
        public string Session { get; set; }
        public string Section{ get; set; }
      
        public string Date { get; set;  }
        public string Month { get; set; }
        public string FatherName { get; set; }
        public string StudentName { get; set; }
    }
}