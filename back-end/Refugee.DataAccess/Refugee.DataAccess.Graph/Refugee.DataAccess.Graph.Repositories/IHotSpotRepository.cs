using Refugee.DataAccess.Generic.Repository;
using Refugee.DataAccess.Graph.Models.Nodes;
using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.DataAccess.Graph.Repositories
{
    public interface IHotSpotRepository : IGenericRepository<HotSpot>
    {
        HotSpot GetByRefugee(RefugeeModel refugee);
    }
}