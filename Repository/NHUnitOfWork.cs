using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.UnitOfWork;
using Infrastructure.Domain;
using NHibernate;

namespace Repository
{
    public class NHUnitOfWork : IUnitOfWork
    {
        public void RegisterAmened(IAggregateRoot entity, IUnitOfWorkRepository unitOfWorkRepository)
        {
            SessionFactory.GetCurrentSession().SaveOrUpdate(entity);
        }

        public void RegisterNew(IAggregateRoot entity, IUnitOfWorkRepository unitOfWorkRepository)
        {
            SessionFactory.GetCurrentSession().Save(entity);
        }

        public void RegisterRemoved(IAggregateRoot entity, IUnitOfWorkRepository unitOfWorkRepository)
        {
            SessionFactory.GetCurrentSession().Delete(entity);
        }

        public void Commit()
        {
            using (ITransaction transaction = SessionFactory.GetCurrentSession().BeginTransaction())
            {
                try
                {
                    transaction.Commit();
                }
                catch(Exception ex)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }
    }
}
