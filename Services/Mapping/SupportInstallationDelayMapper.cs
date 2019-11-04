using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Support;
using Services.ViewModels.Support;

namespace Services.Mapping
{
    public static class SupportInstallationDelayMapper
    {
        public static IEnumerable<SupportInstallationDelayView> ConvertToSupportInstallationDelayViews(
            this IEnumerable<SupportInstallationDelay> supportInstallationDelaies)
        {
            return Mapper.Map<IEnumerable<SupportInstallationDelay>,
                IEnumerable<SupportInstallationDelayView>>(supportInstallationDelaies);
        }

        public static SupportInstallationDelayView ConvertToSupportInstallationDelayView(
            this SupportInstallationDelay supportInstallationDelay)
        {
            return Mapper.Map<SupportInstallationDelay, SupportInstallationDelayView>(supportInstallationDelay);
        }
    }
}
