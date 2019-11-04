using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Base;
using Model.Customers.Interfaces;
using Model.Employees;
using Model.Employees.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;

namespace Services.Implementations
{
    public class CampaignAgentService : ICampaignAgentService
    {
        #region Declar


        private readonly IEmployeeRepository _employeeRepository;

        private readonly ICampaignAgentRepository _campaignAgentRepository;

        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public CampaignAgentService(IEmployeeRepository employeeRepository, ICampaignAgentRepository campaignAgentRepository, IUnitOfWork uow)
        {
            _employeeRepository = employeeRepository;
            _campaignAgentRepository = campaignAgentRepository;
            _uow = uow;
        }

        #endregion

        #region Read

        public GetGeneralResponse<IEnumerable<CampaignAgentView>> GetCampaignAgentes(int pageSize, int pageNumber,
            IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<CampaignAgentView>> response=new GetGeneralResponse<IEnumerable<CampaignAgentView>>();

            try
            {
                int index = (pageNumber - 1)*pageSize;
                int count = pageSize;

                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "CampaignAgent", sort);

                Response<CampaignAgent> campaignagentes = _campaignAgentRepository.FindAll(query);

                response.data = campaignagentes.data.ConvertToCampaignAgentViews();
                response.totalCount = campaignagentes.totalCount;
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

        #region Add

        public GeneralResponse AddCampaignAgent(IEnumerable<AddCampaignAgentRequest> requests,Guid CreateEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (AddCampaignAgentRequest request in requests)
                {
                    CampaignAgent campaignAgent = new CampaignAgent();
                    campaignAgent.ID = Guid.NewGuid();
                    campaignAgent.CampaignAgentName = request.CampaignAgentName;
                    campaignAgent.CreateDate = PersianDateTime.Now;
                    campaignAgent.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    campaignAgent.RowVersion = 1;

                    _campaignAgentRepository.Add(campaignAgent);
                }

                _uow.Commit();

            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException.Message !=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Edit

        public GeneralResponse EditCampaignAgent(IEnumerable<EditCampaignAgentRequest> requests, Guid ModifiedEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (EditCampaignAgentRequest request in requests)
                {
                    CampaignAgent campaignAgent=new CampaignAgent();

                    campaignAgent = _campaignAgentRepository.FindBy(request.ID);
                    campaignAgent.CampaignAgentName = request.CampaignAgentName;
                    campaignAgent.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    campaignAgent.ModifiedDate = PersianDateTime.Now;

                    #region RowVersion - Validation
                    if (campaignAgent.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        campaignAgent.RowVersion += 1;
                    }

                    if (campaignAgent.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in campaignAgent.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _campaignAgentRepository.Save(campaignAgent);
                }
                _uow.Commit();
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

        #region Delete

        public GeneralResponse DeleteCampaignAgenet(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {                 

                    _campaignAgentRepository.RemoveById(request.ID);
                }

                _uow.Commit();
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
    }
}
