#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using Model.Customers;
using Infrastructure.UnitOfWork;
using Model;
using Model.Interfaces;

#endregion

namespace Repository.Repositories
{
    public class MainMenuRepository : Repository<MainMenu>, IMainMenuRepository
    {
        public MainMenuRepository(IUnitOfWork uow)
            : base(uow)
        {
        }
    }
}
