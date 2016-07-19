using NHibernate;
using Refugee.DataAccess.Generic.Dao;

namespace Refugee.DataAccess.NHibernate.Dao
{
    public interface INHibernateDao<TModel, TId> : IGenericDao<TModel, TId>
    {
        void SetSessionFactory(ISessionFactory sessionFactory);
    }
}