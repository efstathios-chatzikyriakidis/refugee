using System;
using System.Reflection;
using EnsureThat;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using Refugee.DataAccess.NHibernate.Naming;

namespace Refugee.DataAccess.NHibernate.Config
{
    public static class NHibernateConfiguration
    {
        #region Private Static Fields

        private static ISessionFactory _sessionFactory;

        private static Configuration _configuration;

        #endregion

        #region Public Static Methods

        public static void Initialize(Assembly assembly, string connectionStringName, string currentSessionContextClass = null)
        {
            Ensure.That(nameof(assembly)).IsNotNull();

            Ensure.That(nameof(connectionStringName)).IsNotNullOrWhiteSpace();

            FluentConfiguration fluentConfiguration =
                Fluently.Configure()
                        .Database(PostgreSQLConfiguration.PostgreSQL82.ConnectionString(o => o.FromConnectionStringWithKey(connectionStringName)))
                        .Mappings(m => m.FluentMappings.AddFromAssembly(assembly).Conventions.Add(new DatabaseConvention()));

            fluentConfiguration.ExposeConfiguration(o => o.SetProperty("hbm2ddl.keywords", "auto-quote"));

            if (currentSessionContextClass != null)
            {
                fluentConfiguration.ExposeConfiguration(o => o.SetProperty("current_session_context_class", currentSessionContextClass));
            }

            fluentConfiguration.ExposeConfiguration(o => _configuration = o);

            _sessionFactory = fluentConfiguration.BuildSessionFactory();
        }

        #endregion

        #region Public Static Properties

        public static Configuration Configuration
        {
            get
            {
                if (_configuration == null)
                {
                    throw new ApplicationException("You should first initialize the configuration.");
                }

                return _configuration;
            }
        }

        public static ISessionFactory SessionFactory
        {
            get
            {
                if (_sessionFactory == null)
                {
                    throw new ApplicationException("You should first initialize the configuration.");
                }

                return _sessionFactory;
            }
        }

        #endregion
    }
}