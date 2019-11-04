using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Support;
using Services.ViewModels.Support;

namespace Services.Mapping
{
    public static class SupportExpertDispatchMapper
    {
        public static IEnumerable<SupportExpertDispatchView> ConvertToSupportExpertDispatchViews(
            this IEnumerable<SupportExpertDispatch> supportExpertDispatches)
        {
            return Mapper.Map<IEnumerable<SupportExpertDispatch>,
                IEnumerable<SupportExpertDispatchView>>(supportExpertDispatches);
        }

        public static SupportExpertDispatchView ConvertToSupportExpertDispatchView(
            this SupportExpertDispatch supportExpertDispatch)
        {
            return Mapper.Map<SupportExpertDispatch, SupportExpertDispatchView>(supportExpertDispatch);
        }




        public static IEnumerable<SupportOwnView> ConvertToSupportOwnViews(
    this IEnumerable<SupportExpertDispatch> supportExpertDispatches)
        {
            return Mapper.Map<IEnumerable<SupportExpertDispatch>,
                IEnumerable<SupportOwnView>>(supportExpertDispatches);
        }

        public static SupportOwnView ConvertToSupportOwnView(
            this SupportExpertDispatch supportExpertDispatch)
        {
            return Mapper.Map<SupportExpertDispatch, SupportOwnView>(supportExpertDispatch);
        }

    }
}
