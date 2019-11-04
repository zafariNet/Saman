using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Leads;
using Services.ViewModels.Leads;

namespace Services.Mapping
{
    public static class LeadResultTemplateMapper
    {
        public static IEnumerable<LeadResultTemplateView> ConvertToLeadResultTemplateViews(
            this IEnumerable<LeadResultTemplate> leadResultTemplates)
        {
            return Mapper.Map<IEnumerable<LeadResultTemplate>,
                IEnumerable<LeadResultTemplateView>>(leadResultTemplates);
        }

        public static LeadResultTemplateView ConvertToLeadResultTemplateView(this LeadResultTemplate leadResultTemplate)
        {
            return Mapper.Map<LeadResultTemplate, LeadResultTemplateView>(leadResultTemplate);
        }
    }
}
