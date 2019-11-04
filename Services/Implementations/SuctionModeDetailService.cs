#region Using

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
using Infrastructure.Querying;
using Model.Customers.Validations.Interfaces;

#endregion
namespace Services.Implementations
{
    public class SuctionModeDetailService : ISuctionModeDetailService
    {

        #region Declare

        private readonly ISuctionModeDetailRepository _suctionModeDetailRepository;

        private readonly IEmployeeRepository _employeeRepository;

        private readonly ISuctionModeRepository _suctionModeRepository;

        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public SuctionModeDetailService(ISuctionModeDetailRepository suctionModeDetailRepository, IEmployeeRepository employeeRepository, ISuctionModeRepository suctionModeRepository, IUnitOfWork uow)
        {
            _suctionModeDetailRepository = suctionModeDetailRepository;
            _employeeRepository = employeeRepository;
            _suctionModeRepository = suctionModeRepository;
            _uow = uow;
        }


        #endregion

        #region New Methods

        #region Read

        public GetGeneralResponse<IEnumerable<SuctionModeDetailview>> GetSuctionModeDetailBySuctionMode(Guid SuctionModeID,int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<SuctionModeDetailview>> response = new GetGeneralResponse<IEnumerable<SuctionModeDetailview>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                if (SuctionModeID != Guid.Empty)
                {

                    Criterion criteriasuctionModeID = new Criterion("SuctionMode.ID", SuctionModeID, CriteriaOperator.Equal);
                    query.Add(criteriasuctionModeID);
                }
                Response<SuctionModeDetail> suctioModeDetailviews = _suctionModeDetailRepository.FindBy(query, index, count,sort);

                response.data = suctioModeDetailviews.data.ConvertToSuctionModeViews();
                response.totalCount = suctioModeDetailviews.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);

            }
            return response;
        }

        public GetGeneralResponse<IEnumerable<SuctionModeDetailview>> GetSuctionModeDetailBySuctionModeAll(Guid SuctionModeID, int pageSize, int pageNumber, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<SuctionModeDetailview>> response = new GetGeneralResponse<IEnumerable<SuctionModeDetailview>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                if (SuctionModeID != Guid.Empty)
                {

                    Criterion criteriasuctionModeID = new Criterion("SuctionMode.ID", SuctionModeID, CriteriaOperator.Equal);
                    query.Add(criteriasuctionModeID);
                }
                Response<SuctionModeDetail> suctioModeDetailviews = _suctionModeDetailRepository.FindBy(query, index, count, sort);
                var temp = suctioModeDetailviews.data.Where(x => x.Discontinued != true);
                response.data = temp.ConvertToSuctionModeViews();
                response.totalCount = temp.Count();
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

        public GeneralResponse AddSuctionModeDetails(IEnumerable<AddSuctionModeDetailRequest> requests,Guid SuctionModeID, Guid CreateEployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (AddSuctionModeDetailRequest request in requests)
                {
                    SuctionModeDetail suctionModeDetail = new SuctionModeDetail();
                    suctionModeDetail.ID = Guid.NewGuid();
                    suctionModeDetail.CreateDate = PersianDateTime.Now;
                    suctionModeDetail.CreateEmployee = _employeeRepository.FindBy(CreateEployeeID);
                    suctionModeDetail.SuctionModeDetailName = request.SuctionModeDetailName;
                    suctionModeDetail.Discontinued = request.Discontinued;
                    suctionModeDetail.SortOrder = GetSortOrder();
                    suctionModeDetail.SuctionMode = _suctionModeRepository.FindBy(SuctionModeID);
                    suctionModeDetail.RowVersion = 1;
                    _suctionModeDetailRepository.Add(suctionModeDetail);
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

        public GeneralResponse EditSuctionModeDetails(IEnumerable<EditSuctionModeDetailRequest> requests, Guid ModifiedemployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (EditSuctionModeDetailRequest request in requests)
                {

                    SuctionModeDetail suctionModedetail = new SuctionModeDetail();
                    suctionModedetail = _suctionModeDetailRepository.FindBy(request.ID);
                    if (request.SuctionModeDetailName != null)
                        suctionModedetail.SuctionModeDetailName = request.SuctionModeDetailName;
                    suctionModedetail.CreateEmployee = _employeeRepository.FindBy(ModifiedemployeeID);
                    if (suctionModedetail.RowVersion != suctionModedetail.RowVersion)
                    {
                        ////response.success = false;
                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        suctionModedetail.RowVersion += 1;
                    }

                    if (suctionModedetail.GetBrokenRules().Count() > 0)
                    {
                        ////response.success = false;
                        foreach (BusinessRule businessRule in suctionModedetail.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }
                    _suctionModeDetailRepository.Save(suctionModedetail);
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

        public GeneralResponse DeleteSuctionModeDetails(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {
                    SuctionModeDetail suctionModeDetail = new SuctionModeDetail();
                    suctionModeDetail = _suctionModeDetailRepository.FindBy(request.ID);

                    _suctionModeDetailRepository.Remove(suctionModeDetail);
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

            // Current Agency is the Request
            SuctionModeDetail currentSuctionModeDetail = new SuctionModeDetail();
            currentSuctionModeDetail = _suctionModeDetailRepository.FindBy(request.ID);

            // Find the Previews Agency
            SuctionModeDetail previewsSuctionModedetail = new SuctionModeDetail();
            try
            {
                previewsSuctionModedetail = _suctionModeDetailRepository.FindAll()
                                .Where(s => s.SortOrder < currentSuctionModeDetail.SortOrder)
                                .OrderByDescending(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentSuctionModeDetail != null && previewsSuctionModedetail != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentSuctionModeDetail.SortOrder;
                    int previews = (int)previewsSuctionModedetail.SortOrder;

                    currentSuctionModeDetail.SortOrder = previews;
                    previewsSuctionModedetail.SortOrder = current;

                    _suctionModeDetailRepository.Save(currentSuctionModeDetail);
                    _suctionModeDetailRepository.Save(previewsSuctionModedetail);
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
            SuctionModeDetail currentSuctionModeDetail = new SuctionModeDetail();
            currentSuctionModeDetail = _suctionModeDetailRepository.FindBy(request.ID);

            // Find the Previews Agency
            SuctionModeDetail nextSuctionModeDetail = new SuctionModeDetail();
            try
            {
                nextSuctionModeDetail = _suctionModeDetailRepository.FindAll()
                                .Where(s => s.SortOrder > currentSuctionModeDetail.SortOrder)
                                .OrderBy(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentSuctionModeDetail != null && nextSuctionModeDetail != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentSuctionModeDetail.SortOrder;
                    int previews = (int)nextSuctionModeDetail.SortOrder;

                    currentSuctionModeDetail.SortOrder = previews;
                    nextSuctionModeDetail.SortOrder = current;

                    _suctionModeDetailRepository.Save(currentSuctionModeDetail);
                    _suctionModeDetailRepository.Save(nextSuctionModeDetail);
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

        #region Private Members

        private int GetSortOrder()
        {
            try
            {
                IEnumerable<SuctionModeDetail> suctionmodeDetails = _suctionModeDetailRepository.FindAll();
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
