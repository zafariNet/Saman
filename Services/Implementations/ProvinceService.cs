
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
    public class ProvinceService : IProvinceService
    {
        #region Declaires
        private readonly IProvinceRepository _provinceRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
#endregion
        #region ctor
        public ProvinceService(IProvinceRepository provinceRepository , IUnitOfWork uow , IEmployeeRepository employeeRepository)
        {
            _provinceRepository = provinceRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region IprovinceService Members

        #region Add
        public GeneralResponse AddProvince(AddProvinceRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                Province province = new Province();
                province.ID = Guid.NewGuid();
                province.CreateDate = PersianDateTime.Now;
                province.CreateEmployee = _employeeRepository.FindBy(request.EmployeeId);
                province.ProvinceName = request.ProvinceName;
                province.RowVersion = 1;
                //Validation
                if (province.GetBrokenRules().Count() > 0)
                {
                    foreach (BusinessRule businessRule in province.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }
                    return response;
                }
                _provinceRepository.Add(province);
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
        public GeneralResponse EditProvince(EditProvinceRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Province province = new Province();
            province = _provinceRepository.FindBy(request.ID);

            if (province != null)
            {
                try
                {
                    province.ModifiedDate = PersianDateTime.Now;

                    province.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    
                    if (request.ProvinceName != null)
                        province.ProvinceName = request.ProvinceName;

                    if (province.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        province.RowVersion += 1;
                    }

                    if (province.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in province.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _provinceRepository.Save(province);
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

                response.ErrorMessages.Add("NoItemToEditKey");
            }
            return response;
        }
        #endregion

        #region Delete
        public GeneralResponse DeleteProvince(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Province province = new Province();
            province = _provinceRepository.FindBy(request.ID);
            if (province != null)
            {
                try
                {
                    _provinceRepository.Remove(province);
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
        public GetGeneralResponse<ProvinceView> GetProvince(GetRequest request)
        {
            GetGeneralResponse<ProvinceView> response = new GetGeneralResponse<ProvinceView>();
            try
            {
                Province province = new Province();
                ProvinceView provinceView = province.ConvertToProvinceView();
                province = _provinceRepository.FindBy(request.ID);
                if (province != null)
                    provinceView = province.ConvertToProvinceView();

                response.data = provinceView;
            }
            catch (Exception ex)
            {

            }
            return response;
        }
        #endregion

        #region Get All
        public GetGeneralResponse<IEnumerable<ProvinceView>> GetProvinces()
        {
            GetGeneralResponse<IEnumerable<ProvinceView>> response = new GetGeneralResponse<IEnumerable<ProvinceView>>();

            try
            {
                IEnumerable<ProvinceView> provinces = _provinceRepository.FindAll().ConvertToProvinceViews();


                response.data = provinces;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<ProvinceView>> GetProvinces(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<ProvinceView>> response = new GetGeneralResponse<IEnumerable<ProvinceView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Province> provinces = _provinceRepository.FindAll(index, count);

                response.data = provinces.data.ConvertToProvinceViews();
                response.totalCount = provinces.totalCount;
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
