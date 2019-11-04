using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        void RegisterAmened(IAggregateRoot entity, IUnitOfWorkRepository unitOfWorkRepository);
        void RegisterNew(IAggregateRoot entity, IUnitOfWorkRepository unitOfWorkRepository);
        void RegisterRemoved(IAggregateRoot entity, IUnitOfWorkRepository unitOfWorkRepository);

        void Commit();
    }
}
