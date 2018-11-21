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
    /// Třída pro získávání kurzu CZK vůči dalším měnám
    /// </summary>
    public static class CnbRatesClient {
        //URL pro API České národní banky
        private const string API_URL = @"http://www.cnb.cz/cs/financni_trhy/devizovy_trh/kurzy_devizoveho_trhu/denni_kurz.txt";

        /// <summary>
        /// Metoda pro asynchronní získání kurzů
        /// </summary>
        /// <returns>Datová tabulka kurzů</returns>
        public static async Task<DataTable> GetRatesAsync() {
            using (var wc = new WebClient()) {
                wc.Encoding = Encoding.UTF8;
                try {
                    var dataString = await wc.DownloadStringTaskAsync(API_URL);
                    return Parse(dataString);
                } catch (Exception) { //Pokud jakákoliv vyjímka, tak vracíme null, tzn. nepodařilo se získat data pro konverzi
                    return null;
                }
            }
        }

        /// <summary>
        /// Metoda pro získání datové tabulky z textového výstupu API České národní banky
        /// </summary>
        /// <param name="rateString">Textový soubor s kurzy</param>
        /// <returns>Datová tabulka z vybranými kurzy</returns>
        private static DataTable Parse(string rateString) {
            var rates = new DataTable();                            //Vytvoření datové tabulky

            rates.Columns.Add("Code", typeof(string));              //Přidání sloupců
            rates.Columns.Add("Rate", typeof(decimal));

            rates.Rows.Add("CZK", 1);                               //Záznam pro CZK, převod CZK - CZK má kurz 1

            foreach (var line in rateString.SplitLines()) {         //Volání řádků po jednom
                var data = line.Split('|');                         //Rozdělení rádku
                var code = data[3];                                 //Získání kódu měny
                var rate = data[4];                                 //Získání kurzu
                if (code == "EUR" || code == "USD" || code == "GBP") {  //Vyběr měn
                    rates.Rows.Add(code, decimal.Parse(rate, CultureInfo.GetCultureInfo("cs-CZ")));     //Přidání záznamu do tabulky
                }
            }

            return rates;
        }

        /// <summary>
        /// Pomocná metoda pro načítání řádků, postupně iteruje přes řádky a vrací jeden za druhým
        /// </summary>
        /// <param name="input">Text s kurzy</param>
        /// <returns>Řádek z textu</returns>
        private static IEnumerable<string> SplitLines(this string input) {
            if (input == null) {                                    //Vstup je prázdný, přerušuje se iterace
                yield break;
            }

            using (var reader = new StringReader(input)) {          //K procházení textu je použit StringReader
                string line;
                var i = 1;
                while ((line = reader.ReadLine()) != null) {        //Iterace dokud zbývá text
                    if (!(i > 2)) {                                 //Přeskočení hlavičky
                        i++;
                        continue;
                    }
                    yield return line;                              //Vrácení řádku
                }
            }
        }
    }
}
