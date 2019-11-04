using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Support;
using Services.ViewModels.Support;

namespace Services.Mapping
{
    public static class SupportQcMapper
    {
        public static IEnumerable<SupportQcView> ConvertToSupportQcViews(
            this IEnumerable<SupportQc> supportQcs)
        {
            return Mapper.Map<IEnumerable<SupportQc>,
                IEnumerable<SupportQcView>>(supportQcs);
        }

        public static SupportQcView ConvertToSupportQcView(
            this SupportQc supportQc)
        {
            return Mapper.Map<SupportQc, SupportQcView>(supportQc);
        }
    }
}
