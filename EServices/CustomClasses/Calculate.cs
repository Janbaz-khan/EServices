using EServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EServices.CustomClasses
{
    public static class Calculate
    {
        public static string AssignGrade(decimal percentage)
        {
            using (DB db = new DB())
            {
                string status = "not calculated";
                var grade = db.GradeSetting.FirstOrDefault();
                if (percentage < grade.PassingMarks)
                {
                    status = "Fail";
                }
                else if (percentage >= grade.PassingMarks && percentage <= grade.Fair_Grade)
                {
                    status = "Fair";
                }
                else if (percentage > grade.Fair_Grade && percentage <= grade.good_Grade)
                {
                    status = "Good";
                }
                else if (percentage > grade.good_Grade && percentage <= grade.Excellent_Grade)
                {
                    status = "Excellent";
                }
                else if (percentage > grade.Excellent_Grade)
                {
                    status = "Extra Ordinary";
                }
                return status;
            }
        }


        public static decimal CalculcatePercentage(decimal total, decimal obtainedTotal)
        {
            var percentage = (obtainedTotal / total) * 100;
            return percentage;
        }

        public static decimal SumMarks(decimal writtenMarks, decimal oralMarks, decimal conceptualMarks)
        {
            var sum = writtenMarks + oralMarks + conceptualMarks;
            return sum;
        }

    }
}