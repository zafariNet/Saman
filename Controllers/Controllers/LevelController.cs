#region Usings
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Controllers.ViewModels;
using Controllers.ViewModels.CustomerCatalog;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.Messaging.StoreCatalogService;
using Services.ViewModels.Customers;
using Services.ViewModels.Employees;
using Services.ViewModels.Store;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class LevelController : BaseController
    {
        #region Declares
        private readonly IEmployeeService _employeeService;

        private readonly ILevelService _levelService;

        private readonly ILevelTypeService _levelTypeService;

        private readonly ILevelConditionService _levelConditionService;

        private readonly IConditionService _conditionService;

        private readonly ILevelLevelService _levelLevelService;
        #endregion

        #region Ctor
        public LevelController(IEmployeeService employeeService, ILevelService levelService
            , ILevelTypeService levelTypeService
            , ILevelConditionService levelConditionService
            , IConditionService conditionService
            , ILevelLevelService levelLevelService)
            : base(employeeService)
        {
            _levelService = levelService;
            _levelTypeService = levelTypeService;
            _employeeService = employeeService;
            _levelConditionService = levelConditionService;
            _conditionService = conditionService;
            _levelLevelService = levelLevelService;
        }
        #endregion

        #region Old Methods

        #region Index

        public ActionResult Index()
        {
            LevelHomePageView levelHomePageView = new LevelHomePageView();
            levelHomePageView.EmployeeView = GetEmployee();
            
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Level_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelHomePageView);
            }
            #endregion

            AjaxGetRequest getRequest = new AjaxGetRequest();
            ViewData["Condition"] = _conditionService.GetConditions();

            getRequest.PageSize = 10;
            getRequest.PageNumber = 1;

            GetGeneralResponse<IEnumerable<LevelView>> levelResponse = this._levelService.GetLevels(getRequest.PageSize, getRequest.PageNumber);

            levelHomePageView.LevelViews = levelResponse.data;
            //levelHomePageView.Count = levelResponse.TotalCount;

            DataSourceRequest request = new DataSourceRequest
            {
                PageSize = 10,
                Page = 1
            };

            Level_Read(request);

            return View(levelHomePageView);
        }

        #endregion

        #region Create
        public ActionResult Create()
        {
            LevelDetailView levelDetailView = new LevelDetailView();
            levelDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Level_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelDetailView);
            }
            #endregion

            #region DropDownList For Stuff
            levelDetailView.EmployeeViews = _employeeService.GetEmployees().EmployeeViews;
            List<DropDownItem> empList = new List<DropDownItem>();

            if (levelDetailView.EmployeeViews != null)
                foreach (EmployeeView employeeView in levelDetailView.EmployeeViews)
                {
                    empList.Add(new DropDownItem { Value = employeeView.ID.ToString(), Text = employeeView.Name });
                }
            var empSelectList = new SelectList(empList, "Value", "Text");
            ViewData["Employees"] = empSelectList;
            #endregion

            #region DropDownList For LevelType
            levelDetailView.LevelTypeViews = _levelTypeService.GetLevelTypes().LevelTypeViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (levelDetailView.LevelTypeViews != null)
                foreach (LevelTypeView levelTypeView in levelDetailView.LevelTypeViews)
                {
                    list.Add(new DropDownItem { Value = levelTypeView.ID.ToString(), Text = levelTypeView.Title });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["LevelTypes"] = selectList;
            #endregion

            #region DropDownList For LevelCondition

            IList<ConditionView> conditionViews = _conditionService.GetConditions().ConditionViews.ToList();
            List<DropDownItem> levelConditionList = new List<DropDownItem>();

            if (conditionViews != null)
                foreach (ConditionView conditionView in conditionViews)
                {
                    levelConditionList.Add(new DropDownItem { Value = conditionView.ID.ToString(), Text = conditionView.ConditionTitle });
                }
            var LVselectList = new SelectList(levelConditionList, "Value", "Text");
            ViewData["LevelConditions"] = LVselectList;

            #endregion

            return View(levelDetailView);
        }

        [HttpPost]
        public ActionResult Create(LevelDetailView levelDetailView)
        {
            levelDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Level_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelDetailView);
            }
            #endregion

            #region Prepairing data for Conditions grid
            levelDetailView.LevelConditionViews = _levelConditionService.GetLevelConditions(levelDetailView.LevelView.ID).LevelConditionViews;
            ViewData["conditionData"] = _conditionService.GetConditions().ConditionViews;
            #endregion

            #region DropDownList For Stuff
            levelDetailView.EmployeeViews = _employeeService.GetEmployees().EmployeeViews;
            List<DropDownItem> empList = new List<DropDownItem>();

            if (levelDetailView.EmployeeViews != null)
                foreach (EmployeeView employeeView in levelDetailView.EmployeeViews)
                {
                    empList.Add(new DropDownItem { Value = employeeView.ID.ToString(), Text = employeeView.Name });
                }
            var empSelectList = new SelectList(empList, "Value", "Text");
            ViewData["Employees"] = empSelectList;
            #endregion

            #region DropDownList For LevelType
            levelDetailView.LevelTypeViews = _levelTypeService.GetLevelTypes().LevelTypeViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (levelDetailView.LevelTypeViews != null)
                foreach (LevelTypeView levelTypeView in levelDetailView.LevelTypeViews)
                {
                    list.Add(new DropDownItem { Value = levelTypeView.ID.ToString(), Text = levelTypeView.Title });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["LevelTypes"] = selectList;
            #endregion

            #region DropDownList For LevelCondition

            IList<ConditionView> conditionViews = _conditionService.GetConditions().ConditionViews.ToList();
            List<DropDownItem> levelConditionList = new List<DropDownItem>();

            if (conditionViews != null)
                foreach (ConditionView conditionView in conditionViews)
                {
                    levelConditionList.Add(new DropDownItem { Value = conditionView.ID.ToString(), Text = conditionView.ConditionTitle });
                }
            var LVselectList = new SelectList(levelConditionList, "Value", "Text");
            ViewData["LevelConditions"] = LVselectList;

            #endregion

            levelDetailView.LevelConditionViews = _levelConditionService.GetLevelConditions(levelDetailView.LevelView.ID).LevelConditionViews;

            if (ModelState.IsValid)
                try
                {
                    AddLevelRequest request = new AddLevelRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.Discontinued = levelDetailView.LevelView.Discontinued;
                    request.LevelTypeID = levelDetailView.LevelView.LevelTypeID;
                    request.LevelTitle = levelDetailView.LevelView.LevelTitle;
                    request.LevelNikname = levelDetailView.LevelView.LevelNikname;
                    request.OnEnterSendEmail = levelDetailView.LevelView.OnEnterSendEmail;
                    request.OnEnterSendSMS = levelDetailView.LevelView.OnEnterSendSMS;
                    request.OnEnter = levelDetailView.LevelView.OnEnter;
                    request.OnExit = levelDetailView.LevelView.OnExit;
                    request.EmailText = levelDetailView.LevelView.EmailText;
                    request.SMSText = levelDetailView.LevelView.SMSText;
                    request.IsFirstLevel = levelDetailView.LevelView.IsFirstLevel;
                    request.Options = levelDetailView.LevelView.Options;
                    request.LevelStaffId = Guid.Parse(levelDetailView.LevelView.LevelStaffID);

                    GeneralResponse response = this._levelService.AddLevel(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(levelDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(levelDetailView);
                }

            return View(levelDetailView);
        }
        #endregion

        #region Edit

        public ActionResult Edit(string id)
        {
            LevelDetailView levelDetailView = new LevelDetailView();
            levelDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Level_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelDetailView);
            }
            #endregion

            levelDetailView.LevelView = GetLevelView(id);

            #region Prepairing data for Conditions grid
            levelDetailView.LevelConditionViews = _levelConditionService.GetLevelConditions(levelDetailView.LevelView.ID).LevelConditionViews;
            ViewData["conditionData"] = _conditionService.GetConditions().ConditionViews;
            #endregion

            #region DropDownList For Stuff
            levelDetailView.EmployeeViews = _employeeService.GetEmployees().EmployeeViews;
            List<DropDownItem> empList = new List<DropDownItem>();

            if (levelDetailView.EmployeeViews != null)
                foreach (EmployeeView employeeView in levelDetailView.EmployeeViews)
                {
                    empList.Add(new DropDownItem { Value = employeeView.ID.ToString(), Text = employeeView.Name });
                }
            var empSelectList = new SelectList(empList, "Value", "Text");
            ViewData["Employees"] = empSelectList;
            #endregion

            #region DropDownList For LevelType
            levelDetailView.LevelTypeViews = _levelTypeService.GetLevelTypes().LevelTypeViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (levelDetailView.LevelTypeViews != null)
                foreach (LevelTypeView levelTypeView in levelDetailView.LevelTypeViews)
                {
                    list.Add(new DropDownItem { Value = levelTypeView.ID.ToString(), Text = levelTypeView.Title });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["LevelTypes"] = selectList;
            #endregion

            #region DropDownList For LevelCondition

            IList<ConditionView> conditionViews = _conditionService.GetConditions().ConditionViews.ToList();
            List<DropDownItem> levelConditionList = new List<DropDownItem>();

            if (conditionViews != null)
                foreach (ConditionView conditionView in conditionViews)
                {
                    levelConditionList.Add(new DropDownItem { Value = conditionView.ID.ToString(), Text = conditionView.ConditionTitle });
                }
            var LVselectList = new SelectList(levelConditionList, "Value", "Text");
            ViewData["LevelConditions"] = LVselectList;

            #endregion

            return View(levelDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, LevelDetailView levelDetailView)
        {
            levelDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Level_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelDetailView);
            }
            #endregion

            #region Prepairing data for Conditions grid
            levelDetailView.LevelConditionViews = _levelConditionService.GetLevelConditions(levelDetailView.LevelView.ID).LevelConditionViews;
            ViewData["conditionData"] = _conditionService.GetConditions().ConditionViews;
            #endregion

            #region DropDownList For Stuff
            levelDetailView.EmployeeViews = _employeeService.GetEmployees().EmployeeViews;
            List<DropDownItem> empList = new List<DropDownItem>();

            if (levelDetailView.EmployeeViews != null)
                foreach (EmployeeView employeeView in levelDetailView.EmployeeViews)
                {
                    empList.Add(new DropDownItem { Value = employeeView.ID.ToString(), Text = employeeView.Name });
                }
            var empSelectList = new SelectList(empList, "Value", "Text");
            ViewData["Employees"] = empSelectList;
            #endregion

            #region DropDownList For LevelType
            levelDetailView.LevelTypeViews = _levelTypeService.GetLevelTypes().LevelTypeViews;
            List<DropDownItem> list = new List<DropDownItem>();

            if (levelDetailView.LevelTypeViews != null)
                foreach (LevelTypeView levelTypeView in levelDetailView.LevelTypeViews)
                {
                    list.Add(new DropDownItem { Value = levelTypeView.ID.ToString(), Text = levelTypeView.Title });
                }
            var selectList = new SelectList(list, "Value", "Text");
            ViewData["LevelTypes"] = selectList;
            #endregion

            #region DropDownList For LevelCondition

            IList<ConditionView> conditionViews = _conditionService.GetConditions().ConditionViews.ToList();
            List<DropDownItem> levelConditionList = new List<DropDownItem>();

            if (conditionViews != null)
                foreach (ConditionView conditionView in conditionViews)
                {
                    levelConditionList.Add(new DropDownItem { Value = conditionView.ID.ToString(), Text = conditionView.ConditionTitle });
                }
            var LVselectList = new SelectList(levelConditionList, "Value", "Text");
            ViewData["LevelConditions"] = LVselectList;

            #endregion

            
            if (ModelState.IsValid)
            {
                try
                {
                    EditLevelRequest request = new EditLevelRequest();

                    request.LevelID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.Discontinued = levelDetailView.LevelView.Discontinued;
                    request.LevelTitle = levelDetailView.LevelView.LevelTitle;
                    request.LevelNikname = levelDetailView.LevelView.LevelNikname;
                    request.LevelTypeID = levelDetailView.LevelView.LevelTypeID;
                    request.OnEnterSendEmail = levelDetailView.LevelView.OnEnterSendEmail;
                    request.OnEnterSendSMS = levelDetailView.LevelView.OnEnterSendSMS;
                    request.OnEnter = levelDetailView.LevelView.OnEnter;
                    request.OnExit = levelDetailView.LevelView.OnExit;
                    request.EmailText = levelDetailView.LevelView.EmailText;
                    request.SMSText = levelDetailView.LevelView.SMSText;
                    request.IsFirstLevel = levelDetailView.LevelView.IsFirstLevel;
                    request.Options = levelDetailView.LevelView.Options;
                    request.LevelStaffId = Guid.Parse(levelDetailView.LevelView.LevelStaffID);
                    request.RowVersion = levelDetailView.LevelView.RowVersion;

                    GeneralResponse response = this._levelService.EditLevel(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(levelDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(levelDetailView);
                }
            }

            return View(levelDetailView);
        }
        #endregion

        #region Details
        public ActionResult Details(string id)
        {
            LevelDetailView levelDetailView = new LevelDetailView();
            levelDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Level_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelDetailView);
            }
            #endregion

            LevelView levelView = this.GetLevelView(id);

            levelDetailView.LevelView = levelView;
            
            return View(levelDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            LevelDetailView levelDetailView = new LevelDetailView();

            levelDetailView.LevelView = this.GetLevelView(id);
            levelDetailView.EmployeeView = GetEmployee();

            return View(levelDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            LevelDetailView levelDetailView = new LevelDetailView();
            levelDetailView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Level_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(levelDetailView);
            }
            #endregion

            levelDetailView.LevelView = this.GetLevelView(id);
            
            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._levelService.DeleteLevel(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(levelDetailView);
            }
        }
        #endregion

        #region Ajax

        public ActionResult Level_Read([DataSourceRequest] DataSourceRequest request)
        {
            LevelHomePageView levelHomePageView = new LevelHomePageView();
            levelHomePageView.EmployeeView = GetEmployee();
            GetGeneralResponse<IEnumerable<LevelView>> levelResponse = new GetGeneralResponse<IEnumerable<LevelView>>();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("Level_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                var _result = new DataSourceResult()
                {
                    Data = levelResponse.data,
                    };
                return Json(_result);
            }
            #endregion

            AjaxGetRequest getRequest = new AjaxGetRequest();

            getRequest.PageSize = request.PageSize == 0 ? 10 : request.PageSize;
            getRequest.PageNumber = request.Page;
            levelResponse = _levelService.GetLevels(getRequest.PageSize, getRequest.PageNumber);

            levelHomePageView.LevelViews = levelResponse.data;
            //levelHomePageView.Count = levelResponse.TotalCount;

            var result = new DataSourceResult()
            {
                Data = levelResponse.data,
                //Total = levelResponse.TotalCount
            };
            return Json(result);
        }

        public ActionResult LevelCondition_Read()
        {
            LevelDetailView levelDetailView = new LevelDetailView();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("LevelCondition_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                var _result = new DataSourceResult()
                {
                    Data = levelDetailView.LevelConditionViews
                    //Total = levelResponse.TotalCount
                };
                return Json(_result);
            }
            #endregion

            
            levelDetailView.LevelConditionViews = _levelConditionService.GetLevelConditions(levelDetailView.LevelView.ID).LevelConditionViews;
            ViewData["conditionData"] = _conditionService.GetConditions().ConditionViews;

            var result = new DataSourceResult()
            {
                Data = levelDetailView.LevelConditionViews
            };

            return Json(result);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LevelCondition_Delete([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<LevelConditionView> levelConditions)
        {

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("LevelCondition_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(ModelState.ToDataSourceResult());
            }
            #endregion

            if (levelConditions != null && ModelState.IsValid)
            {
                foreach (LevelConditionView levelCondition in levelConditions)
                    try
                    {
                        DeleteRequest2 deleteRequest = new DeleteRequest2()
                        {
                            ID1 = levelCondition.LevelID,
                            ID2 = levelCondition.Condition.ID
                        };

                        GeneralResponse response = _levelConditionService.DeleteLevelCondition(deleteRequest);

                        if (!response.success)
                        {
                            foreach (string error in response.ErrorMessages)
                                ModelState.AddModelError("", error);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
            }

            return Json(ModelState.ToDataSourceResult());
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LevelCondition_Create([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")]IEnumerable<LevelConditionView> levelConditions)
        {
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("LevelCondition_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(ModelState.ToDataSourceResult());
            }
            #endregion

            ViewData["conditionData"] = _conditionService.GetConditions().ConditionViews;

            if (levelConditions != null && ModelState.IsValid)
            {
                foreach (LevelConditionView levelCondition in levelConditions)
                    try
                    {
                        AddLevelConditionRequest addRequest = new AddLevelConditionRequest()
                        {
                            LevelID = levelCondition.LevelID,
                            ConditionID = levelCondition.Condition.ID,
                            CreateEmployeeID = GetEmployee().ID
                        };

                        GeneralResponse response = _levelConditionService.AddLevelCondition(addRequest);

                        if (!response.success)
                        {
                            foreach (string error in response.ErrorMessages)
                                ModelState.AddModelError("", error);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }

            }

            return Json(ModelState.ToDataSourceResult());
        }
        /*
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult LevelCondition_Update([DataSourceRequest] DataSourceRequest request, [Bind(Prefix = "models")]IEnumerable<LevelConditionView> levelConditions)
        {
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("LevelCondition_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(ModelState.ToDataSourceResult());
            }
            #endregion

            ViewData["conditionData"] = _conditionService.GetConditions().ConditionViews;
            if (levelConditions != null && ModelState.IsValid)
            {
                foreach (LevelConditionView levelCondition in levelConditions)
                    try
                    {
                        EditLevelConditionRequest editRec = new EditLevelConditionRequest()
                        {
                            ConditionID = levelCondition.Condition.ID,
                            LevelID = levelCondition.LevelID
                        };

                        GeneralResponse response = _levelConditionService.EditLevelCondition(editRec);

                        if (!response.success)
                        {
                            foreach (string error in response.ErrorMessages)
                                ModelState.AddModelError("", error);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
            }

            return Json(ModelState.ToDataSourceResult());
        }
        */
        #endregion

        #region Related Levels

        public ActionResult RelatedLevels(string id)
        {
            LevelLevelHomePageView levelLevelHomePageView = new LevelLevelHomePageView();
            levelLevelHomePageView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("RelatedLevel_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(ModelState.ToDataSourceResult());
            }
            #endregion


            levelLevelHomePageView.LevelView = GetLevelView(id);
            levelLevelHomePageView.LevelLevelViews = _levelLevelService.GetLevelLevels(Guid.Parse(id)).data;
            
            #region DropDownList For RealtedLevel

            IList<LevelView> levelViews = _levelService.GetLevelsByLevelTypeID(levelLevelHomePageView.LevelView.LevelTypeID, -1, -1,null).data.ToList();
            List<DropDownItem> levelList = new List<DropDownItem>();

            if (levelViews != null)
                foreach (LevelView levelView in levelViews)
                {
                    levelList.Add(new DropDownItem { Value = levelView.ID.ToString(), Text = levelView.LevelTitle });
                }
            var selectList = new SelectList(levelList, "Value", "Text");
            ViewData["RelatedLevels"] = selectList;

            #endregion

            return View(levelLevelHomePageView);
        }

        public ActionResult RelatedLevel_Read(string id, [DataSourceRequest] DataSourceRequest request)
        {
            LevelLevelHomePageView levelLevelHomePageView = new LevelLevelHomePageView();
            levelLevelHomePageView.EmployeeView = GetEmployee();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("RelatedLevel_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(ModelState.ToDataSourceResult());
            }
            #endregion



            #region Ajax Request
            AjaxGetRequest getRequest = new AjaxGetRequest
            {
                PageSize = request.PageSize == 0 ? 10 : request.PageSize,
                PageNumber = request.Page,
                ID = Guid.Parse(id)
            };
            #endregion

            GetGeneralResponse<IEnumerable<LevelLevelView>> levelLevelsResponse = _levelLevelService.GetLevelLevels(getRequest);

            levelLevelHomePageView.LevelLevelViews = levelLevelsResponse.data;

            var result = new DataSourceResult()
            {
                Data = levelLevelsResponse.data,
                Total = levelLevelsResponse.totalCount
            };
            return Json(result);
        }

        public JsonResult RelatedLevel_Create(string LevelID, string RelatedLevelID)
        {
            GeneralResponse response=new GeneralResponse();

            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("RelatedLevel_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            if (LevelID != null && RelatedLevelID != null)
            {
                Guid levelID = Guid.Parse(LevelID);
                Guid relatedLevelID = Guid.Parse(RelatedLevelID);

                AddLevelLevelRequest request = new AddLevelLevelRequest()
                {
                    NextLevelID = relatedLevelID,
                    LevelID = levelID
                };

                response = _levelLevelService.AddLevelLevel(request);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                response.ErrorMessages.Add("آیدی ثبت نشده است.");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult RelatedLevel_Delete([DataSourceRequest] DataSourceRequest request,
            [Bind(Prefix = "models")] IEnumerable<LevelLevelView> levelLevels)
        {

            #region Check Access

            bool hasPermission = GetEmployee().IsGuaranteed("RelatedLevel_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(ModelState.ToDataSourceResult());
            }

            #endregion

            if (levelLevels != null && ModelState.IsValid)
            {
                foreach (LevelLevelView levelLevel in levelLevels)
                    try
                    {
                        DeleteRequest2 deleteRequest = new DeleteRequest2()
                        {
                            ID1 = levelLevel.LevelID,
                            ID2 = levelLevel.RelatedLevelID
                        };

                        GeneralResponse response = _levelLevelService.DeleteLevelLevel(deleteRequest);

                        if (!response.success)
                        {
                            foreach (string error in response.ErrorMessages)
                                ModelState.AddModelError("", error);
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
            }

            return Json(ModelState.ToDataSourceResult());
        }

        #endregion

        #endregion


        #region Add Condition

        public JsonResult AddCondition(string LevelID, string ConditionID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access

            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            if (LevelID != null && ConditionID != null)
            {
                Guid levelID = Guid.Parse(LevelID);
                Guid conditionID = Guid.Parse(ConditionID);

                AddLevelConditionRequest request = new AddLevelConditionRequest()
                {
                    ConditionID = conditionID,
                    LevelID = levelID,
                    CreateEmployeeID = GetEmployee().ID
                };

                response = _levelConditionService.AddLevelCondition(request);

                return Json(response, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var result = new {success = "False", ErrorMessages = "IDnotSavedKey"};

                return Json(result, JsonRequestBehavior.AllowGet);
            }
        }

        #endregion

        #region Private Members

        private LevelView GetLevelView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetGeneralResponse<LevelView> response = this._levelService.GetLevel(request);

            return response.data;
        }

        private LevelLevelView GetLevelLevelView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetLevelLevelResponse response = _levelLevelService.GetLevelLevel(request);

            return response.LevelLevelView;
        }

        #endregion
        #region Next Levels Json

        #region Read

        public JsonResult NextLevels_Read(Guid LevelID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<LevelView>> response = new GetGeneralResponse<IEnumerable<LevelView>>();

            #region Check Access

            bool hasPermission = GetEmployee().IsGuaranteed("Level_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            AjaxGetRequest request = new AjaxGetRequest()
            {
                PageNumber = pageNumber == null ? -1 : (int)pageNumber,
                PageSize = pageSize == null ? -1 : (int)pageSize,
                ID = LevelID
            };

            response = _levelService.GetNextLevels(request);



            return Json(response, JsonRequestBehavior.AllowGet);
        }
        public JsonResult NextLevels_Read_NoPermission(Guid LevelID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<LevelView>> response = new GetGeneralResponse<IEnumerable<LevelView>>();
                      
            AjaxGetRequest request = new AjaxGetRequest()
            {
                PageNumber = pageNumber == null ? -1 : (int)pageNumber,
                PageSize = pageSize == null ? -1 : (int)pageSize,
                ID = LevelID
            };

            response = _levelService.GetNextLevels(request);



            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult NextLevels_Insert(Guid LevelID,IEnumerable<LevelLevelView2> nextLevels)
        {
            GeneralResponse response=new GeneralResponse();

            #region Check Access

            bool hasPermission = GetEmployee().IsGuaranteed("Level_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            IList<AddLevelLevelRequest> requests = new List<AddLevelLevelRequest>();

            foreach (var nextLevel in nextLevels)
            {
                if (LevelID != null && nextLevel.ID != null)
                {
                    AddLevelLevelRequest request = new AddLevelLevelRequest()
                    {
                        NextLevelID = nextLevel.ID,
                        LevelID = LevelID
                    };

                    requests.Add(request);
                }
            }

            response = _levelLevelService.AddLevelLevels(requests);
            

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult NextLevels_Delete(IEnumerable<LevelLevelView2> nextLevels,Guid LevelID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Check Access

            bool hasPermission = GetEmployee().IsGuaranteed("Level_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            IList<DeleteRequest2> requests = new List<DeleteRequest2>();

            foreach (var nextLevel in nextLevels)
            {
                DeleteRequest2 deleteRequest = new DeleteRequest2()
                {
                    ID1 = LevelID,
                    ID2 = nextLevel.ID
                };

                requests.Add(deleteRequest);

            }

            response = _levelLevelService.DeleteLevelLevels(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
        
        #endregion


        #region LevelsRead

        #region By Type
        /// <summary>
        /// نمایش همه مراحل مربوط به یک چرخه
        /// </summary>
        /// <returns></returns>
        public JsonResult Levels_Read_ByType(Guid? levelTypeID, int? pageSize, int? pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<LevelView>> response = new GetGeneralResponse<IEnumerable<LevelView>>();

            //#region Access Check

            //bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Read");
            //if (!hasPermission)
            //{
            //    response.ErrorMessages.Add("AccessDenied");
            //    return Json(response, JsonRequestBehavior.AllowGet);
            //}

            //#endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            Guid LevelTypeID = levelTypeID == null ? Guid.Empty : (Guid)levelTypeID;
            response = _levelService.GetLevelsByLevelTypeID(LevelTypeID, PageSize, PageNumber,ConvertJsonToObject(sort));
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion
        public JsonResult First_Levels_Read_ByTypeNoPermission(Guid? levelTypeID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<LevelView>> response = new GetGeneralResponse<IEnumerable<LevelView>>();
            
            

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            Guid LevelTypeID = levelTypeID == null ? Guid.Empty : (Guid)levelTypeID;
            response = _levelService.GetLevelsByLevelTypeID(LevelTypeID, PageSize, PageNumber,null);
            IEnumerable<LevelView> levelViews = response.data.Where(x => x.IsFirstLevel);
            response.data = levelViews;
            
            
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        
        public JsonResult First_Levels_Read_ByType(Guid? levelTypeID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<LevelView>> response = new GetGeneralResponse<IEnumerable<LevelView>>();
            
            #region Check Access
            bool hasPermission = GetEmployee().IsGuaranteed("LevelType_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            Guid LevelTypeID = levelTypeID == null ? Guid.Empty : (Guid)levelTypeID;
            response = _levelService.GetLevelsByLevelTypeID(LevelTypeID, PageSize, PageNumber,null);
            IEnumerable<LevelView> levelViews = response.data.Where(x => x.IsFirstLevel);
            response.data = levelViews;
            
            
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion



        #region By Level

        public JsonResult Levels_Read_ByLevel(Guid? levelID, int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<LevelView>> response=new GetGeneralResponse<IEnumerable<LevelView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("level_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            Guid LevelID = Guid.Empty;
            if (levelID != Guid.Empty && levelID != null)
            {
                LevelID = (Guid)levelID;
            }

            response = _levelService.GetLevelsByLevelID(LevelID, PageSize, PageNumber);
            

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        

        #endregion

        #region LevelTypes Read

        public JsonResult LevelTypes_Read(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<LevelTypeView>> response=new GetGeneralResponse<IEnumerable<LevelTypeView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Level_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            AjaxGetRequest request = new AjaxGetRequest()
            {
                PageNumber = pageNumber == null ? -1 : (int)pageNumber,
                PageSize = pageSize == null ? -1 : (int)pageSize
            };

            response = _levelTypeService.GetLevelTypes(request);

            

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LevelTypes_Read_NoPermission(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<LevelTypeView>> response = new GetGeneralResponse<IEnumerable<LevelTypeView>>();

            AjaxGetRequest request = new AjaxGetRequest()
            {
                PageNumber = pageNumber == null ? -1 : (int)pageNumber,
                PageSize = pageSize == null ? -1 : (int)pageSize
            };

            response = _levelTypeService.GetLevelTypes(request);



            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Conditions Json

        #region Read

        public JsonResult Condition_Read(Guid LevelID)
        {
            GetGeneralResponse<IEnumerable<ConditionView>> response=new GetGeneralResponse<IEnumerable<ConditionView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _levelService.GetConditions(LevelID);

            

            return Json(response, JsonRequestBehavior.AllowGet);
        }

            
        #endregion

        #region List

        public JsonResult Condition_List(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<ConditionView>> response=new GetGeneralResponse<IEnumerable<ConditionView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            AjaxGetRequest request = new AjaxGetRequest()
            {
                PageNumber = pageNumber == null ? -1 : (int)pageNumber,
                PageSize = pageSize == null ? -1 : (int)pageSize
            };


            response = _conditionService.GetConditions(request);

            

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult Condition_Insert(Guid LevelID, IEnumerable<LevelConditionView2> levelConditions)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            IList<AddLevelConditionRequest> requests = new List<AddLevelConditionRequest>();

            foreach (var levelCondition in levelConditions)
            {
                AddLevelConditionRequest request = new AddLevelConditionRequest()
                {
                    ConditionID = levelCondition.ConditionID,
                    LevelID = LevelID,
                    CreateEmployeeID = GetEmployee().ID
                };

                requests.Add(request);
            }

            response = _levelConditionService.AddLevelConditions(requests);
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult Condition_Delete(Guid LevelID, IEnumerable<LevelConditionView2> levelConditions)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Condition_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion


            IList<DeleteRequest2> deleteRequests = new List<DeleteRequest2>();

            foreach (var levelCondition in levelConditions)
            {
                DeleteRequest2 deleteRequest = new DeleteRequest2()
                {
                    ID1 = LevelID,
                    ID2 = levelCondition.ID
                };
                deleteRequests.Add(deleteRequest);
            }

            response = _levelConditionService.DeleteLevelConditions(deleteRequests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Graphical View

        public JsonResult Relations_Read(Guid? levelTypeID)
        {
            GetGeneralResponse<IEnumerable<LevelLevelView3>> response=new GetGeneralResponse<IEnumerable<LevelLevelView3>>();
            IEnumerable<LevelLevelView3> relations=new BindingList<LevelLevelView3>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Relation_Read");
            if (!hasPermission)
            {
                response = new GetGeneralResponse<IEnumerable<LevelLevelView3>>()
                {
                    data = relations,
                    //success = true,
                    totalCount = relations.Count()
                };

                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            Guid LevelTypeID = levelTypeID != null ? (Guid)levelTypeID : Guid.Empty;

            relations = _levelService.GetRelations(LevelTypeID);

            response = new GetGeneralResponse<IEnumerable<LevelLevelView3>>()
            {
                data = relations,
                //success = true,
                totalCount = relations.Count()
            };


            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UpdateGraphicalProperties(Guid levelID, int x, int y, int width, int height, bool enableDragging)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Level_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _levelService.EditGraphicalProperties(levelID, x, y, width, height, enableDragging);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GraphicalData_Read(Guid? levelTypeID)
        {
            GetGeneralResponse<object> response=new GetGeneralResponse<object>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Level_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            Guid LevelTypeID = levelTypeID != null ? (Guid)levelTypeID : Guid.Empty;

            IEnumerable<LevelView> levelViews = _levelService.GetLevelsByLevelTypeID(LevelTypeID, -1, -1,null).data;

            IEnumerable<LevelLevelView3> relations = _levelService.GetRelations(LevelTypeID);

            response = new GetGeneralResponse<object>();

            response.data = new { levelViews, relations };
            
            response.totalCount = -1;

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Email And Sms Update

        [ValidateInput(false)]
        public JsonResult Level_Email_Update(Guid LevelID, string EmailText, int RowVersion)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Level_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = this._levelService.EditLevel_Email(LevelID, EmailText, GetEmployee().ID, RowVersion);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Level_Sms_Update(Guid LevelID, string SmsText, int RowVersion)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Level_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = this._levelService.EditLevel_Sms(LevelID, SmsText, GetEmployee().ID, RowVersion);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert & Update & Delete Level

        public JsonResult Level_Insert(AddLevelRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Level_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            request.CreateEmployeeID = GetEmployee().ID;
            request.Discontinued = false;

            response = _levelService.AddLevel(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Level_Update(EditLevelRequest request)
        {
            GeneralResponse response = new GeneralResponse();
            
            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Level_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion
            
            request.ModifiedEmployeeID = GetEmployee().ID;

            response = _levelService.EditLevel(request);
            
            
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Level_Delete(Guid levelID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Level_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _levelService.DeleteLevel(new DeleteRequest() { ID = levelID });
            

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Options Update

        public JsonResult Level_Options_Update(Guid LevelID, int RowVersion, bool canSale, bool canChangeNetwork, bool canPersenceSupport,
            bool canAddProblem, bool canDocumentsOperation)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Level_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            LevelOptionsView levelOptionsView = new LevelOptionsView()
            {
                CanSale = canSale,
                CanChangeNetwork = canChangeNetwork,
                CanPersenceSupport = canPersenceSupport,
                CanAddProblem = canAddProblem,
                CanDocumentsOperation = canDocumentsOperation
            };

            response = _levelService.EditLevel_Options(LevelID, GetEmployee().ID, RowVersion, levelOptionsView);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
            
        #endregion

        #region Moveing

        public JsonResult Level_MoveUp(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _levelService.MoveUp(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Level_MoveDown(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _levelService.MoveDown(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
