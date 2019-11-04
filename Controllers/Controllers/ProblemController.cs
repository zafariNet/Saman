#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Controllers.ViewModels.SupportCatalog;
using System.Web.Mvc;
//using System.Web.UI.WebControls;
using Services.ViewModels.Support;
using Services.Messaging;
using Services.Messaging.SupportCatalogService;

#endregion

namespace Controllers.Controllers
{
    [Authorize]
    public class ProblemController: BaseController
    {
        #region Declares

        private readonly IEmployeeService _employeeService;

        private readonly IProblemService _problemService;

        #endregion

        #region Ctor

        public ProblemController(IEmployeeService employeeService, IProblemService problemService)
            : base(employeeService)
        {
            this._problemService = problemService;
            this._employeeService = employeeService;
        }

        #endregion

        #region Json

        public JsonResult Problems_Read(Guid customerID, int? pageSize, int? pageNumber,string sort)
        {
            GetGeneralResponse<IEnumerable<ProblemView>> response = new GetGeneralResponse<IEnumerable<ProblemView>>();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Problems_Read");
            if (!hasPermission)
            {
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            int PageSize = pageSize == null ? -1 : (int)pageSize;
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;

            response = _problemService.GetProblems(customerID, PageSize, PageNumber,ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Problem_Delete(Guid problemID)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Problem_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            response = _problemService.DeleteProblem(new DeleteRequest() { ID = problemID });

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Problem_Insert(Guid customerID, string problemTitle, string problemDescription, short priority, short state)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Problem_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            AddProblemRequest request = new AddProblemRequest();
            request.CreateEmployeeID = GetEmployee().ID;
            request.ProblemTitle = problemTitle;
            request.CustomerID = customerID;
            request.ProblemDescription = problemDescription;
            request.Priority = priority;
            request.State = state;

            response = this._problemService.AddProblem(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        public JsonResult Problem_Update(Guid problemID, string problemTitle, string problemDescription, short priority, short state, int rowVersion)
        {
            GeneralResponse response = new GeneralResponse();

            #region Access Check

            bool hasPermission = GetEmployee().IsGuaranteed("Problem_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }

            #endregion

            EditProblemRequest request = new EditProblemRequest();
            request.ID = problemID;
            request.RowVersion = rowVersion;
            request.ModifiedEmployeeID = GetEmployee().ID;
            request.ProblemTitle = problemTitle;
            request.ProblemDescription = problemDescription;
            request.Priority = priority;
            request.State = state;

            response = this._problemService.EditProblem(request);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Private Members

        private ProblemView GetProblemView(string id)
        {
            GetRequest request = new GetRequest();
            request.ID = Guid.Parse(id);

            GetProblemResponse response = this._problemService.GetProblem(request);

            return response.ProblemView;
        }

        #endregion

    }
}
