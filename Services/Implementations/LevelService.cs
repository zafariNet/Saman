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
    public class LevelService : ILevelService
    {
        #region Declares
        private readonly ILevelRepository _levelRepository;
        private readonly ILevelTypeRepository _levelTypeRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly ICustomerLevelRepository _customerLevelRepository;
        #endregion

        #region Ctor
        public LevelService(ILevelRepository levelRepository,
            ILevelTypeRepository levelTypeRepository,
            IUnitOfWork uow,
            ICustomerLevelRepository customerLevelRepository,
            IEmployeeRepository employeeRepository)
        {
            _uow = uow;
            _levelTypeRepository = levelTypeRepository;
            _levelRepository = levelRepository;
            _customerLevelRepository = customerLevelRepository;
            _employeeRepository = employeeRepository;
        }
        #endregion

        #region Add

        public GeneralResponse AddLevel(AddLevelRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            try
            {
                Level level = new Level();
                level.ID = Guid.NewGuid();
                level.CreateDate = PersianDateTime.Now;
                level.CreateEmployee = _employeeRepository.FindBy(request.CreateEmployeeID);
                level.EmailText = request.EmailText;
                level.IsFirstLevel = request.IsFirstLevel;
                level.LevelNikname = request.LevelNikname;
                level.LevelTitle = request.LevelTitle;
                level.LevelType = this._levelTypeRepository.FindBy(request.LevelTypeID);
                level.OnEnter = request.OnEnter;
                level.OnEnterSendEmail = request.OnEnterSendEmail;
                level.OnEnterSendSMS = request.OnEnterSendSMS;
                level.OnExit = request.OnExit;
                level.HasRequireNetwork = request.HasRequireNetwok;
                //level.Options = request.Options;
                level.SMSText = request.SMSText;
                level.Discontinued = request.Discontinued;
                level.LevelStaff = _employeeRepository.FindBy(request.LevelStaffId);
                level.RowVersion = 1;

                #region Validation
                if (level.GetBrokenRules().Count() > 0)
                {
                    

                    foreach (BusinessRule businessRule in level.GetBrokenRules())
                    {
                        response.ErrorMessages.Add(businessRule.Rule);
                    }

                    return response;
                }
                #endregion

                _levelRepository.Add(level);
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

        #region Edit Level, Email, Sms, Options

        public GeneralResponse EditLevel(EditLevelRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            Level level = new Level();
            level = _levelRepository.FindBy(request.LevelID);

            if (level != null)
            {
                try
                {
                    level.ModifiedDate = PersianDateTime.Now;
                    level.ModifiedEmployee = _employeeRepository.FindBy(request.ModifiedEmployeeID);

                    level.LevelNikname = request.LevelNikname;
                    level.LevelTitle = request.LevelTitle;
                    level.HasRequireNetwork = request.HasRequireNetwok;
                    level.OnEnterSendEmail = request.OnEnterSendEmail;
                    level.OnEnterSendSMS = request.OnEnterSendSMS;
                    level.IsFirstLevel = request.IsFirstLevel;
                    level.Discontinued = request.Discontinued;

                    level.LevelStaff = _employeeRepository.FindBy(request.LevelStaffId);

                    #region RowVersion Check

                    if (level.RowVersion != request.RowVersion)
                    {
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        level.RowVersion += 1;
                        response.rowVersion = level.RowVersion;
                    }

                    #endregion

                    #region Validation

                    if (level.GetBrokenRules().Count() > 0)
                    {
                        foreach (BusinessRule businessRule in level.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _levelRepository.Save(level);
                    _uow.Commit();

                }
                catch (Exception ex)
                {
                    response.ErrorMessages.Add(ex.Message);
                }
            }
            else
            {
                
                response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            }
            return response;
        }

        public GeneralResponse EditLevel_Email(Guid LevelID, string EmailText, Guid EmployeeID, int RowVersion)
        {
            GeneralResponse response = new GeneralResponse();
            Level level = new Level();
            level = _levelRepository.FindBy(LevelID);

            if (level != null)
            {
                try
                {
                    level.ModifiedDate = PersianDateTime.Now;
                    level.ModifiedEmployee = _employeeRepository.FindBy(EmployeeID);
                    level.EmailText = EmailText;

                    #region RowVersion
                    
                    if (level.RowVersion != RowVersion)
                    {
                        
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        level.RowVersion += 1;
                        response.rowVersion = level.RowVersion;
                    }

                    #endregion

                    #region Valitaion

                    if (level.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in level.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _levelRepository.Save(level);
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
                
                response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            }
            return response;
        }

        public GeneralResponse EditLevel_Sms(Guid LevelID, string SmsText, Guid EmployeeID, int RowVersion)
        {
            GeneralResponse response = new GeneralResponse();
            Level level = new Level();
            level = _levelRepository.FindBy(LevelID);

            if (level != null)
            {
                try
                {
                    level.ModifiedDate = PersianDateTime.Now;
                    level.ModifiedEmployee = _employeeRepository.FindBy(EmployeeID);
                    level.SMSText = SmsText;

                    #region RowVersion

                    if (level.RowVersion != RowVersion)
                    {
                        
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        level.RowVersion += 1;
                        response.rowVersion = level.RowVersion;
                    }

                    #endregion

                    #region Validation

                    if (level.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in level.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _levelRepository.Save(level);
                    _uow.Commit();
                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add(ex.Message);
                }
            }
            else
            {
                
                response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            }
            return response;
        }

        public GeneralResponse EditLevel_Options(Guid LevelID, Guid EmployeeID, int RowVersion, LevelOptionsView levelOptionsView)
        {
            GeneralResponse response = new GeneralResponse();
            Level level = new Level();
            level = _levelRepository.FindBy(LevelID);


            if (level != null)
            {
                try
                {
                    level.ModifiedDate = PersianDateTime.Now;
                    level.ModifiedEmployee = _employeeRepository.FindBy(EmployeeID);

                    LevelOptions levelOptions = new LevelOptions()
                    {
                        CanAddProblem = levelOptionsView.CanAddProblem,
                        CanChangeNetwork = levelOptionsView.CanChangeNetwork,
                        CanDocumentsOperation = levelOptionsView.CanDocumentsOperation,
                        CanPersenceSupport = levelOptionsView.CanPersenceSupport,
                        CanSale = levelOptionsView.CanSale
                    };
                    level.Options = levelOptions;

                    #region RowVersion Check

                    if (level.RowVersion != RowVersion)
                    {
                        
                        response.ErrorMessages.Add("کارشناس محترم، یک کاربر همزمان با شما در حال ویرایش این رکورد است. بنابراین اطلاعات شما ذخیره نمی شود.");
                        return response;
                    }
                    else
                    {
                        level.RowVersion += 1;
                        response.rowVersion = level.RowVersion;
                    }

                    #endregion

                    #region Validate

                    if (level.GetBrokenRules().Count() > 0)
                    {
                        
                        foreach (BusinessRule businessRule in level.GetBrokenRules())
                        {
                            response.ErrorMessages.Add(businessRule.Rule);
                        }

                        return response;
                    }

                    #endregion

                    _levelRepository.Save(level);
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
                
                response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            }
            return response;

        }

        #endregion

        #region Delete

        public GeneralResponse DeleteLevel(DeleteRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            //Level level = new Level();
            //level = _levelRepository.FindBy(request.ID);

            //if (level != null)
            //    if (level.CustomerLevels != null && level.CustomerLevels.Count() == 0)
            //    {
            //        try
            //        {
            //            _levelRepository.Remove(level);
            //            _uow.Commit();

            //        }
            //        catch (Exception ex)
            //        {
            //            response.ErrorMessages.Add(ex.Message);
            //        }
            //    }
            //    else
            //    {
            //        // به علت وجود سابقه مشتریان در این مرحله امکان حذف مرحله وجود ندارد. در صورت نیاز از گزینه «غیرفعال» استفاده کنید
            //        response.ErrorMessages.Add("CustomerExitstAndCannotDelete");
            //    }

            return response;
        }

        #endregion

        #region Get One

        public GetGeneralResponse<LevelView> GetLevel(GetRequest request)
        {
            GetGeneralResponse<LevelView> response = new GetGeneralResponse<LevelView>();

            try
            {
                Level level = new Level();
                LevelView levelView = level.ConvertToLevelView();

                level = _levelRepository.FindBy(request.ID);
                if (level != null)
                    levelView = level.ConvertToLevelView();

                response.data = levelView;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #region Get Some

        #region All

        public GetGeneralResponse<IEnumerable<LevelView>> GetLevels(int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<LevelView>> response = new GetGeneralResponse<IEnumerable<LevelView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Level> levelsResponse = _levelRepository.FindAll(index, count);
                response.data = levelsResponse.data.ConvertToLevelViews();
                response.totalCount = levelsResponse.totalCount;
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #region By Type

        public GetGeneralResponse<IEnumerable<LevelView>> GetLevelsByLevelTypeID(Guid LevelTypeID, int pageSize, int pageNumber,IList<Sort> sort)
        {
            GetGeneralResponse<IEnumerable<LevelView>> response = new GetGeneralResponse<IEnumerable<LevelView>>();

            try
            {
                int index = (pageNumber - 1) * pageSize;
                int count = pageSize;

                Response<Level> levels = new Response<Level>();

                // اطلاعات یک چرخه خاص
                if (LevelTypeID != Guid.Empty)
                {
                    Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                    Criterion criteria = new Criterion("LevelType.ID", LevelTypeID, CriteriaOperator.Equal);

                    query.Add(criteria);

                    levels = _levelRepository.FindBy(query, index, count,sort);
                }
                // همه اطلاعات
                else
                {
                    levels = _levelRepository.FindAllWithSort(index, count,sort);
                }

                response.data = levels.data.ConvertToLevelViews();
                response.totalCount = levels.totalCount;

            }
            catch (Exception ex)
            {
                
                return response;
            }

            return response;
        }

        #endregion

        #region By Find Type From LevelID

        public GetGeneralResponse<IEnumerable<LevelView>> GetLevelsByLevelID(Guid levelID, int pageSize, int pageNumber)
        {
            GetGeneralResponse<IEnumerable<LevelView>> response = new GetGeneralResponse<IEnumerable<LevelView>>();
            Level level = new Level();

            try
            {
                if (levelID != Guid.Empty)
                {
                    level = _levelRepository.FindBy(levelID);
                    response = GetLevelsByLevelTypeID(level.LevelType.ID, pageSize, pageNumber,null);
                }
                else
                {
                    response = GetLevels(pageSize, pageNumber);
                }

            }
            catch (Exception ex)
            {

            }


            return response;
        }

        #endregion

        #endregion

        #region Get Next Levels

        /// <summary>
        /// بدست آوردن مراحل بعدی
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public GetGeneralResponse<IEnumerable<LevelView>> GetNextLevels(AjaxGetRequest request)
        {
            GetGeneralResponse<IEnumerable<LevelView>> response = new GetGeneralResponse<IEnumerable<LevelView>>();

            Level currentLevel = new Level();
            try
            {
                currentLevel = _levelRepository.FindBy(request.ID);
                response.data = currentLevel.NextLevels().ConvertToLevelViews();
                ////response.success = true;
                response.totalCount = currentLevel.NextLevels().Count();
            }
            catch (Exception ex)
            {
                
                return response;
            }


            return response;
        }

        #endregion

        #region Get Conditions

        public GetGeneralResponse<IEnumerable<ConditionView>> GetConditions(Guid LevelID)
        {
            GetGeneralResponse<IEnumerable<ConditionView>> response = new GetGeneralResponse<IEnumerable<ConditionView>>();

            try
            {
                Level level = _levelRepository.FindBy(LevelID);

                response.data = level.Conditions.ConvertToConditionViews();
                response.totalCount = level.Conditions.Count();
            }
            catch (Exception ex)
            {
                throw;
            }

            return response;
        }

        #endregion

        #region Get Relations

        public IEnumerable<LevelLevelView3> GetRelations(Guid levelTypeID)
        {
            IList<Level> levels = new List<Level>();

            if (levelTypeID == Guid.Empty)
                levels = _levelRepository.FindAll().ToList();
            else
            {
                Infrastructure.Querying.Query query = new Infrastructure.Querying.Query();
                Criterion criteria = new Criterion("LevelType.ID", levelTypeID, CriteriaOperator.Equal);

                query.Add(criteria);

                levels = _levelRepository.FindBy(query).ToList();
            }

            IList<LevelLevelView3> response = new List<LevelLevelView3>();
            foreach (var level in levels)
                foreach (Level nextLevel in level.NextLevels())
                {
                    try
                    {
                        response.Add(new LevelLevelView3() { source = level.ID, target = nextLevel.ID });
                    }
                    catch (Exception ex)
                    {

                    }
                }

            return response;
        }

        #endregion

        #region Graphical Properties

        public GeneralResponse EditGraphicalProperties(Guid LevelID, int X, int Y, int Width, int Height, bool EnableDragging)
        {
            GeneralResponse response = new GeneralResponse();

            Level level = _levelRepository.FindBy(LevelID);


            if (level != null)
            {
                try
                {
                    GraphicalProperties graphicalProperties = new GraphicalProperties();

                    graphicalProperties.X = X;
                    graphicalProperties.Y = Y;
                    graphicalProperties.Width = Width;
                    graphicalProperties.Height = Height;
                    graphicalProperties.EnableDragging = EnableDragging;

                    level.GraphicalObjectProperties = graphicalProperties;

                    _levelRepository.Save(level);
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
                
                response.ErrorMessages.Add("هیچ موردی جهت ویرایش وجود ندارد.");
            }
            return response;
        }

        #endregion

        #region Moving

        public MoveResponse MoveUp(MoveRequest request)
        {
            MoveResponse response = new MoveResponse();

            // Current Price is the Request
            Level currentLevel = new Level();
            currentLevel = _levelRepository.FindBy(request.ID);

            // Find the Previews Price
            Level previewsLevel = new Level();
            try
            {
                currentLevel = _levelRepository.FindAll()
                                .Where(s => s.SortOrder < currentLevel.SortOrder)
                                .OrderByDescending(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentLevel != null && previewsLevel != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentLevel.SortOrder;
                    int previews = (int)previewsLevel.SortOrder;

                    currentLevel.SortOrder = previews;
                    previewsLevel.SortOrder = current;

                    _levelRepository.Save(currentLevel);
                    _levelRepository.Save(previewsLevel);
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

            // Current Price is the Request
            Level currentLevel = new Level();
            currentLevel = _levelRepository.FindBy(request.ID);

            // Find the Previews Price
            Level nextLevel = new Level();
            try
            {
                nextLevel = _levelRepository.FindAll()
                                .Where(s => s.SortOrder > currentLevel.SortOrder)
                                .OrderBy(s => s.SortOrder)
                                .First();
            }
            catch (Exception ex)
            {

            }

            if (currentLevel != null && nextLevel != null)
            {
                try
                {
                    // replacing SortOrders
                    int current = (int)currentLevel.SortOrder;
                    int previews = (int)nextLevel.SortOrder;

                    currentLevel.SortOrder = previews;
                    nextLevel.SortOrder = current;

                    _levelRepository.Save(currentLevel);
                    _levelRepository.Save(nextLevel);
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

    }
}
