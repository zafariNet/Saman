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
using Infrastructure.Querying;
using Infrastructure.Domain;
#endregion

namespace Services.Implementations
{
    public class LevelConditionService : ILevelConditionService
    {
        #region Declares
        private readonly ILevelConditionRepository _levelConditionRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly IConditionRepository _conditionRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor
        public LevelConditionService(ILevelConditionRepository levelConditionRepository, IUnitOfWork uow)
        {
            _levelConditionRepository = levelConditionRepository;
            _uow = uow;
        }

        public LevelConditionService(ILevelConditionRepository levelConditionRepository, ILevelRepository levelRepository,
            IConditionRepository conditionRepository, IUnitOfWork uow
            , IEmployeeRepository employeeRepository)
            : this(levelConditionRepository, uow)
        {
            this._conditionRepository = conditionRepository;
            this._levelRepository = levelRepository;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Add

        public GeneralResponse AddLevelCondition(AddLevelConditionRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                LevelCondition levelCondition = new LevelCondition();
                levelCondition.ID = Guid.NewGuid();
                levelCondition.CreateDate = PersianDateTime.Now;
                levelCondition.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                levelCondition.Condition = this._conditionRepository.FindBy(request.ConditionID);
                levelCondition.Level = this._levelRepository.FindBy(request.LevelID);
                levelCondition.RowVersion = 1;

                #region Validation
                if (levelCondition.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in levelCondition.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
                #endregion

                _levelConditionRepository.Add(levelCondition);
                _uow.Commit();

                ////response.success = true;

            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        public GeneralResponse AddLevelConditions(IEnumerable<AddLevelConditionRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (AddLevelConditionRequest request in requests)
                {
                    GetRequest2 getreq = new GetRequest2() { ID1 = request.LevelID, ID2 = request.ConditionID };
                    LevelCondition lc = LevelCondition_Get(getreq);

                    if (lc == null)
                    {
                        LevelCondition levelCondition = new LevelCondition();
                        levelCondition.ID = Guid.NewGuid();
                        levelCondition.CreateDate = PersianDateTime.Now;
                        levelCondition.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                        levelCondition.Condition = this._conditionRepository.FindBy(request.ConditionID);
                        levelCondition.Level = this._levelRepository.FindBy(request.LevelID);
                        levelCondition.RowVersion = 1;

                        #region Validation
                        if (levelCondition.GetBrokenRules().Count() > 0)
                        {
                            

                            foreach (BusinessRule businessRule in levelCondition.GetBrokenRules())
                            {
                                response.ErrorMessages.Add(businessRule.Rule);
                            }

                            return response;
                        }
                        #endregion

                        _levelConditionRepository.Add(levelCondition);
                    }
                    else
                    {
                        
                        response.ErrorMessages.Add("DuplicateCondition");
                        return response;
                    }
                }

                _uow.Commit();

                ////response.success = true;

            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }

            return response;
        }

        #endregion

        #region Edit

        public GeneralResponse EditLevelCondition(EditLevelConditionRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            LevelCondition levelCondition = new LevelCondition();
            levelCondition = _levelConditionRepository.FindBy(request.ID);

            if (levelCondition != null)
            {
                try
                {
                    levelCondition.ModifiedDate = PersianDateTime.Now;
                    levelCondition.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.ConditionID != levelCondition.Condition.ID)
                        levelCondition.Condition = this._conditionRepository.FindBy(request.ConditionID);
                    if (request.LevelID != levelCondition.Level.ID)
                        levelCondition.Level = this._levelRepository.FindBy(request.LevelID);

                    if (levelCondition.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        levelCondition.RowVersion += 1;
                    }

                    if (levelCondition.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in levelCondition.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _levelConditionRepository.Save(levelCondition);
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

                response.ErrorMessages.Add("NoItemToEditKey");
            }
            return response;
        }
        #endregion

        #region Delete

        public GeneralResponse DeleteLevelCondition(DeleteRequest2 request)
        {
            GeneralResponse response = new GeneralResponse();

            LevelCondition levelCondition = new LevelCondition();

            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
            Criterion criterion1 = new Criterion("Level.ID", request.ID1, CriteriaOperator.Equal);
            Criterion criterion2 = new Criterion("Condition.ID", request.ID2, CriteriaOperator.Equal);

            query.Add(criterion1);
            query.Add(criterion2);

            levelCondition = _levelConditionRepository.FindBy(query).FirstOrDefault();

            if (levelCondition != null)
            {
                try
                {
                    _levelConditionRepository.Remove(levelCondition);
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

        public GeneralResponse DeleteLevelConditions(IEnumerable<DeleteRequest2> requests)
        {
            GeneralResponse response = new GeneralResponse();

            foreach (var request in requests)
            {
                LevelCondition levelCondition = new LevelCondition();

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criterion1 = new Criterion("Level.ID", request.ID1, CriteriaOperator.Equal);
                Criterion criterion2 = new Criterion("Condition.ID", request.ID2, CriteriaOperator.Equal);

                query.Add(criterion1);
                query.Add(criterion2);

                levelCondition = _levelConditionRepository.FindBy(query).FirstOrDefault();

                if (levelCondition != null)
                {
                    _levelConditionRepository.Remove(levelCondition);
                }
            }
            try
            {
                _uow.Commit();

                ////response.success = true;
            }
            catch (Exception ex)
            {
                
                response.ErrorMessages.Add(ex.Message);
            }


            return response;
        }

        #endregion

        #region Get One

        public GetLevelConditionResponse GetLevelCondition(GetRequest2 request)
        {
            GetLevelConditionResponse response = new GetLevelConditionResponse();

            try
            {
                LevelCondition levelCondition = new LevelCondition();
                LevelConditionView levelConditionView = levelCondition.ConvertToLevelConditionView();

                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria1 = new Criterion("LevelID", request.ID1, CriteriaOperator.Equal);
                Criterion criteria2 = new Criterion("ConditionID", request.ID2, CriteriaOperator.Equal);

                query.Add(criteria1); query.Add(criteria2);

                levelCondition = _levelConditionRepository.FindBy(query).FirstOrDefault();
                if (levelCondition != null)
                    levelConditionView = levelCondition.ConvertToLevelConditionView();

                response.LevelConditionView = levelConditionView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        private LevelCondition LevelCondition_Get(GetRequest2 request)
        {
            LevelCondition levelCondition = new LevelCondition();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria1 = new Criterion("Level.ID", request.ID1, CriteriaOperator.Equal);
                Criterion criteria2 = new Criterion("Condition.ID", request.ID2, CriteriaOperator.Equal);

                query.Add(criteria1); query.Add(criteria2);

                levelCondition = _levelConditionRepository.FindBy(query).FirstOrDefault();
            }
            catch (Exception ex)
            {

            }

            return levelCondition;
        }

        #endregion

        #region Get All

        public GetLevelConditionsResponse GetLevelConditions()
        {
            GetLevelConditionsResponse response = new GetLevelConditionsResponse();

            try
            {
                IEnumerable<LevelConditionView> levelConditions = _levelConditionRepository.FindAll()
                    .ConvertToLevelConditionViews();

                response.LevelConditionViews = levelConditions;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetLevelConditionsResponse GetLevelConditions(Guid LevelID)
        {
            GetLevelConditionsResponse response = new GetLevelConditionsResponse();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("Level.ID", LevelID, CriteriaOperator.Equal);
                query.Add(criteria);

                IEnumerable<LevelConditionView> levelConditions = _levelConditionRepository.FindBy(query)
                    .ConvertToLevelConditionViews();

                response.LevelConditionViews = levelConditions;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<LevelConditionView>> GetLevelConditions(AjaxGetRequest request)
        {
            GetGeneralResponse<IEnumerable<LevelConditionView>> response = new GetGeneralResponse<IEnumerable<LevelConditionView>>();
            Response<LevelCondition> levelConditionsResponse = new Response<LevelCondition>();

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                // if request.id is not null => Find By LevelID
                if (request.ID != Guid.Empty)
                {
                    Guid levelID = request.ID;
                }
                else
                {
                    levelConditionsResponse = _levelConditionRepository.FindAll(index, count);
                }

                response.data = levelConditionsResponse.data.ConvertToLevelConditionViews();
                response.totalCount = levelConditionsResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion
    }
}
