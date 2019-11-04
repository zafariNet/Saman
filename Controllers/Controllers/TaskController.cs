using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;

namespace Controllers.Controllers
{
    public class TaskController:BaseController
    {

        #region Declair

        private readonly IEmployeeService _employeeService;
        private readonly ITaskService _taskService;

        #endregion

        #region Ctor

        public TaskController(IEmployeeService employeeService,ITaskService taskService):base(employeeService)
        {
            _employeeService = employeeService;
            _taskService = taskService;
        }

        #endregion

        #region Add

        public JsonResult Task_Insert(AddTaskRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            //AddTaskRequest request=new AddTaskRequest();

            //request.EndDate = "1393/05/21";
            //request.EndTime = "12:23 AM";
            //request.RemindTime = "1393/05/21 11:30";
            //request.Reminder = true;
            //request.SendSms = true;
            //request.StartDate = "1393/05/21";
            //request.StartTime = "1393/03/03";
            //request.EndDate = "1393/05/22";
            //request.EndTime = "1393/05/21";
            //request.ToDoDescription = "توضیحات وظیفه";
            //request.ToDoTitle = "عنوان وظیفه";

            response = _taskService.AddTask(request,GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read By Employee

        public JsonResult Tasks_Read_ByEmployee(int? pageSize, int? pageNumber,string sort)
        {
            var response = new GetGeneralResponse<IEnumerable<TaskOwnView>>();

            int PageNumber = pageNumber == null ? -1 : (int) pageNumber;
            int PageSize = pageSize == null ? -1 : (int) pageSize;
            response = _taskService.GetEmployeeTasks(GetEmployee().ID, PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Task_FirstPage(string TaskType,int? pageSize,int? pageNumber)
        {

            var response = new GetGeneralResponse<IEnumerable<TaskOwnView>>();
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            response = _taskService.GetFirstPageTasks(TaskType, GetEmployee().ID, PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Close Secondary

        public JsonResult Task_CloseSecondary(Guid ToDoResultID, string ToDoResultDescription)
        {
            GeneralResponse response=new GeneralResponse();
            response = _taskService.CloseSecondary(GetEmployee().ID, ToDoResultID, ToDoResultDescription);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Primary Close

        public JsonResult Task_ClosePrimary(Guid ToDoID)
        {
            GeneralResponse response=new GeneralResponse();

            response = _taskService.ClosePrimary(GetEmployee().ID, ToDoID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Attach File To Secondary Task

        public JsonResult Task_AttachFileToScondary(Guid ToDoResultID, string AttacmentName, HttpPostedFileBase file)
        {
            GeneralResponse response=new GeneralResponse();

            if (_taskService.CanAttachSecondary(ToDoResultID, GetEmployee().ID))
            {

                string fileName = string.Empty;
                string path = string.Empty;

                try
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        // extract the extention
                        var fileExtention = Path.GetExtension(file.FileName);

                        // Create a unique file name
                        fileName = "Task_" + ToDoResultID + fileExtention;

                        // Create File Path
                        path = Path.Combine(Server.MapPath("~/data/TaskFiles"), fileName);
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

                    response = _taskService.AttachFileToSecondary(ToDoResultID, GetEmployee().ID,path, fileName);


                }
                catch (Exception ex)
                {
                    
                    response.ErrorMessages.Add("خطا در افزودن فایل به وظیفه.");
                }
            }
            else
            {
                response.ErrorMessages.Add("عملیات افزودن فایل به وظیفه برای شما امکانپذیر نمیباشد.");
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Attach File To Primary Task

        public JsonResult Task_AttachFileToPrimary(Guid ToDoID, string AttacmentName, HttpPostedFileBase file)
        {
            GeneralResponse response = new GeneralResponse();

            if (_taskService.CanAttachPrimary(GetEmployee().ID,ToDoID))
            {

                string fileName = string.Empty;
                string path = string.Empty;

                try
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileExtention = Path.GetExtension(file.FileName);

                        fileName = "Task_" + ToDoID + fileExtention;
                        path = Path.Combine(Server.MapPath("~/data/TaskFiles"), fileName);
                        new FileInfo(path).Directory.Create();

                        // Uploading
                        using (var fs = new FileStream(path, FileMode.Create))
                        {
                            var buffer = new byte[file.InputStream.Length];
                            file.InputStream.Read(buffer, 0, buffer.Length);

                            fs.Write(buffer, 0, buffer.Length);
                        }
                    }

                    response = _taskService.AttachFileToPrimary(ToDoID, GetEmployee().ID, path,fileName);


                }
                catch (Exception ex)
                {

                    response.ErrorMessages.Add("خطا در افزودن فایل به وظیفه.");
                }
            }
            else
            {
                response.ErrorMessages.Add("عملیات افزودن فایل به وظیفه برای شما امکانپذیر نمیباشد.");
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Task Search

        public JsonResult New_Task_Read(Guid EmployeeID, string Tasktype,string Status, string StartDate, string EndDate)
        {
            var response = new GetGeneralResponse<IEnumerable<TaskOwnView>>();
            response = _taskService.GetTasks(EmployeeID, Tasktype, Status, StartDate, EndDate,GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetTasks(IEnumerable<Guid?> CreateEmployeeIDs, IEnumerable<Guid?> ReferedEmployeeIDs,
            int? Status, int? Type,  string sort, string StartDate,
            string EndDate)
        {
            GetGeneralResponse<IEnumerable<TaskOwnView>> response=new GetGeneralResponse<IEnumerable<TaskOwnView>>();

            response = _taskService.GetTasks(CreateEmployeeIDs, ReferedEmployeeIDs, Status, Type,
                ConvertJsonToObject(sort), GetEmployee().ID, StartDate, EndDate,GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete Attachment

        public JsonResult ToDo_DeleteAttacment(Guid ToDoID)
        {
            var response = new GeneralResponse();

            response = _taskService.DeleteAttachment(ToDoID);

            if (!response.hasError)
            {
                if(System.IO.File.Exists(response.ObjectAdded.ToString()))
                    System.IO.File.Delete(response.ObjectAdded.ToString());
            }

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
