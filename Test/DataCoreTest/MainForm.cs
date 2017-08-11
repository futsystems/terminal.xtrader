using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;
using TradingLib.DataCore;

namespace DataCoreTest
{
    public partial class MainForm : Form
    {
        ILog logger = LogManager.GetLogger("MainForm");

        public MainForm()
        {
            InitializeComponent();
            ControlLogFactoryAdapter.SendDebugEvent += new Action<string>(ControlLogFactoryAdapter_SendDebugEvent);

            btnStart.Click += new EventHandler(btnStart_Click);
            
        }

        Dictionary<int, DataAPI> apimap = new Dictionary<int, DataAPI>();
        void btnStart_Click(object sender, EventArgs e)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(o => { Run(); });
        }

        void Run()
        {
            for (int i = 0; i < int.Parse(clientNum.Text); i++)
            {
                DataAPI api = new DataAPI(i, ipaddress.Text, int.Parse(port.Text));
                api.Start();
                apimap.Add(i, api);
                System.Threading.Thread.Sleep(2000);
            }

        }

        void ControlLogFactoryAdapter_SendDebugEvent(string obj)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<string>(ControlLogFactoryAdapter_SendDebugEvent), new object[] { obj });
            }
            else
            {
                debugControl1.GotDebug(obj);
            }
        }


    }
}
