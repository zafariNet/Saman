using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;
using Model.Employees;
using Model.Employees.Interfaces;
using Model.Lead.Interfaces;
using Model.Leads;
using Model.Leads.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.Leadcatalogservice;
using Services.ViewModels.Leads;

namespace Services.Implementations
{
    public class NegotiationService : INegotiationService
    {
        #region Declare

        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeadTitleTemplateRepository _leadTitleTemplateRepository;
        private readonly ILeadResultTemplateRepository _leadResultTemplateRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly INegotiationRepository _negotiationRepository;
        private readonly IUnitOfWork _uow;

        #endregion

        #region  Ctor

        public NegotiationService(IEmployeeRepository employeeRepository,
            ILeadTitleTemplateRepository leadTitleTemplateRepository,
            ILeadResultTemplateRepository leadResultTemplateRepository, ICustomerRepository customerRepository,
            IUnitOfWork uow, INegotiationRepository negotiationRepository)
        {
            _employeeRepository = employeeRepository;
            _leadResultTemplateRepository = leadResultTemplateRepository;
            _leadTitleTemplateRepository = leadTitleTemplateRepository;
            _customerRepository = customerRepository;
            _negotiationRepository = negotiationRepository;
            _uow = uow;
        }

        #endregion

        #region Read Own

        public GetGeneralResponse<IEnumerable<NegotiationView>> GetOwnNegotiation(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<NegotiationView>> response = new GetGeneralResponse<IEnumerable<NegotiationView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "Negotiation", sort);

                Response<Negotiation> negotiations = _negotiationRepository.FindAll(query, index, count);

                response.data = negotiations.data.ConvertToNegotiationViews();
                response.totalCount = negotiations.totalCount;
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

        #region Read Child Negotiations

