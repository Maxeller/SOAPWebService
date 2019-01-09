using System.Runtime.Serialization;

namespace SOAPWebService
{
    /// <summary>
    /// Person class
    /// </summary>
    [DataContract]
    public class Person
    {
        /// <summary>
        /// Name property
        /// </summary>
        [DataMember] public string Name { get; set; }
        /// <summary>
        /// Position property
        /// </summary>
        [DataMember] public string Position { get; set; }

        /// <summary>
        /// Person class constructor, creates a person
        /// </summary>
        /// <param name="name">Name of the person</param>
        /// <param name="position">Position of the person</param>
        public Person(string name, string position)
        {
            Name = name;
            Position = position;
        }
    }
}
