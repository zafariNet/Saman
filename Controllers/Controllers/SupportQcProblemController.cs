using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;

namespace Controllers.Controllers
{
    public class SupportQcProblemController : BaseController
    {
        #region Declare

        private readonly ISupportService _supportService;

        private readonly ICustomerService _customerService;

        private readonly IEmployeeService _employeeService;

        private readonly ISupportQcProblemService _supportQcProblemService;

        #endregion

        #region ctor

        public SupportQcProblemController(ISupportQcProblemService supportQcProblemService,
            ISupportService supportService, ICustomerService customerService, IEmployeeService employeeService)
            : base(employeeService)
        {
            _supportService = supportService;
            _customerService = customerService;
            _employeeService = employeeService;
            _supportQcProblemService = supportQcProblemService;
        }

        #endregion
    }
}
