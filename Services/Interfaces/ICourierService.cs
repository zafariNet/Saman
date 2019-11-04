using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Querying;
using Services.Messaging;
using Services.Messaging.SaleCatalogService;
using Services.ViewModels.Sales;

namespace Services.Interfaces
{
    public interface ICourierService
    {
        GetGeneralResponse<IEnumerable<CourierView>> GetAllCouriers(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort);

        GetGeneralResponse<IEnumerable<CourierView>> GetAllCouriersByEmployee(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort, Guid EmployeeID);

        GetGeneralResponse<IEnumerable<CourierView>> GetCstomerCouriers(int pageSize, int pageNumber,Guid CustomerID);

        GeneralResponse AddCourier(AddCourierRequest request, Guid CreateEmployeeID);

        GeneralResponse EditCourier(EditCourierRequest request, Guid ModifiedEmployeeID);

        GeneralResponse DoCourierAction(Guid CourierID, int CourierStatuse, string ExpertComment,Guid CourierEployeeID,
            Guid ModifiedEmployeeID);

        GeneralResponse DeleteCourier(DeleteRequest request);
    }
}
