
#region using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Persian;
using Infrastructure.UnitOfWork;
using Model.Base;
using Model.Customers;
using Model.Customers.Interfaces;
using Model.Employees;
using Model.Employees.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Customers;
using Infrastructure.Domain;
#endregion
namespace Services.Implementations
{
    /// <summary>
    /// ایجاد شده توسط محمد ظفری
    /// </summary>
    public class CityService:ICityService
    {
        #region Declaires
        private readonly ICityRepository _cityRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion
        
        #region ctor
        public CityService(ICityRepository cityRepository , IUnitOfWork uow , IEmployeeRepository employeeRepository)
        {
            _cityRepository = cityRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }
#endregion

        #region IcityCervice Members
        #region Add
        public GeneralResponse AddCity(AddCityRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                City city = new City();
                city.ID = Guid.NewGuid();
                city.CreateDate = PersianDateTime.Now;
                city.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                city.CityName = request.CityName;
                city.RowVersion = 1;
                //Validation
                if (city.GetBrokenRules().Count() > 0)
                {
                    foreach (BusinessRule businessRule in city.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }
                    return response;
                }
                _cityRepository.Add(city);
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add(ex.InnerException.Message);
                }
            }
            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditCity(EditCityRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            City city = new City();
            city = _cityRepository.FindBy(request.ID);
            
            if (city != null)
            {
                try
                {
                    city.ModifiedDate = PersianDateTime.Now;

                    city.ModifiedEmployee = _employeeRepository.FindBy(request.MdifiedEmployeeID);

                    if (request.CityName != null)
                        city.CityName = request.CityName;

                    if (city.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        city.RowVersion += 1;
                    }

                    if (city.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in city.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _cityRepository.Save(city);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                    if (ex.InnerException != null)
                        response.ErrorMessages.Add(ex.InnerException.Message);
                }
            }
            else
            {

                response.ErrorMessages.Add("NoItemToEditText");
            }
            return response;
        }
        #endregion
        #region Delete
        public GeneralResponse DeleteCity(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            City city = new City();
            city = _cityRepository.FindBy(request.ID);
            if (city != null)
            {
                try
                {
                    _cityRepository.Remove(city);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                }
            }
            return response;
        }
        #endregion

        #region Get One
        public GetGeneralResponse<CityView> GetCity(GetRequest request)
        {
            GetGeneralResponse<CityView> response = new GetGeneralResponse<CityView>();
            try
            {
                City city = new City();
                CityView cityView = city.ConvertToCityView();
                city = _cityRepository.FindBy(request.ID);
                if (city != null)
                    cityView = city.ConvertToCityView();

                response.data = cityView;
            }
            catch (Exception ex)
            {

            }
            return response;
        }
        #endregion

        #region Get All
        public GetGeneralResponse<IEnumerable<CityView>> GetCities()
        {
            GetGeneralResponse<IEnumerable<CityView>> response = new GetGeneralResponse<IEnumerable<CityView>>();

            try
            {
                IEnumerable<CityView> city = _cityRepository.FindAll().ConvertToCityViews();


                response.data = city;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<CityView>> GetCities(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<CityView>> response = new GetGeneralResponse<IEnumerable<CityView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<City> city = _cityRepository.FindAll(index, count);

                response.data = city.data.ConvertToCityViews();
                response.totalCount = city.totalCount;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion
        #region Moving
        public MoveResponse MoveUp(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            return response;
        }

        public MoveResponse MoveDown(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();
            return response;
        }

        private int GetSortOrder()
        {
            return 0;
        }
        #endregion

        #endregion



    }
}
