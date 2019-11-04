using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using Services.ViewModels.Customers;
using Infrastructure.Querying;

namespace Services.Interfaces
{
    public interface ICodeService
    {
        GeneralResponse AddCode(AddCodeRequestOld request);
        GeneralResponse EditCode(EditCodeRequestOld request);
        GeneralResponse DeleteCode(DeleteRequest request);
        GetCodeResponse GetCode(GetRequest request);
        GetCodesResponse GetCodes(Guid CenterID);

        GetGeneralResponse<IEnumerable<CodeView>> GetCodes(int pageSize, int pageNumber,IList<Sort> sort);
        GetGeneralResponse<IEnumerable<CodeView>> GetCodes(Guid centerID, int pageSize, int pageNumber,IList<Sort> sort);

        GeneralResponse EditCode(IEnumerable<EditCodeRequest> request, Guid ModifiedEmployeeID);
        GeneralResponse AddCode(IEnumerable<AddCodeRequest> requests , Guid CenterID, Guid CreateEmployeeID);

         GeneralResponse DeleteCode(IEnumerable<DeleteRequest> requests);
        GeneralResponse ChangeCodeName();
        GeneralResponse ChangeCenter(Guid ID, Guid CenterID);
    }
}
