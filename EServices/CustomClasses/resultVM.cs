using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EServices.CustomClasses
{
    public class resultVM
    {
        public long id { get; set; }
        public string Name { get; set; }
            public string Class { get; set; }
            public string Section { get; set; }
            public decimal Marks { get; set; }
            public decimal TMarks { get; set; }
            public string date { get; set; }
        public string subject { get; set; }

        public bool Read { get; set; }
    }

    public class ExamResultVM
    {
        public long id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public decimal Marks { get; set; }
        public decimal TMarks { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string subject { get; set; }
        public bool Read { get; set; }
    }
    public class HomeWorkVM
    {
        public long id { get; set; }
        public string Class { get; set; }
        public string date { get; set; }
        public string type { get; set; }
        public string subject { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }
    }
    public class AttendanceVM
    {
        public long id { get; set; }
        public string Name { get; set; }
        public string Class { get; set; }
        public string Date { get; set; }
        public bool Read { get; set; }
    }
}