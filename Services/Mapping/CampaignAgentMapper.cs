using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Employees;
using Services.ViewModels.Employees;

namespace Services.Mapping
{
    public static class CampaignAgentMapper
    {
        public static IEnumerable<CampaignAgentView> ConvertToCampaignAgentViews(
            this IEnumerable<CampaignAgent> campaignAgentes)
        {
            return Mapper.Map<IEnumerable<CampaignAgent>,
                IEnumerable<CampaignAgentView>>(campaignAgentes);
        }

        public static CampaignAgentView ConvertToCampaignAgentView(this CampaignAgent campaignAgent)
        {
            return Mapper.Map<CampaignAgent, CampaignAgentView>(campaignAgent);
        }
    }
}
