using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.ViewModels.Customers;
using AutoMapper;

namespace Services.Mapping
{
    public static class FollowStatusMapper
    {
        public static IEnumerable<FollowStatusView> ConvertToFollowStatusViews(
            this IEnumerable<FollowStatus> followStatuss)
        {
            return Mapper.Map<IEnumerable<FollowStatus>,
                IEnumerable<FollowStatusView>>(followStatuss);
        }

        public static FollowStatusView ConvertToFollowStatusView(this FollowStatus followStatus)
        {
            return Mapper.Map<FollowStatus, FollowStatusView>(followStatus);
        }
    }
}
