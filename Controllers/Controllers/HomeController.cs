#region Usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Services.Interfaces;
using Controllers.ViewModels;

#endregion

namespace Controllers.Controllers
{
    //[Authorize]
    public class HomeController : Controller
    {
        #region Delcares

        private readonly IEmployeeService _employeeService;

        #endregion

        #region Ctor

        public HomeController(IEmployeeService employeeService)
            //: base(employeeService)
        {
            _employeeService = employeeService;
        }

        #endregion

        #region Base

        public ActionResult Base()
        {
            return Index();
        }

        #endregion

        #region Settings

        public ActionResult Settings()
        {
            return Index();
        }

        #endregion

        #region Index

        public ActionResult Index()
        {
            //HomePageView homePageView = new HomePageView();
            //homePageView.EmployeeView = GetEmployee();

            return View();
        }

        #endregion

        #region Store

        public ActionResult Store()
        {
            return Index();
        }

        #endregion

        #region Employee

        public ActionResult Employee()
        {
            return Index();
        }

        #endregion

        #region WorkFlow

        public ActionResult Workflow()
        {
            return Index();
        }

        #endregion

        #region MoneyAccount

        public ActionResult MoneyAccount()
        {
            return Index();
        }

        #endregion

        #region Query

        public ActionResult Query()
        {
            return Index();
        }

        #endregion

        #region Test

        public JsonResult Test()
        {
            string LOGIN_URL = "http://www.1544.ir/login.aspx";
            string SECRET_PAGE_URL = "http://www.1544.ir/Personal/Users/Default.aspx?menu=main";

            // have a cookie container ready to receive the forms auth cookie
            CookieContainer cookies = new CookieContainer();

            // first, request the login form to get the viewstate value
            HttpWebRequest webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
            webRequest.CookieContainer = cookies;
            StreamReader responseReader = new StreamReader(
                  webRequest.GetResponse().GetResponseStream()
               );
            string responseData = responseReader.ReadToEnd();
            responseReader.Close();

            string postData = "txtUsername=xxxxx&txtPassword=xxxxx";

            // now post to the login form
            webRequest = WebRequest.Create(LOGIN_URL) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.CookieContainer = cookies;

            // write the form values into the request message
            StreamWriter requestWriter = new StreamWriter(webRequest.GetRequestStream());
            requestWriter.Write(postData);
            requestWriter.Close();

            // we don't need the contents of the response, just the cookie it issues
            webRequest.GetResponse().Close();

            // now we can send out cookie along with a request for the protected page
            webRequest = WebRequest.Create(SECRET_PAGE_URL) as HttpWebRequest;
            webRequest.CookieContainer = cookies;
            responseReader = new StreamReader(webRequest.GetResponse().GetResponseStream());

            // and read the response
            responseData = responseReader.ReadToEnd();
            responseReader.Close();

            //Response.Write(responseData);

            return Json(responseData, JsonRequestBehavior.AllowGet);
        }

        #endregion

    }

}
