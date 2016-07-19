namespace Refugee.DataAccess.Relational.Models
{
    public class User
    {
        #region Public Constant Fields

        public const int USER_NAME_LENGTH = 50;

        public const int REAL_NAME_LENGTH = 100;

        public const int PASSWORD_LENGTH = 50;

        #endregion

        #region Public Properties

        public virtual int Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string RealName { get; set; }

        public virtual string Password { get; set; }

        #endregion
    }
}