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
    public class ConditionService : IConditionService
    {
        #region Declares
        private readonly IConditionRepository _conditionRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor
        public ConditionService(IConditionRepository conditionRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository)
        {
            _conditionRepository = conditionRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Old Methods

        #region Add
        public GeneralResponse AddCondition(AddConditionRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Condition condition = new Condition();
                condition.ID = Guid.NewGuid();
                condition.CreateDate = PersianDateTime.Now;
                condition.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                condition.ConditionTitle = request.ConditionTitle;
                //condition.CriteriaOperator = request.CriteriaOperator;
                condition.ErrorText = request.ErrorText;
                condition.nHibernate = request.nHibernate;
                //condition.PropertyName = request.PropertyName;
                condition.QueryText = request.QueryText;
                //condition.Value = request.Value;
                condition.RowVersion = 1;

                #region Validation
                if (condition.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in condition.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
                #endregion

                _conditionRepository.Add(condition);
                _uow.Commit();

                ////response.success = true;

            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);

                if (ex.InnerException != null)
                {
                    response.ErrorMessages.Add("FIRST INNER EXPCEPTION: " + ex.InnerException.Message);
                    if (ex.InnerException.InnerException != null)
                        response.ErrorMessages.Add("SECOND INNER EXPCEPTION: " + ex.InnerException.InnerException.Message);
                }
            }

            return response;
        }
        #endregion

        #region Edit
        public GeneralResponse EditCondition(EditConditionRequestOld request)
        {
            GeneralResponse response = new GeneralResponse();
            Condition condition = new Condition();
            condition = _conditionRepository.FindBy(request.ID);

            if (condition != null)
            {
                try
                {
                    condition.ModifiedDate = PersianDateTime.Now;
                    condition.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    condition.ConditionTitle = request.ConditionTitle;
                    condition.ErrorText = request.ErrorText;
                    condition.nHibernate = request.nHibernate;
                    condition.QueryText = request.QueryText;

                    #region RowVersion Check
                    if (condition.RowVersion != request.RowVersion)
                    {
                        
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        condition.RowVersion += 1;
                    }
                    #endregion

                    #region Validation
                    if (condition.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in condition.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }
                    #endregion

                    _conditionRepository.Save(condition);
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

                response.ErrorMessages.Add("NoItemToEditText");
            }
            return response;
        }
        #endregion

        #region Delete
        public GeneralResponse DeleteCondition(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            Condition condition = new Condition();
            condition = _conditionRepository.FindBy(request.ID);

            if (condition != null)
            {
                try
                {
                    _conditionRepository.Remove(condition);
                    _uow.Commit();

                    ////response.success = true;
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);

                    if (ex.Message.Substring(0, ex.Message.Length > 61 ? 61 : ex.Message.Length) == "The DELETE statement conflicted with the REFERENCE constraint"
                        || ex.InnerException.Message.Substring(0, ex.InnerException.Message.Length > 61 ? 61 : ex.InnerException.Message.Length) == "The DELETE statement conflicted with the REFERENCE constraint")
                    {
                        response.ErrorMessages.Clear();
                        response.ErrorMessages.Add("ThisConditionCanNotDeleteKey");
                    }
                    else
                    {
                        if (ex.InnerException != null)
                        {
                            response.ErrorMessages.Add("FIRST INNER EXPCEPTION: " + ex.InnerException.Message);
                            if (ex.InnerException.InnerException != null)
                                response.ErrorMessages.Add("SECOND INNER EXPCEPTION: " + ex.InnerException.InnerException.Message);
                        }
                    }
                }
            }

            return response;
        }
        #endregion

        #region Get One
        public GetConditionResponse GetCondition(GetRequest request)
        {
            GetConditionResponse response = new GetConditionResponse();

            try
            {
                Condition condition = new Condition();
                ConditionView conditionView = condition.ConvertToConditionView();

                condition = _conditionRepository.FindBy(request.ID);
                if (condition != null)
                    conditionView = condition.ConvertToConditionView();

                response.ConditionView = conditionView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        #endregion

        #region Get All
        public GetConditionsResponse GetConditions()
        {
            GetConditionsResponse response = new GetConditionsResponse();

            try
            {
                IEnumerable<ConditionView> Conditions = _conditionRepository.FindAll()
                    .ConvertToConditionViews();

                response.ConditionViews = Conditions.OrderBy(o => o.ConditionTitle);
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<ConditionView>> GetConditions(AjaxGetRequest request)
        {
            GetGeneralResponse<IEnumerable<ConditionView>> response = new GetGeneralResponse<IEnumerable<ConditionView>>();

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                Response<Condition> conditionsResponse = _conditionRepository.FindAll(index, count);

                response.data = conditionsResponse.data.ConvertToConditionViews();
                response.totalCount = conditionsResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #endregion

        #region New Methds

        #region Read

        public GetGeneralResponse<IEnumerable<ConditionView>> GetConditions(int pageSize, int pageNumber, IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<ConditionView>> response = new GetGeneralResponse<IEnumerable<ConditionView>>();
            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Condition> conditions = _conditionRepository.FindAllWithSort(index, count, sort);

                response.data = conditions.data.ConvertToConditionViews();
                response.totalCount = conditions.totalCount;
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

        public GeneralResponse AddConditions(IEnumerable<AddConditionRequest> requests, Guid CreateEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();
            try
            {
                foreach (AddConditionRequest request in requests)
                {
                    Condition condition = new Condition();
                    condition.ID = Guid.NewGuid();
                    condition.CreateDate = PersianDateTime.Now;
                    condition.CreateEmployee = _employeeRepository.FindBy(CreateEmployeeID);
                    condition.ConditionTitle = request.ConditionTitle;
                    //condition.CriteriaOperator = request.CriteriaOperator;
                    condition.ErrorText = request.ErrorText;
                    condition.nHibernate = request.nHibernate;
                    //condition.PropertyName = request.PropertyName;
                    condition.QueryText = request.QueryText;
                    //condition.Value = request.Value;
                    condition.RowVersion = 1;

                    #region Validation

                    if (condition.GetBrokenRules().Count() > 0)
                    {


                        foreach (BusinessRule businessRule in condition.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _conditionRepository.Add(condition);
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

        public GeneralResponse EditConditions(IEnumerable<EditConditionRequest> requests, Guid ModifiedEmployeeID)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (EditConditionRequest request in requests)
                {
                    Condition condition = new Condition();
                    condition.ModifiedDate = PersianDateTime.Now;
                    condition.ModifiedEmployee = _employeeRepository.FindBy(ModifiedEmployeeID);
                    condition.ConditionTitle = request.ConditionTitle;
                    condition.ErrorText = request.ErrorText;
                    condition.nHibernate = request.nHibernate;
                    condition.QueryText = request.QueryText;

                    #region RowVersion Check
                    if (condition.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        condition.RowVersion += 1;
                    }
                    #endregion

                    #region Validation
                    if (condition.GetBrokenRules().Count() > 0)
                    {

                        foreach (BusinessRule businessRule in condition.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }
                    #endregion

                    _conditionRepository.Save(condition);
                }
                _uow.Commit();
            }
            catch (Exception ex)
            {

                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #endregion

        #region Delete

        public GeneralResponse DeleteConditions(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (DeleteRequest request in requests)
                {
                    _conditionRepository.RemoveById(request.ID);
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
