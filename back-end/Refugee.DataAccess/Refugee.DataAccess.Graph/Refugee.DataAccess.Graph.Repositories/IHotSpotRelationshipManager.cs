using Refugee.DataAccess.Graph.Models.Nodes;
using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.DataAccess.Graph.Repositories
{
    public interface IHotSpotRelationshipManager
    {
        void Relate(RefugeeModel refugee, HotSpot hotSpot);

        void UnRelate(RefugeeModel refugee);
    }
}