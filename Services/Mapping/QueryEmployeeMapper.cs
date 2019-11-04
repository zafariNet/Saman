using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class QueryEmployeeMapper
    {
        public static IEnumerable<QueryEmployeeView> ConvertToQueryEmployeeViews(
            this IEnumerable<QueryEmployee> queryEmployees)
        {
            List<QueryEmployeeView> returnQueryEmployee = new List<QueryEmployeeView>();

            foreach (QueryEmployee queryEmployee in queryEmployees)
            {
                returnQueryEmployee.Add(queryEmployee.ConvertToQueryEmployeeView());
            }

            return returnQueryEmployee;
        }

        public static QueryEmployeeView ConvertToQueryEmployeeView(this QueryEmployee queryEmployee)
        {
            return Mapper.Map<QueryEmployee, QueryEmployeeView>(queryEmployee);
        }
    }
}
