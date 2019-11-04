using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class NetworkCenterMapper
    {
        public static IEnumerable<NetworkCenterView> ConvertToNetworkCenterViews(
            this IEnumerable<NetworkCenter> networkCenters)
        {
            List<NetworkCenterView> returnNetworkCenters = new List<NetworkCenterView>();

            foreach (NetworkCenter networkCenter in networkCenters)
            {
                returnNetworkCenters.Add(networkCenter.ConvertToNetworkCenterView());
            }

            return returnNetworkCenters;
        }

        public static NetworkCenterView ConvertToNetworkCenterView(this NetworkCenter networkCenter)
        {
            return Mapper.Map<NetworkCenter, NetworkCenterView>(networkCenter);
        }
    }
}
