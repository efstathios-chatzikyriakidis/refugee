using System;
using System.Collections.Generic;
using Refugee.DataAccess.Generic.Models;

namespace Refugee.DataAccess.Generic.Repository
{
    public interface IGenericRepository<TModel> where TModel : IEntity
    {
        IList<TModel> GetAll();

        TModel GetById(Guid id);

        TModel Save(TModel model);

        TModel Update(TModel model);

        void Delete(TModel model);
    }
}