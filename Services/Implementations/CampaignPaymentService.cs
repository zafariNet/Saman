using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Base;
using Model.Customers.Validations.Interfaces;
using Model.Employees;
using Model.Employees.Interfaces;
using Model.Sales;
using Model.Sales.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.ReportCatalogService;
using Services.Messaging.SaleCatalogService;
using Services.ViewModels.Sales;
using Services.ViewModels.Reports;

namespace Services.Implementations
{
    public class CampaignPaymentService:ICampaignPaymentService
    {
        #region Declar

        private readonly IEmployeeRepository _employeeRepository;

        private readonly ICampaignPaymentRepository _campaignPaymentRepository;

        private readonly ICampaignAgentRepository _campaignAgentRepository;

        private readonly ISuctionModeDetailRepository _suctionModeDetailRepository;

        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public CampaignPaymentService(IEmployeeRepository employeeRepository,
            ICampaignPaymentRepository campaignPaymentRepository, ICampaignAgentRepository campaignAgentRepository, ISuctionModeDetailRepository suctionModeDetailRepository, IUnitOfWork uow)
        {

            _employeeRepository = employeeRepository;
            _campaignPaymentRepository = campaignPaymentRepository;
            _campaignAgentRepository = campaignAgentRepository;
            _suctionModeDetailRepository = suctionModeDetailRepository;
            _uow = uow;
        }

        #endregion

        #region Read 

        #region Read All

        public GetGeneralResponse<IEnumerable<CampaignPaymentView>> GetCampaignPayments(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<CampaignPaymentView>> response=new GetGeneralResponse<IEnumerable<CampaignPaymentView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "CampaignPayment", sort);

                Response<CampaignPayment> campaignPaymentes = _campaignPaymentRepository.FindAll(query, index, count);

                response.data = campaignPaymentes.data.ConvertToCampaignPaymentViews();
                response.totalCount = campaignPaymentes.totalCount;

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException!=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Read By Agent

