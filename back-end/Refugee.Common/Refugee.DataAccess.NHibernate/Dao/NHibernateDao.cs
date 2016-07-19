using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using NHibernate;
using NHibernate.Linq;

namespace Refugee.DataAccess.NHibernate.Dao
{
    public class NHibernateDao<TModel, TId> : INHibernateDao<TModel, TId>
    {
        #region IGenericDao<TModel, TId> Implementation

        public IList<TModel> GetAll()
        {
            return CurrentSession.Query<TModel>().ToList();
        }

        public TModel GetById(TId id)
        {
            return CurrentSession.Get<TModel>(id);
        }

        #endregion

        #region INHibernateDao<TModel, TId> Implementation

        public void SetSessionFactory(ISessionFactory sessionFactory)
        {
            Ensure.That(nameof(sessionFactory)).IsNotNull();

            _sessionFactory = sessionFactory;
        }

        #endregion

        #region Protected Properties

        protected ISession CurrentSession
        {
            get
            {
                if (_sessionFactory == null)
                {
                    throw new ApplicationException("You should first initialize the session factory.");
                }

                return _sessionFactory.GetCurrentSession();
            }
        }

        #endregion

        #region Private Fields

        private ISessionFactory _sessionFactory;

        #endregion
    }
}