using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Querying;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Interfaces
{
    public interface ISupportDeliverServiceService
    {

        GetGeneralResponse<IEnumerable<SupportDeliverServiceView>> GetSupportDeliverServices(int pageSize,
            int pageNumber,IList<FilterData> filter);

        GetGeneralResponse<SupportDeliverServiceView> GetSupportDeliverService(Guid SupportID);

        GeneralResponse AddSeupportDeliverService(AddSupportDeliverServiceRequest request, Guid CreateEmployeeID);

        GeneralResponse EditSupportDeliverService(EditSupportDeliverServiceRequest request, Guid ModifiedEmployeeID);

        GeneralResponse DeleteSupportDeliverService(DeleteRequest request);
    }
}
