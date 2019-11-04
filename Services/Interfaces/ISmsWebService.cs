using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Web.Services.Protocols;
using System.Web.Services;
using System.Diagnostics;
using System.Xml.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Net;

namespace Services.Interfaces
{
    [System.Web.Services.WebServiceBindingAttribute(Name = "Mehr", Namespace = "http://tempuri.org/")]
    public partial class ISmsWebService : SoapHttpClientProtocol
    {

        public ISmsWebService()
        {

            WebProxy myProxy = new WebProxy();

            string cAddressProxy = "";
            string cUsernameProxy = "";
            string cPasswordProxy = "";

            if (!string.IsNullOrEmpty(cAddressProxy))
            {
                myProxy.Address = new Uri(cAddressProxy);
                myProxy.Credentials = new NetworkCredential(cUsernameProxy, cPasswordProxy);
                this.Proxy = myProxy;
            }


            this.Url = "http://mehrafraz.com/WebService/Service.asmx";
            //this.Url = "http://localhost:49200/WebService/Service.asmx";

        }

        //
        //
        // By Hojjat
        public bool SendSms(string body, string phoneNumber)
        {
            string returnCode = SendSms("edm", "87277", body, phoneNumber, "0", 1, 2, "", "edm", "");

            // Detect Error:
            long err;
            long.TryParse(returnCode, out err);

            if (err >= 0)
                return true;
            else
                return false;
        }


        [SoapDocumentMethodAttribute("http://tempuri.org/SendSms1To1",
            RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public Int64[] SendSms1To1(string cDomainname, string cUserName, string cPassword, string[] aBody, string[] aMobileno, Int64[] aGetid, int[] aCMessage, string[] aFromNumber, int[] atypeUnicodemessage, string[] am_SchedulDate, int nTypeSent, int nSpeedsms, int nPeriodmin)
        {
            object[] results = this.Invoke("SendSms1To1", new object[] { cDomainname, cUserName, cPassword, aBody, aMobileno, aGetid, aCMessage, aFromNumber, atypeUnicodemessage, am_SchedulDate, nTypeSent, nSpeedsms, nPeriodmin });
            return ((Int64[])(results[0]));
        }

        [SoapDocumentMethodAttribute("http://tempuri.org/getMessageIds1to1",
            RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public Int64[] getMessageIds1to1(string cUserName, string cPassword, string cDomainname, Int64[] nCustomerid)
        {
            object[] results = this.Invoke("getMessageIds1to1", new object[] { cUserName, cPassword, cDomainname, nCustomerid });
            return ((Int64[])(results[0]));
        }

        [SoapDocumentMethodAttribute("http://tempuri.org/GetMessageDelivery1to1",
            RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public int[] GetMessageDelivery1to1(string cUserName, string cPassword, string cDomainname, Int64[] longid)
        {
            object[] results = this.Invoke("GetMessageDelivery1to1", new object[] { cUserName, cPassword, cDomainname, longid });
            return ((int[])(results[0]));
        }

        [SoapDocumentMethodAttribute("http://tempuri.org/SendSms",
            RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public string SendSms(string cUserName, string cPassword, string cBody, string cSmsnumber, string cGetid, int nCMessage, int nTypeSent, string m_SchedulDate, string cDomainname, string cFromNumber)
        {
            object[] results = this.Invoke("SendSms", new object[] { cUserName, cPassword, cBody, cSmsnumber, cGetid, nCMessage, nTypeSent, m_SchedulDate, cDomainname, cFromNumber });
            return ((string)(results[0]));
        }

        [SoapDocumentMethodAttribute("http://tempuri.org/ShowError",
          RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public string ShowError(string cErrorCode, string cLanShow)
        {
            object[] results = this.Invoke("ShowError", new object[] { cErrorCode, cLanShow });
            return ((string)(results[0]));

        }

        [SoapDocumentMethodAttribute("http://tempuri.org/ReceiveSms",
            RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public DataTable ReceiveSms(string cUserName, string cPassword, bool lReceiveAllmsg, string cDomainName, string cFromnumber, string cFromDate, string cToDate)
        {
            object[] results = this.Invoke("ReceiveSms", new object[] { cUserName, cPassword, lReceiveAllmsg, cDomainName, cFromnumber, cFromDate, cToDate });// cFromDate = "1391/01/01"  cToDate = "1391/01/20"
            return ((DataTable)(results[0]));
        }


        [SoapDocumentMethodAttribute("http://tempuri.org/ChangePassword",
                RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/",
                Use = System.Web.Services.Description.SoapBindingUse.Literal,
                ParameterStyle = SoapParameterStyle.Wrapped)]
        public string ChangePassword(string cUserName, string cPassword, string cNewPassword)
        {
            object[] results = this.Invoke("ChangePassword", new object[] { cUserName, cPassword, cNewPassword });
            return ((string)(results[0]));
        }

        [SoapDocumentMethodAttribute("http://tempuri.org/GetMessageLength",
                RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/",
                Use = System.Web.Services.Description.SoapBindingUse.Literal,
                ParameterStyle = SoapParameterStyle.Wrapped)]
        public int GetMessageLength(string cBody)
        {
            object[] results = this.Invoke("GetMessageLength", new object[] { cBody });
            return ((int)(results[0]));
        }

        [SoapDocumentMethodAttribute("http://tempuri.org/GetUnicodeMessage",
            RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public int GetUnicodeMessage(string cBody)
        {
            object[] results = this.Invoke("GetUnicodeMessage", new object[] { cBody });
            return ((int)(results[0]));
        }

        [SoapDocumentMethodAttribute("http://tempuri.org/GetuserInfo",
                RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/",
                Use = System.Web.Services.Description.SoapBindingUse.Literal,
                ParameterStyle = SoapParameterStyle.Wrapped)]
        public string[] GetuserInfo(string cUserName, string cPassword, string cDomainName)
        {
            object[] results = this.Invoke("GetuserInfo", new object[] { cUserName, cPassword, cDomainName });
            return ((string[])(results[0]));
        }

        [SoapDocumentMethodAttribute("http://tempuri.org/GetDeliveryWithGetid",
          RequestNamespace = "http://tempuri.org/", ResponseNamespace = "http://tempuri.org/",
            Use = System.Web.Services.Description.SoapBindingUse.Literal,
            ParameterStyle = SoapParameterStyle.Wrapped)]
        public string[] GetDeliveryWithGetid(string cUserName, string cPassword, string cGetid, bool lReturnSid)
        {
            object[] results = this.Invoke("GetDeliveryWithGetid", new object[] { cUserName, cPassword, cGetid, lReturnSid });
            return ((string[])(results[0]));
        }

    }
}