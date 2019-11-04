using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Base
{
    /// <summary>
    /// قوانین مربوط به اعتبارسنجی داده ها
    /// </summary>
    public class BusinessRule
    {
        string _property;
        string _rule;

        /// <summary>
        /// متد سازنده کلاس
        /// </summary>
        /// <param name="property">خصوصیت</param>
        /// <param name="rule">قانون</param>
        public BusinessRule(string property, string rule)
        {
            _property = property;
            _rule = rule;
        }

        public string Property
        {
            get { return _property; }
            set { _property = value; }
        }

        public string Rule
        {
            get { return _rule; }
            set { _rule = value; }
        }
    }
}
