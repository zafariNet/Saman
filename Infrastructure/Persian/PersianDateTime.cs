using System;
using System.Globalization;

namespace Infrastructure.Persian
{
    public static class PersianDateTime
    {

        #region Properties

        public static string Now
        {
            get
            {
                return GetCurrentShamsiDate();
            }
        }

        public static string WeekStartDate {
            get
            {
                return GetFirstDateOfWeek(DayOfWeek.Saturday);
            }
        }
        public static string WeekEndDate{
            get
            {
                return GetLastDateOfWeek(DayOfWeek.Friday);
            }
        }

        public static string MonthStartDate
        {
            get
            {
                return GetFirstDateOfCurrentMonth();
            }
        }


        public static string MonthEndtDate
        {
            get
            {
                return GetLastDateOfCurrentMonth();
            }
        }


        public static string LastMonthStartDate
        {
            get
            {
                return GetFirstDateOfLastMonth();
            }
        }


        public static string LastMonthEndtDate
        {
            get
            {
                return GetLastDateOfLastMonth();
            }
        }

        public static string yesterday
        {
            get
            {
                return Yesterday();
            }
        }

        #endregion

        #region Private Methods

        #region Current Time

        private static string GetCurrentShamsiDate()
        {
            PersianCalendar pc = new PersianCalendar();

            int year = pc.GetYear(DateTime.Now);
            int month = pc.GetMonth(DateTime.Now);
            int day = pc.GetDayOfMonth(DateTime.Now);

            string monthStr = month < 10 ? "0" + month.ToString() : month.ToString();
            string dayStr = day < 10 ? "0" + day.ToString() : day.ToString();

            return string.Format("{0}/{1}/{2} {3}", year, monthStr, dayStr, DateTime.Now.ToString("HH:mm:ss"));
        }

        #endregion

        #region Date Diff

        public static int DateDiff(string startingDate, string endingDate)
        {
            PersianCalendar pc = new PersianCalendar();

            DateTime stDate = pc.ToDateTime(int.Parse(startingDate.Substring(0, 4)),
                int.Parse(startingDate.Substring(5, 2)), int.Parse(startingDate.Substring(8, 2)), 0, 0, 0, 0);

            DateTime enDate = pc.ToDateTime(int.Parse(endingDate.Substring(0, 4)),
                int.Parse(endingDate.Substring(5, 2)), int.Parse(endingDate.Substring(8, 2)), 0, 0, 0, 0);

             double diff = (enDate - stDate).TotalDays;
             PersianCalendar p = new PersianCalendar();
            
             return (int)diff;
        }

        #endregion

        #region First daye date of week

        /// <summary>
        /// تاریخ روز اول هفته
        /// </summary>
        /// <param name="firstDay"></param>
        /// <returns></returns>
        private static string GetFirstDateOfWeek(DayOfWeek firstDay)
        {
            DateTime firstDayInWeek = DateTime.Now;
            while (firstDayInWeek.DayOfWeek != firstDay)
                firstDayInWeek = firstDayInWeek.AddDays(-1);

            PersianCalendar pc = new PersianCalendar();

            int year = pc.GetYear(firstDayInWeek);
            int month = pc.GetMonth(firstDayInWeek);
            int day = pc.GetDayOfMonth(firstDayInWeek);

            string monthStr = month < 10 ? "0" + month.ToString() : month.ToString();
            string dayStr = day < 10 ? "0" + day.ToString() : day.ToString();

            return string.Format("{0}/{1}/{2} {3}", year, monthStr, dayStr, DateTime.Now.ToString("HH:mm:ss"));
        }

        #endregion

        #region Last day date of week 

        /// <summary>
        /// تاریخ روز آخر هفته
        /// </summary>
        /// <param name="firstDay"></param>
        /// <returns></returns>
        /// 
        private static string GetLastDateOfWeek(DayOfWeek firstDay)
        {
            DateTime lastDayInWeek = DateTime.Now;
            while (lastDayInWeek.DayOfWeek != firstDay)
                lastDayInWeek = lastDayInWeek.AddDays(1);

            PersianCalendar pc = new PersianCalendar();

            int year = pc.GetYear(lastDayInWeek);
            int month = pc.GetMonth(lastDayInWeek);
            int day = pc.GetDayOfMonth(lastDayInWeek);

            string monthStr = month < 10 ? "0" + month.ToString() : month.ToString();
            string dayStr = day < 10 ? "0" + day.ToString() : day.ToString();

            return string.Format("{0}/{1}/{2} {3}", year, monthStr, dayStr, DateTime.Now.ToString("HH:mm:ss"));
        }

