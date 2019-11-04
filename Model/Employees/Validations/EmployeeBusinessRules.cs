using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Employees.Validations
{
    public class EmployeeBusinessRules
    {
        public static readonly BusinessRule LastNameRequired = new BusinessRule("LastName", "نام خانوادگی باید وارد شود");
        public static readonly BusinessRule FirstNameRequired = new BusinessRule("FirstName", "نام کارمند باید وارد شود");
        public static readonly BusinessRule GroupRequired = new BusinessRule("Group", "گروهی که کارمند عضو آن است باید وارد شود");
        public static readonly BusinessRule LoginNameRequired = new BusinessRule("LoginName", "نام کاربری باید وارد شود");
        public static readonly BusinessRule PermissionsRequired = new BusinessRule("Permissions", "دسترسی های کارمند باید وارد شود");
    }
}
