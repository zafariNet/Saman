using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Model.Base.Validations
{
    /// <summary>
    /// اعتبارسنجی فرمت تاریخ فارسی
    /// </summary>
    public class PersianDateTimeValidationSpecification
    {
        /// <summary>
        /// تعریف فرمت داده تاریخ فارسی
        /// </summary>
        private static Regex _dateTimeRegex =
            new Regex(@"[0-9][0-9][0-9][0-9]/(0[1-9]|1[0-2])/(0[1-9]|[1-2][0-9]|3[0-1])( (0[0-9]|1[0-9]|2[0-3]):(0[0-9]|[1-5][0-9]):(0[0-9]|[1-5][0-9])|)");

        /// <summary>
        /// چک کردن اینکه آیا تاریخ تاریخ داده شده به فرمت تاریخ فارسی میخورد یا خیر
        /// </summary>
        /// <param name="dateTime">تاریخ فارسی بصورت متنی</param>
        /// <returns>True/False</returns>
        public bool IsSatisfiedBy(string dateTime)
        {
            return _dateTimeRegex.IsMatch(dateTime);
        }
    }
}
