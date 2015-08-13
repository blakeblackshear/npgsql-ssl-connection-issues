using System;

namespace sslbegintransacton
{
  using ServiceStack;

  [Route("/npgsql")]
  public class NpgsqlMessage : IReturn<int> { }

  [Route("/ormlite")]
  public class OrmliteMessage : IReturn<int> { }

  [Route("/nhibernate")]
  public class NhibernateMessage : IReturn<int> { }

  [Route("/times")]
  public class TimesMessage : IReturn<int> { }
}

