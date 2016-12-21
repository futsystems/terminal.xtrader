using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.DataCore;


namespace TradingLib.DataFarmManager
{
    public partial class fmCurrentTickSrv : Form
    {
        public fmCurrentTickSrv()
        {
            InitializeComponent();

            this.Load += new EventHandler(fmCurrentTickSrv_Load);
        }

        void fmCurrentTickSrv_Load(object sender, EventArgs e)
        {
            DataCoreService.EventContrib.RegisterCallback("DataFarm", "QryTickSrv", OnQryTickSrv);
            DataCoreService.DataClient.ReqContribRequest("DataFarm", "QryTickSrv", "");
        }

        void OnQryTickSrv(string json, bool islast)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string, bool>(OnQryTickSrv), new object[] { json, islast });
            }
            else
            {
                TradingLib.Mixins.Json.JsonData data =  TradingLib.Mixins.Json.JsonMapper.ToObject(json);
                lbCurrentTickSrv.Text = data["Payload"]["Server"].ToString();
            }
        }
    }
}
