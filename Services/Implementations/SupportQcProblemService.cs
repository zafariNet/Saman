using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.UnitOfWork;
using Model.Customers.Interfaces;
using Model.Support;
using Model.Support.Interfaces;
using Services.Interfaces;
using Services.Mapping;
using Services.Messaging;
using Services.ViewModels.Support;

namespace Services.Implementations
{
    public class SupportQcProblemService : ISupportQcProblemService
    {
        #region Declares

        private readonly ISupportQcProblemRepository _supportQcProblemRepository;

        private readonly ICustomerRepository _customerRepository;

        private readonly IEmployeeService _employeeService;

        private readonly IUnitOfWork _uow;

        private readonly ISupportRepository _supportRepository;

        #endregion

        #region Ctor

        public SupportQcProblemService(ISupportQcProblemRepository supportQcProblemRepository,
            ICustomerRepository customerRepository, IEmployeeService employeeService,
            IUnitOfWork uow, ISupportRepository supportRepository
            )
        {
            _supportQcProblemRepository = supportQcProblemRepository;
            _customerRepository = customerRepository;
            _employeeService = employeeService;
            _uow = uow;
            _supportRepository = supportRepository;
        }

        #endregion

        #region Read

        #region All

        public GetGeneralResponse<IEnumerable<SupportQcProblemView>> GetSupportQcproblems()
        {
            GetGeneralResponse<IEnumerable<SupportQcProblemView>> response=new GetGeneralResponse<IEnumerable<SupportQcProblemView>>();

            try
            {
                IEnumerable<SupportQcProblem> supportQcProblems = _supportQcProblemRepository.FindAll();

                response.data = supportQcProblems.ConvertToSupportQcProblemViews();
                response.totalCount = supportQcProblems.Count();
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                if (ex.InnerException != null)
                    response.ErrorMessages.Add(ex.InnerException.Message);
            }

            return response;
        }

        #endregion

        #endregion

        #region Send SMS

        public void SendSmsVoid(object data)
        {
            ISmsWebService smsWebService = new ISmsWebService();
            SmsData smsData = (SmsData)data;
            smsWebService.SendSms(smsData.body, smsData.phoneNumber);
        }




        #endregion
    }
}
