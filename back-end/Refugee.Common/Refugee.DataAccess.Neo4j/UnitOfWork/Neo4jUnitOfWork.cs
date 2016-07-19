using System;
using EnsureThat;
using Neo4jClient;
using Refugee.DataAccess.Generic.UnitOfWork;

namespace Refugee.DataAccess.Neo4j.UnitOfWork
{
    public class Neo4jUnitOfWork : IGenericUnitOfWork
    {
        #region Constructors

        protected Neo4jUnitOfWork(GraphClient graphClient)
        {
            Ensure.That(nameof(graphClient)).IsNotNull();

            _graphClient = graphClient;
        }

        #endregion

        #region Interface Implementation

        public void Dispose()
        {
            if (_graphClient.InTransaction)
            {
                _graphClient.Transaction.Dispose();
            }

            _graphClient.Dispose();
        }

        public void BeginTransaction()
        {
            if (!_graphClient.IsConnected)
            {
                throw new ApplicationException("Not connected!");
            }

            if (_graphClient.InTransaction)
            {
                throw new ApplicationException("Already running transaction!");
            }

            _graphClient.BeginTransaction();
        }

        public void CommitTransaction()
        {
            if (!_graphClient.IsConnected)
            {
                throw new ApplicationException("Not connected!");
            }

            if (!_graphClient.InTransaction)
            {
                throw new ApplicationException("No transaction exists!");
            }

            if (!_graphClient.Transaction.IsOpen)
            {
                throw new ApplicationException("Transaction is not open!");
            }

            _graphClient.Transaction.Commit();
        }

        public void RollbackTransaction()
        {
            if (!_graphClient.IsConnected)
            {
                throw new ApplicationException("Not connected!");
            }

            if (!_graphClient.InTransaction)
            {
                throw new ApplicationException("No transaction exists!");
            }

            if (!_graphClient.Transaction.IsOpen)
            {
                throw new ApplicationException("Transaction is not open!");
            }

            _graphClient.Transaction.Rollback();
        }

        #endregion

        #region Private Readonly Fields

        private readonly GraphClient _graphClient;

        #endregion
    }
}