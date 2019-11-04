using Infrastructure.Querying;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Controllers.Controllers
{
    public class NotificationController : BaseController
    {
        #region Declare

        private readonly IEmployeeService _employeeService;
        private readonly INotificationService _notificationService;
        private readonly IGroupService _groupService;

        #endregion

        #region Ctor

        public NotificationController(IEmployeeService employeeService, INotificationService notificationService, IGroupService groupService)
            : base(employeeService)
        {
            _employeeService = employeeService;
            _notificationService = notificationService;
            _groupService = _groupService;
        }

        #endregion

        #region Read

        public JsonResult Notification_Read_ByEmployee( int? pageSize, int? pageNumber, IList<FilterData> filter, string sort)
        {
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            GetGeneralResponse<IEnumerable<NotificationView>> response = new GetGeneralResponse<IEnumerable<NotificationView>>();

            response = _notificationService.NotificationReadByEmployee(GetEmployee().ID, PageSize, PageNumber, filter, ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Notification
        public JsonResult Notofications_Get(int? pageSize,int? pageNumber)
        {

            GetGeneralResponse<IEnumerable<NotificationView>> response = new GetGeneralResponse<IEnumerable<NotificationView>>();

            #region Prepaairing Fiter

            IList<FilterData> filter = new List<FilterData>();

            filter.Add(new FilterData()
            {
                data = new data()
                {
                    comparison = "eq",
                    type = "string",
                    value = new[] { GetEmployee().ID.ToString() }
                },
                field = "ReferedEmployee.ID"
            });


            filter.Add(new FilterData()
            {
                data = new data()
                {
                    comparison = "eq",
                    type = "boolean",
                    value = new[] { bool.FalseString }
                },
                field = "Visited"
            });
            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _notificationService.GetNotifications(filter, null, PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Set as Visited

        public JsonResult Notofication_Get(Guid NotificationID)
        {
            NotificationView response = new NotificationView();

            response = _notificationService.GetNotification(NotificationID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region By Creator

        public JsonResult Notification_GetByCreator(int? pageSize, int? pageNumber)
        {
            GetGeneralResponse<IEnumerable<NotificationView>> response = new GetGeneralResponse<IEnumerable<NotificationView>>();
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            response = _notificationService.GetNotificationsByCreator(GetEmployee().ID, PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult Notification_Insert(AddNotificationRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            //IList<Guid?> l = new List<Guid?>();
            //l.Add((Guid)Guid.Parse("12D942E9-9B2F-42A9-82D5-66D661FAC17D"));

            //AddNotificationRequest res = new AddNotificationRequest();
            //res.ReferedEmployeeIDs = l;
            //res.NotificationTitle = "این یک پیغام جدید است";
            //res.NotificationComment = "یک پیام خیلی خفن آلود";

            response = _notificationService.AddNotification(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion


    }
}

