using EnsureThat;
using Refugee.DataAccess.Graph.Models.Nodes;
using RefugeeModel = Refugee.DataAccess.Graph.Models.Nodes.Refugee;

namespace Refugee.DataAccess.Graph.Models.Helpers
{
    public class RefugeeWithHotSpot
    {
        #region Constructors

        public RefugeeWithHotSpot(RefugeeModel refugee, HotSpot hotSpot)
        {
            Ensure.That(nameof(refugee)).IsNotNull();

            Ensure.That(nameof(hotSpot)).IsNotNull();

            Refugee = refugee;

            HotSpot = hotSpot;
        }

        #endregion

        #region Public Properties

        public RefugeeModel Refugee { get; protected set; }

        public HotSpot HotSpot { get; protected set; }

        #endregion
    }
}