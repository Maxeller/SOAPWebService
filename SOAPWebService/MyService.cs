using System;
using System.Data;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;

namespace SOAPWebService {
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
            return new Person("Test", "Position");
        }

        public Person GetPerson(string name, string position)
        {
            return new Person(name, position);
        }

        public async Task<decimal> GetExchange(string fromCurr, string toCurr, int amount) {
            var rates = await CnbRatesClient.GetRatesAsync();
            if (rates == null) //Nepodaří se získat kurzy
                throw new FaultException<OperationCanceledException>(new OperationCanceledException(),
                            new FaultReason("Službě se nepodařilo získat data pro konverzi měn"));

            fromCurr = fromCurr.ToUpper();
            toCurr = toCurr.ToUpper();

            decimal result = 0;
            if (toCurr == "CZK" && (fromCurr == "EUR" || fromCurr == "USD" || fromCurr == "GBP" || fromCurr == "CZK")) { //Do CZK
                var rate = (from r in rates.AsEnumerable()
                            where r.Field<string>("Code").Equals(fromCurr)
                            select r.Field<decimal>("Rate")).First();
                result = rate * amount;
            } else if (fromCurr == "CZK" && (toCurr == "EUR" || toCurr == "USD" || toCurr == "GBP" || toCurr == "CZK")) { //Z CZK
                var rate = 1 / (from r in rates.AsEnumerable()
                                where r.Field<string>("Code").Equals(toCurr)
                                select r.Field<decimal>("Rate")).First();
                result = rate * amount;
            } else {
                throw new FaultException<NotImplementedException>(new NotImplementedException(), 
                            new FaultReason("Služba umí převádět pouze z CZK do EUR, USD, GBP a naopak"));
            }

            return decimal.Round(result, 2, MidpointRounding.AwayFromZero);
        }
    }
}
