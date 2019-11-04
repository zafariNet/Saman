using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Fiscals;
using Services.ViewModels.Fiscals;
using AutoMapper;

namespace Services.Mapping
{
    public static class MoneyAccountEmployeeMapper
    {
        public static IEnumerable<MoneyAccountEmployeeView> ConvertToMoneyAccountEmployeeViews(
            this IEnumerable<MoneyAccountEmployee> moneyAccountEmployees)
        {
            List<MoneyAccountEmployeeView> returnMoneyAccountEmployees = new List<MoneyAccountEmployeeView>();

            foreach (MoneyAccountEmployee networkCenter in moneyAccountEmployees)
            {
                returnMoneyAccountEmployees.Add(networkCenter.ConvertToMoneyAccountEmployeeView());
            }

            return returnMoneyAccountEmployees;
        }

        public static MoneyAccountEmployeeView ConvertToMoneyAccountEmployeeView(this MoneyAccountEmployee moneyAccountEmployee)
        {
            return Mapper.Map<MoneyAccountEmployee, MoneyAccountEmployeeView>(moneyAccountEmployee);
        }
    }
}
