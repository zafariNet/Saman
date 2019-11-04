#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Customers.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Model.Customers;
using Services.ViewModels.Customers;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;
using Infrastructure.Domain;

#endregion

namespace Services.Implementations
{
    public class LevelTypeService: ILevelTypeService
    {
        #region Declares
        private readonly ILevelTypeRepository _levelTypeRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor
        public LevelTypeService(ILevelTypeRepository levelTypeRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository)
        {
            _levelTypeRepository = levelTypeRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Old Methods

        #region Add
        public GeneralResponse AddLevelType(AddLevelTypeRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                LevelType levelType = new LevelType();
                levelType.ID = Guid.NewGuid();
                levelType.Title = request.Title;
                levelType.CreateDate = PersianDateTime.Now;

                _levelTypeRepository.Add(levelType);
                _uow.Commit();

                ////response.success = true;

                #region Validation
                //if (levelType.GetBrokenRules().Count() > 0)
                //{
                //    response.hasCenter = false;

                //    foreach (BusinessRule businessRule in levelType.GetBrokenRules())
                //    {
                //        response.ErrorMessages.Add(businessRule.Rule);
                //    }

                //    return response;
                //}
                #endregion
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditLevelType(EditLevelTypeRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            LevelType levelType = new LevelType();
            levelType = _levelTypeRepository.FindBy(request.ID);

            if (levelType != null)
            {
                try
                {
                    if (request.Title != null)
                        levelType.Title = request.Title;

                    _levelTypeRepository.Save(levelType);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }
            }
            else
            {
                
                response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            }
            return response;
        }
        #endregion

        #region Delete
        public GeneralResponse DeleteLevelType(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            LevelType levelType = new LevelType();
            levelType = _levelTypeRepository.FindBy(request.ID);

            if (levelType != null)
            {
                try
                {
                    _levelTypeRepository.Remove(levelType);
                    _uow.Commit();

                    ////response.success = true;
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
        public GetLevelTypeResponse GetLevelType(GetRequest request)
        {
            GetLevelTypeResponse response = new GetLevelTypeResponse();

            try
            {
                LevelType levelType = new LevelType();
                LevelTypeView levelTypeView = levelType.ConvertToLevelTypeView();

                levelType = _levelTypeRepository.FindBy(request.ID);
                if (levelType != null)
                    levelTypeView = levelType.ConvertToLevelTypeView();

                response.LevelTypeView = levelTypeView;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }
        #endregion

        #region Get All

        public GetLevelTypesResponse GetLevelTypes()
        {
            GetLevelTypesResponse response = new GetLevelTypesResponse();

            try
            {
                IEnumerable<LevelTypeView> levelTypes = _levelTypeRepository.FindAll()
                    .ConvertToLevelTypeViews();

                response.LevelTypeViews = levelTypes;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<LevelTypeView>> GetLevelTypes(AjaxGetRequest request)
        {
            GetGeneralResponse<IEnumerable<LevelTypeView>> response = new GetGeneralResponse<IEnumerable<LevelTypeView>>();

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                Infrastructure.Domain.Response<LevelType> levelTypesResponse = _levelTypeRepository.FindAll(index, count);

                response.data = levelTypesResponse.data.ConvertToLevelTypeViews();
                response.totalCount = levelTypesResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #endregion

        #region New Methods

        #region Read

        public GetGeneralResponse<IEnumerable<LevelTypeView>> GetLevelTypes(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<LevelTypeView>> response = new GetGeneralResponse<IEnumerable<LevelTypeView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<LevelType> levelTypesResponse = _levelTypeRepository.FindAll(index, count);

                response.data = levelTypesResponse.data.ConvertToLevelTypeViews();
                response.totalCount = levelTypesResponse.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Insert

        public GeneralResponse AddLevelTypes(IEnumerable<AddLevelTypeRequest> requests, Guid EmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (AddLevelTypeRequest request in requests)
                {

                    LevelType levelType = new LevelType();
                    levelType.ID = Guid.NewGuid();
                    levelType.Title = request.Title;
                    levelType.CreateDate = PersianDateTime.Now;
                    levelType.CreateEmployee = _employeeRepository.FindBy(EmployeeID);

                    #region validation

                    if (levelType.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in levelType.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }
                    #endregion

                    _levelTypeRepository.Add(levelType);
                }
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;

        }

        #endregion

        #region Edit

        public GeneralResponse EditleveTypes(IEnumerable<EditLevelTypeRequest> requests, Guid EmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (EditLevelTypeRequest request in requests)
                {
                    LevelType levelTypes = new LevelType();
                    levelTypes = _levelTypeRepository.FindBy(request.ID);
                    levelTypes.ModifiedDate = PersianDateTime.Now;
                    levelTypes.ModifiedEmployee = _employeeRepository.FindBy(EmployeeID);
                    if (request.Title != null)
                        levelTypes.Title = request.Title;

                    #region Validation

                    if (levelTypes.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        levelTypes.RowVersion += 1;
                    }

                    if (levelTypes.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in levelTypes.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _levelTypeRepository.Save(levelTypes);
                }
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Delete

        public GeneralResponse DeleteLeveleTypes(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {

                    _levelTypeRepository.RemoveById(request.ID);
                }
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }
        #endregion

        #endregion
    }
}
