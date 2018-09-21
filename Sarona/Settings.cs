using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace Sarona
{
    public static class Settings
    {
        public const string AdminsRole = "Admins";
        public const string TrafficRole = "Traffic";
        public const string UsersRole = "Users";
        public const int PageSize  = 50;
        public static string TableClickableClass => "table table-bordered table-striped table-hover clickable-row table-sm";
        public static string TableClass => "table table-bordered table-striped table-hover table-sm";
        public static string Col3 => "col-md-3";
        public static string Col9 => "col-md-9";

        public static string GetDateTimeNowFile()
        {
            var now = DateTime.Now;
            PersianCalendar calendar = new PersianCalendar();
            return String.Format("{0:0000}{1:00}{2:00} {3:00}{4:00}{5:00}"
                , calendar.GetYear(now), calendar.GetMonth(now), calendar.GetDayOfMonth(now),
                now.Hour, now.Minute, now.Second);
            
        }

        public static string GetPersianDate(this DateTime date)
        {
            PersianCalendar jc = new PersianCalendar();
            return string.Format("{0:0000}/{1:00}/{2:00} {3:00}:{4:00}:{5:00}", jc.GetYear(date), jc.GetMonth(date), jc.GetDayOfMonth(date)
                ,date.Hour,date.Minute,date.Second);
        }


        
    }
}
