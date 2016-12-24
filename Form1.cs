using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;

namespace MLANN
{
    public partial class Form1 : Form
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
        }

        public Form1()
        {
            InitializeComponent();
        }

        private void GA1_Click(object sender, EventArgs e)
        {
            //here a simple implementation of GA
            //var data = DownloadData("AAPL", 2010);

            //https://www.elitetrader.com/et/threads/c-retrieving-yahoo-historical-prices.80912/
        }

        public static List<HistoricalStock> DownloadData(string ticker, int yearToStartFrom)
        {
            int month = 0;
            int day = 22;

            List<HistoricalStock> retval = new List<HistoricalStock>();

            using (WebClient web = new WebClient())
            {
                var url = string.Format(
                    "http://ichart.finance.yahoo.com/table.csv?s={0}&a={1}&b={2}&&c={3}", ticker, month, day, yearToStartFrom);

                string data = web.DownloadString(url);

                string[] rows = data.Split('\n');

                //First row is headers so Ignore it
                for (int i = 1; i < rows.Length; i++)
                {
                    if (rows[i].Replace("n", "").Trim() == "") continue;

                    string[] cols = rows[i].Split(',');

                    HistoricalStock hs = new HistoricalStock();
                    hs.Date = Convert.ToDateTime(cols[0]);
                    hs.Open = Convert.ToDouble(cols[1]);
                    hs.High = Convert.ToDouble(cols[2]);
                    hs.Low = Convert.ToDouble(cols[3]);
                    hs.Close = Convert.ToDouble(cols[4]);
                    hs.Volume = Convert.ToDouble(cols[5]);
                    hs.AdjClose = Convert.ToDouble(cols[6]);

                    retval.Add(hs);
                }

                return retval;
            }
        }

    }
}
