using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class ProductCategoryBusinessRules
    {
        public static readonly BusinessRule ProductCategoryNameRequired = new BusinessRule("ProductCategoryName", "نام نمایندگی باید وارد شود");
    }
}
