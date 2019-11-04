using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class SuctionModeDetailMapper
    {
        public static IEnumerable<SuctionModeDetailview> ConvertToSuctionModeViews(
            this IEnumerable<SuctionModeDetail> suctionModeDetails)
        {
            return Mapper.Map<IEnumerable<SuctionModeDetail>,
                IEnumerable<SuctionModeDetailview>>(suctionModeDetails);
        }

        public static SuctionModeDetailview ConvertToSuctionModeView(this SuctionModeDetail suctionModeDetails)
        {
            return Mapper.Map<SuctionModeDetail, SuctionModeDetailview>(suctionModeDetails);
        }
    }
}
