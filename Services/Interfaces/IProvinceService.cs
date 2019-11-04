using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging;
using System.Collections;
using Services.ViewModels.Customers;
namespace Services.Interfaces
{
    /// <summary>
    /// ایجاد شده توسط محمد ظفری
    /// </summary>
    public interface IProvinceService
    {
        GeneralResponse AddProvince(AddProvinceRequest request);
        GeneralResponse EditProvince(EditProvinceRequest request);
        GeneralResponse DeleteProvince(DeleteRequest request);
        GetGeneralResponse<ProvinceView> GetProvince(GetRequest request);
        GetGeneralResponse<IEnumerable<ProvinceView>> GetProvinces();
        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);

        GetGeneralResponse<IEnumerable<ProvinceView>> GetProvinces(int pageSize, int pageNumber);
    }
}
