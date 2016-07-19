using System;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Neo4jClient;
using Refugee.BusinessLogic.Infrastructure.Logging;
using Refugee.DataAccess.Graph.UnitOfWork;
using Refugee.DataAccess.NHibernate.Config;
using Refugee.DataAccess.NHibernate.Transaction;
using Refugee.DataAccess.Relational.Dao;
using Refugee.DataAccess.Relational.Dao.NHibernate;
using Refugee.Server.Properties;
using Refugee.Server.Utilities;
using Refugee.Server.Validators;

namespace Refugee.Server
{
    public static class UnityConfig
    {
        #region Private Readonly Fields

        private static readonly IUnityContainer _container = new UnityContainer();

        #endregion

        #region Public Methods

        public static IUnityContainer GetContainer()
        {
            return _container;
        }

        public static void Initialize()
        {
            #region Neo4j

            _container.RegisterType<NeoServerConfiguration>(new ContainerControlledLifetimeManager(), new InjectionFactory(o => NeoServerConfiguration.GetConfiguration(new Uri(Settings.Default.Neo4jServerUrl), Settings.Default.Neo4jUserName, Settings.Default.Neo4jPassword)));

            _container.RegisterType<IGraphClientFactory, GraphClientFactory>(new ContainerControlledLifetimeManager());

            _container.RegisterType<IUnitOfWorkFactory, UnitOfWorkFactory>(new ContainerControlledLifetimeManager());

            #endregion

            #region NHibernate

            _container.RegisterInstance(NHibernateConfiguration.SessionFactory, new ContainerControlledLifetimeManager());

            #endregion

            #region AOP

            #region Call Handlers

            _container.RegisterType<ICallHandler, LoggingCallHandler>(typeof(LoggingCallHandler).Name, new ContainerControlledLifetimeManager(), new InjectionProperty(nameof(LoggingCallHandler.Order), 1));

            _container.RegisterType<ICallHandler, TransactionCallHandler>(typeof(TransactionCallHandler).Name, new ContainerControlledLifetimeManager(), new InjectionProperty(nameof(TransactionCallHandler.Order), 2));

            #endregion

            #region Controllers Interception

            _container.AddNewExtension<Interception>();

            Interception interception = _container.Configure<Interception>();

            Assembly.GetExecutingAssembly().GetTypes().Where(o => o.IsSubclassOf(typeof(ApiController))).ToList().ForEach(o => interception.SetInterceptorFor(o, new VirtualMethodInterceptor()));

            #endregion

            #endregion

            #region DAOs

            _container.RegisterType<IUserDao>(new ContainerControlledLifetimeManager(), new InjectionFactory(o =>
            {
                var dao = new UserDao();

                dao.SetSessionFactory(NHibernateConfiguration.SessionFactory);

                return dao;
            }));

            #endregion
            
            #region Validators

            _container.RegisterType<CreateHotSpotInputDtoValidator>(new ContainerControlledLifetimeManager());

            _container.RegisterType<CreateRefugeeInputDtoValidator>(new ContainerControlledLifetimeManager());

            _container.RegisterType<UpdateRefugeeInputDtoValidator>(new ContainerControlledLifetimeManager());

            _container.RegisterType<UpdateRefugeeInputDtoValidator>(new ContainerControlledLifetimeManager());

            _container.RegisterType<AuthenticationInputDtoValidator>(new ContainerControlledLifetimeManager());

            _container.RegisterType<CreateRefugeesFamilyRelationshipInputDtoValidator>(new ContainerControlledLifetimeManager());

            #endregion

            #region Utilities

            _container.RegisterType<ICurrentHttpRequest, CurrentHttpRequest>(new ContainerControlledLifetimeManager());

            #endregion
        }

        #endregion
    }
}