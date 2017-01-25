using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using MySql.Data.MySqlClient;

namespace MLANN
{
    public class HistoricalStock
    {
        public DateTime Date { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }
        public double AdjClose { get; set; }
        public string DataString { get; set; }
        public string Header { get; set; }
    }

    class Utils
    {
        public void ConnectToMySql()
        {
            MySqlConnection conn = null;
            string myConnectionString;

            myConnectionString = GetConnectionString();

            try
            {
                conn = new MySqlConnection(myConnectionString);
                conn.Open();
                Console.WriteLine("MySQL version : {0}", conn.ServerVersion);
            }
            catch (MySqlException ex)
            {
                Console.Write(ex.Message);
                switch (ex.Number)
                {
                    case 0:
                        Console.WriteLine("Cannot connect to server.  Contact administrator");
                        break;
                    case 1045:
                        Console.WriteLine("Invalid username/password, please try again");
                        break;
                }

                throw ex;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        private static string GetConnectionString()
        {
            return "server=127.0.0.1;uid=root;" +
                "pwd=Admin321;database=sakila;";
        }

        public MySqlConnection GetConnection()
        {
            MySqlConnection conn = null;
            string myConnectionString;

            myConnectionString = GetConnectionString();
            conn = new MySqlConnection(myConnectionString);
            conn.Open();

            return conn;

        }

        public static List<HistoricalStock> DownloadDataYahoo(string ticker, int yearToStartFrom, int monthStart = 10, int dayStart = 1)
        {
            int monthEnd = 4;
            int dayEnd = 25;
            int yearEnd = 2014;

            List<HistoricalStock> retval = new List<HistoricalStock>();

            using (WebClient web = new WebClient())
            {
                var url = string.Format(
                    "http://ichart.finance.yahoo.com/table.csv?s={0}&a={1}&b={2}&c={3}&d={4}&e={5}&f={6}",
                    ticker,
                    monthStart,
                    dayStart,
                    yearToStartFrom,
                    monthEnd,
                    dayEnd,
                    yearEnd);

                string data = web.DownloadString(url);

                var rows = data.Split('\n').Where(x => x.Length > 1).Skip(1).ToList();

                foreach (var row in rows)
                {
                    string[] cols = row.Split(',');

                    HistoricalStock hs = new HistoricalStock();
                    hs.Date = Convert.ToDateTime(cols[0]);
                    hs.Open = Convert.ToDouble(cols[1]);
                    hs.High = Convert.ToDouble(cols[2]);
                    hs.Low = Convert.ToDouble(cols[3]);
                    hs.Close = Convert.ToDouble(cols[4]);
                    hs.Volume = Convert.ToDouble(cols[5]);
                    hs.AdjClose = Convert.ToDouble(cols[6]);
                    hs.DataString = row;

                    retval.Add(hs);
                }
                return retval;
            }
        }

        public static List<HistoricalStock> DownloadIntradayDataYahoo(string ticker, int yearToStartFrom, int month = 0, int day = 1)
        {
            //http://chartapi.finance.yahoo.com/instrument/1.0/GOOG/chartdata;type=quote;range=1d/csv
            //http://chartapi.finance.yahoo.com/instrument/1.0/AAPL/chartdata;type=quote;range=5y/csv

            List<HistoricalStock> retval = new List<HistoricalStock>();

            using (WebClient web = new WebClient())
            {
                var url = string.Format(
                    "http://ichart.finance.yahoo.com/table.csv?s={0}&a={1}&b={2}&&c={3}",
                    ticker,
                    month,
                    day,
                    yearToStartFrom);

                string data = web.DownloadString(url);

                var rows = data.Split('\n').Where(x => x.Length > 1).Skip(1).ToList();

                foreach (var row in rows)
                {
                    string[] cols = row.Split(',');

                    HistoricalStock hs = new HistoricalStock();
                    hs.Date = Convert.ToDateTime(cols[0]);
                    hs.Open = Convert.ToDouble(cols[1]);
                    hs.High = Convert.ToDouble(cols[2]);
                    hs.Low = Convert.ToDouble(cols[3]);
                    hs.Close = Convert.ToDouble(cols[4]);
                    hs.Volume = Convert.ToDouble(cols[5]);
                    hs.AdjClose = Convert.ToDouble(cols[6]);
                    hs.DataString = row;

                    retval.Add(hs);
                }
                return retval;
            }
        }

        public static List<string> DownloadIntradayDataGoogle(string ticker, string exchange, int intervals, string period)
        {
            List<string> pippo = new List<string>();

            WebClient web = new WebClient();

            var url = string.Format(
                "https://www.google.com/finance/getprices?q={0}&x={1}&i={2}&p={3}&f=d,c,h,l,o,v",
                ticker,    //For example, GOOG for Google or EURUSD for the Euro/Dollar currency pair.
                exchange,  //The exchange where the security is listed. For example, NASDAQ for GOOG or CURRENCY for EURUSD.
                intervals, //Google groups the data into intervals whose length in seconds is defined by this parameter.Its minimum value is 60 seconds.
                period     //The period of time from which data will be returned. Google always returns the most recent data. Examples of this parameter are 1d (one day), 1w (one week), 1m (one month), or 1y (one year).
                );

            string data = web.DownloadString(url);

            var rows = data.Split('\n').Where(x => x.Length > 1).Skip(1).Reverse().ToList();

            return rows;
        }

        public void ExecuteQuery(string query)
        {
            using (MySqlCommand cmd = new MySqlCommand())
            {
                cmd.CommandText = "mytable";
                cmd.Connection = GetConnection();
                cmd.CommandText = query;

                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader[0] + "," + reader[1] + "," + reader[2] + "," + reader[3]);
                }
            }
        }
    }
}
