#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.CustomerCatalog;
using System.Web.Mvc;
using Services.ViewModels.Customers;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class DocumentStatusController : BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly IDocumentStatusService _documentStatusService;

        #endregion

        #region ctor

        public DocumentStatusController(IEmployeeService employeeService, IDocumentStatusService documentStatusService)
            : base(employeeService)
        {
            this._documentStatusService = documentStatusService;
            this._employeeService = employeeService;
        }
        #endregion

        #region Index

        public ActionResult Index()
        {
            DocumentStatusPageView documentStatusHomePageView = new DocumentStatusPageView();
            documentStatusHomePageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("DocumentStatus_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return View(documentStatusHomePageView);
            }
            #endregion

            //documentStatusHomePageView.DocumentStatusViews = this._documentStatusService.GetDocumentStatuss().DocumentStatusViews;

            return View(documentStatusHomePageView);
        }

        #endregion

        #region Create

        public ActionResult Create()
        {
            DocumentStatusDetailView documentStatusDetailView = new DocumentStatusDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("DocumentStatus_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(documentStatusDetailView);
            }
            #endregion

            
            documentStatusDetailView.EmployeeView = GetEmployee();

            return View(documentStatusDetailView);
        }

        [HttpPost]
        public ActionResult Create(DocumentStatusDetailView documentStatusDetailView)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("DocumentStatus_Insert");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return View(documentStatusDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    AddDocumentStatusRequest request = new AddDocumentStatusRequest();
                    request.CreateEmployeeID = GetEmployee().ID;
                    request.CompleteStatus = documentStatusDetailView.DocumentStatusView.CompleteStatus;
                    request.DocumentStatusName = documentStatusDetailView.DocumentStatusView.DocumentStatusName;
                    request.DefaultStatus = documentStatusDetailView.DocumentStatusView.DefaultStatus;
                    request.SortOrder = documentStatusDetailView.DocumentStatusView.SortOrder;

                    response = this._documentStatusService.AddDocumentStatus(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(documentStatusDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(documentStatusDetailView);
                }

            return View(documentStatusDetailView);
        }
        #endregion

        #region Edit

        public ActionResult Edit(string id)
        {
            DocumentStatusDetailView documentStatusDetailView = new DocumentStatusDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("DocumentStatus_Update");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(documentStatusDetailView);
            }
            #endregion

            documentStatusDetailView.DocumentStatusView = this.GetDocumentStatusView(id);
            //documentStatusDetailView.EmployeeView = GetEmployee();

            return View(documentStatusDetailView);
        }

        [HttpPost]
        public ActionResult Edit(string id, DocumentStatusDetailView documentStatusDetailView)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("DocumentStatus_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return View(documentStatusDetailView);
            }
            #endregion

            if (ModelState.IsValid)
                try
                {
                    EditDocumentStatusRequest request = new EditDocumentStatusRequest();

                    request.ID = Guid.Parse(id);
                    request.ModifiedEmployeeID = GetEmployee().ID;
                    request.CompleteStatus = documentStatusDetailView.DocumentStatusView.CompleteStatus;
                    request.DocumentStatusName = documentStatusDetailView.DocumentStatusView.DocumentStatusName;
                    request.DefaultStatus = documentStatusDetailView.DocumentStatusView.DefaultStatus;
                    request.SortOrder = documentStatusDetailView.DocumentStatusView.SortOrder;
                    request.RowVersion = documentStatusDetailView.DocumentStatusView.RowVersion;

                    response = this._documentStatusService.EditDocumentStatus(request);

                    if (response.success)
                        return RedirectToAction("Index");
                    else
                    {
                        foreach (string error in response.ErrorMessages)
                            ModelState.AddModelError("", error);
                        return View(documentStatusDetailView);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                    return View(documentStatusDetailView);
                }

            return View(documentStatusDetailView);
        }
        #endregion

        #region Details

        public ActionResult Details(string id)
        {
            DocumentStatusDetailView documentStatusDetailView = new DocumentStatusDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("DocumentStatus_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(documentStatusDetailView);
            }
            #endregion

            DocumentStatusView documentStatusView = this.GetDocumentStatusView(id);

            documentStatusDetailView.DocumentStatusView = documentStatusView;

            return View(documentStatusDetailView);
        }
        #endregion

        #region Delete

        public ActionResult Delete(string id)
        {
            DocumentStatusDetailView documentStatusDetailView = new DocumentStatusDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("DocumentStatus_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(documentStatusDetailView);
            }
            #endregion

            documentStatusDetailView.DocumentStatusView = this.GetDocumentStatusView(id);

            return View(documentStatusDetailView);
        }

        [HttpPost]
        public ActionResult Delete(string id, FormCollection collection)
        {

            DocumentStatusDetailView documentStatusDetailView = new DocumentStatusDetailView();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("DocumentStatus_Delete");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(documentStatusDetailView);
            }
            #endregion

            documentStatusDetailView.DocumentStatusView = this.GetDocumentStatusView(id);
            //documentStatusDetailView.EmployeeView = GetEmployee();

            DeleteRequest request = new DeleteRequest() { ID = Guid.Parse(id) };

            GeneralResponse response = this._documentStatusService.DeleteDocumentStatus(request);

            if (response.success)
                return RedirectToAction("Index");
            else
            {
                foreach (string error in response.ErrorMessages)
                    ModelState.AddModelError("", error);
                return View(documentStatusDetailView);
            }
        }
        #endregion

        #region Moveing

        public JsonResult DocumentStatus_MoveUp(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _documentStatusService.MoveUp(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DocumentStatus_MoveDown(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _documentStatusService.MoveDown(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Members

        private DocumentStatusView GetDocumentStatusView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetDocumentStatusResponse response = this._documentStatusService.GetDocumentStatus(request);

            return response.DocumentStatusView;
        }

        #endregion

    }
}

