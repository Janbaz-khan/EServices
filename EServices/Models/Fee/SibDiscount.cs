using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EServices.Models.Fee
{
    public class SibDiscount
    {
        public long Id { get; set; }
        public decimal Sib2_Discount { get; set; }
        public decimal Sib3_Discount { get; set; }
        public decimal Sib4_Discount { get; set; }
        public decimal Orphan_Discount { get; set; }
    }
}