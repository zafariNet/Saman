using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;

namespace Model.Employees.Validations
{
    public class TaskBusinessRule
    {
        public static readonly BusinessRule MasterTaskRequired = new BusinessRule("MasterTask","این وظیفه فاقد وظیفه اصلی است.");

        public static readonly BusinessRule TaskTitleRequired = new BusinessRule("TaskTitle",
            "عنوان وظیفه باید وارد شود");
        public static readonly BusinessRule TaskDecriptionRequired = new BusinessRule("TaskDecription", "توضیحات وظیفه باید وارد شود.");
        public static readonly BusinessRule EmployeeRequired = new BusinessRule("Employee", "کارمند ارجاع شونده نمیتواند خالی باشد.");
        public static readonly BusinessRule SecondaryDescriptionRequired = new BusinessRule("SecondaryDescription", "توضیحات بستن نمیتواند خالی باشد.");
        

    }
}
