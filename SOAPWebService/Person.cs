using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SOAPWebService
{
    [DataContract]
    public class Person
    {
        [DataMember]
        public string Name { get; }
        [DataMember]
        public string Position { get; }

        public Person(string name, string position)
        {
            Name = name;
            Position = position;
        }

    }
}
