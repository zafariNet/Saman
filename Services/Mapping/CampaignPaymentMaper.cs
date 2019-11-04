using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Sales;
using Services.ViewModels.Sales;

namespace Services.Mapping
{
    public static class CampaignPaymentMaper
    {
        public static IEnumerable<CampaignPaymentView> ConvertToCampaignPaymentViews(
            this IEnumerable<CampaignPayment> campaignPaymentes)
        {
            return Mapper.Map<IEnumerable<CampaignPayment>,
                IEnumerable<CampaignPaymentView>>(campaignPaymentes);
        }

        public static CampaignPaymentView ConvertToCampaignPaymentView(this CampaignPayment campaignPayment)
        {
            return Mapper.Map<CampaignPayment, CampaignPaymentView>(campaignPayment);
        }
    }
}
