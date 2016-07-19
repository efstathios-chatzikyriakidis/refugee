using Refugee.DataAccess.Generic.Models;
using Refugee.DataAccess.Graph.Models.Helpers;

namespace Refugee.DataAccess.Graph.Models.Nodes
{
    public class Refugee : BaseEntity
    {
        #region Public Properties

        public string Name { get; set; }

        public string Nationality { get; set; }

        public GenderType GenderType { get; set; }

        public string Passport { get; set; }

        public int BirthYear { get; set; }

        #endregion
    }
}