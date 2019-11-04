using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class SimpleCustomerMapper
    {
        public static IEnumerable<SimpleCustomerView> ConvertToSimpleCustomerViews(
    this IEnumerable<SimpleCustomer> simpleCustomers)
        {
            return Mapper.Map<IEnumerable<SimpleCustomer>,
                IEnumerable<SimpleCustomerView>>(simpleCustomers);
        }

        public static SimpleCustomerView ConvertToSimpleCustomerView(this SimpleCustomer simpleCustomer)
        {
            return Mapper.Map<SimpleCustomer, SimpleCustomerView>(simpleCustomer);
        }
    }
}
