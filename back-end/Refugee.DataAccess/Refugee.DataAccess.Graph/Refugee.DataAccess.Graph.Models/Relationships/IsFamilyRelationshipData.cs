using Refugee.DataAccess.Graph.Models.Helpers;

namespace Refugee.DataAccess.Graph.Models.Relationships
{
    public class IsFamilyRelationshipData
    {
        #region Constructors

        public IsFamilyRelationshipData()
        {
        }

        public IsFamilyRelationshipData(FamilyRelationshipDegree degree)
        {
            Degree = degree;
        }

        #endregion

        #region Public Properties

        public FamilyRelationshipDegree Degree { get; protected set; }

        #endregion
    }
}