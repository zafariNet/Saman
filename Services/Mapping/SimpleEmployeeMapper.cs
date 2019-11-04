using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Services.Mapping
{
    public static class SimpleEmployeeMapper
    {
        public static IEnumerable<SimpleEmployeeView> ConvertToSimpleEmployeeViews(
    this IEnumerable<SimpleEmployee> simpleEmployees)
        {
            List<SimpleEmployeeView> returnSimpleEmployees = new List<SimpleEmployeeView>();

            foreach (SimpleEmployee simpleEmployee in simpleEmployees)
            {
                returnSimpleEmployees.Add(simpleEmployee.ConvertToSimpleEmployeeView());
            }

            return returnSimpleEmployees;
        }

        public static SimpleEmployeeView ConvertToSimpleEmployeeView(this SimpleEmployee employee)
        {
            return Mapper.Map<SimpleEmployee, SimpleEmployeeView>(employee);
        }
    }
}
