using Neo4jClient;
using Refugee.DataAccess.Graph.Models.Nodes;
using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.DataAccess.Graph.Models.Relationships
{
    public class LivesInRelationship : Relationship, IRelationshipAllowingSourceNode<RefugeeModel>, IRelationshipAllowingTargetNode<HotSpot>
    {
        #region Public Static Readonly Fields

        public static readonly string TypeKey = "LIVES_IN";

        #endregion

        #region Constructors

        public LivesInRelationship(NodeReference targetNode) : base(targetNode)
        {
        }

        #endregion

        #region Public Overriden Properties

        public override string RelationshipTypeKey => TypeKey;

        #endregion
    }
}