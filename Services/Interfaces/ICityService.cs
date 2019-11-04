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
    public interface ICityService
    {
        GeneralResponse AddCity(AddCityRequest request);
        GeneralResponse EditCity(EditCityRequest request);
        GeneralResponse DeleteCity(DeleteRequest request);
        GetGeneralResponse<CityView> GetCity(GetRequest request);
        GetGeneralResponse<IEnumerable<CityView>> GetCities();
        MoveResponse MoveUp(MoveRequest request);
        MoveResponse MoveDown(MoveRequest request);

        GetGeneralResponse<IEnumerable<CityView>> GetCities(int pageSize, int pageNumber);
    }
}
