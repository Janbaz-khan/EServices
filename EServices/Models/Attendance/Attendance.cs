using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.Attendace
{
    [Table("Attendance")]
    public class Attendance
    {
        [Key]
        public long Attendace_Id { get; set; }
        public long? AddmissionNo { get; set; }
        [Required(ErrorMessage ="Field required")]
        public long? ClassId { get; set; }
        [Required(ErrorMessage = "Field required")]

        public long? SessionId { get; set; }
        public long? SectionId { get; set; }

        public string Date { get; set; }
        public string Time { get; set; }
        public string Status { get; set; }
        [NotMapped]
        public bool SMS_Status { get; set; }
        public bool ReadStatus { get; set; }
        public Attendance()
        {
            ReadStatus = false;
        }

        [ForeignKey("AddmissionNo")]
        public StudentRegistrationModel Students { get; set; }
        [ForeignKey("SessionId")]
        public Session Sessions { get; set; }
        [ForeignKey("SectionId")]
        public Section Sections { get; set; }
        [ForeignKey("ClassId")]
        public Class Classes { get; set; }
    }

}