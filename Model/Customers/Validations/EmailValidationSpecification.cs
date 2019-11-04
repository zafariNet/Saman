using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Model.Customers.Validations
{
    /// <summary>
    /// اعتبارسنجی ایمیل
    /// </summary>
    public class EmailValidationSpecification
    {
        /// <summary>
        /// فرمت قابل قبول برای ایمیل
        /// </summary>
        private static Regex _emailregex 
                 = new Regex(@"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

        /// <summary>
        /// چک کردن اینکه ایمیل وارد شده معتبر می باشد یا خیر
        /// </summary>
        /// <param name="email">ایمیل</param>
        /// <returns>True/False</returns>
        public bool IsSatisfiedBy(string email)
        {
            return _emailregex.IsMatch(email);
        }
    }
}
