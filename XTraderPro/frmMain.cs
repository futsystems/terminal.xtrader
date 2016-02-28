using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.TraderControl;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using Common.Logging;


namespace XTraderPro
{
    public partial class frmMain : Form
    {
        ILog logger = LogManager.GetLogger("MainForm");
        public frmMain()
        {
            InitializeComponent();

            this.Load += new EventHandler(Form1_Load);

        }

        TLClient<TLSocket_TCP> client;
        void Form1_Load(object sender, EventArgs e)
        {
            ctlChart1.LoadChart();
            ctlChart1.ApplyChartStyle(new ChartStyle());
            ctlChart1.AddIndicator(EnumIndicator.indBollingerBands);
            //ctlChart1.AddIndicator(EnumIndicator.indMACD);

            btnStartClient.Click += new EventHandler(btnStartClient_Click);
            btnQryBar.Click += new EventHandler(btnQryBar_Click);

            HookManager.KeyDown += new KeyEventHandler(HookManager_KeyDown);
            HookManager.KeyUp += new KeyEventHandler(HookManager_KeyUp);
            //this.KeyDown += new KeyEventHandler(frmMain_KeyDown);
        }

        void HookManager_KeyUp(object sender, KeyEventArgs e)
        {
            logger.Info("hook keyup sender:" + sender.ToString());
            //switch (e.KeyCode)
            //{
            //    case Keys.Down:
            //        {
            //            ctlChart1.ZoomIn();
            //            break;
            //        }
            //    case Keys.Up:
            //        {
            //            ctlChart1.ZoomOut();
            //            break;
            //        }
            //}
        }

        void frmMain_KeyDown(object sender, KeyEventArgs e)
        {
            logger.Info("key down:" + e.KeyCode.ToString());
            
        }

         

        void HookManager_KeyDown(object sender, KeyEventArgs e)
        {
            logger.Info("hook keyDown sender:"+sender.ToString());
            switch (e.KeyCode)
            {
                case Keys.Down:
                    {
                        ctlChart1.ZoomIn();
                        break;
                    }
                case Keys.Up:
                    {
                        ctlChart1.ZoomOut();
                        break;
                    }
            }
        }

        void btnQryBar_Click(object sender, EventArgs e)
        {
            QryBarRequest request = RequestTemplate<QryBarRequest>.CliSendRequest(0);
            request.FromEnd = fromEnd.Checked;
            request.Symbol = symbol.Text;
            request.MaxCount = (int)maxCount.Value;
            request.Interval = (int)interval.Value;
            request.Start = start.Value;
            request.End = end.Value;

            client.TLSend(request);
        }

        void btnStartClient_Click(object sender, EventArgs e)
        {
            client = new TLClient<TLSocket_TCP>("127.0.0.1", 5060, "XTraderDataClient");
            client.OnPacketEvent += new Action<IPacket>(client_OnPacketEvent);
            client.Start();

        }

        List<Bar> barlist = new List<Bar>();
        void client_OnPacketEvent(IPacket obj)
        {
            switch (obj.Type)
            {
                case MessageTypes.BARRESPONSE:
                    {

                        RspQryBarResponse response = obj as RspQryBarResponse;
                        barlist.Add(response.Bar);
                        if (response.IsLast)
                        {
                            ctlChart1.LoadBars(barlist);
                            barlist.Clear();
                        }
                        
                        break;
                    }
                default:
                    break;
            }
        }

        private void btnResetChart_Click(object sender, EventArgs e)
        {
            ctlChart1.ResetStockChartX();
        }

        private void btnScrollLeft_Click(object sender, EventArgs e)
        {
            ctlChart1.ScrollLeft();
        }

        private void nScrollRight_Click(object sender, EventArgs e)
        {
            ctlChart1.ScrollRight();
        }

        private void btnZoomIn_Click(object sender, EventArgs e)
        {
            ctlChart1.ZoomIn();
        }

        private void btnZoomOut_Click(object sender, EventArgs e)
        {
            ctlChart1.ZoomOut();
        }
    }
}
