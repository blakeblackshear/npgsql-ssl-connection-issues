using System;
using ServiceStack;
using NHibernate;
using ServiceStack.Text;
using System.Threading;
using System.Collections.Generic;

namespace sslbegintransacton
{
  using System.Linq;

  public class QueryService : Service
  {
    public ISessionFactory NHSessionFactory { get; set; }
    public IList<long> Times { get; set; }

    public int Get(QueryMessage request)
    {
      using (var session = NHSessionFactory.OpenSession())
      {
        var beforeTransaction = DateTime.UtcNow.ToUnixTimeMs();
        using (var trans = session.BeginTransaction())
        {
          Times.Add(DateTime.UtcNow.ToUnixTimeMs() - beforeTransaction);
          session.CreateSQLQuery("select * from test;").ExecuteUpdate();
          trans.Commit();
        }
      }

      return 0;// Session.CreateSQLQuery("select * from test;").ExecuteUpdate();
    }
  }
}

