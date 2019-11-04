using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Customers;
using Model.Support;
using Services.ViewModels.Customers;
using Services.ViewModels.Support;

namespace Services.Mapping
{
    public static class SupportMapper
    {
        public static IEnumerable<SupportView> ConvertToSupportViews(
             this IEnumerable<Support> supports)
        {
            return Mapper.Map<IEnumerable<Support>,
                IEnumerable<SupportView>>(supports);
        }

        public static SupportView ConverttoSupportView(this Support support)
        {
            return Mapper.Map<Support, SupportView>(support);
        }
    }
}