        public GetGeneralResponse<IEnumerable<CampaignPaymentView>> GetCampaignPaymentsByAgent(int pageSize, int pageNumber, Guid CampaignAgentID)
        {
            GetGeneralResponse<IEnumerable<CampaignPaymentView>> response=new GetGeneralResponse<IEnumerable<CampaignPaymentView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Query query=new Query();
                Criterion CampaignAgentIDCriteria = new Criterion("CampaignAgent.ID", CampaignAgentID, CriteriaOperator.Equal);
                query.Add(CampaignAgentIDCriteria);

                Response<CampaignPayment> campaignpaymentes = _campaignPaymentRepository.FindBy(query,index,count);

                response.data = campaignpaymentes.data.ConvertToCampaignPaymentViews();
                response.totalCount = campaignpaymentes.totalCount;
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

        #region Add

        public GeneralResponse AddCampaignPayment(IEnumerable<AddCampignPaymentRequest> requests, Guid CampaignAgentID, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                CampaignAgent campaignAgent = new CampaignAgent();
                campaignAgent = _campaignAgentRepository.FindBy(CampaignAgentID);
                foreach (var request in requests)
                {


                    CampaignPayment campaignPayment = new CampaignPayment();

                    campaignPayment.ID = Guid.NewGuid();
                    campaignPayment.CreateDate = PersianDateTime.Now;
                    campaignPayment.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    campaignPayment.SuctionModeDetail = _suctionModeDetailRepository.FindBy(request.SuctionModeDetailID);
                    campaignPayment.CampaignAgent = _campaignAgentRepository.FindBy(CampaignAgentID);
                    campaignPayment.Amount = request.Amount;
                    campaignPayment.PaymentDate = request.PaymentDate;
                    campaignPayment.Note = request.Note;
                    campaignPayment.RowVersion = 1;

                    campaignAgent.TotalPayment += campaignPayment.Amount;

                    _campaignPaymentRepository.Add(campaignPayment);
                }
                _campaignAgentRepository.Save(campaignAgent);
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

        public GeneralResponse EditCampaignPayment(IEnumerable<EditCampignPaymentRequest> requests, Guid CampaignAgentID, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    CampaignPayment campaignPayment = new CampaignPayment();

                    campaignPayment = _campaignPaymentRepository.FindBy(request.ID);
                    campaignPayment.ModifiedDate = PersianDateTime.Now;
                    campaignPayment.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    campaignPayment.SuctionModeDetail = _suctionModeDetailRepository.FindBy(request.SuctionModeDetailID);
                    campaignPayment.PaymentDate = request.PaymentDate;
                    //campaignPayment.Amount = request.Amount;

                    #region RowVersion - Validation

                    if (campaignPayment.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        campaignPayment.RowVersion += 1;
                    }

                    if (campaignPayment.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in campaignPayment.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _campaignPaymentRepository.Save(campaignPayment);
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

        public GeneralResponse DeleteCampaignPayment(IEnumerable<DeleteRequest> requests)
        {
            return new GeneralResponse();
            //GeneralResponse response=new GeneralResponse();
            //try
            //{
            //    foreach (DeleteRequest request in requests)
            //    {
            //        CampaignPayment campaignPayment=new CampaignPayment();

            //        campaignPayment = _campaignPaymentRepository.FindBy(request.ID);

            //        _campaignPaymentRepository.Remove(campaignPayment);
            //    }
            //}

            //catch (Exception ex)
            //{
            //    response.ErrorMessages.Add(ex.Message);
            //    if (ex.InnerException != null)
            //        response.ErrorMessages.Add(ex.InnerException.Message);
            //}

            //return response;
        }

        #endregion

        #region Report 1

        public GetGeneralResponse<IEnumerable<GetSuctionModeCost1View>> GetSuctionModeCostReport1(IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<GetSuctionModeCost1View>> response = new GetGeneralResponse<IEnumerable<GetSuctionModeCost1View>>();

            string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "CampaignPayment", sort);

            Response<CampaignPayment> campaignPayment = _campaignPaymentRepository.FindBy(query);
            IList<GetSuctionModeCost1View> list = new List<GetSuctionModeCost1View>();
            foreach (var item in campaignPayment.data)
            {
                GetSuctionModeCost1View getSuctionModeCost1View = new GetSuctionModeCost1View();
                getSuctionModeCost1View.CampaignAgentName = item.CampaignAgent.CampaignAgentName;
                getSuctionModeCost1View.Amount = item.Amount;
                getSuctionModeCost1View.SuctionModeDetailName = item.SuctionModeDetail.SuctionModeDetailName;
                getSuctionModeCost1View.SuctionModeName = item.SuctionModeDetail.SuctionMode.SuctionModeName;
                getSuctionModeCost1View.PaymentDate = item.PaymentDate;

                list.Add(getSuctionModeCost1View);
            }

            response.data = list;
            response.totalCount = list.Count();

            return response;
        }

        #endregion

        #region Report 2

        public GetGeneralResponse<IEnumerable<GetSuctionModeCost2View>> GetSuctionModeCostReport2(IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<GetSuctionModeCost2View>> response = new GetGeneralResponse<IEnumerable<GetSuctionModeCost2View>>();

            string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "CampaignPayment", sort);

            Response<CampaignPayment> campaignPayment = _campaignPaymentRepository.FindBy(query);
            var result = campaignPayment.data.GroupBy(l => l.CampaignAgent.ID)
                .Select(lg =>
                    new
                    {
                        ID = lg.Key,
                        Sum = lg.Sum(x => x.Amount),
                        CampaignAgent=lg.FirstOrDefault().CampaignAgent.CampaignAgentName,
                    });
            IList<GetSuctionModeCost2View> list = new List<GetSuctionModeCost2View>();
            foreach (var item in result)
            {
                GetSuctionModeCost2View getSuctionModeCost1View = new GetSuctionModeCost2View();
                getSuctionModeCost1View.ID = item.ID;
                getSuctionModeCost1View.CampaignAgentName = item.CampaignAgent;
                getSuctionModeCost1View.Amount = item.Sum;

                list.Add(getSuctionModeCost1View);
            }

            response.data = list;
            response.totalCount = list.Count();

            return response;
        }

        #endregion


        #region report 3

        public GetGeneralResponse<IEnumerable<GetcampaignAgents>> GetSuctionModeCostReport3(IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<GetcampaignAgents>> response = new GetGeneralResponse<IEnumerable<GetcampaignAgents>>();

            string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "CampaignPayment", sort);

            Response<CampaignPayment> campaignPayment = _campaignPaymentRepository.FindBy(query);

            IList<GetcampaignAgents> list=new List<GetcampaignAgents>();
            foreach (var item in campaignPayment.data)
            {
                list.Add(new GetcampaignAgents()
                {
                    PaymentDate = item.PaymentDate,
                    SuctionModeDetailID = item.SuctionModeDetail.ID,
                    SuctionModeID = item.SuctionModeDetail.SuctionMode.ID,
                    SuctionMoedName = item.SuctionModeDetail.SuctionModeDetailName,
                    SuctionModeDetailName = item.SuctionModeDetail.SuctionMode.SuctionModeName,
                    Amount = item.Amount
                });
            }
            response.data = list;
            response.totalCount = list.Count();

            return response;
        }

        public GetGeneralResponse<IEnumerable<SuctionModeCost>> GetSuctionModeReport3(
            GetsuctionModeRequestForReport3 request)
        {
            GetGeneralResponse<IEnumerable<SuctionModeCost>> response=new GetGeneralResponse<IEnumerable<SuctionModeCost>>();

            IEnumerable<SuctionModeCost> suctionModeCosts = _campaignPaymentRepository.Report(
                request.RegisterStartDate, request.RegisterEndDate, request.PaymentStartDate, request.PaymentEndDate,3,request.IsRanje,request.SupportInputStartDate,request.SupportInputEndDate,request.HasFiscal,request.SuctionModeDetailsIDs,request.SuctionModeIDs,request.CenterIDs);

            response.data = suctionModeCosts;
            response.totalCount = suctionModeCosts.Count();

            return response;
        }

        public GetGeneralResponse<IEnumerable<SuctionModeCost>> GetSuctionModeReport4(
    GetsuctionModeRequestForReport3 request)
        {
            GetGeneralResponse<IEnumerable<SuctionModeCost>> response = new GetGeneralResponse<IEnumerable<SuctionModeCost>>();

            IEnumerable<SuctionModeCost> suctionModeCosts = _campaignPaymentRepository.Report(
                request.RegisterStartDate, request.RegisterEndDate, request.PaymentStartDate, request.PaymentEndDate, 4, request.IsRanje, request.SupportInputStartDate, request.SupportInputEndDate, request.HasFiscal, request.SuctionModeDetailsIDs, request.SuctionModeIDs, request.CenterIDs);

            response.data = suctionModeCosts;
            response.totalCount = suctionModeCosts.Count();

            return response;
        }

        #endregion
    }
}
