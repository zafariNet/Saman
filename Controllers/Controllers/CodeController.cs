using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model.Customers;
using Services.Interfaces;
using Controllers.ViewModels.CustomerCatalog;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Services.ViewModels.Customers;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace Controllers.Controllers
{
    [Authorize]
    public class CodeController : BaseController
    {
        #region Declare
        private readonly IEmployeeService _employeeService;
        private readonly ICodeService _codeService;
        private readonly ICenterService _centerService;
        #endregion

        #region Ctor
        public CodeController(IEmployeeService employeeService, ICodeService codeService,
            ICenterService centerService)
            : base(employeeService)
        {
            _codeService = codeService;
            _employeeService = employeeService;
            _centerService = centerService;
        }
        #endregion

        #region Code CRUD
        public JsonResult Codes_Read(int? pageSize, int? pageNumber,string sort)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            response = _codeService.GetCodes(PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Codes_Read_ByCenterID(Guid CenterID, int? pageSize, int? pageNumber,string sort)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            response = _codeService.GetCodes(CenterID, PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Codes_Update(IEnumerable<EditCodeRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = this._codeService.EditCode(requests, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Codes_Insert(IEnumerable<AddCodeRequest> requests,Guid CenterID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = this._codeService.AddCode(requests,CenterID, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Codes_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _codeService.DeleteCode(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Old Methods
        #region Ajax Read
        public ActionResult Code_Read(string id, [DataSourceRequest] DataSourceRequest request)
        {
            CodeHomePageView CodeHomePageView = new CodeHomePageView();
            CodeHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AddcessDenied");
                return Json(CodeHomePageView.CodeViews.ToDataSourceResult(request));
            }
            #endregion

            CodeHomePageView.CodeViews = this._codeService.GetCodes(Guid.Parse(id)).CodeViews;

            return Json(CodeHomePageView.CodeViews.ToDataSourceResult(request));
        }
        #endregion

        #region Index
        public ActionResult Index(string id)
        {
            CodeHomePageView codeHomePageView = new CodeHomePageView();
            codeHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AddcessDenied");
                return View(codeHomePageView);
            }
            #endregion

            codeHomePageView.CodeViews = _codeService.GetCodes(Guid.Parse(id)).CodeViews;

            GetRequest getCenterRequest = new GetRequest();
            getCenterRequest.ID = Guid.Parse(id);
            codeHomePageView.CenterView = _centerService.GetCenter(getCenterRequest).CenterView;
            codeHomePageView.CenterViews = _centerService.GetCenters().CenterViews;

            return View(codeHomePageView);
        }
        #endregion

        #region Create
        public ActionResult Create(string id)
        {
            CodeDetailView codeDetailView = new CodeDetailView();
            codeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AddcessDenied");
                return View(codeDetailView);
            }
            #endregion

            GetRequest getCenterRequest = new GetRequest() { ID = Guid.Parse(id) };
            try
            {
                CodeView codeView = new CodeView();

                codeView.CenterName = _centerService.GetCenter(getCenterRequest).CenterView.CenterName;
                codeView.CenterID = _centerService.GetCenter(getCenterRequest).CenterView.ID;

                codeDetailView.CodeView = codeView;
            }
            catch (Exception ex)
            {
                throw;
            }
            return View(codeDetailView);
        }

        [HttpPost]
        public ActionResult Create(CodeDetailView CodeDetailView)
        {
            CodeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AddcessDenied");
                return View(CodeDetailView);
            }
            #endregion

            GetRequest getCenterRequest = new GetRequest() { ID = CodeDetailView.CodeView.CenterID };
            CodeDetailView.CodeView.CenterName = _centerService.GetCenter(getCenterRequest).CenterView.CenterName;

            if (ModelState.IsValid)
                try
                {
                    AddCodeRequestOld request = new AddCodeRequestOld();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.CenterID = CodeDetailView.CodeView.CenterID;
                    request.CodeName = CodeDetailView.CodeView.CodeName;

                    GeneralResponse response = _codeService.AddCode(request);

                    if (response.success)
                        return RedirectToAction("Index/" + request.CenterID);
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(CodeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(CodeDetailView);
                }

            return View(CodeDetailView);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string id)
        {
            CodeDetailView codeDetailView = new CodeDetailView();
            codeDetailView.CodeView = this.GetCodeView(id);

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AddcessDenied");
                return View(codeDetailView);
            }
            #endregion

            return View(codeDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, CodeDetailView codeDetailView)
        {
            codeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(codeDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditCodeRequestOld request = new EditCodeRequestOld();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.CenterID = codeDetailView.CodeView.CenterID;
                    request.CodeName = codeDetailView.CodeView.CodeName;
                    request.RowVersion = codeDetailView.CodeView.RowVersion;

                    GeneralResponse response = this._codeService.EditCode(request);

                    if (response.success)
                        RedirectToAction("Index/" + request.CenterID);
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(codeDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(codeDetailView);
                }

            return View(codeDetailView);
        }
        #endregion

        #region Delete
        public ActionResult Delete(string id)
        {
            CodeDetailView codeDetailView = new CodeDetailView();
            codeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(codeDetailView);
            }
            #endregion

            codeDetailView.CodeView = this.GetCodeView(id);
            codeDetailView.EmployeeView = GetEmployee();

            return View(codeDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {
            CodeDetailView codeDetailView = new CodeDetailView();
            codeDetailView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(codeDetailView);
            }
            #endregion

            codeDetailView.CodeView = this.GetCodeView(id);

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._codeService.DeleteCode(request);

            if (response.success)
                return RedirectToAction("Index/" + codeDetailView.CodeView.CenterID);
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(codeDetailView);
            }
        }
        #endregion

        #endregion
        
        #region Change Code Center

        public JsonResult ChangeCode_Center(Guid ID, Guid? CenterID)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Code_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _codeService.ChangeCenter(ID, CenterID==null?Guid.Empty:(Guid)CenterID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Members

        private CodeView GetCodeView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetCodeResponse response = this._codeService.GetCode(request);

            return response.CodeView;
        }
        #endregion
    }
}
