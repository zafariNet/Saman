using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;

namespace Services.Interfaces
{
    public interface ISupportStatusRelationService
    {
        GetGeneralResponse<IEnumerable<SupportStatusRelationView>> GetSupportStatuseRelations();
        GetGeneralResponse<IEnumerable<SupportStatusRelationView>> GetSupportStatuseRelations(Guid SupportStatusID);
        GeneralResponse AddSupportStatusRelation(IEnumerable<SupportStatusRelationView> requests, Guid SupportStatusID);
        GeneralResponse EditSupportStatusRelation(IEnumerable<EditSupportStatusRelationRequest> requests,Guid SupportStatusID);
        GeneralResponse DeleteSupportStatusRelations(IEnumerable<SupportStatusRelationView> requests);
    }
}
