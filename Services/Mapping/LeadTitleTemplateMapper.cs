using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Customers;
using Model.Lead;
using Model.Leads;
using Services.ViewModels.Customers;
using Services.ViewModels.Leads;

namespace Services.Mapping
{
    public static class LeadTitleTemplateMapper
    {
        public static IEnumerable<LeadTitleTemplateView> ConvertToLeadTitleTemplateViews(
     this IEnumerable<LeadTitleTemplate> leadTitleTemplates)
        {
            return Mapper.Map<IEnumerable<LeadTitleTemplate>,
                IEnumerable<LeadTitleTemplateView>>(leadTitleTemplates);
        }

        public static LeadTitleTemplateView ConvertToAgencyView(this LeadTitleTemplate leadTitleTemplate)
        {
            return Mapper.Map<LeadTitleTemplate, LeadTitleTemplateView>(leadTitleTemplate);
        }
    }
}
