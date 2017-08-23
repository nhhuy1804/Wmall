using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.ServiceModel;
using System.ServiceModel.Description;
using HanuriServiceTest;

using System.Text.RegularExpressions;
using System.IO;
using WSWD.WmallPos.Service.Shared;

namespace HanuriServiceApp
{
    public partial class MainForm : Form
    {
        ServiceHost host = null;
        public MainForm()
        {
            InitializeComponent();

            this.FormClosed += new FormClosedEventHandler(MainForm_FormClosed);
        }

        void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Close the ServiceHost.       
            host.Close();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            BindService();
        }

        void BindService()
        {
            // Create the ServiceHost.
            host = new ServiceHost(typeof(SampleService));
            host.Opened += new EventHandler(host_Opened);
            host.Closed += new EventHandler(host_Closed);

            // Open the ServiceHost to start listening for messages. Since
            // no endpoints are explicitly configured, the runtime will create
            // one endpoint per base address for each service contract implemented
            // by the service.
            host.Open();
        }

        void host_Closed(object sender, EventArgs e)
        {
            label1.Text = "Service closed";
        }

        void host_Opened(object sender, EventArgs e)
        {
            label1.Text = string.Format("Service started {0}", host.BaseAddresses[0].ToString());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string query = "select * from bsm031t where cd_astore = :cd_astore and fg_use = '1'";
            //string connString = "Data Source=124.137.10.26:1521/wmalldev;User Id=wmalldev;Password=wmall#dev;Persist Security Info=True;";
            string connString = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=124.137.10.26)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=wmalldev)));User Id=wmalldev;Password=wmall#dev;";

            OracleDacHelper.DefaultConnectionString = connString;
            using (var dac = new OracleDacHelper())
            {
                var ds = dac.ExecuteQuery(query,
                    new string[] {
                        ":cd_astore"
                    }, new object[] {
                        "01"
                    });
                dataGridView1.DataSource = ds.Tables[0];
                    
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string s = "WSGetPoint".GetCommand();
        }
    }

    public static class StringHelper
    {
        static StringHelper()
        {
            
        }

        static public string GetCommand(this string commandId)
        {
            string filePath = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "ServiceCommands.sql");
            // HttpContext.Current.Server.MapPath("~/App_Data/ServiceCommands.sql");

            // read file and load to command list
            string fileContent = File.ReadAllText(filePath);
            Match m = Regex.Match(fileContent, "<" + commandId + @">\s*(.+?)\s*</" + commandId + ">");
            if (m.Success)
            {
                return m.Groups[0].Value;
            }


            return string.Empty;
        }
    }
}
