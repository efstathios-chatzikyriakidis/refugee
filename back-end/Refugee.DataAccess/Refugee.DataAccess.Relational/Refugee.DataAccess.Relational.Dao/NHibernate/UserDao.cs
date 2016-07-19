using System.Linq;
using EnsureThat;
using NHibernate.Linq;
using Refugee.DataAccess.NHibernate.Dao;
using Refugee.DataAccess.Relational.Models;

namespace Refugee.DataAccess.Relational.Dao.NHibernate
{
    public class UserDao : NHibernateDao<User, int>, IUserDao
    {
        #region Interface Implementation

        public User GetByUserNameAndPassword(string userName, string password)
        {
            Ensure.That(nameof(userName)).IsNotNullOrWhiteSpace();

            Ensure.That(nameof(password)).IsNotNullOrWhiteSpace();

            return (from o in CurrentSession.Query<User>()
                    where o.UserName == userName
                       && o.Password == password
                    select o).SingleOrDefault();
        }

        public bool ExistsByUserNameAndPassword(string userName, string password)
        {
            Ensure.That(nameof(userName)).IsNotNullOrWhiteSpace();

            Ensure.That(nameof(password)).IsNotNullOrWhiteSpace();

            return (from o in CurrentSession.Query<User>()
                    where o.UserName == userName
                       && o.Password == password
                    select o.Id).Any();
        }

        #endregion
    }
}