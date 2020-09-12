﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.Subject
{
    [Table("Teacher_Subject_Mapping")]
    public class Subject_Teacher_Mapping
    {
       [Key]
        public long MapId { get; set; }
        [Display(Name ="Subject")]
        public long SubjectId { get; set; }
        [Display(Name ="Class")]
        public long ClassId { get; set; }
        [Display(Name ="Session")]
        public long SessionId { get; set; }
        [Display(Name = "Section")]
        public long SectionId { get; set; }
        [Display(Name ="Teacher")]
        public long EmpId { get; set; }
        [ForeignKey("SubjectId")]
        public Subject Subjects { get; set; }
        [ForeignKey("ClassId")]
        public Class Classes { get; set; }
        [ForeignKey("EmpId")]
        public Staff.Staff Teacher { get; set; }
        [ForeignKey("SessionId")]
        public Session Sessions { get; set; }
        [ForeignKey("SectionId")]
        public Section Sections { get; set; }

    }
}