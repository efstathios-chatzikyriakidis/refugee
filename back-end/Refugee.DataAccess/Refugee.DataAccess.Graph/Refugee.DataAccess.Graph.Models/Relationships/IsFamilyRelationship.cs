using Neo4jClient;
using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.DataAccess.Graph.Models.Relationships
{
    public class IsFamilyRelationship : Relationship<IsFamilyRelationshipData>, IRelationshipAllowingSourceNode<RefugeeModel>, IRelationshipAllowingTargetNode<RefugeeModel>
    {
        #region Public Static Readonly Fields

        public static readonly string TypeKey = "IS_FAMILY";

        #endregion

        #region Constructors

        public IsFamilyRelationship(NodeReference targetNode, IsFamilyRelationshipData isFamilyRelationshipData) : base(targetNode, isFamilyRelationshipData)
        {
        }

        #endregion

        #region Public Overriden Properties

        public override string RelationshipTypeKey => TypeKey;

        #endregion
    }
}