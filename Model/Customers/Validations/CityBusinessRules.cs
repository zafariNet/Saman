using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Base;
namespace Model.Customers.Validations
{
    class CityBusinessRules
    {
        public static readonly BusinessRule CityNameRequired=new BusinessRule("CityName","نام شهر باید وارد شود");
    }
}
