using System;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using Refugee.DataAccess.NHibernate.Config;
using Refugee.DataAccess.Relational.Mapping;
using Refugee.DataAccess.Relational.Models;

namespace Refugee.DataAccess.Relational.Creation
{
    public class Program
    {
        static void Main()
        {
            NHibernateConfiguration.Initialize(typeof(UserMap).Assembly, "Refugee");

            using (ISession session = NHibernateConfiguration.SessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    Console.WriteLine("Database creation started.");

                    try
                    {
                        #region Create Schema

                        SchemaExport schemaExport = new SchemaExport(NHibernateConfiguration.Configuration);

                        schemaExport.Execute(true, true, false, session.Connection, null);

                        #endregion

                        #region Import Data

                        session.Save(new User { UserName = "alice", Password = "alice", RealName = "Alice" });

                        session.Save(new User { UserName = "bob", Password = "bob", RealName = "Bob" });

                        #endregion

                        transaction.Commit();

                        Console.WriteLine();

                        Console.WriteLine("Database creation finished.");
                    }
                    catch (Exception exception)
                    {
                        transaction.Rollback();

                        Console.WriteLine();

                        Console.WriteLine(exception.Message);

                        Console.WriteLine();

                        if (exception.InnerException != null)
                        {
                            Console.WriteLine(exception.InnerException.Message);
                        }

                        Console.WriteLine();

                        Console.WriteLine("Database creation rollbacked.");
                    }
                }
            }

            Console.WriteLine();

            Console.WriteLine("Press any key to close the program.");

            Console.ReadKey();
        }
    }
}