using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Model.Customers;
using Services.ViewModels.Customers;

namespace Services.Mapping
{
    public static class NetworkCenterPriorityMapper
    {
        public static IEnumerable<NetworkCenterPriorityView> ConvertToNetworkCenterPriorityViews(
            this IEnumerable<NetworkCenterPriority> networkCenterPriorities)
        {
            return Mapper.Map<IEnumerable<NetworkCenterPriority>,
                IEnumerable<NetworkCenterPriorityView>>(networkCenterPriorities);
        }

        public static NetworkCenterPriorityView ConvertToNetworkCenterPriorityView(
            this NetworkCenterPriority networkCenterPriority)
        {
            return Mapper.Map<NetworkCenterPriority, NetworkCenterPriorityView>(networkCenterPriority);
        }
    }
}
