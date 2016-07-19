using FluentNHibernate.Mapping;
using Refugee.DataAccess.Relational.Models;

namespace Refugee.DataAccess.Relational.Mapping
{
    public sealed class UserMap : ClassMap<User>
    {
        public UserMap()
        {
            Id(o => o.Id);

            Map(o => o.RealName)
                .Length(User.REAL_NAME_LENGTH)
                .Not.Nullable();

            Map(o => o.UserName)
                .Length(User.USER_NAME_LENGTH)
                .Unique()
                .Not.Nullable();

            Map(o => o.Password)
                .Length(User.PASSWORD_LENGTH)
                .Not.Nullable();
        }
    }
}