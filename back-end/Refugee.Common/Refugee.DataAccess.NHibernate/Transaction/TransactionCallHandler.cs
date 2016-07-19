using System.Data;
using EnsureThat;
using Microsoft.Practices.Unity.InterceptionExtension;
using NHibernate;

namespace Refugee.DataAccess.NHibernate.Transaction
{
    public class TransactionCallHandler : ICallHandler
    {
        #region Constructors

        public TransactionCallHandler(ISessionFactory sessionFactory)
        {
            Ensure.That(nameof(sessionFactory)).IsNotNull();

            _sessionFactory = sessionFactory;
        }

        #endregion

        #region Interface Implementation

        #region Public Methods

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            ISession session = _sessionFactory.GetCurrentSession();

            using (ITransaction transaction = session.BeginTransaction(IsolationLevel.ReadCommitted))
            {
                IMethodReturn result = getNext()(input, getNext);

                if (transaction.IsActive)
                {
                    if (result.Exception != null)
                    {
                        transaction.Rollback();
                    }
                    else
                    {
                        transaction.Commit();
                    }
                }

                return result;
            }
        }

        #endregion

        #region Public Properties

        public int Order { get; set; }

        #endregion

        #endregion

        #region Private Readonly Fields

        private readonly ISessionFactory _sessionFactory;

        #endregion
    }
}