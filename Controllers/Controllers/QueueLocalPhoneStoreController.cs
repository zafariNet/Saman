using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Infrastructure.Querying;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.EmployeeCatalogService;
using Services.ViewModels.Employees;

namespace Controllers.Controllers
{
    
    public class QueueLocalPhoneStoreController:BaseController
    {

        #region Declare

        private readonly IEmployeeService _employeeService;

        private readonly IQueueLocalPhoneStoreService _queueLocalPhoneStoreService;

        #endregion

        #region Ctor

        public QueueLocalPhoneStoreController(IEmployeeService employeeService, IQueueLocalPhoneStoreService queueLocalPhoneStoreService):base(employeeService)
        {
            _employeeService = employeeService;
            _queueLocalPhoneStoreService = queueLocalPhoneStoreService;
        }

        #endregion

        #region Read All

        public JsonResult QueueLocalPhoneStore_Read(int? pageSize, int? pageNumber, IList<FilterData> filter,
            string sort)
        {
            GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreView>> response =
                new GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreView>>();

            
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            response = _queueLocalPhoneStoreService.GetAllQueueLocalPhones(PageSize, PageNumber, filter,
                ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read By Employee

        public JsonResult QueueLocalPhoneStore_Read_ByEployee(Guid OwnerEmployeeID)
        {
            GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreView>> response=new GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreView>>();

            response = _queueLocalPhoneStoreService.GetqueueLocalPhoneStoreByEmployee(OwnerEmployeeID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Insert

        public JsonResult QueueLocalPhoneStore_Insert(IEnumerable<AddQueueLocalPhoneRequest> requests, Guid OwnerEmployeeID)
        {
            GeneralResponse response=new GeneralResponse();

            //IList<AddQueueLocalPhoneRequest> req=new List<AddQueueLocalPhoneRequest>();
            //req.Add(new AddQueueLocalPhoneRequest()
            //{
            //    DangerousRing = 100,
            //    DangerousSeconds = 200,
            //    LocalPhoneStoreEmployeeID = Guid.Parse("E53AA42E-6E7B-4825-871F-C69D692C0C59"),
            //    SendSmsToOffLineUserOnDangerous=true,
            //    SendSmsToOnLineUserOnDangerous = false,
            //    Smstext = "سلام مورچه"
            //});

            response = _queueLocalPhoneStoreService.AddQueueLocalPhoneStore(requests, OwnerEmployeeID, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult QueueLocalPhonStore_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();

            response = _queueLocalPhoneStoreService.DeleteQueueLocalPhoneStore(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Info

        public JsonResult GetInfo(string QueueName,int QueueCount)
        {
            GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreInfoView1>> response =
                new GetGeneralResponse<IEnumerable<QueueLocalPhoneStoreInfoView1>>();

            response = _queueLocalPhoneStoreService.GetQueueEmployee(QueueName, QueueCount);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }
}
