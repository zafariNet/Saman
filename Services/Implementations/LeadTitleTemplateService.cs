using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Infrastructure.UnitOfWork;
using Model.Base;
using Model.Employees.Interfaces;
using Model.Lead.Interfaces;
using Model.Leads;
using NHibernate;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.Messaging.Leadcatalogservice;
using Services.ViewModels.Leads;

namespace Services.Implementations
{
    public class LeadTitleTemplateService:ILeadTitleTemplateService
    {
        #region Decalar

        private readonly IEmployeeRepository _employeeRepository;
        private readonly ILeadTitleTemplateRepository _leadTitleTemplateRepository;
        private readonly IGroupRepository _groupRepository;
        private readonly IUnitOfWork _uow;

        #endregion

        #region Ctor

        public LeadTitleTemplateService(IEmployeeRepository employeeRepository, ILeadTitleTemplateRepository leadTitleTemplateRepository,IUnitOfWork uow,IGroupRepository groupRepository)
        {
            _employeeRepository = employeeRepository;
            _leadTitleTemplateRepository = leadTitleTemplateRepository;
            _groupRepository = groupRepository;
            _uow = uow;
        }

        #endregion

        #region Read

        public GetGeneralResponse<IEnumerable<LeadTitleTemplateView>> GetLeadTitleTemplates(int pageSize, int pageNumber, IList<FilterData> filter, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<LeadTitleTemplateView>> response=new GetGeneralResponse<IEnumerable<LeadTitleTemplateView>>();

            try
            {
                int index = (pageNumber - 1)*pageSize;
                int count = pageSize;
                string query = FilterUtilityService.GenerateFilterHQLQuery(filter, "LeadTitleTemplate", sort);   
                Response<LeadTitleTemplate> leadTitleTemplates = _leadTitleTemplateRepository.FindAll(query,index,count);

                response.data = leadTitleTemplates.data.ConvertToLeadTitleTemplateViews();
                response.totalCount = leadTitleTemplates.totalCount;
            }
            catch (Exception ex)
            {
               response.ErrorMessages.Add(ex.Message);
                if(ex.InnerException !=null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #region Insert

        public GeneralResponse AddLeadTitleTemplate(IEnumerable<AddLeadTitleTemplateRequest> requests, Guid EmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    LeadTitleTemplate leadTitleTemplate=new LeadTitleTemplate();
                    leadTitleTemplate.ID = Guid.NewGuid();
                    leadTitleTemplate.CreateDate = PersianDateTime.Now;
                    leadTitleTemplate.CreateEmployee = _employeeRepository.FindBy(EmployeeID);
                    leadTitleTemplate.Title = request.Title;
                    leadTitleTemplate.Description = request.Description;
                    leadTitleTemplate.Group = _groupRepository.FindBy(request.GroupID);
                    leadTitleTemplate.RowVersion = 1;

                    if (leadTitleTemplate.GetBrokenRules().Any())
                    {
                        foreach (var item in leadTitleTemplate.GetBrokenRules()) 
                            response.ErrorMessages.Add(item.Rule);

                        return response;
                    }
                    _leadTitleTemplateRepository.Add(leadTitleTemplate);
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

        public GeneralResponse EditLeadTitleTemplate(IEnumerable<EditLeadTitleTemplateRequest> requests, Guid employeeID)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    LeadTitleTemplate leadTitleTemplate = _leadTitleTemplateRepository.FindBy(request.ID);
                    leadTitleTemplate.ModifiedDate = PersianDateTime.Now;
                    leadTitleTemplate.ModifiedEmployee = _employeeRepository.FindBy(employeeID);
                    leadTitleTemplate.Description = request.Description;
                    leadTitleTemplate.Group = _groupRepository.FindBy(request.GroupID);
                    leadTitleTemplate.Title = request.Title;

                    #region RowVersion - Validation

                    if (leadTitleTemplate.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        leadTitleTemplate.RowVersion += 1;
                    }

                    if (leadTitleTemplate.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in leadTitleTemplate.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _leadTitleTemplateRepository.Save(leadTitleTemplate);
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

        public GeneralResponse DeleteLeadTitleTemplate(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    LeadTitleTemplate leadTitleTemplate = _leadTitleTemplateRepository.FindBy(request.ID);
                    if(leadTitleTemplate !=null)
                        _leadTitleTemplateRepository.Remove(leadTitleTemplate);
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
