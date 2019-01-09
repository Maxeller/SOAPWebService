using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SOAPWebService {
    /// <summary>
    /// Simple rate retrieving client
    /// </summary>
    public static class CnbRatesClient {
        //API URL for Czech National Bank
        private const string API_URL = @"http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

        /// <summary>
        /// Asynchronous method for retrieving rates
        /// </summary>
        /// <returns>Rate datatable</returns>
        public static async Task<DataTable> GetRatesAsync() {
            using (var wc = new WebClient()) {
                wc.Encoding = Encoding.UTF8;
                try {
                    var dataString = await wc.DownloadStringTaskAsync(API_URL);
                    return Parse(dataString);
                } catch (Exception) { //If there is any exception, data for conversion have not been retrieved
                    return null;
                }
            }
        }

        /// <summary>
        /// Method for retrieving data from CNB API
        /// </summary>
        /// <param name="rateString">Text file with rates</param>
        /// <returns>Datatable with selected rates</returns>
        private static DataTable Parse(string rateString) {
            var rates = new DataTable();                            //Datatable initialization

            rates.Columns.Add("Code", typeof(string));              //Column initialization, creating headers
            rates.Columns.Add("Rate", typeof(decimal));

            rates.Rows.Add("CZK", 1);                               //CZK to CZK rate is 1

            foreach (var line in rateString.SplitLines()) {         //Using SplitLines method to iterate over the text file
                var data = line.Split('|');                         //Spliting data in current line
                var code = data[3];                                 //Currency code
                var rate = data[4];                                 //Rate
                if (code == "EUR" || code == "USD" || code == "GBP") {  //Currency restriction, for testing purposes mainly
                    rates.Rows.Add(code, decimal.Parse(rate, CultureInfo.GetCultureInfo("cs-CZ")));     //Adding selected rate to the datatable
                }
            }

            return rates;
        }

        /// <summary>
        /// Method for iterating over text file
        /// </summary>
        /// <param name="input">Text file with rates</param>
        /// <returns>Line from the text file</returns>
        private static IEnumerable<string> SplitLines(this string input) {
            if (input == null) {                                    //Input is empty, interrupting iteration
                yield break;
            }

            using (var reader = new StringReader(input)) {          //Using StringReader for reading the text file
                string line;
                var i = 1;
                while ((line = reader.ReadLine()) != null) {        //Iterate until there is text left
                    if (!(i > 2)) {                                 //Skip header
                        i++;
                        continue;
                    }
                    yield return line;                              //Return current line
                }
            }
        }
    }
}
