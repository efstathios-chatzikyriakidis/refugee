using System;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Refugee.DataAccess.NHibernate.Transaction
{
    [AttributeUsage(AttributeTargets.Method, Inherited = false)]
    public class TransactionAttribute : HandlerAttribute
    {
        public override ICallHandler CreateHandler(IUnityContainer container)
        {
            return container.Resolve<ICallHandler>(typeof(TransactionCallHandler).Name);
        }
    }
}