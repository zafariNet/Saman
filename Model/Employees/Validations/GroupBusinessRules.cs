using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Employees.Validations
{
    public class GroupBusinessRules
    {
        public static readonly BusinessRule GroupNameRequired = new BusinessRule("GroupName", "گروه کاربری باید وارد شود");
        public static readonly BusinessRule PermissionsRequired = new BusinessRule("Permissions", "دسترسیها باید وارد شود");
    }
}
