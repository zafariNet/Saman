using System;
using System.Collections.Generic;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class AgencyMapper
    {
        public static IEnumerable<AgencyView> ConvertToAgencyViews(
            this IEnumerable<Agency> agencys)
        {
            return Mapper.Map<IEnumerable<Agency>,
                IEnumerable<AgencyView>>(agencys);
        }

        public static AgencyView ConvertToAgencyView(this Agency agency)
        {
            return Mapper.Map<Agency, AgencyView>(agency);
        }
    }
}
