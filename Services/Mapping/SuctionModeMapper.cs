using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class SuctionModeMapper
    {
        public static IEnumerable<SuctionModeView> ConvertToSuctionModeViews(
            this IEnumerable<SuctionMode> suctionModes)
        {
            return Mapper.Map<IEnumerable<SuctionMode>,
                IEnumerable<SuctionModeView>>(suctionModes);
        }

        public static SuctionModeView ConvertToSuctionModeView(this SuctionMode suctionMode)
        {
            return Mapper.Map<SuctionMode, SuctionModeView>(suctionMode);
        }
    }
}
