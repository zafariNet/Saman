using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Employees;
using Services.ViewModels.Employees;
using AutoMapper;

namespace Services.Mapping
{
    public static class EmployeeMapper
    {
        public static IEnumerable<EmployeeView> ConvertToEmployeeViews(
            this IEnumerable<Employee> employees)
        {
            List<EmployeeView> returnEmployees = new List<EmployeeView>();

            foreach (Employee employee in employees)
            {
                returnEmployees.Add(employee.ConvertToEmployeeView());
            }

            return returnEmployees;
        }

        public static EmployeeView ConvertToEmployeeView(this Employee employee)
        {
            return Mapper.Map<Employee, EmployeeView>(employee);
        }

        public static IEnumerable<EmployeeWithChildView> ConvertToEmployeeWithChildsViews(
    this IEnumerable<Employee> employees)
        {
            List<EmployeeWithChildView> returnEmployees = new List<EmployeeWithChildView>();

            foreach (Employee employee in employees)
            {
                returnEmployees.Add(employee.ConvertToEmployeeWithChildsView());
            }

            return returnEmployees;
        }

        public static EmployeeWithChildView ConvertToEmployeeWithChildsView(this Employee employee)
        {
            return Mapper.Map<Employee, EmployeeWithChildView>(employee);
        }
    }
}
