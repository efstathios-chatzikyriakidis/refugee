using System;

namespace Refugee.DataAccess.Generic.UnitOfWork
{
    public interface IGenericUnitOfWork : IDisposable
    {
        void BeginTransaction();

        void CommitTransaction();

        void RollbackTransaction();
    }
}