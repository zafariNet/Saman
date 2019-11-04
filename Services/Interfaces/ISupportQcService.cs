using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Interfaces
{
    public interface ISupportQcService
    {
        GetGeneralResponse<IEnumerable<SupportQcView>> GetSupportQcs();
        
        GetGeneralResponse<SupportQcView> GetSupportQc(Guid SupportID);

        GeneralResponse AddSupportQc(AddSupportQcRequest request, Guid CreateEmployeeID);

        GeneralResponse EditSupportQc(EditSupportQcRequest request, Guid ModifiedEmployeeID);

        GeneralResponse DeleteSupportQc(DeleteRequest request);
    }
}
