using FluentNHibernate.Conventions;
using FluentNHibernate.Conventions.Instances;
using Refugee.DataAccess.Generic.Naming;

namespace Refugee.DataAccess.NHibernate.Naming
{
    public class DatabaseConvention : IPropertyConvention,
                                      IIdConvention,
                                      IClassConvention
    {
        #region Private Constant Fields

        private const string Quote = @"""";

        #endregion

        #region Interfaces Implementation

        public void Apply(IPropertyInstance instance)
        {
            instance.Column($"{Quote}{instance.Property.Name}{Quote}");
        }

        public void Apply(IIdentityInstance instance)
        {
            instance.Column($"{Quote}Id{Quote}");
        }

        public void Apply(IClassInstance instance)
        {
            instance.Table($"{Quote}{Pluralizer.ToPlural(instance.EntityType.Name)}{Quote}");
        }

        #endregion
    }
}