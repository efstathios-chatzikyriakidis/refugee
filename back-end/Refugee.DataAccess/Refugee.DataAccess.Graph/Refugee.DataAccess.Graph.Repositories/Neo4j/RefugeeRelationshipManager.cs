using EnsureThat;
using Refugee.DataAccess.Graph.Models.Relationships;
using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.DataAccess.Graph.Repositories.Neo4j
{
    public class RefugeeRelationshipManager : RelationshipManager, IRefugeeRelationshipManager
    {
        #region Interface Implementation

        public void Relate(RefugeeModel source, RefugeeModel target, IsFamilyRelationshipData isFamilyRelationshipData)
        {
            Ensure.That(nameof(source)).IsNotNull();

            Ensure.That(nameof(target)).IsNotNull();

            string refugeeLabel = typeof(RefugeeModel).Name;

            GraphClient.Cypher.Match($"(s:{refugeeLabel})", $"(t:{refugeeLabel})")
                              .Where((RefugeeModel s) => s.Id == source.Id)
                              .AndWhere((RefugeeModel t) => t.Id == target.Id)
                              .CreateUnique($"(s)-[:{IsFamilyRelationship.TypeKey} {{relationshipData}}]->(t)")
                              .WithParam("relationshipData", isFamilyRelationshipData)
                              .ExecuteWithoutResults();
        }

        public void UnRelate(RefugeeModel refugeeSource, RefugeeModel refugeeTarget)
        {
            Ensure.That(nameof(refugeeSource)).IsNotNull();

            Ensure.That(nameof(refugeeTarget)).IsNotNull();

            string refugeeLabel = typeof(RefugeeModel).Name;

            GraphClient.Cypher.Match($"(source:{refugeeLabel})-[o:{IsFamilyRelationship.TypeKey}]-(target:{refugeeLabel})")
                              .Where((RefugeeModel source) => source.Id == refugeeSource.Id)
                              .AndWhere((RefugeeModel target) => target.Id == refugeeTarget.Id)
                              .Delete("o")
                              .ExecuteWithoutResults();
        }

        #endregion
    }
}