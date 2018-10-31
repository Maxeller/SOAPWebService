using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SOAPWebService
{
    [ServiceContract(Namespace = "4it475.vse.cz")]
    public interface IMyService
    {
        [OperationContract]
        string Say(string s);

        [OperationContract]
        int Increment(int i);

        [OperationContract(Name = "GetTestPerson")]
        Person GetPerson();

        [OperationContract]
        Person GetPerson(string name, string position);
    }
}
