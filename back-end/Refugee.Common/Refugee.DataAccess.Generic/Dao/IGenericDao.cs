using System.Collections.Generic;

namespace Refugee.DataAccess.Generic.Dao
{
    public interface IGenericDao<TModel, TId>
    {
        IList<TModel> GetAll();

        TModel GetById(TId id);
    }
}