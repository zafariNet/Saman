using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;
namespace Model.Employees.Validations
{
    public class ToDoBusinessRules
    {
        public static readonly BusinessRule ToDoTitlerequired = new BusinessRule("ToDoTitle", "عنوان وظیفه نمتواند خالی باشد");
        public static readonly BusinessRule ToDoDescriptionRequired = new BusinessRule("ToDoDescription", "توضیحات وظیفه نمیتواند خالی باشد");
        public static readonly BusinessRule StartDateRequired = new BusinessRule("ToDoDescription", "تاریخ شروع وظیفه باید وارد شود");
        public static readonly BusinessRule EndDateDateRequired = new BusinessRule("EndDate", "تاریخ پایان وظیفه نمیتواند خالی باشد");
        public static readonly BusinessRule StartTimeDateRequired = new BusinessRule("EndDate", "زمان شروع وظیفه نمیتواند خالی باشد");
        public static readonly BusinessRule EndTimeDateRequired = new BusinessRule("EndDate", "زمان پایان وظیفه نمیتواند خالی باشد");
    }
}
