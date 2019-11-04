using System;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Infrastructure.Querying;
using System.Collections.Generic;

namespace Services.Interfaces
{
    public interface IDocumentService
    {
        GeneralResponse AddDocument(AddDocumentRequest request);
        GeneralResponse EditDocument(EditDocumentRequest request);
        GeneralResponse DeleteDocument(DeleteRequest request);
        GetDocumentResponse GetDocument(GetRequest request);
        GetDocumentsResponse GetDocumentsBy(Guid customerID,IList<Sort> sort);
    }
}
