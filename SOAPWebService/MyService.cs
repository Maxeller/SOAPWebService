using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SOAPWebService
{
    [ServiceBehavior(Namespace = "4it475.vse.cz")]
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

        public Person GetPerson()
        {
            return new Person("Test Person", "Test Position");
        }

        public Person GetPerson(string name, string position)
        {
            return new Person(name, position);
        }
    }
}
