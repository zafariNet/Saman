using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Model.Base;

namespace Model.Customers.Validations
{
    /// <summary>
    /// قوانین اعتبارسنجی موجودیت مشتری
    /// </summary>
    public class CustomerBusinessRules
    {
        public static readonly BusinessRule EmailIsInvalid = 
            new BusinessRule("Email", "ایمیل مشتری معتبر نمی باشد. لطفاَ یک ایمیل معتبر وارد کنید");
    }
}
