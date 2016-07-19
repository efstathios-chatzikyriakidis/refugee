using EnsureThat;
using Refugee.DataAccess.Graph.Models.Nodes;
using Refugee.DataAccess.Graph.Models.Relationships;

using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.DataAccess.Graph.Models.Helpers
{
    public class FamilyRelationshipsWithHotSpots
    {
        #region Constructors

        public FamilyRelationshipsWithHotSpots(HotSpot hotSpot1,
                                               RefugeeModel refugee1,
                                               IsFamilyRelationshipData isFamilyRelationshipData1,
                                               RefugeeModel refugee2,
                                               HotSpot hotSpot2,
                                               IsFamilyRelationshipData isFamilyRelationshipData2 = null,
                                               RefugeeModel refugee3 = null,
                                               HotSpot hotSpot3 = null)
        {
            Ensure.That(nameof(hotSpot1)).IsNotNull();

            Ensure.That(nameof(refugee1)).IsNotNull();

            Ensure.That(nameof(isFamilyRelationshipData1)).IsNotNull();

            Ensure.That(nameof(refugee2)).IsNotNull();

            Ensure.That(nameof(hotSpot2)).IsNotNull();

            HotSpot1 = hotSpot1;

            Refugee1 = refugee1;

            IsFamilyRelationshipData1 = isFamilyRelationshipData1;

            Refugee2 = refugee2;

            HotSpot2 = hotSpot2;

            IsFamilyRelationshipData2 = isFamilyRelationshipData2;

            Refugee3 = refugee3;

            HotSpot3 = hotSpot3;
        }

        #endregion

        #region Public Properties

        public HotSpot HotSpot1 { get; protected set; }

        public RefugeeModel Refugee1 { get; protected set; }

        public IsFamilyRelationshipData IsFamilyRelationshipData1 { get; protected set; }

        public RefugeeModel Refugee2 { get; protected set; }

        public HotSpot HotSpot2 { get; protected set; }

        public IsFamilyRelationshipData IsFamilyRelationshipData2 { get; protected set; }

        public RefugeeModel Refugee3 { get; protected set; }

        public HotSpot HotSpot3 { get; protected set; }

        #endregion
    }
}