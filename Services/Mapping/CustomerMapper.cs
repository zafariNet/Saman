using System;
using System.Collections.Generic;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class CustomerMapper
    {
        public static IEnumerable<CustomerView> ConvertToCustomerViews(
            this IEnumerable<Customer> customers)
        {
            return Mapper.Map<IEnumerable<Customer>, IEnumerable<CustomerView>>(customers);
        }

        public static CustomerView ConvertToCustomerView(this Customer customer)
        {
            return Mapper.Map<Customer, CustomerView>(customer);
        }
    }
}
