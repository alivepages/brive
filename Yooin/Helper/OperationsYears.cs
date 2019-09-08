using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Brive.Middleware.PdfGenerator.Yooin.Helper
{
    public class OperationsYears
    {
        public static int CalculateYearsOld(DateTime? birthday)
        {
            if (!birthday.HasValue)
                return 0;

            var today = DateTime.Today;
            var age = today.Year - birthday.Value.Year;
            if (birthday > today.AddYears(age)) age--;
            return age;
        }

        public static string CalculateExperience(Brive.Yooin.Contracts.Experience[] experienceList)
        {
            double experienceDay = 0;
            DateTime temporalJobEnd = new DateTime();
            DateTime today = DateTime.Now;

            if (experienceList.Length == 0)
                return "0 años 0 meses";

            experienceList = experienceList.OrderBy(e => e.StartDate).ToArray();
            for (int i = 0; i < experienceList.Length; ++i)
            {
                DateTime jobStart = (DateTime)experienceList[i].StartDate;
                DateTime jobEnd = experienceList[i].EndDate.HasValue ? (DateTime)experienceList[i].EndDate : today;

                if (i > 0)
                    temporalJobEnd = experienceList[i - 1].EndDate.HasValue ? (DateTime)experienceList[i - 1].EndDate : today;

                if (i > 0 && temporalJobEnd > jobEnd)
                    jobStart = temporalJobEnd;

                experienceDay += (jobEnd - jobStart).TotalDays;
            }

            double years = experienceDay / 365.242199;
            int wholeYears = (int)Math.Floor(years);
            double partYears = years - wholeYears;
            double approxMonths = partYears * 12;
            int wholeMonths = (int)Math.Floor(approxMonths);

            string resultYearExperience = wholeYears > 1 ? wholeYears + " años" : wholeYears + " año";
            string resultMonthExperience = wholeMonths > 1 ? wholeMonths + " meses" : wholeMonths + " mes";

            return resultYearExperience + " " + resultMonthExperience;
        }
    }
}
