using MySql.Data.MySqlClient;
using System;

namespace ResnexusConverterApp
{
    internal class Program
    {
        public MySqlConnection CreateConnection()
        {
            //Create the connection string
            string server = "";
            string database = "";
            string username = "";
            string password = "";
            string connectionString = $"SERVER={server};DATABASE={database};UID={username};PASSWORD={password};";

            //create the connection
            MySqlConnection connection = new MySqlConnection(connectionString);

            return connection;
        }

        public decimal GetExchangeRate(string currency, MySqlConnection connection)
        {
            decimal exchangeRate;

            //query to get the rate from the databse based on the CurrencyCode field
            string query = " SELECT ExchangeRate FROM conversions WHERE CurrencyCode = '" + currency + "'";
            MySqlCommand cmd = new MySqlCommand(query, connection);
            exchangeRate = System.Convert.ToDecimal(cmd.ExecuteScalar());

            return exchangeRate;
        }

        public decimal ConvertCurrency(string currencyFrom, string currencyTo, decimal amount)
        {
            //Validate the currency codes
            if (currencyFrom.Length < 3 || currencyTo.Length < 3)
            {
                Console.WriteLine("Error: All currency codes must be exactly 3 characters");
            }

            //open the connection to the db
            MySqlConnection connection = CreateConnection();
            connection.Open();

            //get currencyFrom exchange rate
            decimal rateFrom = GetExchangeRate(currencyFrom, connection);

            //get currencyto exchange rate
            decimal rateTo = GetExchangeRate(currencyTo, connection);


            //Calculate the conversion rate between the two currencies
            decimal conversionRate = rateFrom / rateTo;

            //Calculate the new amount based on the conversion rate
            decimal result = conversionRate * amount;


            return result;
        }

        static void Main(string[] args)
        {
            Program p = new Program();

            string currencyFrom = "PHP";
            string currencyTo = "USD";
            decimal amount = 100;

            decimal convertedCurrency = p.ConvertCurrency(currencyFrom, currencyTo, amount);

            Console.WriteLine(convertedCurrency);

        }
    }
}
