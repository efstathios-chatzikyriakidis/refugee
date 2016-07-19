using System.Linq;
using EnsureThat;
using Refugee.DataAccess.Graph.Models.Nodes;
using Refugee.DataAccess.Graph.Models.Relationships;
using Refugee.DataAccess.Neo4j.Repository;
using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.DataAccess.Graph.Repositories.Neo4j
{
    public class HotSpotRepository : Neo4jRepository<HotSpot>, IHotSpotRepository
    {
        #region Interface Implementation

        public HotSpot GetByRefugee(RefugeeModel refugee)
        {
            Ensure.That(nameof(refugee)).IsNotNull();

            string refugeeLabel = typeof(RefugeeModel).Name;

            string hotSpotLabel = typeof(HotSpot).Name;

            return GraphClient.Cypher.Match($"(r:{refugeeLabel})-[:{LivesInRelationship.TypeKey}]->(h:{hotSpotLabel})")
                                     .Where((RefugeeModel r) => r.Id == refugee.Id)
                                     .Return(h => h.As<HotSpot>())
                                     .Results
                                     .Single();
        }

        #endregion
    }
}