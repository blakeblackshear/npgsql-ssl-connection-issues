using System;
using NHibernate;
using NHibernate.Context;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using ServiceStack;
using Funq;
using System.Threading.Tasks;
using System.Net;
using ServiceStack.Text;
using System.Threading;
using ServiceStack.Web;
using System.Collections.Generic;
using Npgsql;
using NHibernate.Cfg;
using ServiceStack.Data;
using ServiceStack.OrmLite;

namespace sslbegintransacton
{
  public class AppHost : AppSelfHostBase
  {
    private ISessionFactory _factory;
    private IList<long> _times;
    private bool _ssl;
    private NpgsqlConnection _conn;

    public AppHost(bool ssl = true)
      : base("SslTest", typeof(AppHost).Assembly)
    {
      _ssl = ssl;
    }

    public ISessionFactory CreateSessionFactory(string connectionString)
    {
      return Fluently
        .Configure()
        .Database(
          PostgreSQLConfiguration.Standard
          .ConnectionString(connectionString))
        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<ItemMap>())

        .BuildSessionFactory();
    }

    public override void Configure(Container container)
    {
      // register list to track time
      _times = new List<long>();
      container.Register(x => _times);

      log4net.Config.XmlConfigurator.Configure();

      var connectionString = "Server=localhost;Port=5432;Database=test;Username=test;Password=password;Timeout=1000;SslMode=disable";
      if (_ssl)
      {
        connectionString = "Server=localhost;Port=5432;Database=test;Username=test;Password=password;Timeout=1000;SslMode=require;Ssl=true";
      }

      //NHibernate factory
      _factory = CreateSessionFactory(connectionString);
      container.Register(x => _factory);

      //Npgsql connection
      NpgsqlEventLog.EchoMessages = true;
      NpgsqlEventLog.Level = LogLevel.Normal;
      _conn = new NpgsqlConnection(connectionString);
      _conn.Open();
      container.Register(x => _conn);

      //ORMLite
      container.Register<IDbConnectionFactory>(new OrmLiteConnectionFactory(connectionString, PostgreSqlDialect.Provider));
    }

    protected override void Dispose(bool disposing)
    {
      _conn.Close();
      base.Dispose(disposing);
    }
  }
}

