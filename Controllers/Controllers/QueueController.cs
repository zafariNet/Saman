using Infrastructure.Querying;
using Services.Implementations;
using Services.Interfaces;
using Services.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;

namespace Controllers.Controllers
{
    public class QueueController:BaseController
    {
        #region Declare

        private readonly IEmployeeService _employeeService;

        private readonly IQueueService _queueService;
        #endregion

        #region Ctor

        public QueueController(IEmployeeService employeeService,IQueueService queueService):base(employeeService)
        {
            _employeeService=employeeService;
            _queueService = queueService;
        }

        #endregion

        #region Update Queues

        public JsonResult Queue_Update()
        {
            GeneralResponse queues = _queueService.GetQueuesFromAsterisk();
            return Json(queues, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update Persian Name

        public JsonResult Queue_Update_PersianName(IEnumerable<EditQueueRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            //IList<EditQueueRequest> req = new List<EditQueueRequest>();
            //req.Add(new EditQueueRequest()
            //{
            //    ID = Guid.Parse("91B45E58-F903-481D-9070-77D0BCE7775D"),
            //    PersianName = "پشتیبانی فتی"
            //});

            response = _queueService.QueueUpdatePersianName(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read All

        public JsonResult Queues_ReadAll(int? pageSize, int? pageNumber, IList<FilterData> filter, string sort)
        {
            GetGeneralResponse<IEnumerable<QueueView>> response=new GetGeneralResponse<IEnumerable<QueueView>>();

            int PageNumber = pageNumber == null ? -1 : (int) pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _queueService.GetAllQueue(PageSize, PageNumber, filter, ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
 