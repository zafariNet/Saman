using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Base;
using Model.Employees.Interfaces;
using Model.Leads;
using Model.Leads.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.Leadcatalogservice;
using Services.ViewModels.Leads;

namespace Services.Implementations
{
    public class LeadResultTemplateService:ILeadResultTemplateService
    {
        #region Declare

        private readonly IEmployeeRepository _employeerepository;
        private readonly ILeadResultTemplateRepository _leadResultTemplateRepository;
        private readonly IUnitOfWork _uow;
        private readonly IGroupRepository _groupRepository;

        #endregion
        #region Ctor

        public LeadResultTemplateService(IEmployeeRepository employeeRepository,
            ILeadResultTemplateRepository leadResultTemplateRepository, IUnitOfWork uow,
            IGroupRepository groupRepository)
        {
            _employeerepository = employeeRepository;
            _groupRepository = groupRepository;
            _leadResultTemplateRepository = leadResultTemplateRepository;
            _uow = uow;
        }

        #endregion

        #region Get All

        public GetGeneralResponse<IEnumerable<LeadResultTemplateView>> GetLeadResultTemplates(int pageSize,
            int pageNumber, IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<LeadResultTemplateView>> response=new GetGeneralResponse<IEnumerable<LeadResultTemplateView>>();
            try
            {
                
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "LeadResultTemplate", sort);
                Response<LeadResultTemplate> leadResultTemplates = _leadResultTemplateRepository.FindAll(query,index,count);
                response.data = leadResultTemplates.data.ConvertToLeadResultTemplateViews();
                response.totalCount = leadResultTemplates.totalCount;
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

        public GeneralResponse AddLeadResultTemplate(IEnumerable<AddLeadResultTemplateRequest> requests, Guid EmployeeID)
        {
            GeneralResponse response=new GeneralResponse();
            try
            {
                foreach (var request in requests)
                {
                    LeadResultTemplate leadResultTemplate=new LeadResultTemplate();
                    leadResultTemplate.ID = Guid.NewGuid();
                    leadResultTemplate.CreateDate = PersianDateTime.Now;
                    leadResultTemplate.CreateEmployee = _employeerepository.FindBy(EmployeeID);
                    leadResultTemplate.LeadResulTitle = request.LeadResulTitle;
                    leadResultTemplate.Description = request.Description;
                    leadResultTemplate.Group = _groupRepository.FindBy(request.GroupID);
                    leadResultTemplate.RowVersion = 1;
                    // Validation
                    if (leadResultTemplate.GetBrokenRules().Any())
                    {
                        foreach (BusinessRule businessRule in leadResultTemplate.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }
                    _leadResultTemplateRepository.Add(leadResultTemplate);
                    _uow.Commit();
                }
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

        #region  Edit 

        public GeneralResponse EditLeadResultTemplate(IEnumerable<EditLeadResultTemplateRequest> requests,
            Guid EmployeeID)
        {
            GeneralResponse response=new GeneralResponse();
            try
            {
                foreach (var request in requests)
                {
                    LeadResultTemplate leadresTitleTemplate = _leadResultTemplateRepository.FindBy(request.ID);
                    leadresTitleTemplate.LeadResulTitle = request.LeadResulTitle;
                    leadresTitleTemplate.Description = request.Description;
                    leadresTitleTemplate.Group = _groupRepository.FindBy(request.GroupID);

                    #region RowVersion - Validation
                    if (leadresTitleTemplate.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        leadresTitleTemplate.RowVersion += 1;
                    }

                    if (leadresTitleTemplate.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in leadresTitleTemplate.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _leadResultTemplateRepository.Save(leadresTitleTemplate);
                    _uow.Commit();

                }
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

        public GeneralResponse DeleteLeadResultTemplate(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();
            try
            {
                foreach (var request in requests)
                {
                    LeadResultTemplate leadResultTemplate = _leadResultTemplateRepository.FindBy(request.ID);
                    _leadResultTemplateRepository.Remove(leadResultTemplate);
                    _uow.Commit();
                }
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
