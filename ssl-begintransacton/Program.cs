using System;
using System.Linq;
using NHibernate.Cfg;
using ServiceStack.Text;
using Mono.Unix;
using Mono.Unix.Native;
using System.Collections.Generic;

namespace sslbegintransacton
{
  class MainClass
  {
    public static void Main (string[] args)
    {
      IList<long> times = new List<long>();
      new AppHost(times, args.Length == 0).Init().Start("http://*:9832/");
      "ServiceStack is listening".Print();
      UnixSignal [] signals = new UnixSignal[] { 
        new UnixSignal(Signum.SIGINT), 
        new UnixSignal(Signum.SIGTERM), 
      };

      // Wait for a unix signal
      for (bool exit = false; !exit; )
      {
        int id = UnixSignal.WaitAny(signals);

        if (id >= 0 && id < signals.Length)
        {
          if (signals[id].IsSet) exit = true;
        }
      }
      "Exiting... {0}".Print(times.Average());
    }
  }
}
