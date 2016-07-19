using Neo4jClient;
using Refugee.DataAccess.Generic.Models;
using Refugee.DataAccess.Generic.Repository;

namespace Refugee.DataAccess.Neo4j.Repository
{
    public interface INeo4jRepository<TModel> : IGenericRepository<TModel> where TModel : IEntity
    {
        void SetGraphClient(IGraphClient graphClient);
    }
}