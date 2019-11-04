using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Store.Validations
{
    public class ProductBusinessRules
    {
        public static readonly BusinessRule ProductCategoryRequired = new BusinessRule("ProductCategory", "گروه کالا باید وارد شود");
        public static readonly BusinessRule ProductNameRequired = new BusinessRule("ProductName", "نام کالا باید وارد شود");
    }
}
