using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Store;
using Services.ViewModels.Store;
using AutoMapper;

namespace Services.Mapping
{
    public static class NetworkMapper
    {
        public static IEnumerable<NetworkView> ConvertToNetworkViews(this IEnumerable<Network> networks)
        {
            return Mapper.Map<IEnumerable<Network>, IEnumerable<NetworkView>>(networks);
        }

        public static NetworkView ConvertToNetworkView(this Network network)
        {
            return Mapper.Map<Network, NetworkView>(network);
        }

        public static IEnumerable<NetworkSummaryView> ConvertToNetworkSummaryViews(this IEnumerable<Network> networks)
        {
            return Mapper.Map<IEnumerable<Network>, IEnumerable<NetworkSummaryView>>(networks);
        }

        public static NetworkSummaryView ConvertToNetworkSummaryView(this Network network)
        {
            return Mapper.Map<Network, NetworkSummaryView>(network);
        }
    }
}
