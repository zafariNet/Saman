using System.Collections.Generic;
using AutoMapper;
using Model.Customers;
using Services.ViewModels.Customers;

namespace Services.Mapping
{
    public static class CustomerContactTemplateMapper
    {
        public static IEnumerable<CustomerContactTemplateView> ConvertToCustomerContactTemplateViews(
            this IEnumerable<CustomerContactTemplate> customerContactTemplates)
        {
            return Mapper.Map<IEnumerable<CustomerContactTemplate>,
                IEnumerable<CustomerContactTemplateView>>(customerContactTemplates);
        }

        public static CustomerContactTemplateView ConvertToCustomerContactTemplateView(
            this CustomerContactTemplate customerContactTemplate)
        {
            return Mapper.Map<CustomerContactTemplate, CustomerContactTemplateView>(customerContactTemplate);
        }
    }
}
