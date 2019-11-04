#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.ViewModels.Customers;
#endregion

namespace Services.Interfaces
{
    public interface IDocumentStatusService
    {
        GeneralResponse AddDocumentStatus(AddDocumentStatusRequest request);
        GeneralResponse EditDocumentStatus(EditDocumentStatusRequest request);
        GeneralResponse DeleteDocumentStatus(DeleteRequest request);
        GetDocumentStatusResponse GetDocumentStatus(GetRequest request);
        GetDocumentStatussResponse GetDocumentStatuss();
        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);

        GeneralResponse AddDocumentStatuss(IEnumerable<AddDocumentStatusRequest> requests);
        GeneralResponse EditDocumentStatuss(IEnumerable<EditDocumentStatusRequest> requests);

        GetGeneralResponse<IEnumerable<DocumentStatusView>> GetDocumentStatuss(int pageSize, int pageNumber);
    }
}
