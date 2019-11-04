using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;
namespace Model.Customers.Validations
{
    /// <summary>
    /// اضافه شده توسط محمد ظفری
    /// </summary>
    class ProvinceBusinessRules
    {
        public static readonly BusinessRule ProvinceNameRequired=new BusinessRule("ProvinceName","نام استان باید وارد شود");
    }
}
