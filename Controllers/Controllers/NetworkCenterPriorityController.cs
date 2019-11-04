using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using NHibernate.Cfg.MappingSchema;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Services.ViewModels.Customers;

namespace Controllers.Controllers
{
    public class NetworkCenterPriorityController:BaseController
    {
        #region Declare

        private readonly IEmployeeService _employeeService;
        private readonly INetworkCenterPriorityService _networkCenterPriorityService;
        
        #endregion

        #region Ctor

        public NetworkCenterPriorityController(IEmployeeService employeeService, INetworkCenterPriorityService networkCenterPriorityService)
            : base(employeeService)
        {
            _employeeService = employeeService;
            _networkCenterPriorityService = networkCenterPriorityService;
        }

        #endregion

        #region Read

        public JsonResult NetworkCenterPriority_Read(Guid CenterID)
        {
            GetGeneralResponse<IEnumerable<NetworkCenterPriorityView>> response=new GetGeneralResponse<IEnumerable<NetworkCenterPriorityView>>();

            response = _networkCenterPriorityService.GetNetworkCenter(CenterID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion


        #region Update All

        public JsonResult UpdateAll()
        {
            GeneralResponse response=new GeneralResponse();

            response = _networkCenterPriorityService.UpdateAll();

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Moveing

        public JsonResult MoneyAccount_MoveUp(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _networkCenterPriorityService.MoveUp(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult MoneyAccount_MoveDown(Guid ID)
        {
            GeneralResponse response = new GeneralResponse();

            MoveResponse move = new MoveResponse();
            move = _networkCenterPriorityService.MoveDown(new MoveRequest() { ID = ID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
