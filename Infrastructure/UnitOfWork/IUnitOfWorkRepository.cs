using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Infrastructure.Domain;

namespace Infrastructure.UnitOfWork
{
    public interface IUnitOfWorkRepository
    {
        void PresistCreationOf(IAggregateRoot entity);
        void PresistUpdateOf(IAggregateRoot entity);
        void PresistDeletionOf(IAggregateRoot entity);
    }
}