        #endregion

        #region First day date of current month

        /// <summary>
        /// تاریخ روز اول ماه جاری
        /// </summary>
        /// <param name="firstDay"></param>
        /// <returns></returns>
        public static string GetFirstDateOfCurrentMonth()
        {
            DateTime firstDayInMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(DateTime.Now);
            int month = pc.GetMonth(DateTime.Now);
            int day = 1;

            string monthStr = month < 10 ? "0" + month.ToString() : month.ToString();
            string dayStr = day < 10 ? "0" + day.ToString() : day.ToString();

            return string.Format("{0}/{1}/{2} {3}", year, monthStr, dayStr, DateTime.Now.ToString("HH:mm:ss"));
        }
        #endregion

        #region last day date of current month
        /// <summary>
        /// تاریخ روز آخر ماه جاری
        /// </summary>
        /// <param name="firstDay"></param>
        /// <returns></returns>
        public static string GetLastDateOfCurrentMonth()
        {
            DateTime lastDayOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //lastDayOfTheMonth=lastDayOfTheMonth.AddMonths(1).AddDays(-1);

            PersianCalendar pc = new PersianCalendar();

            int year = pc.GetYear(DateTime.Now);
            int month = pc.GetMonth(DateTime.Now);
            int day = 31;

            string monthStr = month < 10 ? "0" + month.ToString() : month.ToString();
            string dayStr = day < 10 ? "0" + day.ToString() : day.ToString();

            return string.Format("{0}/{1}/{2} {3}", year, monthStr, dayStr, DateTime.Now.ToString("HH:mm:ss"));
        }

        #endregion

        #region First day date of last month

        /// <summary>
        /// تاریخ روز اول ماه قبل
        /// </summary>
        /// <param name="firstDay"></param>
        /// <returns></returns>
        public static string GetFirstDateOfLastMonth()
        {
            DateTime firstDayInMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            PersianCalendar pc = new PersianCalendar();

            int year = pc.GetYear(DateTime.Now);
            int month = pc.GetMonth(DateTime.Now);
            int day = 1;

            string monthStr = month < 10 ? "0" + (month-1).ToString() : (month-1).ToString();
            string dayStr = day < 10 ? "0" + day.ToString() : day.ToString();

            return string.Format("{0}/{1}/{2} {3}", year, monthStr, dayStr, DateTime.Now.ToString("HH:mm:ss"));
        }

        #endregion

        #region Last day date of last month
        /// <summary>
        /// تاریخ روز آخر ماه جاری
        /// </summary>
        /// <param name="firstDay"></param>
        /// <returns></returns>
        private static string GetLastDateOfLastMonth()
        {
            DateTime firstDayInMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            PersianCalendar pc = new PersianCalendar();

            int year = pc.GetYear(DateTime.Now);
            int month = pc.GetMonth(DateTime.Now);
            int day = 31;

            string monthStr = month < 10 ? "0" + (month - 1).ToString() : (month - 1).ToString();
            string dayStr = day < 10 ? "0" + day.ToString() : day.ToString();

            return string.Format("{0}/{1}/{2} {3}", year, monthStr, dayStr, DateTime.Now.ToString("HH:mm:ss"));
        }
        #endregion

        #region Yesterday

        public static string Yesterday()
        {
            DateTime lastDayOfTheMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);

            lastDayOfTheMonth = lastDayOfTheMonth.AddDays(-1);
            PersianCalendar pc = new PersianCalendar();
            int year = pc.GetYear(lastDayOfTheMonth);
            int month = pc.GetMonth(lastDayOfTheMonth);
            int day = pc.GetDayOfMonth(lastDayOfTheMonth);
            string monthStr = month < 10 ? "0" + month.ToString() : month.ToString();
            string dayStr = day < 10 ? "0" + day.ToString() : day.ToString();

            return string.Format("{0}/{1}/{2} {3}", year, monthStr, dayStr, DateTime.Now.ToString("HH:mm:ss"));
        }

        #endregion

        #endregion
    }
}