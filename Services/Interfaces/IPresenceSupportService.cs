using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.SupportCatalogService;
using Services.Messaging;
using Services.ViewModels.Support;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface IPersenceSupportService
    {
        GeneralResponse AddPersenceSupport(AddPersenceSupportRequest request);
        GeneralResponse EditPersenceSupport(EditPersenceSupportRequest request);
        GeneralResponse DeletePersenceSupport(DeleteRequest request);
        GetPersenceSupportResponse GetPersenceSupport(GetRequest request);
        GetPersenceSupportsResponse GetPersenceSupports();

        //GetGeneralResponse<IEnumerable<PersenceSupportView>> GetPersenceSupports(Guid customerID, int pageSize, int pageNumber);
        //Added By Zafari
        GetGeneralResponse<IEnumerable<PersenceSupportView>> GetCustomizedPersenceSupports(Guid? CreateEmployeeID, Guid? CustomerID, int? SupportType,
            string StartCreateDate ,string EndCreateDate, string StartPlanDate, string EndPlanDate, Guid? Installer, bool? Deliverd, string StartDeliverDate, 
            string EndDeliverDate, int PageSize, int PageNumber,IList<Sort> sort);
        GeneralResponse DeliverPersenceSupport(Guid PersenceSupportID, Guid InstallerID, bool Delivered, string DeliverDate, string DeliverNote, string DeliverTime, long ReceivedCost, int RowVersion);
    }
}
