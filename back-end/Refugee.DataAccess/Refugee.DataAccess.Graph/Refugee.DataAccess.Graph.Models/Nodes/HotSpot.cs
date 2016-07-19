using Refugee.DataAccess.Generic.Models;

namespace Refugee.DataAccess.Graph.Models.Nodes
{
    public class HotSpot : BaseEntity
    {
        #region Public Properties

        public string Name { get; set; }

        public double Longitude { get; set; }

        public double Latitude { get; set; }

        #endregion
    }
}