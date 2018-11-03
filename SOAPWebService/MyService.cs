using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<decimal> GetExchange(string fromCurr, string toCurr, int amount) {
            //TODO: Proper exception handling
            var rates = await CnbRatesClient.GetRatesAsync();
            decimal result = 0;

            if (fromCurr != "CZK") {
                var rate = (from r in rates.AsEnumerable()
                    where r.Field<string>("Code").Equals(fromCurr)
                    select r.Field<decimal>("Rate")).First();
                result = rate * amount;
            } else {
                var rate = 1 / (from r in rates.AsEnumerable()
                               where r.Field<string>("Code").Equals(toCurr)
                               select r.Field<decimal>("Rate")).First();
                result = rate * amount;
            }
            return decimal.Round(result, 2, MidpointRounding.AwayFromZero);
        }
    }
}
