using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Services.ViewModels.Store;
using AutoMapper;

namespace Services.Mapping
{
    public static class NetworkCreditMapper
    {
        public static IEnumerable<NetworkCreditView> ConvertToNetworkCreditViews(
            this IEnumerable<NetworkCredit> networkCredits)
        {
            return Mapper.Map<IEnumerable<NetworkCredit>,
                IEnumerable<NetworkCreditView>>(networkCredits);
        }

        public static NetworkCreditView ConvertToNetworkCreditView(this NetworkCredit networkCredit)
        {
            return Mapper.Map<NetworkCredit, NetworkCreditView>(networkCredit);
        }
    }
}
