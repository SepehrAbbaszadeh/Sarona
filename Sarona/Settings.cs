using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona
{
    public static class Settings
    {
        public static int PageSize  => 25;
        public static string TableClickableClass => "table thead-light table-bordered table-striped table-hover clickable-row table-sm";
        public static string TableClass => "table thead-light table-bordered table-striped table-hover table-sm";
        public static string Col3 => "col-md-3";
        public static string Col9 => "col-md-9";

        public static string GetDateTimeNow()
        {
            var now = DateTime.Now;
            PersianCalendar calendar = new PersianCalendar();
            return $"{calendar.GetYear(now)}_{calendar.GetMonth(now)}_{calendar.GetDayOfMonth(now)}T{now.Hour}_{now.Minute}_{now.Second}";
        }
    }
}
