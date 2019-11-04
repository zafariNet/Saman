using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Querying;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;
using Services.ViewModels.Support;
using Services.ViewModels.Reports;

namespace Services.Interfaces
{
    public interface ISupportService
    {
        GetGeneralResponse<IEnumerable<SupportView>> GetSupports(int pageSize,int pageNumber , IList<FilterData> filter,IList<Sort> sort,int LastState);
        GetGeneralResponse<IEnumerable<SupportOwnView>> GetOwnSupports(Guid EmployeeID, int pageSize, int pageNumber, IList<FilterData> filter, IList<Sort> sort);

        GetGeneralResponse<IEnumerable<SupportView>> GetSupports(int pageSize, int pageNumber,
            Guid customerID, IList<Sort> sort);

        GetGeneralResponse<SupportView> GetOneSupport(Guid SupportID);

        GetGeneralResponse<IEnumerable<SupportView>> GetSupports();
     
        //GetGeneralResponse<IEnumerable<SupportView>> GetSupportsbyEmployee(Guid EmployeeID);
        
        GeneralResponse AddSupport(AddSupportRequest request, Guid CreateEmployeeID);
        
        GeneralResponse EditSupport(EditSupportRequest request, Guid ModifiedEmployeeID);
        
        GeneralResponse DeleteSupport(DeleteRequest request);

        InstallFormReportView GetInstallReport(Guid SupportID);
    }
}
