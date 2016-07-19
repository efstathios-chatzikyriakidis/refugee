using Neo4jClient;

namespace Refugee.DataAccess.Graph.Repositories
{
    public interface IRelationshipManager
    {
        void SetGraphClient(IGraphClient graphClient);
    }
}