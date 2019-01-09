using System;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SOAPWebService {
    /// <summary>
    /// Service class, implementing IMyService interface
    /// </summary>
    [ServiceBehavior(Namespace = "4it475.vse.cz")]
    public class MyService : IMyService
    {
        /// <summary>
        /// Simple method that returns input
        /// </summary>
        /// <param name="s">Input</param>
        /// <returns>Input</returns>
        public string Say(string s)
        {
            return $"You sent {s}";
        }

        /// <summary>
        /// Simple method that returns input+1
        /// </summary>
        /// <param name="i">Input</param>
        /// <returns>Input + 1</returns>
        public int Increment(int i)
        {
            return ++i;
        }

        /// <summary>
        /// Simple method that returns test person object
        /// </summary>
        /// <returns>Test person object</returns>
        public Person GetPerson()
        {
            return new Person("Test", "Position");
        }

        /// <summary>
        /// Simple method that returns person object
        /// </summary>
        /// <param name="name">Name input</param>
        /// <param name="position">Position input</param>
        /// <returns>Person object</returns>
        public Person GetPerson(string name, string position)
        {
            return new Person(name, position);
        }

        /// <summary>
        /// Method for currency rates calculation
        /// </summary>
        /// <param name="fromCurr">Input currency</param>
        /// <param name="toCurr">Output currency</param>
        /// <param name="amount">Amount</param>
        /// <returns>Calculated amount of selected currency</returns>
        public async Task<decimal> GetExchange(string fromCurr, string toCurr, int amount) {
            var rates = await CnbRatesClient.GetRatesAsync();                                               //Table of rates retrieval
            if (rates == null)                                                                              //Table of rates retrieval was not successful
                throw new FaultException<OperationCanceledException>(new OperationCanceledException(),
                            new FaultReason("Service could not retrieve data for currency conversion"));

            fromCurr = fromCurr.ToUpper();                                                                  //Code conversion to capitals
            toCurr = toCurr.ToUpper();

            decimal result = 0;
            if (toCurr == "CZK" && (fromCurr == "EUR" || fromCurr == "USD" || fromCurr == "GBP" || fromCurr == "CZK")) { //To CZK
                var rate = (from r in rates.AsEnumerable()                              //Rate retrieval
                            where r.Field<string>("Code").Equals(fromCurr)
                            select r.Field<decimal>("Rate")).First();
                result = rate * amount;                                                 //Calculation
            } else if (fromCurr == "CZK" && (toCurr == "EUR" || toCurr == "USD" || toCurr == "GBP" || toCurr == "CZK")) { //From CZK
                var rate = 1 / (from r in rates.AsEnumerable()                          //1/rate retrieval
                                where r.Field<string>("Code").Equals(toCurr)
                                select r.Field<decimal>("Rate")).First();
                result = rate * amount;                                                 //Calculation
            } else {
                throw new FaultException<NotImplementedException>(new NotImplementedException(),            //Fault for other inputs
                            new FaultReason("Only CZK to EUR, USD, GBP conversion and vice versa is available"));
            }

            return decimal.Round(result, 2, MidpointRounding.AwayFromZero);             //Return calculated amount with 2 decimal points
        }
    }
}
