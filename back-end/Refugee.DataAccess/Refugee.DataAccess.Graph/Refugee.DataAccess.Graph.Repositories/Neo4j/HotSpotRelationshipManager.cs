using EnsureThat;
using Refugee.DataAccess.Graph.Models.Nodes;
using Refugee.DataAccess.Graph.Models.Relationships;
using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.DataAccess.Graph.Repositories.Neo4j
{
    public class HotSpotRelationshipManager : RelationshipManager, IHotSpotRelationshipManager
    {
        #region Interface Implementation

        public void Relate(RefugeeModel refugee, HotSpot hotSpot)
        {
            Ensure.That(nameof(refugee)).IsNotNull();

            Ensure.That(nameof(hotSpot)).IsNotNull();

            string refugeeLabel = typeof(RefugeeModel).Name;

            string hotSpotLabel = typeof(HotSpot).Name;

            GraphClient.Cypher.Match($"(r:{refugeeLabel})", $"(h:{hotSpotLabel})")
                              .Where((RefugeeModel r) => r.Id == refugee.Id)
                              .AndWhere((HotSpot h) => h.Id == hotSpot.Id)
                              .CreateUnique($"(r)-[:{LivesInRelationship.TypeKey}]->(h)")
                              .ExecuteWithoutResults();
        }

        public void UnRelate(RefugeeModel refugee)
        {
            Ensure.That(nameof(refugee)).IsNotNull();

            string refugeeLabel = typeof(RefugeeModel).Name;

            string hotSpotLabel = typeof(HotSpot).Name;

            GraphClient.Cypher.Match($"(r:{refugeeLabel})-[o:{LivesInRelationship.TypeKey}]->(h:{hotSpotLabel})")
                              .Where((RefugeeModel r) => r.Id == refugee.Id)
                              .Delete("o")
                              .ExecuteWithoutResults();
        }

        #endregion
    }
}