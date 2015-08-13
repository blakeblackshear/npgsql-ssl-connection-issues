using System;
using ServiceStack;
using NHibernate;
using ServiceStack.Text;
using System.Collections.Generic;
using Npgsql;
using System.Linq;

namespace sslbegintransacton
{
  public class QueryService : Service
  {
    public ISessionFactory NHSessionFactory { get; set; }
    public IList<long> Times { get; set; }
    public NpgsqlConnection Connection { get; set; }

    public int Get(NpgsqlMessage request)
    {
      using (var connection = Connection.Clone())
      {
        var beforeTransaction = DateTime.UtcNow.ToUnixTimeMs();
        using (var t = connection.BeginTransaction())
        {
          Times.Add(DateTime.UtcNow.ToUnixTimeMs() - beforeTransaction);
          t.Commit();
        }
      }
      return 0;
    }

    public int Get(NhibernateMessage request)
    {
      using (var session = NHSessionFactory.OpenSession())
      {
        var beforeTransaction = DateTime.UtcNow.ToUnixTimeMs();
        using (var trans = session.BeginTransaction())
        {
          Times.Add(DateTime.UtcNow.ToUnixTimeMs() - beforeTransaction);
          trans.Commit();
        }
      }
      return 0;
    }

    public int Get(OrmliteMessage request)
    {
      using (var db = DbFactory.OpenDbConnection())
      {
        var beforeTransaction = DateTime.UtcNow.ToUnixTimeMs();
        using (var trans = db.BeginTransaction())
        {
          Times.Add(DateTime.UtcNow.ToUnixTimeMs() - beforeTransaction);
          trans.Commit();
        }
      }
      return 0;
    }

    public double Get(TimesMessage request)
    {
      var avg = Times.Average();
      Times.Clear();
      return avg;
    }
  }
}

