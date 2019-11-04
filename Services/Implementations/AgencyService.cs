#region Usings

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
using System.Web.Script.Serialization;
#endregion

namespace Services.Implementations
{
    public class AgencyService : IAgencyService
    {
        #region Declares
        private readonly IAgencyRepository _agencyRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor
        public AgencyService(IAgencyRepository agencyRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository)
        {
            _agencyRepository = agencyRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region IAgencyService Members

        #region Add

        public GeneralResponse AddAgency(AddAgencyRequest request, Guid createEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
                try
                {
                    Agency agency = new Agency();
                    agency.ID = Guid.NewGuid();
                    agency.CreateDate = PersianDateTime.Now;
                    agency.CreateEmployee = _employeeRepository.FindBy(createEmployeeID);
                    agency.AgencyName = request.AgencyName;
                    agency.ManagerName = request.ManagerName;
                    agency.Phone1 = request.Phone1;
                    agency.Phone2 = request.Phone2;
                    agency.Mobile = request.Mobile;
                    agency.Address = request.Address;
                    agency.Note = request.Note;
                    agency.Discontinued = request.Discontinued;
                    agency.SortOrder = GetMaxSortOrder();
                    agency.RowVersion = 1;

                    // Validation
                    if (agency.GetBrokenRules().Count() > 0)
                    {
                        foreach (BusinessRule businessRule in agency.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }
                        return response;
                    }

                    _agencyRepository.Add(agency);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                    if (ex.InnerException != null)
                        response.ErrorMessages.Add(ex.InnerException.Message);
                    return response;
                }
            
            return response;
        }

        public GeneralResponse AddAgency(AddAgencyRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Agency agency = new Agency();
                agency.ID = Guid.NewGuid();
                agency.CreateDate = PersianDateTime.Now;
                agency.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                agency.AgencyName = request.AgencyName;
                agency.ManagerName = request.ManagerName;
                agency.Phone1 = request.Phone1;
                agency.Phone2 = request.Phone2;
                agency.Mobile = request.Mobile;
                agency.Address = request.Address;
                agency.Note = request.Note;
                agency.Discontinued = request.Discontinued;
                agency.SortOrder = GetMaxSortOrder();
                agency.RowVersion = 1;

                // Validation
                if (agency.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in agency.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }

                _agencyRepository.Add(agency);
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
        public GeneralResponse EditAgency(EditAgencyRequest request, Guid modifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            
                try
                {
                    Agency agency = new Agency();
                    agency = _agencyRepository.FindBy(request.ID);

                    if (agency != null)
                    {
                        agency.ModifiedDate = PersianDateTime.Now;

                        agency.ModifiedEmployee = _employeeRepository.FindBy(modifiedEmployeeID);
                        if (request.AgencyName != null)
                            agency.AgencyName = request.AgencyName;
                        if (request.ManagerName != null)
                            agency.ManagerName = request.ManagerName;
                        if (request.Phone1 != null)
                            agency.Phone1 = request.Phone1;
                        if (request.Phone2 != null)
                            agency.Phone2 = request.Phone2;
                        if (request.Mobile != null)
                            agency.Mobile = request.Mobile;
                        if (request.Address != null)
                            agency.Address = request.Address;
                        if (request.Note != null)
                            agency.Note = request.Note;

                        agency.Discontinued = request.Discontinued;

                        #region RowVersion - Validation
                        if (agency.RowVersion != request.RowVersion)
                        {

                            response.ErrorMessages.Add("EditConcurrencyKey");
                            return response;
                        }
                        else
                        {
                            agency.RowVersion += 1;
                        }

                        if (agency.GetBrokenRules().Count() > 0)
                        {

                            foreach (BusinessRule businessRule in agency.GetBrokenRules())
                            {
                                response.ErrorMessages.Add(businessRule.Rule);
                            }

                            return response;
                        }

                        #endregion

                        _agencyRepository.Save(agency);
                    }
                    else
                    {

                        response.ErrorMessages.Add("NoItemToEditKey");
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
        public GeneralResponse EditAgency(EditAgencyRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            Agency agency = new Agency();
            agency = _agencyRepository.FindBy(request.ID);

            if (agency != null)
            {
                try
                {
                    agency.ModifiedDate = PersianDateTime.Now;

                    agency.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.AgencyName != null)
                        agency.AgencyName = request.AgencyName;
                    if (request.ManagerName != null)
                        agency.ManagerName = request.ManagerName;
                    if (request.Phone1 != null)
                        agency.Phone1 = request.Phone1;
                    if (request.Phone2 != null)
                        agency.Phone2 = request.Phone2;
                    if (request.Mobile != null)
                        agency.Mobile = request.Mobile;
                    if (request.Address != null)
                        agency.Address = request.Address;
                    if (request.Note != null)
                        agency.Note = request.Note;

                    agency.Discontinued = request.Discontinued;

                    if (agency.RowVersion != request.RowVersion)
                    {
                        
                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        agency.RowVersion += 1;
                    }

                    if (agency.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in agency.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _agencyRepository.Save(agency);
                    _uow.Commit();
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

        #region IEnumerabl Edit

        public GeneralResponse EditAgencies(IEnumerable<EditAgencyRequest> Agencies, Guid ModifiedemployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (EditAgencyRequest agencyView in Agencies)
                {
                    
                    Agency agency = new Agency();
                    agency = _agencyRepository.FindBy(agencyView.ID);
                    agency.Discontinued = agencyView.Discontinued;
                    agency.ModifiedDate = PersianDateTime.Now;
                    agency.ModifiedEmployee = _employeeRepository.FindBy(ModifiedemployeeID);

                    _agencyRepository.Save(agency);
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

        #region Delete
        public GeneralResponse DeleteAgency(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            foreach (var request in requests)
            {
                    try
                    {
                        _agencyRepository.RemoveById(request.ID);
                    }
                    catch (Exception ex)
                    {
                        response.ErrorMessages.Add(ex.Message);
                        return response;
                    }
            }

            try
            {
                _uow.Commit();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }
        public GeneralResponse DeleteAgency(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

                try
                {
                    _agencyRepository.RemoveById(request.ID);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                }

            return response;
        }
        #endregion

        #region Get One
        public GetAgencyResponse GetAgency(GetRequest request)
        {
            GetAgencyResponse response = new GetAgencyResponse();

            try
            {
                Agency agency = new Agency();
                AgencyView agencyView = agency.ConvertToAgencyView();

                agency = _agencyRepository.FindBy(request.ID);
                if (agency != null)
                    agencyView = agency.ConvertToAgencyView();

                response.AgencyView = agencyView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All

        public GetGeneralResponse<IEnumerable<AgencyView>> GetAgencies()
        {
            GetGeneralResponse<IEnumerable<AgencyView>> response = new GetGeneralResponse<IEnumerable<AgencyView>>();

            try
            {
                IList<Sort> sortOrders = new List<Sort>();
                sortOrders.Add(new Sort("SortOrder"));

                Response<Agency> agencies = _agencyRepository.FindAllWithSort(-1, -1, sortOrders);

                response.data = agencies.data.ConvertToAgencyViews();
                response.totalCount = agencies.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<AgencyView>> GetAgencies(bool? Discontinued,int pageSize, int pageNumber,IList<Sort> sort,IList<FilterData> filter)
        {
            GetGeneralResponse<IEnumerable<AgencyView>> response = new GetGeneralResponse<IEnumerable<AgencyView>>();

            try
            {
                

                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
                Response<Agency> agencies=new Response<Agency>();
                //IList<Sort> sortOrders = new List<Sort>();
                //sortOrders.Add(new Sort("SortOrder"));
                if (filter != null)
                {
                    string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Agency", sort);
                    agencies = _agencyRepository.FindAll(query, index, count);
                    
                }
                else
                {
                    agencies = _agencyRepository.FindAllWithSort(index, count, sort);
                }
                if (Discontinued == false)
                    response.data = agencies.data.ConvertToAgencyViews().Where(x => x.Discontinued == false);
                else
                    response.data = agencies.data.ConvertToAgencyViews();

                response.totalCount = agencies.totalCount;
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<AgencyView>> GetActiveAgencies()
        {
            GetGeneralResponse<IEnumerable<AgencyView>> response = new GetGeneralResponse<IEnumerable<AgencyView>>();

            try
            {
                IEnumerable<Agency> agencies = _agencyRepository.FindAll().Where(x => x.Discontinued == false);

                response.data = agencies.ConvertToAgencyViews();
                response.totalCount = agencies.Count();
            }

            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException.Message != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Moving
        public MoveResponse MoveUp(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            // Current Agency is the Request
            Agency currentAgency = _agencyRepository.FindBy(request.ID);

            // Find the Previews Agency
            Agency previewsAgency = new Agency();
            try
            {
                previewsAgency = _agencyRepository.FindAll()
                                .Where(s => s.SortOrder < currentAgency.SortOrder)
                                .OrderByDescending(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentAgency != null && previewsAgency != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = currentAgency.SortOrder;
                    int previews = previewsAgency.SortOrder;

                    currentAgency.SortOrder = previews;
                    previewsAgency.SortOrder = current;

                    _agencyRepository.Save(currentAgency);
                    _agencyRepository.Save(previewsAgency);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }
            }
            return response;
        }

        public MoveResponse MoveDown(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            // Current Agency is the Request
            Agency currentAgency = new Agency();
            currentAgency = _agencyRepository.FindBy(request.ID);

            // Find the Previews Agency
            Agency nextAgency = new Agency();
            try
            {
                nextAgency = _agencyRepository.FindAll()
                                .Where(s => s.SortOrder > currentAgency.SortOrder)
                                .OrderBy(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentAgency != null && nextAgency != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = currentAgency.SortOrder;
                    int previews = nextAgency.SortOrder;

                    currentAgency.SortOrder = previews;
                    nextAgency.SortOrder = current;

                    _agencyRepository.Save(currentAgency);
                    _agencyRepository.Save(nextAgency);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                    return response;
                }
            }

            return response;
        }

        private int GetMaxSortOrder()
        {
            try
            {
                IList<Sort> sortOrders = new List<Sort>();
                sortOrders.Add(new Sort("SortOrder", false));

                Response<Agency> agencies = _agencyRepository.FindAllWithSort(0, 1, sortOrders);

                return agencies.data.Max(s => s.SortOrder) + 1;
            }
            catch (Exception ex)
            {
                return 1;
            }
        }
        #endregion

        #endregion
    }
}
