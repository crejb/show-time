using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShowTime.Services
{
    public class DateTimeToHumanReadableFormatConverter
    {
        private const string JUST_NOW = "Just now";
        private const string AGO = "ago";
        private const string MINUTE = "minute";
        private const string HOUR = "hour";
        private const string DAY = "day";
        private const string WEEK = "week";
        private const string YEAR = "year";

        public string ConvertDateTimeToHumanReadableFormat(DateTime dateTime)
        {
            TimeSpan ago = DateTime.Now.Subtract(dateTime);

            if (ago.TotalMinutes < 1)
            {
                return JUST_NOW;
            }

            if (ago.TotalHours < 1)
            {
                int minutes = (int)(ago.TotalMinutes);
                return CombineValueAndRequiredForm(minutes, MINUTE);
            }

            if (ago.TotalHours < 24)
            {
                int hours = (int)(ago.TotalHours);
                return CombineValueAndRequiredForm(hours, HOUR);
            }

            if (ago.TotalDays < 7)
            {
                int days = (int)(ago.TotalDays);
                return CombineValueAndRequiredForm(days, DAY);
            }

            if (ago.TotalDays < 365)
            {
                int weeks = (int)(ago.TotalDays / 7);
                return CombineValueAndRequiredForm(weeks, WEEK);
            }

            int years = (int)(ago.TotalDays / 365);
            return CombineValueAndRequiredForm(years, YEAR);
        }



        private string CombineValueAndRequiredForm(int value, string template)
        {
            return string.Format("{0} {1} {2}", value, MakePluralIfRequired(value, template), AGO);
        }

        private string MakePluralIfRequired(int value, string singular)
        {
            return value == 1 ? singular : singular + "s";
        }
    }
}
