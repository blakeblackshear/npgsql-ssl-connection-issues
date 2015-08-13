# Connecting to PostgreSQL with SSL under load

I am seeing issues with Npgsql (2.2.5) when connecting over SSL when there is
a reasonable amount of concurrency and only 1 cpu core available.

In order to reproduce, I am using ServiceStack to create http endpoints for
NHibernate, OrmLite, and Npgsql. Each of these endpoints just open a connection
and a transaction and then close. No other requests are made to the database.

I then use ApacheBench to hit each endpoint 200 times with 100 concurrent
connections. During each request I track the milliseconds needed to open run
`BeginTransaction()` and fetch the average time at the end.

Here is a snippet of the service using plain Npgsql:
```
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
```

Instructions:

- vagrant up
- run `sh /home/vagrant/sync/configs/run-tests.sh` from inside the vm

I consistently see the following output (times are average milliseconds to run
`BeginTransaction()`):
```
 ===> Starting SSL testing

 ===> Testing OrmLite
 ===> OrmLite 744.495 ms

 ===> Testing Nhibernate
 ===> Nhibernate 2364.635 ms

 ===> Testing Npgsql
 ===> Npgsql 637.96 ms

 ===> Starting non-SSL testing

 ===> Testing OrmLite
 ===> OrmLite 1.47 ms

 ===> Testing Nhibernate
 ===> Nhibernate 38.985 ms

 ===> Testing Npgsql
 ===> Npgsql 0.895 ms
```
