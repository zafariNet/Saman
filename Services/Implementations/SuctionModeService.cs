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
using Infrastructure.Querying;

#endregion

namespace Services.Implementations
{
    public class SuctionModeService : ISuctionModeService
    {
        #region Delcares 

        private readonly ISuctionModeRepository _suctionModeRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region Ctor

        public SuctionModeService(ISuctionModeRepository suctionModeRepository, IUnitOfWork uow
             , IEmployeeRepository employeeRepository)
        {
            _suctionModeRepository = suctionModeRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region Old Methods

        public GeneralResponse AddSuctionMode(AddSuctionModeRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                SuctionMode suctionMode = new SuctionMode();
                suctionMode.ID = Guid.NewGuid();
                suctionMode.CreateDate = PersianDateTime.Now;
                suctionMode.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                suctionMode.SuctionModeName = request.SuctionModeName;
                suctionMode.SortOrder = this.GetSortOrder();
                suctionMode.RowVersion = 1;

                #region Validation

                if (suctionMode.GetBrokenRules().Count() > 0)
                {


                    foreach (BusinessRule businessRule in suctionMode.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                #endregion

                _suctionModeRepository.Add(suctionMode);
                _uow.Commit();

            }
            catch (Exception ex)
            {

                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GeneralResponse EditSuctionMode(EditSuctionModeRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            SuctionMode suctionMode = new SuctionMode();
            suctionMode = _suctionModeRepository.FindBy(request.ID);

            if (suctionMode != null)
            {
                try
                {
                    suctionMode.ModifiedDate = PersianDateTime.Now;
                    suctionMode.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.SuctionModeName != null)
                        suctionMode.SuctionModeName = request.SuctionModeName;

                    if (suctionMode.RowVersion != request.RowVersion)
                    {
                        
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        suctionMode.RowVersion += 1;
                    }

                    if (suctionMode.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in suctionMode.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _suctionModeRepository.Save(suctionMode);
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

        public GeneralResponse DeleteSuctionMode(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            SuctionMode suctionMode = new SuctionMode();
            suctionMode = _suctionModeRepository.FindBy(request.ID);

            if (suctionMode != null)
            {
                try
                {
                    _suctionModeRepository.Remove(suctionMode);
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

        public GetSuctionModeResponse GetSuctionMode(GetRequest request)
        {
            GetSuctionModeResponse response = new GetSuctionModeResponse();

            try
            {
                SuctionMode suctionMode = new SuctionMode();
                SuctionModeView suctionModeView = suctionMode.ConvertToSuctionModeView();

                suctionMode = _suctionModeRepository.FindBy(request.ID);
                if (suctionMode != null)
                    suctionModeView = suctionMode.ConvertToSuctionModeView();

                response.SuctionModeView = suctionModeView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetSuctionModesResponse GetSuctionModes()
        {
            GetSuctionModesResponse response = new GetSuctionModesResponse();

            try
            {
                IEnumerable<SuctionModeView> suctionModes = _suctionModeRepository.FindAll()
                    .OrderBy(o => o.SortOrder)
                    .ConvertToSuctionModeViews();

                response.SuctionModeViews = suctionModes;
            }
            catch (Exception ex)
            {

            }

            return response;
        }


        #endregion

        #region new Methods

        #region Read

        public GetGeneralResponse<IEnumerable<SuctionModeView>> Get_SuctionModes()
        {
            GetGeneralResponse<IEnumerable<SuctionModeView>> response = new GetGeneralResponse<IEnumerable<SuctionModeView>>();
            //IEnumerable<SuctionMode> suctionModes = _suctionModeRepository.FindAll();

            response.data = _suctionModeRepository.FindAll().Where(x=>x.Discontinued!=true).ConvertToSuctionModeViews();


            return response;
        }

        public GetGeneralResponse<IEnumerable<SuctionModeView>> Get_SuctionModes(int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<SuctionModeView>> response = new GetGeneralResponse<IEnumerable<SuctionModeView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<SuctionMode> suctionModes = _suctionModeRepository.FindAllWithSort(0, 100000,sort);

                response.data = suctionModes.data.OrderBy(o => o.SortOrder).ConvertToSuctionModeViews();
                response.totalCount = suctionModes.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }


            return response;
        }
        #endregion

        #region Insert

        public GeneralResponse AddSuctionModes(IEnumerable<AddSuctionModeRequest> requests, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                foreach (AddSuctionModeRequest request in requests)
                {
                    SuctionMode suctionMode = new SuctionMode();

                    suctionMode.ID = Guid.NewGuid();
                    suctionMode.CreateDate = PersianDateTime.Now;
                    suctionMode.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    suctionMode.RowVersion = 1;
                    suctionMode.SuctionModeName = request.SuctionModeName;
                    suctionMode.SortOrder = GetSortOrder();

                    _suctionModeRepository.Add(suctionMode);
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

        public GeneralResponse EditSuctionModes(IEnumerable<EditSuctionModeRequest> requests, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (EditSuctionModeRequest request in requests)
                {
                    SuctionMode suctionMode = new SuctionMode();
                    suctionMode = _suctionModeRepository.FindBy(request.ID);

                    suctionMode.ModifiedDate = PersianDateTime.Now;
                    suctionMode.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    suctionMode.SuctionModeName = request.SuctionModeName;

                    if (suctionMode.RowVersion != suctionMode.RowVersion)
                    {
                        ////response.success = false;
                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        suctionMode.RowVersion += 1;
                    }

                    if (suctionMode.GetBrokenRules().Count() > 0)
                    {
                        ////response.success = false;
                        foreach (BusinessRule businessRule in suctionMode.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _suctionModeRepository.Save(suctionMode);
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

        public GeneralResponse DeleteSuctionModes(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {
                    SuctionMode suctionMode = new SuctionMode();
                    suctionMode = _suctionModeRepository.FindBy(request.ID);

                    _suctionModeRepository.Remove(suctionMode);
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

        #region Moveing

        public MoveResponse MoveUp(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            // Current SuctionMode is the Request
            SuctionMode currentSuctionMode = new SuctionMode();
            currentSuctionMode = _suctionModeRepository.FindBy(request.ID);

            // Find the Previews SuctionMode
            SuctionMode previewsSuctionMode = new SuctionMode();
            try
            {
                previewsSuctionMode = _suctionModeRepository.FindAll()
                                .Where(s => s.SortOrder < currentSuctionMode.SortOrder)
                                .OrderByDescending(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentSuctionMode != null && previewsSuctionMode != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentSuctionMode.SortOrder;
                    int previews = (int)previewsSuctionMode.SortOrder;

                    currentSuctionMode.SortOrder = previews;
                    previewsSuctionMode.SortOrder = current;

                    _suctionModeRepository.Save(currentSuctionMode);
                    _suctionModeRepository.Save(previewsSuctionMode);
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }

                ////response.success = true;
            }

            return response;
        }

        public MoveResponse MoveDown(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            // Current Agency is the Request
            SuctionMode currentSuctionMode = new SuctionMode();
            currentSuctionMode = _suctionModeRepository.FindBy(request.ID);

            // Find the Previews Agency
            SuctionMode nextSuctionMode = new SuctionMode();
            try
            {
                nextSuctionMode = _suctionModeRepository.FindAll()
                                .Where(s => s.SortOrder > currentSuctionMode.SortOrder)
                                .OrderBy(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentSuctionMode != null && nextSuctionMode != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentSuctionMode.SortOrder;
                    int previews = (int)nextSuctionMode.SortOrder;

                    currentSuctionMode.SortOrder = previews;
                    nextSuctionMode.SortOrder = current;

                    _suctionModeRepository.Save(currentSuctionMode);
                    _suctionModeRepository.Save(nextSuctionMode);
                    _uow.Commit();
                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }

                ////response.success = true;
            }

            return response;
        }
        #endregion

        #endregion

        #region Report

        
        #endregion

        #region Private Members

        private int GetSortOrder()
        {
            try
            {
                IEnumerable<SuctionMode> suctionmodeDetails = _suctionModeRepository.FindAll();
                return suctionmodeDetails.Max(s => (int)s.SortOrder) + 1;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }

        #endregion


    }
}
