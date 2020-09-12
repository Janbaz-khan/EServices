using EServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EServices.CustomClasses
{
    public class SibDisc
    {
        public decimal GetSibDiscount(string Category)
        {
            using (DB db = new DB())
            {
                var sibDis = db.SibDiscount.FirstOrDefault();
                decimal sib=0;
                switch (Category)
                {

                    case "Sib-2":
                        sib = sibDis.Sib2_Discount;
                        break;
                    case "Sib-3":
                        sib = sibDis.Sib3_Discount;
                        break;
                    case "Sib-4":
                        sib = sibDis.Sib4_Discount;
                        break;
                    case "Orphan":
                        sib = sibDis.Orphan_Discount;
                        break;
                    default:
                        break;
                }
                return sib;
            }
        }
    }
}