using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using Model.Support;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Interfaces
{
    public interface ISupportExpertDispatchService
    {
        GetGeneralResponse<IEnumerable<SupportExpertDispatchView>> GetSupportExpertDispaches();

        GetGeneralResponse<SupportExpertDispatchView> GetSupportExpertDispach(Guid SupportID);

        GeneralResponse AddSupportExpertDispatch(AddSupportExpertDispatchRequest request, Guid CreateemployeeID);

        GeneralResponse EditSupportExpertDispatch(EditSupportExpertDispatchRequest request, Guid ModifiedEmployeeID);

        GeneralResponse DeleteSupportExpertDispatch(DeleteRequest request);
    }
}
