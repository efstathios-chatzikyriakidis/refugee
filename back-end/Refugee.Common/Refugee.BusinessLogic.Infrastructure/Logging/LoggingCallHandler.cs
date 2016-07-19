using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using Refugee.BusinessLogic.Infrastructure.Exceptions;
using Serilog;

namespace Refugee.BusinessLogic.Infrastructure.Logging
{
    public class LoggingCallHandler : ICallHandler
    {
        #region Interface Implementation

        #region Public Methods

        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            string standardMessageFormat = $"The [{{ControllerName}}.{{ActionName}}]";

            IList<object> standardArguments = new List<object> { input.MethodBase.DeclaringType?.Name, input.MethodBase.Name };

            StringBuilder messageFormat = new StringBuilder(standardMessageFormat);

            IList<object> firstArguments = new List<object>(standardArguments);

            if (input.Inputs.Count > 0)
            {
                messageFormat.Append(" with [{@ActionInputs}] inputs");

                firstArguments.Add(input.Inputs.OfType<object>());
            }

            Log.Information($"{messageFormat} is executing.", firstArguments.ToArray());

            IMethodReturn result = getNext()(input, getNext);

            if (result.Exception is RestException)
            {
                IList<object> newArguments = new List<object>(standardArguments);

                newArguments.Add(result.Exception.InnerException);

                Log.Error($"{standardMessageFormat} is failed with [{{@ActionException}}] exception.", newArguments.ToArray());
            }
            else if (result.Exception == null)
            {
                messageFormat = new StringBuilder(standardMessageFormat);

                messageFormat.Append(" is succeeded");

                IList<object> newArguments = new List<object>(standardArguments);

                MethodInfo methodInfo = input.MethodBase as MethodInfo;

                if (methodInfo != null && methodInfo.ReturnType != typeof(void))
                {
                    messageFormat.Append(" with [{@ActionOutput}] output");

                    newArguments.Add(result.ReturnValue);
                }

                Log.Information($"{messageFormat}.", newArguments.ToArray());
            }

            return result;
        }

        #endregion

        #region Public Properties

        public int Order { get; set; }

        #endregion

        #endregion
    }
}