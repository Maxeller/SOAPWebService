using System.Runtime.Serialization;

namespace SOAPWebService
{
    [DataContract]
    public class Person
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public string Position { get; set; }

        public Person()
        {

        }

        public Person(string name, string position)
        {
            Name = name;
            Position = position;
        }
    }
}
