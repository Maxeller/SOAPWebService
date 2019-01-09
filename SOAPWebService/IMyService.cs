using System;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SOAPWebService {
    /// <summary>
    /// Service interface with operation and fault contracts
    /// </summary>
    [ServiceContract(Namespace = "4it475.vse.cz")]
    public interface IMyService
    {
        /// <summary>
        /// Contract for Say method
        /// </summary>
        /// <param name="s">Input</param>
        /// <returns>Input</returns>
        [OperationContract]
        string Say(string s);

        /// <summary>
        /// Contract for Increment method
        /// </summary>
        /// <param name="i">Input</param>
        /// <returns>Input + 1</returns>
        [OperationContract]
        int Increment(int i);

        /// <summary>
        /// Contract for GetPerson method with test parametres
        /// </summary>
        /// <returns>Test person object</returns>
        [OperationContract(Name = "GetTestPerson")]
        Person GetPerson();

        /// <summary>
        /// Contract for GetPerson method
        /// </summary>
        /// <param name="name">Name input</param>
        /// <param name="position">Position input</param>
        /// <returns>Person object</returns>
        [OperationContract]
        Person GetPerson(string name, string position);

        /// <summary>
        /// Contract for GetExchange method
        /// </summary>
        /// <param name="from">Input currency</param>
        /// <param name="to">Output currency</param>
        /// <param name="amount">Amount</param>
        /// <returns>Calculated amount of selected currency</returns>
        [OperationContract]
        [FaultContract(typeof(NotImplementedException))]
        [FaultContract(typeof(OperationCanceledException))]
        Task<decimal> GetExchange(string from, string to, int amount);
    }
}
