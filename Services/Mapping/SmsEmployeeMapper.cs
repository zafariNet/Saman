using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;

namespace Services.Mapping
{
    public static class SmsEmployeeMapper
    {
        public static IEnumerable<SmsEmployeeView> ConvertTosmsEmployeeViews(
            this IEnumerable<SmsEmployee> smsEmployees)
        {
            return Mapper.Map<IEnumerable<SmsEmployee>,
                IEnumerable<SmsEmployeeView>>(smsEmployees);
        }

        public static SmsEmployeeView ConvertTosmsEmployeView(this SmsEmployee smsEmployee)
        {
            return Mapper.Map<SmsEmployee, SmsEmployeeView>(smsEmployee);
        }
    }
}
