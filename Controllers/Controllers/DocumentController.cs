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
using System.Web;
using System.Globalization;
using System.IO;

#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class DocumentController : BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly IDocumentService _documentService;

        private readonly IDocumentStatusService _documentStatusService;

        private readonly ICustomerService _customerService;

        #endregion

        #region ctor

        public DocumentController(IEmployeeService employeeService,
            IDocumentService documentService,
            ICustomerService customerService,
            IDocumentStatusService documentStatusService
            )
            : base(employeeService, customerService)
        {
            this._documentService = documentService;
            this._employeeService = employeeService;
            this._customerService = customerService;
            this._documentStatusService = documentStatusService;
        }

        #endregion

        #region Index

        #region Page Load

        public ActionResult Index(string id)
        {
            DocumentPageView documentPageView = new DocumentPageView();
            documentPageView.EmployeeView = GetEmployee();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Document_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return View(documentPageView);
            }
            #endregion

            documentPageView.CustomerView = GetCustomer(id);


            return View(documentPageView);
        }

        #endregion

        #region Read

        public JsonResult Document_Read(Guid customerID,string sort)
        {
            GetDocumentsResponse response=new GetDocumentsResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Document_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response.DocumentViews, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _documentService.GetDocumentsBy(customerID,ConvertJsonToObject(sort));

            return Json(response.DocumentViews, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult Document_Insert(Guid customerID, DocumentView document, HttpPostedFileBase file)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Document_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            string fileName = string.Empty;
            string path = string.Empty;

            try
            {
                #region Upload file

                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    // extract the extention
                    var fileExtention = Path.GetExtension(file.FileName);
                    // create filename
                    //string fileName = response.ID + "." + fileExtention;
                    // fileName = Path.GetFileName(file.FileName);

                    // Create a unique file name
                    fileName = Guid.NewGuid() + "." + fileExtention;

                    // Gettin current Year and Month
                    PersianCalendar pc = new PersianCalendar();
                    int year = pc.GetYear(DateTime.Now);
                    int month = pc.GetMonth(DateTime.Now);

                    // Create File Path
                    path = Path.Combine(Server.MapPath("~/data/" + year + "/" + month), fileName);
                    // Create reqired directried if not exist
                    new FileInfo(path).Directory.Create();

                    // Uploading
                    using (var fs = new FileStream(path, FileMode.Create))
                    {
                        var buffer = new byte[file.InputStream.Length];
                        file.InputStream.Read(buffer, 0, buffer.Length);

                        fs.Write(buffer, 0, buffer.Length);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                //response.success = false;
                response.ErrorMessages.Add("در آپلود کردن فایل خطایی به وجود آمده است.");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #region Add Document

            AddDocumentRequest request = new AddDocumentRequest();

            request.CreateEmployeeID = GetEmployee().ID;
            request.CustomerID = customerID;
            
            request.DocumentName = document.DocumentName;
            request.Photo = path;
            request.Note = document.Note;

            response = _documentService.AddDocument(request);

            #endregion

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        /*
        public ActionResult Undex()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Undex(HttpPostedFileBase file)
        {
            string fileName = string.Empty;
            string path = string.Empty;

            try
            {
                #region Upload file

                // Verify that the user selected a file
                if (file != null && file.ContentLength > 0)
                {
                    // extract the extention
                    var fileExtention = Path.GetExtension(file.FileName);
                    // create filename
                    //string fileName = response.ID + "." + fileExtention;
                    // fileName = Path.GetFileName(file.FileName);

                    // Create a unique file name
                    fileName = Guid.NewGuid() + fileExtention;

                    // Gettin current Year and Month
                    PersianCalendar pc = new PersianCalendar();
                    int year = pc.GetYear(DateTime.Now);
                    int month = pc.GetMonth(DateTime.Now);
                    string strMonth = month < 10 ? "0" + month.ToString() : month.ToString();

                    // Create File Path
                    path = Path.Combine(Server.MapPath("~/data/" + year + "/" + strMonth), fileName);
                    // Create reqired directried if not exist
                    new FileInfo(path).Directory.Create();

                    // Uploading
                    using (var fs = new FileStream(path, FileMode.Create))
                    {
                        var buffer = new byte[file.InputStream.Length];
                        file.InputStream.Read(buffer, 0, buffer.Length);

                        fs.Write(buffer, 0, buffer.Length);
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
            }

            #region Rename The file

            // extract the extention
            var fileExtention2 = Path.GetExtension(path);
            // Get directory
            var directory = Path.GetDirectoryName(path);
            // create filename
            string fileName2 = directory + "/HOJJAT_" + Guid.NewGuid() + fileExtention2;
            // Rename file
            System.IO.File.Move(path, fileName2);

            #endregion


            return RedirectToAction("Undex");
        }
         */

        #endregion

        #region Update

        public JsonResult Document_Update(DocumentView document)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Document_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion
            
            EditDocumentRequest request = new EditDocumentRequest();

            request.ID = document.ID;
            request.ModifiedEmployeeID = GetEmployee().ID;
            request.DocumentName = document.DocumentName;
            request.Photo = document.Photo;
            request.Note = document.Note;
            request.RowVersion = document.RowVersion;

            response = _documentService.EditDocument(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult Document_Delete(Guid documentID)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Document_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            DeleteRequest request = new DeleteRequest();
            request.ID = documentID;

            response = _documentService.DeleteDocument(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Status

        #region Page Load

        public ActionResult Status()
        {
            DocumentStatusPageView documentStatusPageView = new DocumentStatusPageView();
            documentStatusPageView.EmployeeView = GetEmployee();


            return View(documentStatusPageView);
        }

        #endregion

        #region Read

        public JsonResult Status_Read()
        {
            GetDocumentStatussResponse response=new GetDocumentStatussResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("DocumentStatus_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("","AccessDenied");
                return Json(response.DocumentStatusViews, JsonRequestBehavior.AllowGet);
            }
            #endregion

             response = _documentStatusService.GetDocumentStatuss();

            return Json(response.DocumentStatusViews, JsonRequestBehavior.AllowGet);
        }


        public JsonResult Status_Read_NoPermission()
        {
            GetDocumentStatussResponse response = new GetDocumentStatussResponse();

            response = _documentStatusService.GetDocumentStatuss();

            return Json(response.DocumentStatusViews, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Statuss_Read(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<DocumentStatusView>> response=new GetGeneralResponse<IEnumerable<DocumentStatusView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("DocumentStatus_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

             response = _documentStatusService.GetDocumentStatuss(PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult Status_Insert(IEnumerable<DocumentStatusView> documentStatuss)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("DocumentStatus_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            IList<AddDocumentStatusRequest> requests = new List<AddDocumentStatusRequest>();

            foreach (DocumentStatusView documentStatus in documentStatuss)
            {
                AddDocumentStatusRequest request = new AddDocumentStatusRequest();
                request.CreateEmployeeID = GetEmployee().ID;
                request.CompleteStatus = documentStatus.CompleteStatus;
                request.DefaultStatus = documentStatus.DefaultStatus;
                request.DocumentStatusName = documentStatus.DocumentStatusName;

                requests.Add(request);
            }

            response = _documentStatusService.AddDocumentStatuss(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult Status_Update(IEnumerable<DocumentStatusView> documentStatuss)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("DocumentStatus_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            IList<EditDocumentStatusRequest> requests = new List<EditDocumentStatusRequest>();

            foreach (DocumentStatusView documentStatus in documentStatuss)
            {
                EditDocumentStatusRequest request = new EditDocumentStatusRequest();

                request.ID = documentStatus.ID;
                request.ModifiedEmployeeID = GetEmployee().ID;
                request.CompleteStatus = documentStatus.CompleteStatus;
                request.DefaultStatus = documentStatus.DefaultStatus;
                request.DocumentStatusName = documentStatus.DocumentStatusName;
                request.RowVersion = documentStatus.RowVersion;

                requests.Add(request);
            }

            response = _documentStatusService.EditDocumentStatuss(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult Status_Delete(Guid documentStatusID)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Document_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            DeleteRequest request = new DeleteRequest();
            request.ID = documentStatusID;

            response = _documentStatusService.DeleteDocumentStatus(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #endregion

        #region Private Members

        private DocumentView GetDocumentView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetDocumentResponse response = this._documentService.GetDocument(request);

            return response.DocumentView;
        }
        #endregion

    }
}