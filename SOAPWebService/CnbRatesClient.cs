using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace SOAPWebService {
    public static class CnbRatesClient {
        private const string API_URL = @"http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

        public static async Task<DataTable> GetRatesAsync() {
            using (var wc = new WebClient()) {
                wc.Encoding = Encoding.UTF8;
                var dataString = await wc.DownloadStringTaskAsync(API_URL);
                return Parse(dataString);
            }
        }

        private static DataTable Parse(string rateString) {
            //TODO: Proper exception handling
            var rates = new DataTable();

            rates.Columns.Add("Code", typeof(string));
            rates.Columns.Add("Rate", typeof(decimal));

            foreach (var line in rateString.SplitLines()) {
                var data = line.Split('|');
                var code = data[3];
                var rate = data[4];
                if (code == "EUR" || code == "USD" || code == "GBP") {
                    rates.Rows.Add(code, decimal.Parse(rate, CultureInfo.GetCultureInfo("cs-CZ")));
                }
            }

            return rates;
        }

        private static IEnumerable<string> SplitLines(this string input) {
            if (input == null) {
                yield break;
            }

            using (var reader = new StringReader(input)) {
                string line;
                var i = 1;
                while ((line = reader.ReadLine()) != null) {
                    if (!(i > 2)) {
                        i++;
                        continue;
                    }
                    yield return line;
                }
            }
        }
    }
}
