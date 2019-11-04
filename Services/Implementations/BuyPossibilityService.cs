#region Usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Services.Interfaces;
using Model.Customers.Interfaces;
using Infrastructure.UnitOfWork;
using Services.Messaging;
using Services.Messaging.CustomerCatalogService;
using Model.Customers;
using Services.ViewModels.Customers;
using Services.Mapping;
using Infrastructure.Persian;
using Model.Base;
using Model.Employees.Interfaces;

#endregion

namespace Services.Implementations
{
    public class BuyPossibilityService : IBuyPossibilityService
    {
        #region Declares

        private readonly IBuyPossibilityRepository _buyPossibilityRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region Ctor

        public BuyPossibilityService(IBuyPossibilityRepository buyPossibilityRepository, IUnitOfWork uow
             , IEmployeeRepository employeeRepository)
        {
            _buyPossibilityRepository = buyPossibilityRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }

        #endregion

        public GetGeneralResponse<IEnumerable<BuyPossibilityView>> GetBuyPossibilities()
        {
            GetGeneralResponse<IEnumerable<BuyPossibilityView>> fresponse = new GetGeneralResponse<IEnumerable<BuyPossibilityView>>();
            Infrastructure.Domain.Response<BuyPossibility> response = _buyPossibilityRepository.FindAll(-1, -1);
            //Edited By Zafari
            //برای مرتب سازی
            fresponse.data = _buyPossibilityRepository.FindAll().ConvertToBuyPossibilityViews().OrderBy(x=>x.RowVersion);
            fresponse.totalCount = response.totalCount;

            return fresponse;
        }

    }
}
