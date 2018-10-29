using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SOAPWebService
{
    public class MyService : IMyService
    {
        public string Say(string s)
        {
            return $"You sent {s}";
        }

        public int Increment(int i)
        {
            return ++i;
        }
    }
}
