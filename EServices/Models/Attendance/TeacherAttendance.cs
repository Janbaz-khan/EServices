using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.Attendance
{
        [Table("TeacherAttendance")]
    public class TeacherAttendance
    {
            [Key]
            public long Attendace_Id { get; set; }
            public long? TeacherId { get; set; }
            [Required(ErrorMessage = "Field required")]
            public long? SessionId { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string Status { get; set; }
            [ForeignKey("TeacherId")]
            public Staff.Staff Teachers { get; set; }
            [ForeignKey("SessionId")]
            public Session Sessions { get; set; }
            [NotMapped]
            public bool SMS_Status { get; set; }


    }
}