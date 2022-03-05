using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace LearningContest.Application.Extension
{
    public static class DateTimeExtensions
    {
        public static string ConvertToPersianDate(this DateTime inputDate)
        {
            PersianCalendar pc = new PersianCalendar();
            return pc.GetYear(inputDate).ToString().PadLeft(4, '0') + "/" + pc.GetMonth(inputDate).ToString().PadLeft(2, '0') + "/" + pc.GetDayOfMonth(inputDate).ToString().PadLeft(2, '0');

        }

        public static DateTime ConvertToGeorgianDate(this string persianDate)
        {

            var info = persianDate.Split('/');
            if (info.Length >= 3)
            {
                PersianCalendar pc = new PersianCalendar();
                DateTime dt = new DateTime(int.Parse(info[0]), int.Parse(info[1]), int.Parse(info[2]), pc);

                return dt;
            }
            else
            {
                return DateTime.MinValue;
            }
        }
    }
}
