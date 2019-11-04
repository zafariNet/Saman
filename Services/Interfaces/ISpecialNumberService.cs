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
    public interface ISpecialNumberService
    {
        GeneralResponse AddSpecialNumber(AddSpecialNumberRequestOld request);
        GeneralResponse EditSpecialNumber(EditSpecialNumberRequestOld request);
        GeneralResponse DeleteSpecialNumber(DeleteRequest request);
        GetSpecialNumberResponse GetSpecialNumber(GetRequest request);
        GetSpecialNumbersResponse GetSpecialNumbers();

        GetGeneralResponse<IEnumerable<SpecialNumberView>> GetSpecialNumbers(int pageSize, int pageNumber, IList<Sort> sort);
        GeneralResponse AddSpecialNumbers(IEnumerable<AddSpecialNumberRequest> requests, Guid CreateEmployeeID);
        GeneralResponse EditSpecialNumbers(IEnumerable<EditSpecialNumberRequest> requests, Guid ModifiedEmployeeID);
        GeneralResponse DeleteSpecialNumbers(IEnumerable<DeleteRequest> requests);
    }
}
