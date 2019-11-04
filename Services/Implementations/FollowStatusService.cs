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
    public class FollowStatusService : IFollowStatusService
    {
        #region Declares

        private readonly IFollowStatusRepository _followStatusRepository;
        private readonly IUnitOfWork _uow;
        private readonly IEmployeeRepository _employeeRepository;

        #endregion

        #region Ctor

        public FollowStatusService(IFollowStatusRepository followStatusRepository, IUnitOfWork uow
             , IEmployeeRepository employeeRepository)
        {
            _followStatusRepository = followStatusRepository;
            _uow = uow;
            _employeeRepository = employeeRepository;
        }

        #endregion

        public GetGeneralResponse<IEnumerable<FollowStatusView>> GetFollowStatuss()
        {
            GetGeneralResponse<IEnumerable<FollowStatusView>> fresponse = new GetGeneralResponse<IEnumerable<FollowStatusView>>();
            Infrastructure.Domain.Response<FollowStatus> response = _followStatusRepository.FindAll(-1, -1);
            //Edited By Zafari
            //برای مرتب سازی
            fresponse.data = _followStatusRepository.FindAll().ConvertToFollowStatusViews().OrderBy(x=>x.RowVersion);
            fresponse.totalCount = response.totalCount;

            return fresponse;
        }

    }
}
