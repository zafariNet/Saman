using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Infrastructure.Persian;
using Infrastructure.Querying;
using Model.Leads;
using Newtonsoft.Json;
using Services.Interfaces;
using Services.Messaging;
using Services.Messaging.Leadcatalogservice;
using Services.ViewModels.Leads;

namespace Controllers.Controllers
{
    public class NegotiationController:BaseController
    {
        #region Declare

        private readonly  IEmployeeService _employeeService;
        private readonly INegotiationService _negotiationService;

        #endregion

        #region Ctor

        public NegotiationController(IEmployeeService employeeService, INegotiationService negotiationService)
            : base(employeeService)
        {
            _employeeService = employeeService;
            _negotiationService = negotiationService;
        }

        #endregion

        #region Read Own

        public JsonResult Negotiations_Read(int? pageSize, int? pageNumber, IList<FilterData> filter, string sort, Guid? CustomerID, bool? ForChild)
        {


            var response=new GetGeneralResponse<IEnumerable<NegotiationView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Negotiation_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;

            IList<FilterData> Filters=new BindingList<FilterData>();

                if (filter != null)
                    foreach (var item in filter)
                        Filters.Add(item);
            if (ForChild!=null)
            {
                if (ForChild == true)
                {
                    response = _negotiationService.GetChildNegotiations(GetEmployee().ID, PageSize, PageNumber, Filters,
                        ConvertJsonToObject(sort));

                    return Json(response, JsonRequestBehavior.AllowGet);
                }

            }
            if (CustomerID == null)
            {
                Filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eqOr",
                        type = "string",
                        value = new[] {GetEmployee().ID.ToString()}
                    },
                    field = "ReferedEmployee.ID"
                });
            }

            if (CustomerID != null)
            {
                Filters.Add(new FilterData()
                {
                    data = new data()
                    {
                        comparison = "eq",
                        type = "string",
                        value = new[] { CustomerID.ToString() }
                    },
                    field = "Customer.ID"
                });
            }



            response = _negotiationService.GetOwnNegotiation(PageSize, PageNumber, Filters, ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Read Child

        public JsonResult Negotiations_ReadChilds(int? pageSize, int? pageNumber, IList<FilterData> filter, string sort, bool ForChild)
        {

            GetGeneralResponse<IEnumerable<NegotiationView>> response=new GetGeneralResponse<IEnumerable<NegotiationView>>();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Negotiation_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion
                        
            int PageNumber = pageNumber == null ? -1 : (int)pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            response = _negotiationService.GetChildNegotiations(GetEmployee().ID, PageSize, PageNumber, filter,
                ConvertJsonToObject(sort));

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion
         
        #region Add

        public JsonResult Negotiation_Insert(AddNegotiationRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Negotiation_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _negotiationService.AddNegotiation(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Close Negotiation

        public JsonResult Negotiation_Close(Guid NegotioationID, Guid LeadResultTemplateID,
            string NegotiationResultDescription)
        {

            GeneralResponse response = new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Negotiation_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion


            response = _negotiationService.CloseNegotiation(NegotioationID, GetEmployee().ID, LeadResultTemplateID,
                NegotiationResultDescription, 1);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Create negotiation for customers

        public JsonResult Negotiation_CreateForCustomers(IEnumerable<Guid> CustomerIDs, Guid ReferedEmployeeID,
            Guid LeadTitleTemplateID,
            string NegotiationDesciption,
            string NegotiationDate,
            string NegotiationTime,
            string RememberDate,
            string RememberTime,
            bool? SendSms)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Negotiation_Insert");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _negotiationService.CreateNegotiationForCustomers(CustomerIDs, ReferedEmployeeID,
                LeadTitleTemplateID, NegotiationDesciption, NegotiationDate, NegotiationTime, RememberDate, RememberTime,
                SendSms,GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Customer's Negotiations Read

        public JsonResult Negotiations_Customer_Read(Guid CustomerID, int? pageSize, int? pageNumber,
            IList<FilterData> filter, string sort)
        {
            GetGeneralResponse<IEnumerable<NegotiationView>> response=new GetGeneralResponse<IEnumerable<NegotiationView>>();



            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Negotiation_Read");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            int PageNumber = pageNumber == null ? -1 : (int) pageNumber;
            int PageSize = pageSize == null ? -1 : (int)pageSize;
            
            response = _negotiationService.GetCustomerNegotiations(CustomerID, filter, ConvertJsonToObject(sort),
                PageSize, PageNumber);

            return Json(response, JsonRequestBehavior.AllowGet);
        }


        #endregion

        #region Negotiation Delay

        public JsonResult Negotiation_Delay(Guid NegotiationID, string NegotiationDate,
            string NegotiationTime, string RememberDate, string RememberTime)
        {
            GeneralResponse response=new GeneralResponse();


            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Negotiation_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _negotiationService.NegotiationDelay(NegotiationID, GetEmployee().ID, NegotiationDate,
                NegotiationTime, RememberDate, RememberTime);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Update

        public JsonResult Negotiation_Edit(EditNegotiationRequest request)
        {
            GeneralResponse response=new GeneralResponse();

            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Negotiation_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _negotiationService.EditNegotiation(request, GetEmployee().ID);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete

        public JsonResult Negotiation_Delete(IEnumerable<DeleteRequest> requests)
        {
            GeneralResponse response=new GeneralResponse();


            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Negotiation_Delete");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _negotiationService.DeleteNegotiations(requests);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Chang Refered Employee

        public JsonResult Negotiation_ChangeReferedEmployee(IEnumerable<Guid> NegotiationIDs, Guid ReferedEmployeeID,string NegotiationDate,string NegotiationTime,string RememberDate,string RememberTime)
        {
            GeneralResponse response=new GeneralResponse();


            #region Access Check
            bool hasPermission = GetEmployee().IsGuaranteed("Negotiation_Update");
            if (!hasPermission)
            {
                response.ErrorMessages.Add("AccessDenied");
                return Json(response, JsonRequestBehavior.AllowGet);
            }
            #endregion

            response = _negotiationService.ChangeNegotiationReferedEmployee(NegotiationIDs, GetEmployee().ID,
                ReferedEmployeeID,NegotiationDate,NegotiationTime,RememberDate,RememberTime);

            return Json(response, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Get Current Negotiation

        public JsonResult Negotiation_GetCurrent()
        {
            
            IList<PopUpNegotiationView> popUpNegotiationViews=new List<PopUpNegotiationView>();
            var con = ConfigurationManager.ConnectionStrings["SamanCnn"].ToString();
            string query =
                "select t2.FirstName,t2.LastName,t2.ADSLPhone,t3.Title,t1.NegotiationDate,t1.negotiationtime from Lead.Negotiation t1 inner join cus.Customer t2 on t1.CustomerID=t2.CustomerID inner join lead.LeadTitleTemplate t3";
            query += " on t1.leadTitletemplateId=t3.LeadTitleTemplateID ";
            query += " where t2.EmployeeID='" + GetEmployee().ID + "' and t1.negotiationDate='" +
                     PersianDateTime.Now.Substring(0, 10) + "' and t1.negotiationTime='" +
                     PersianDateTime.Now.Substring(11, 5) + "'";
            using (SqlConnection myConnection = new SqlConnection(con))
            {
                SqlCommand oCmd = new SqlCommand(query, myConnection);
                myConnection.Open();
                using (SqlDataReader oReader = oCmd.ExecuteReader())
                {
                    while (oReader.Read())
                    {
                        PopUpNegotiationView popUpNegotiationView = new PopUpNegotiationView();
                        popUpNegotiationView.ADSLPhone = oReader["ADSLPhone"].ToString();
                        popUpNegotiationView.FirstName = oReader["FirstName"].ToString();
                        popUpNegotiationView.Lastname = oReader["LastName"].ToString();
                        popUpNegotiationView.LeadTitleTemplate = oReader["Title"].ToString();
                        popUpNegotiationView.NegotiationDate = oReader["NegotiationDate"].ToString();
                        popUpNegotiationView.NegotiationTime = oReader["NegotiationTime"].ToString();

                        popUpNegotiationViews.Add(popUpNegotiationView);
                    }
                    myConnection.Close();
                }
            }
            return Json(popUpNegotiationViews, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
