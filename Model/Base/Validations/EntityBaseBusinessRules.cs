using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model.Base.Validations
{
    /// <summary>
    /// اعتبارسنجی مربوط به کلاس پایه
    /// </summary>
    public class EntityBaseBusinessRules
    {
        public static readonly BusinessRule CreateDateRequired = new BusinessRule("CreateDate", "تاریخ ایجاد رکورد باید وارد شود");
        public static readonly BusinessRule CreateDateIsInvalid = new BusinessRule("CreateDateIsInvalid", "تاریخ ایجاد رکورد نامعتبر است");

        public static readonly BusinessRule MidifiedDateIsInvalid = new BusinessRule("CreateDateIsInvalid", "تاریخ ویرایش رکورد نامعتبر است");
    }
}