        public GetGeneralResponse<IEnumerable<NegotiationView>> GetChildNegotiations(Guid EmployeeID, int pageSize,
            int pageNumber, IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<NegotiationView>> response = new GetGeneralResponse<IEnumerable<NegotiationView>>();
            try
            {
                Employee employee = _employeeRepository.FindBy(EmployeeID);
                IList<Guid> ChildIds = new List<Guid>();
                var temp=employee.GetAllChild();
                foreach (var item in temp)
                    ChildIds.Add(item.ID);
                IList<FilterData> Filters = new List<FilterData>();
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
                if (filter != null)
                    foreach (var item in filter)
                        Filters.Add(item);
                IList<string> Ids = new List<string>();
                foreach (var item in ChildIds)
                {
                    Ids.Add(item.ToString());
                }
                Filters.Add(new FilterData()
                {

                    data = new data()
                    {
                        comparison = "eq",
                        type = "list",
                        value = Ids.ToArray()
                    },
                    field = "ReferedEmployee.ID"
                });

                string query = FilterUtilityService.GenerateFilterHQLQuery(Filters, "Negotiation", sort);
                Response<Negotiation> negotiations = _negotiationRepository.FindAll(query, index, count);

                response.data = negotiations.data.ConvertToNegotiationViews();
                response.totalCount = negotiations.totalCount;
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

        #region Add Negotiation

        public GeneralResponse AddNegotiation(AddNegotiationRequest request, Guid EmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Negotiation negotiation = new Negotiation();

                negotiation.ID = Guid.NewGuid();
                negotiation.CreateDate = PersianDateTime.Now;
                negotiation.CreateEmployee = _employeeRepository.FindBy(EmployeeID);
                negotiation.RowVersion = 1;
                negotiation.Customer = _customerRepository.FindBy(request.CustomerID);
                negotiation.LeadTitleTemplate = _leadTitleTemplateRepository.FindBy(request.LeadTitleTemplateID);
                negotiation.NegotiationDate = request.NegotiationDate;
                negotiation.NegotiationTime = request.NegotiationTime;
                negotiation.RememberTime = request.RememberTime;
                negotiation.SendSms = request.SendSms != null && (bool)request.SendSms;
                negotiation.ReferedEmployee = request.ReferedEmployeeID == null
                    ? _employeeRepository.FindBy(EmployeeID)
                    : _employeeRepository.FindBy((Guid)request.ReferedEmployeeID);
                negotiation.NegotiationDesciption = request.NegotiationDesciption;


                _negotiationRepository.Add(negotiation);
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

        #region Close

        public GeneralResponse CloseNegotiation(Guid NegotiationID, Guid EmployeeID, Guid LeadResultTemplateID,
            string NegotiationResultDescription, int Status)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Negotiation negotiation = _negotiationRepository.FindBy(NegotiationID);

                negotiation.Closed = true;
                negotiation.CloseDate = PersianDateTime.Now;
                negotiation.NegotiationStatus = NegotiationStatuses.Closed;
                negotiation.LeadResultTemplate = _leadResultTemplateRepository.FindBy(LeadResultTemplateID);
                negotiation.NeqotiationResultDescription = NegotiationResultDescription;

                _negotiationRepository.Save(negotiation);
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

        #region Change Owner

        public GeneralResponse ChangeNegotiationReferedEmployee(IEnumerable<Guid> NegotiationIDs, Guid EmployeeID, Guid ReferedEmployeeID, string NegotiationDate, string NegotiationTime, string RememberDate, string RememberTime)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (var item in NegotiationIDs)
                {
                    Negotiation negotiation = _negotiationRepository.FindBy(item);
                    if (negotiation.Closed)
                    {
                        response.ErrorMessages.Add("مذاکره بسته شده را نمیتوان ارجاع داد.");
                        return response;
                    }
                    negotiation.ReferedEmployee = _employeeRepository.FindBy(ReferedEmployeeID);
                    negotiation.ModifiedDate = PersianDateTime.Now;
                    negotiation.ModifiedEmployee = _employeeRepository.FindBy(EmployeeID);
                    negotiation.NegotiationStatus = NegotiationStatuses.Refered;
                    negotiation.NegotiationDate = NegotiationDate;
                    negotiation.NegotiationTime = NegotiationTime;
                    negotiation.RememberTime = RememberTime;

                    _negotiationRepository.Save(negotiation);
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

        #region Create Negotiation For Customers

        public GeneralResponse CreateNegotiationForCustomers(IEnumerable<Guid> CustomerIDs, Guid ReferedEmployeeID,
            Guid LeadTitleTemplateID,
            string NegotiationDesciption,
            string NegotiationDate,
            string NegotiationTime,
            string RememberDate,
            string RememberTime,
            bool? SendSms, Guid EmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                foreach (var item in CustomerIDs)
                {
                    Negotiation negotiation = new Negotiation();

                    negotiation.ID = Guid.NewGuid();
                    negotiation.CreateDate = PersianDateTime.Now;
                    negotiation.CreateEmployee = _employeeRepository.FindBy(EmployeeID);
                    negotiation.RowVersion = 1;
                    negotiation.Customer = _customerRepository.FindBy(item);
                    negotiation.LeadTitleTemplate = _leadTitleTemplateRepository.FindBy(LeadTitleTemplateID);
                    negotiation.NegotiationDate = NegotiationDate;
                    negotiation.NegotiationTime = NegotiationTime;
                    negotiation.RememberTime = RememberTime;
                    negotiation.ReferedEmployee = _employeeRepository.FindBy(ReferedEmployeeID);
                    negotiation.NegotiationDesciption = NegotiationDesciption;
                    negotiation.SendSms = SendSms != null && (bool)SendSms;

                    _negotiationRepository.Add(negotiation);
                    _uow.Commit();
                }
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

        #region Get Cstomer Negotiations

        public GetGeneralResponse<IEnumerable<NegotiationView>> GetCustomerNegotiations(Guid CustomerID,
            IList<FilterData> filter, IList<Sort> sort, int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<NegotiationView>> response = new GetGeneralResponse<IEnumerable<NegotiationView>>();
            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;
                IList<FilterData> Filters = new List<FilterData>();
                if (filter != null)
                    foreach (var item in filter)
                        Filters.Add(item);
                Filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eq",
                        type = "string",
                        value = new[] { CustomerID.ToString() }
                    },
                    field = "Customer.ID"
                });

                string query = FilterUtilityService.GenerateFilterHQLQuery(Filters, "Negotiation", sort);
                Response<Negotiation> negotiations = _negotiationRepository.FindAll(query, index, count);

                response.data = negotiations.data.ConvertToNegotiationViews();
                response.totalCount = negotiations.totalCount;

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

        #region negotiation Delay

        public GeneralResponse NegotiationDelay(Guid NegotiationID, Guid EmployeeID, string NegotiationDate,
            string NegotiationTime, string RememberDate, string RememberTime)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                Negotiation negotiation = _negotiationRepository.FindBy(NegotiationID);
                negotiation.NegotiationDate = NegotiationDate;
                negotiation.NegotiationTime = NegotiationTime;
                negotiation.RememberTime = RememberTime;
                negotiation.ModifiedDate = PersianDateTime.Now;
                negotiation.ModifiedEmployee = _employeeRepository.FindBy(EmployeeID);

                _negotiationRepository.Save(negotiation);
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

        public GeneralResponse EditNegotiation(EditNegotiationRequest request, Guid EmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Negotiation negotiation = _negotiationRepository.FindBy(request.ID);

                if (!negotiation.Closed)
                {
                    negotiation.Customer = _customerRepository.FindBy(request.CustomerID);
                    negotiation.LeadTitleTemplate = _leadTitleTemplateRepository.FindBy(request.LeadTitleTemplateID);
                    negotiation.NegotiationDate = request.NegotiationDate;
                    negotiation.NegotiationTime = request.NegotiationTime;
                    negotiation.NegotiationDesciption = request.NegotiationDesciption;
                    negotiation.RememberTime = request.RememberTime;
                    negotiation.ReferedEmployee =
                        _employeeRepository.FindBy(request.ReferedEmployeeID == null
                            ? (Guid)EmployeeID
                            : (Guid)request.ReferedEmployeeID);
                    negotiation.SendSms = request.SendSms != null && (bool)request.SendSms;

                    _negotiationRepository.Save(negotiation);
                    _uow.Commit();
                }
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

        public GeneralResponse DeleteNegotiations(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                foreach (var deleteRequest in requests)
                {
                    Negotiation negotiation = _negotiationRepository.FindBy(deleteRequest.ID);
                    if (!negotiation.Closed)
                    {
                        _negotiationRepository.Remove(negotiation);
                    }
                    else
                    {
                        response.ErrorMessages.Add("مذاکره فروش بسته شده لذا امکان حذف ان وجود ندارد.");
                        return response;
                    }
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
    }
}
