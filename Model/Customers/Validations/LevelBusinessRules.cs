using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Customers.Validations
{
    public class LevelBusinessRules
    {
        public static readonly BusinessRule LevelTypeRequired = new BusinessRule("LevelType", "نوع چرخه باید وارد شود");
        public static readonly BusinessRule LevelTitleRequired = new BusinessRule("LevelTitle", "عنوان مرحله باید وارد شود");
        public static readonly BusinessRule LevelNiknameRequired = new BusinessRule("LevelNikname", "نام مستعار باید وارد شود");
    }
}
