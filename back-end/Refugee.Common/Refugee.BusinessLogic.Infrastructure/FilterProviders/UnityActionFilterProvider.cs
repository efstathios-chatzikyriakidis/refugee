using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using EnsureThat;
using Microsoft.Practices.Unity;

namespace Refugee.BusinessLogic.Infrastructure.FilterProviders
{
    public class UnityActionFilterProvider : ActionDescriptorFilterProvider, IFilterProvider
    {
        #region Private Readonly Fields

        private readonly IUnityContainer _container;

        #endregion

        #region Constructors

        public UnityActionFilterProvider(IUnityContainer container)
        {
            Ensure.That(nameof(container)).IsNotNull();

            _container = container;
        }

        #endregion

        #region Public Methods

        public new IEnumerable<FilterInfo> GetFilters(HttpConfiguration configuration, HttpActionDescriptor actionDescriptor)
        {
            Ensure.That(nameof(configuration)).IsNotNull();

            Ensure.That(nameof(actionDescriptor)).IsNotNull();

            IList<FilterInfo> filterInfos = base.GetFilters(configuration, actionDescriptor).ToList();

            foreach (FilterInfo filterInfo in filterInfos)
            {
                _container.BuildUp(filterInfo.Instance.GetType(), filterInfo.Instance);
            }

            return filterInfos;
        }

        #endregion

        #region Public Static Methods

        public static void Register(IUnityContainer container, HttpConfiguration configuration)
        {
            Ensure.That(nameof(container)).IsNotNull();

            Ensure.That(nameof(configuration)).IsNotNull();

            IList<IFilterProvider> providers = configuration.Services.GetFilterProviders().ToList();

            IFilterProvider defaultProvider = providers.Single(o => o is ActionDescriptorFilterProvider);

            configuration.Services.Remove(typeof(IFilterProvider), defaultProvider);

            configuration.Services.Add(typeof(IFilterProvider), new UnityActionFilterProvider(container));
        }

        #endregion
    }
}