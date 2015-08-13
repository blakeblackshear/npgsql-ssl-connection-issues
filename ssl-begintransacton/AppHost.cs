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

namespace sslbegintransacton
{
  public class AppHost : AppSelfHostBase
  {
    private ISessionFactory _factory;
    private IList<long> _times;
    private bool _ssl;

    public AppHost(IList<long> times, bool ssl = true)
      : base("SslTest", typeof(AppHost).Assembly)
    {
      _times = times;
      _ssl = ssl;
    }

    public ISessionFactory CreateSessionFactory()
    {
      var connectionString = "Server=localhost;Port=5432;Database=test;Username=test;Password=password;Timeout=1000;SslMode=disable";
      if (_ssl)
      {
        connectionString = "Server=localhost;Port=5432;Database=test;Username=test;Password=password;Timeout=1000;SslMode=require;Ssl=true";
      }
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
      var factory = CreateSessionFactory();

      container.Register(x => _times);
      container.Register(x => factory).ReusedWithin(ReuseScope.Container);
    }

    protected override Task ProcessRequestAsync(HttpListenerContext context)
    {
//      Console.WriteLine("{0} --> starting request thread {1}", DateTime.UtcNow.ToUnixTimeMs(), Thread.CurrentThread.ManagedThreadId);
//      var session = _factory.OpenSession();
//      session.BeginTransaction();
//      CurrentSessionContext.Bind(session);
//      session.CreateSQLQuery("select * from test;").ExecuteUpdate();
      return base.ProcessRequestAsync(context);
    }

    public override void OnEndRequest(IRequest request = null)
    {
      //closeSession();
      base.OnEndRequest();
//      Console.WriteLine("{0} <-- finished request thread {1}", DateTime.UtcNow.ToUnixTimeMs(), Thread.CurrentThread.ManagedThreadId);
    }

    private void LogException(Exception ex)
    {
      "{0}{1}".Fmt(ex.Message, (ex.StackTrace ?? string.Empty).Replace(Environment.NewLine, string.Empty)).Print();
      if(ex.InnerException != null) LogException(ex.InnerException);
    }

    private void closeSession()
    {
      if (!CurrentSessionContext.HasBind(_factory))
      {
        return;
      }

      var session = _factory.GetCurrentSession();

      if (session == null)
      {
        return;
      }

      try
      {
        Console.WriteLine("{0} committing transaction for request thread {1}", DateTime.UtcNow.ToUnixTimeMs(), Thread.CurrentThread.ManagedThreadId);
        session.Transaction.Commit();
        Console.WriteLine("{0} transaction committed for request thread {1}", DateTime.UtcNow.ToUnixTimeMs(), Thread.CurrentThread.ManagedThreadId);
      }
      catch (Exception ex)
      {
        Console.WriteLine("{0} rolling back transaction for request thread {1}", DateTime.UtcNow.ToUnixTimeMs(), Thread.CurrentThread.ManagedThreadId);
        session.Transaction.Rollback();
        Console.WriteLine("{0} transaction rolled back for request thread {1}", DateTime.UtcNow.ToUnixTimeMs(), Thread.CurrentThread.ManagedThreadId);
        throw;
      }
      finally
      {
        Console.WriteLine("{0} unbinding session context for request thread {1}", DateTime.UtcNow.ToUnixTimeMs(), Thread.CurrentThread.ManagedThreadId);
        CurrentSessionContext.Unbind(_factory);
        session.Close();
        session.Dispose();
      }
    }
  }
}

