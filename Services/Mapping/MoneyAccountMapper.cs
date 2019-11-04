using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Fiscals;
using Services.ViewModels.Fiscals;
using AutoMapper;

namespace Services.Mapping
{
    public static class MoneyAccountMapper
    {
        public static IEnumerable<MoneyAccountView> ConvertToMoneyAccountViews(
            this IEnumerable<MoneyAccount> moneyAccounts)
        {
            List<MoneyAccountView> returnMoneyAccounts = new List<MoneyAccountView>();

            foreach (MoneyAccount networkCenter in moneyAccounts)
            {
                returnMoneyAccounts.Add(networkCenter.ConvertToMoneyAccountView());
            }

            return returnMoneyAccounts;
        }

        public static MoneyAccountView ConvertToMoneyAccountView(this MoneyAccount moneyAccount)
        {
            return Mapper.Map<MoneyAccount, MoneyAccountView>(moneyAccount);
        }
    }
}
