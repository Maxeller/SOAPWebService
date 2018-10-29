using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace SOAPWebService
{
    [ServiceContract]
    public interface IMyService
    {
        [OperationContract]
        string Say(string s);

        [OperationContract]
        int Increment(int i);
    }
}
