using System;
using System.Collections.Generic;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class CustomerLevelMapper
    {
        public static IEnumerable<CustomerLevelView> ConvertToCustomerLevelViews(
            this IEnumerable<CustomerLevel> customerLevels)
        {
            return Mapper.Map<IEnumerable<CustomerLevel>,
                IEnumerable<CustomerLevelView>>(customerLevels);
        }

        public static CustomerLevelView ConvertToCustomerLevelView(this CustomerLevel customerLevel)
        {
            return Mapper.Map<CustomerLevel, CustomerLevelView>(customerLevel);
        }
    }
}
