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
#endregion

namespace Services.Implementations
{
    public class LevelLevelService : ILevelLevelService
    {
        #region Declares
        private readonly ILevelLevelRepository _levelLevelRepository;
        private readonly ILevelRepository _levelRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        #endregion

        #region Ctor

        public LevelLevelService(ILevelLevelRepository levelLevelRepository, IUnitOfWork uow)
        {
            _levelLevelRepository = levelLevelRepository;
            _uow = uow;
        }

        public LevelLevelService(ILevelLevelRepository levelLevelRepository,
            ILevelRepository levelRepository, IUnitOfWork uow, IEmployeeRepository employeeRepository)
            : this(levelLevelRepository, uow)
        {
            this._levelRepository = levelRepository;
            _employeeRepository = employeeRepository;
        }

        #endregion

        #region Add One

        public GeneralResponse AddLevelLevel(AddLevelLevelRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                LevelLevel levelLevel = new LevelLevel();
                levelLevel.Level = this._levelRepository.FindBy(request.LevelID);
                levelLevel.RelatedLevel = this._levelRepository.FindBy(request.NextLevelID);

                #region Validation
                if (levelLevel.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in levelLevel.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
                #endregion

                if (GetLevelLevel(request.LevelID, request.NextLevelID).LevelLevelView.LevelID != Guid.Empty)
                {
                    response.ErrorMessages.Add("ThisComminicutionAllreadysavedkey");
                    return response;
                }

                _levelLevelRepository.Add(levelLevel);
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
        #region Add Some
        
        public GeneralResponse AddLevelLevels(IEnumerable<AddLevelLevelRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                foreach (var request in requests)
                {
                    LevelLevel levelLevel = new LevelLevel();
                    levelLevel.Level = this._levelRepository.FindBy(request.LevelID);
                    levelLevel.RelatedLevel = this._levelRepository.FindBy(request.NextLevelID);

                    #region Validation

                    if (levelLevel.GetBrokenRules().Count() > 0)
                    {
                        foreach (BusinessRule businessRule in levelLevel.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }
                    #endregion

                    // ارتباط قبلا ثبت نشده باشد
                    if (GetLevelLevel(request.LevelID, request.NextLevelID).LevelLevelView.LevelID != Guid.Empty)
                    {
                        response.ErrorMessages.Add("RelationAlreadStored");
                        return response;
                    }

                    // خود مرحله به عنوان مرحله بعد انتخاب نشود
                    if (request.LevelID == request.NextLevelID)
                    {
                        response.ErrorMessages.Add("MainAndNextAreTheSame");
                    }

                    _levelLevelRepository.Add(levelLevel);
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


        #region Edit

        public GeneralResponse EditLevelLevel(EditLevelLevelRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            LevelLevel levelLevel = new LevelLevel();
            levelLevel = _levelLevelRepository.FindBy(request.ID);

            if (levelLevel != null)
            {
                try
                {
                    levelLevel.ModifiedDate = PersianDateTime.Now;
                    levelLevel.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);
                    if (request.LevelID != levelLevel.Level.ID)
                        levelLevel.Level = this._levelRepository.FindBy(request.LevelID);
                    if (request.NextLevelID != levelLevel.RelatedLevel.ID)
                        levelLevel.RelatedLevel = this._levelRepository.FindBy(request.NextLevelID);

                    if (levelLevel.RowVersion != request.RowVersion)
                    {

                        response.ErrorMessages.Add("EditConcurrencyKey");
                        return response;
                    }
                    else
                    {
                        levelLevel.RowVersion += 1;
                    }

                    if (levelLevel.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in levelLevel.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    _levelLevelRepository.Save(levelLevel);
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

        #region Delete One

        public GeneralResponse DeleteLevelLevel(DeleteRequest2 request)
        {
            GeneralResponse response = new GeneralResponse();

            Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
            Criterion criterion1 = new Criterion("Level.ID", request.ID1, CriteriaOperator.Equal);
            Criterion criterion2 = new Criterion("RelatedLevel.ID", request.ID2, CriteriaOperator.Equal);
            query.Add(criterion1);
            query.Add(criterion2);

            LevelLevel levelLevel = new LevelLevel();

            levelLevel = _levelLevelRepository.FindBy(query).FirstOrDefault();

            if (levelLevel != null)
            {
                try
                {
                    _levelLevelRepository.Remove(levelLevel);
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
                
                response.ErrorMessages.Add("این رکورد وجود ندارد.");
            }

            return response;
        }

        #endregion
        #region Delete Some

        public GeneralResponse DeleteLevelLevels(IEnumerable<DeleteRequest2> requests)
        {
            GeneralResponse response = new GeneralResponse();

            foreach (var request in requests)
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criterion1 = new Criterion("Level.ID", request.ID1, CriteriaOperator.Equal);
                Criterion criterion2 = new Criterion("RelatedLevel.ID", request.ID2, CriteriaOperator.Equal);
                query.Add(criterion1);
                query.Add(criterion2);

                LevelLevel levelLevel = new LevelLevel();

                levelLevel = _levelLevelRepository.FindBy(query).FirstOrDefault();

                if (levelLevel != null)
                {
                    _levelLevelRepository.Remove(levelLevel);
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

        public GetLevelLevelResponse GetLevelLevel(GetRequest request)
        {
            GetLevelLevelResponse response = new GetLevelLevelResponse();

            try
            {
                LevelLevel levelLevel = new LevelLevel();
                LevelLevelView levelLevelView = levelLevel.ConvertToLevelLevelView();

                levelLevel = _levelLevelRepository.FindBy(request.ID);
                if (levelLevel != null)
                    levelLevelView = levelLevel.ConvertToLevelLevelView();

                response.LevelLevelView = levelLevelView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetLevelLevelResponse GetLevelLevel(Guid levelID, Guid relatedLevelID)
        {
            GetLevelLevelResponse response = new GetLevelLevelResponse();

            try
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criterion1 = new Criterion("Level.ID", levelID, CriteriaOperator.Equal);
                Criterion criterion2 = new Criterion("RelatedLevel.ID", relatedLevelID, CriteriaOperator.Equal);
                query.Add(criterion1);
                query.Add(criterion2);

                LevelLevel levelLevel = new LevelLevel();
                LevelLevelView levelLevelView = levelLevel.ConvertToLevelLevelView();

                levelLevel = _levelLevelRepository.FindBy(query).FirstOrDefault();

                if (levelLevel != null)
                    levelLevelView = levelLevel.ConvertToLevelLevelView();

                response.LevelLevelView = levelLevelView;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        #endregion

        #region Get All

        public GetGeneralResponse<IEnumerable<LevelLevelView>> GetLevelLevels()
        {
            GetGeneralResponse<IEnumerable<LevelLevelView>> response = new GetGeneralResponse<IEnumerable<LevelLevelView>>();

            try
            {
                IEnumerable<LevelLevelView> levelLevels = _levelLevelRepository.FindAll()
                    .ConvertToLevelLevelViews();

                response.data = levelLevels;
            }
            catch (Exception ex)
            {
                
            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<LevelLevelView>> GetLevelLevels(AjaxGetRequest request)
        {
            GetGeneralResponse<IEnumerable<LevelLevelView>> response = new GetGeneralResponse<IEnumerable<LevelLevelView>>();
            Guid levelID = request.ID;

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                Infrastructure.Domain.Response<LevelLevel> levelLevelsResponse = _levelLevelRepository
                    .FindAllWithSort(index, count, null);

                IEnumerable<LevelLevelView> levelLevels = levelLevelsResponse.data
                    .Where(w => w.Level.ID == levelID)
                    .ConvertToLevelLevelViews();

                response.data = levelLevels;
                response.totalCount = levelLevelsResponse.totalCount;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<LevelView>> GetRelatedLevels(AjaxGetRequest request)
        {
            GetGeneralResponse<IEnumerable<LevelView>> response = new GetGeneralResponse<IEnumerable<LevelView>>();
            Guid levelID = request.ID;

            try
            {
                int index = (request.PageNumber - 1) * request.PageSize;
                int count = request.PageSize;

                Infrastructure.Domain.Response<Level> levelsResponse = _levelRepository
                    .FindAllWithSort(index, count, null);

                IEnumerable<LevelView> levels = levelsResponse.data
                    //.Where(w => w.Level.ID == levelID)
                    .ConvertToLevelViews();

                response.data = levels;
                response.totalCount = levelsResponse.totalCount;
                ////response.success = true;
            }
            catch (Exception ex)
            {

            }

            return response;
        }

        public GetGeneralResponse<IEnumerable<LevelLevelView>> GetLevelLevels(Guid levelID)
        {
            GetGeneralResponse<IEnumerable<LevelLevelView>> response = new GetGeneralResponse<IEnumerable<LevelLevelView>>();

            try
            {
                IEnumerable<LevelLevelView> levelLevels = _levelLevelRepository.FindAll()
                    .Where(w => w.Level.ID == levelID)
                    .ConvertToLevelLevelViews();

                response.data = levelLevels;
            }
            catch (Exception ex)
            {
                
            }

            return response;
        }

        #endregion

    }
}
