using Refugee.DataAccess.Generic.Dao;
using Refugee.DataAccess.Relational.Models;

namespace Refugee.DataAccess.Relational.Dao
{
    public interface IUserDao : IGenericDao<User, int>
    {
        User GetByUserNameAndPassword(string userName, string password);

        bool ExistsByUserNameAndPassword(string userName, string password);
    }
}