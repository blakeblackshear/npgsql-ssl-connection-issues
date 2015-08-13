using System;

namespace sslbegintransacton
{
  using ServiceStack;

  [Route("/query")]
  public class QueryMessage : IReturn<int> { }
}

