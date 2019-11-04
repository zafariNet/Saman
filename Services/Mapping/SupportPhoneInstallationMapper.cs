using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Support;
using Services.ViewModels.Support;

namespace Services.Mapping
{
    public static class SupportPhoneInstallationMapper
    {
        public static IEnumerable<SupportPhoneInstallationView> ConvertToSupportPhoneInstallationViews(
            this IEnumerable<SupportPhoneInstallation> supportPhoneInstallations)
        {
            return Mapper.Map<IEnumerable<SupportPhoneInstallation>,
                IEnumerable<SupportPhoneInstallationView>>(supportPhoneInstallations);
        }

        public static SupportPhoneInstallationView ConvertToSupportPhoneInstallationView(
            this SupportPhoneInstallation supportPhoneInstallation)
        {
            return Mapper.Map<SupportPhoneInstallation, SupportPhoneInstallationView>(supportPhoneInstallation);
        }
    }
}
