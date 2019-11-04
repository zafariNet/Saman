using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.ViewModels.Customers;

namespace Services.Interfaces
{
    public interface IFollowStatusService
    {
        GetGeneralResponse<IEnumerable<FollowStatusView>> GetFollowStatuss();
    }
}
