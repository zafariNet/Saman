using Model.Employees;
using NHibernate.Id;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace Controllers.Controllers
{
    public class ToDoController:BaseController
    {
        #region Declar

        private readonly IEmployeeService _employeeService;
        private readonly IGroupService _groupService;
        private readonly IToDoService _toDoService;
        private readonly IToDoResultService _toDoResulService;
        
        #endregion

        #region Ctor

        public ToDoController(IEmployeeService employeeService, IGroupService groupService, IToDoService todoservice, IToDoResultService toDoResulService)
            :base(employeeService)
        {
            _employeeService = employeeService;
            _groupService = groupService;
            _toDoService = todoservice;
            _toDoResulService = toDoResulService;
        }
        #endregion

        #region new Methods

        #region Read All

        public JsonResult ToDos_Read_All(int? pageSize, int? pageNumber)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("ToDo_Read");
            if (!hasPermission)
            {
                ModelState.AddModelError("", "AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _toDoService.GetAllToDos(PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);

        }
        #endregion

        #region Read By Creator

        public JsonResult ToDos_Read(bool? PrimaryClosed,bool? SecondaryClosed,int? pageSize, int? pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<ToDoView>> response = new GetGeneralResponse<IEnumerable<ToDoView>>();

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _toDoService.GetCreatorEmployeeToDos(GetEmployee().ID,PrimaryClosed, SecondaryClosed, PageSize, PageNumber);

            return Json(response,JsonRequestBehavior.AllowGet);

        }

        #endregion

        #region Read By refered

        public JsonResult ToDos_ReferedEmployee_Read(Guid? CreateEmployeeID ,Guid? ReferedEployeeID, Guid? CustomerID,int? Close,int? TaskStatusID,string StartDateRange,
            string EndDateRange, int? pageSize, int? pageNumber, string sort)
        {
            GetGeneralResponse<IEnumerable<ToDoView>> response = new GetGeneralResponse<IEnumerable<ToDoView>>();
            EmployeeView employee = GetEmployee();
            int PageSize = -1;// pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            Guid referedEmployeeID = ReferedEployeeID == null ?Guid.Empty : (Guid)ReferedEployeeID;
            Guid createEmployeeID = CreateEmployeeID == null ?GetEmployee().ID: (Guid)CreateEmployeeID;
            Guid customerID = CustomerID == null ? Guid.Empty : (Guid)CustomerID;
            response = _toDoService.GetReferedEmployeeToDos(createEmployeeID, referedEmployeeID, Close, TaskStatusID, StartDateRange, EndDateRange, customerID, PageSize, 
                PageNumber,ConvertJsonToObject(sort));
            if(response.data!=null)
            foreach (var item in response.data)
            {
                if (item.CreateEmployeeID == employee.ID)
                    item.IsMine = true;
                foreach (var _item in item.ToDoResults)
                {
                    if (_item.ReferedEmployeeID == employee.ID)
                    {
                        _item.IsMine = true;
                        
                    }
                }
            }
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Secondary Close

        public JsonResult ToDoResult_SecondaryClose(Guid ToDoResultID, string ToDoResultDescription)
        {
            GeneralResponse response = new GeneralResponse();

            response = _toDoService.SecondaryClose(ToDoResultID, ToDoResultDescription,GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region primary Close

        public JsonResult ToDo_PrimaryClose(Guid ToDoID)
        {
            GeneralResponse response = new GeneralResponse();

            response = _toDoService.PrimaryClose(ToDoID, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult ToDo_Insert(AddToDoRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            //IList<Guid> id=new List<Guid>();
            //id.Add(Guid.Parse("12D942E9-9B2F-42A9-82D5-66D661FAC17D"));
            //AddToDoRequest test = new AddToDoRequest() { 
            //    EmployeeIDs=id,
            //    EndDate="1392/10/10",
            //    EndTime="10:20",
            //    PrimaryClosed=false,
            //    PriorityType=(int)PriorityType.High,
            //    StartDate="1392/10/02",
            //    StartTime="20:10",
            //    ToDoDescription="این کار خیلی مهمه بچه ها",
            //    ToDoTitle="یک کار مهم"
            //};

            response = _toDoService.AddTodo(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Attach file to ToDo

        public JsonResult ToDo_AttachFile(Guid ToDoID,string AttacmentName, HttpPostedFileBase file)
        {
            GeneralResponse response = new GeneralResponse();

            ToDoView toDoView = _toDoService.GetToDo(ToDoID).data;

            if (toDoView.Attachment != null)
            {
                response.ErrorMessages.Add("برای این مورد قبلا یک فایل ذخیره شده است. برای افزودن فایل جدید ابتدا فایل قبلی را حذف کنید");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

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
                    fileName = "ToDo_" + ToDoID  + fileExtention;

                    // Gettin current Year and Month
                    PersianCalendar pc = new PersianCalendar();
                    int year = pc.GetYear(DateTime.Now);
                    int month = pc.GetMonth(DateTime.Now);

                    // Create File Path
                    path = Path.Combine(Server.MapPath("~/data/ToDoFiles"), fileName);
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

            AddToDoAttachmentRequest request = new AddToDoAttachmentRequest();
            request.ToDoID = ToDoID;
            request.Attachment = path;
            request.AttachmentName = AttacmentName;
            

            response = _toDoService.AddToDoAttachment(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Attach file to ToDo results

        public JsonResult ToDoResult_AttachFile(Guid ToDoResultID, string AttacmentName, HttpPostedFileBase file)
        {
            GeneralResponse response = new GeneralResponse();
            ToDoResultView toDoResult = _toDoService.GetToDoResult(ToDoResultID);
            if (toDoResult.Attachment != null)
            {
                response.ErrorMessages.Add("برای این مورد قبلا یک فایل ذخیره شده است. برای افزودن فایل جدید ابتدا فایل قبلی را حذف کنید");
                return Json(response,JsonRequestBehavior.AllowGet);
            }
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
                    fileName = "ToDoResult_" + ToDoResultID + fileExtention;

                    // Gettin current Year and Month
                    PersianCalendar pc = new PersianCalendar();
                    int year = pc.GetYear(DateTime.Now);
                    int month = pc.GetMonth(DateTime.Now);

                    // Create File Path
                    path = Path.Combine(Server.MapPath("~/data/ToDoFiles"), fileName);
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

            AddToDoResultAttachmentRequest request = new AddToDoResultAttachmentRequest();
            request.ToDoResultID = ToDoResultID;
            request.Attachment = path;
            request.AttachmentName = AttacmentName;

            response = _toDoService.AddToDoResultAttachment(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Delete ToDo Attach File

        public JsonResult ToDo_DeleteAttacment(Guid ToDoID)
        {
            GeneralResponse response = new GeneralResponse();

            response = _toDoService.DeleteToDoAttachment(ToDoID, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Edit ToDo Result

        public JsonResult ToDoResult_Edit(EditToDoResultRequest request)
        {
            GeneralResponse response = new GeneralResponse();

            //EditToDoResultRequest test = new EditToDoResultRequest() { 
            //    ID=Guid.Parse("8991010F-845B-4DDD-9ADE-89198C8BC0BB"),
            //    RowVersion=1,
            //    SecondaryClosed=true,
            //    ToDoResultDescription="بسته شد",
            //};
            response = _toDoService.EditToDoResult(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete ToDo Result attachment File

        public JsonResult ToDoResult_DeleteAttachment(Guid ToDoResultID)
        {
            GeneralResponse response = new GeneralResponse();

            response = _toDoService.DeleteToDoResultAttachment(ToDoResultID, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get ToDo Results

        public JsonResult ToDoResults_Read(Guid ToDoID)
        {
            GetGeneralResponse<IEnumerable<ToDoResultView>> response = new GetGeneralResponse<IEnumerable<ToDoResultView>>();

            response = _toDoService.GetToDoResults(GetEmployee().ID,ToDoID);
            foreach (var item in response.data)
            {
                if (item.ReferedEmployeeID == GetEmployee().ID)
                    item.IsMine = true;
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //public JsonResult ToDoResults_Read(IEnumerable<Guid> ToDoResultsID)
        //{
        //    GetGeneralResponse<IEnumerable<ToDoResultView>> response = new GetGeneralResponse<IEnumerable<ToDoResultView>>();

        //    response = _toDoService.GetToDoResults(ToDoResultsID);
        //    foreach (var item in response.data)
        //    {
        //        if (item.ReferedEmployeeID == GetEmployee().ID)
        //            item.IsMine = true;
        //    }

        //    return Json(response, JsonRequestBehavior.AllowGet);
        //}

        #endregion

        #endregion

        public JsonResult GetAllChildren()
        {
            GetGeneralResponse<IEnumerable<SimpleEmployeeView>> response = new GetGeneralResponse<IEnumerable<SimpleEmployeeView>>();

            response = _toDoService.GetAllChildren(GetEmployee().ID);


            return Json(response, JsonRequestBehavior.AllowGet);
        }
    }
}
