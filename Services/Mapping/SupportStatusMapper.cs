using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Support;
using Services.ViewModels.Support;

namespace Services.Mapping
{
    public static class SupportStatusMapper
    {
        public static IEnumerable<SupportStatusView> ConvertToSupportStatusViews(
            this IEnumerable<SupportStatus> supportStatuses)
        {
            return Mapper.Map<IEnumerable<SupportStatus>,
                IEnumerable<SupportStatusView>>(supportStatuses);
        }

        public static SupportStatusView ConvertToSupportStatusView(this SupportStatus supportStatus)
        {
            return Mapper.Map<SupportStatus, SupportStatusView>(supportStatus);
        }
    }
}
