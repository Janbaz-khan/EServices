using EServices.Models.Fee;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.Results
{
    [Table("HomeWork")]
    public class HomeWork
    {
        [Key]
        public long HomeWorkId { get; set; }
        [Display(Name ="Type")]
        [Required(ErrorMessage ="field required")]
        public long HomeWorkTypeId { get; set; }
        [Display(Name = "Class")]
        [Required(ErrorMessage ="field required")]
        public long? ClassId { get; set; }
        public long? SessionId { get; set; }
        [Display(Name = "Section")]
        [Required(ErrorMessage ="field required")]
        public long? SecionId { get; set; }
        [Display(Name = "Subject")]
        [Required(ErrorMessage ="field required")]
        public long? SubjectId { get; set; }
        public long? TeacherId { get; set; }
        public string Date { get; set; }
        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage ="field required")]
        public string Content { get; set; }

        [ForeignKey("HomeWorkTypeId")]
        public HomeWorkType HomeWorks { get; set; }

        [ForeignKey("ClassId")]
        public Class Classes { get; set; }
        [ForeignKey("SessionId")]
        public Session Sessions { get; set; }

        [ForeignKey("SubjectId")]
        public Subject.Subject Subjects { get; set; }
        [ForeignKey("TeacherId")]
        public MemberRegistration Staff { get; set; }
        [ForeignKey("SecionId")]
        public Section Sections { get; set; }
        [NotMapped]
        public bool SMS_Status { get; set; }
        public bool ReadStatus { get; set; }
        public HomeWork()
        {
            ReadStatus = false;
        }

    }
}