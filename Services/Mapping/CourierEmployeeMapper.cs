using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using Services.ViewModels.Customers;
using AutoMapper;
using Services.ViewModels.Employees;

namespace Services.Mapping
{
    public static class CourierEmployeeMapper
    {
        public static IEnumerable<CourierEmployeeView> ConvertToCourierEmployeeViews(
            this IEnumerable<CourierEmployee> courierEmployee)
        {
            return Mapper.Map<IEnumerable<CourierEmployee>,
                IEnumerable<CourierEmployeeView>>(courierEmployee);
        }

        public static CourierEmployeeView ConvertToCourierEmployeeView(this CourierEmployee courierEmployee)
        {
            return Mapper.Map<CourierEmployee, CourierEmployeeView>(courierEmployee);
        }
    }
}
