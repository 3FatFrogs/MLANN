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
        public Form1()
        {
            InitializeComponent();
        }

        private void GA1_Click(object sender, EventArgs e)
        {
            var data = Utils.DownloadDataYahoo("AAPL", 2010);



                foreach (var singleEntry in data)
            {
                this.listBox1.Items.Add(singleEntry.DataString);
            }

            var data1 = Utils.DownloadIntradayDataGoogle("AAPL", "", 86400, "100d");

            foreach (var singleEntry in data1)
            {
                this.listBox2.Items.Add(singleEntry);
            }
        }

        private void LoadDB_Click(object sender, EventArgs e)
        {
            var conn = new Utils();
            conn.ConnectToMySql();
        }
    }
}
