using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.Results
{
    [Table("HomeWorkType")]
    public class HomeWorkType
    {
        [Key]
        public long HomeWorkTypeId { get; set; }
        public string HomeWorkName { get; set; }
        public List<HomeWork> HomeWork { get; set; }
    }
}