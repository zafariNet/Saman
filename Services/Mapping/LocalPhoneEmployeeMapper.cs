using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Mapping
{
    public static class LocalPhoneEmployeeMapper
    {
        public static IEnumerable<LocalPhoneStoreEmployeeView> ConvertToLocalPhoneEmployeeViews(
        this IEnumerable<LocalPhoneStoreEmployee> localPhoneEmployees)
        {
            return Mapper.Map<IEnumerable<LocalPhoneStoreEmployee>,
                IEnumerable<LocalPhoneStoreEmployeeView>>(localPhoneEmployees);
        }

        public static LocalPhoneStoreEmployeeView ConvertToLocalPhoneEmployeeView(this LocalPhoneStoreEmployee localPhoneEmployee)
        {
            return Mapper.Map<LocalPhoneStoreEmployee, LocalPhoneStoreEmployeeView>(localPhoneEmployee);
        }
    }
}
