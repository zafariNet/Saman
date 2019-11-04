using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Infrastructure.Helpers
{
    public static class PriceHelper
    {
        /// <summary>
        /// قالب بندی مربوط به فرمت پولی
        /// </summary>
        /// <param name="price">قیمت</param>
        /// <returns></returns>
        public static string FormatMoney(this decimal price)
        {
            return string.Format("ريال{0}", price);
        }
    }
}
