using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace EServices.Models.Fee
{
    public class Stationary_Fee
    {
        [Key]
        public long StationaryFeeID { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Books { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Homework_Dairy { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Note_Books { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Uniform { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Cap { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Scarf { get; set; }
        [Required(ErrorMessage = "Field required")]
        [Display(Name ="Stationary")]
        public Decimal Staionary { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal Shoes { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal socks { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal SchoolBag { get; set; }
        [Required(ErrorMessage = "Field required")]
        public Decimal School_IDCard { get; set; }
        public Decimal TotalStationary { get; set; }
        public List<FeePayment> Fee { get; set; }
    }
}