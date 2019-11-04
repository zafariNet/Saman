using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Support;
using Services.ViewModels.Support;

namespace Services.Mapping
{
    public static class SupportDeliverServiceMapper
    {
        public static IEnumerable<SupportDeliverServiceView> ConvertToSupportDeliverServiceViews(
            this IEnumerable<SupportDeliverService> supportDeliverServices)
        {
            return Mapper.Map<IEnumerable<SupportDeliverService>,
                IEnumerable<SupportDeliverServiceView>>(supportDeliverServices);
        }

        public static SupportDeliverServiceView ConvertToSupportDeliverServiceView(
            this SupportDeliverService supportDeliverService)
        {
            return Mapper.Map<SupportDeliverService, SupportDeliverServiceView>(supportDeliverService);
        }
    }
}
