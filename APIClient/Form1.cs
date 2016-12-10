using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Threading;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.XLProtocol;
using TradingLib.XLProtocol.Client;
using TradingLib.XLProtocol.V1;
using Common.Logging;
using Newtonsoft.Json;

namespace APIClient
{
    public partial class Form1 : Form
    {
        ILog logger = LogManager.GetLogger("APIClient");
        public Form1()
        {
            InitializeComponent();
            ControlLogFactoryAdapter.SendDebugEvent += new Action<string>(ControlLogFactoryAdapter_SendDebugEvent);
            WireEvent();
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

        void WireEvent()
        {
            this.FormClosing += new FormClosingEventHandler(Form1_FormClosing);
            btnStartEx.Click += new EventHandler(btnStartEx_Click);
            btnStopEx.Click += new EventHandler(btnStopEx_Click);

            btnExLogin.Click += new EventHandler(btnExLogin_Click);
        }

        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            ControlLogFactoryAdapter.SendDebugEvent -= new Action<string>(ControlLogFactoryAdapter_SendDebugEvent);
            if (_apiTrader != null)
            {
                _apiTrader.Release();
            }
        }

        uint _requestId = 0;


        void btnExLogin_Click(object sender, EventArgs e)
        {
            if (_apiTrader == null) return;
            XLReqLoginField req = new XLReqLoginField();
            req.UserID = exUser.Text;
            req.Password = exPass.Text;
            req.UserProductInfo = "APIClient";
            req.MacAddress = "xxx";
            bool ret = _apiTrader.ReqUserLogin(req, ++_requestId);
            logger.Info(string.Format("ReqUserLogin Send Success:{0}", ret));
        }

        
        


        APITrader _apiTrader = null;
        void btnStartEx_Click(object sender, EventArgs e)
        {
            _apiTrader = new APITrader(exAddress.Text, int.Parse(exPort.Text));
            _apiTrader.OnServerConnected += new Action(_apiTrader_OnServerConnected);
            _apiTrader.OnServerDisconnected += new Action<int>(_apiTrader_OnServerDisconnected);
            _apiTrader.OnRspError += new Action<ErrorField>(_apiTrader_OnRspError);
            _apiTrader.OnRspUserLogin += new Action<XLRspLoginField, ErrorField, uint, bool>(_apiTrader_OnRspUserLogin);
            new Thread(() =>
            {

                _apiTrader.Init();
                _apiTrader.Join();
                logger.Info("API Thread Stopped");
            }).Start();
        }

        void _apiTrader_OnRspUserLogin(XLRspLoginField arg1, ErrorField arg2, uint arg3, bool arg4)
        {
            logger.Info(string.Format("Field:{0} Rsp:{1} RequestID:{2} IsLast:{3}", JsonConvert.SerializeObject(arg1), JsonConvert.SerializeObject(arg2), arg3, arg4));
        }

        void _apiTrader_OnRspError(ErrorField obj)
        {
            logger.Info(string.Format("OnRspError ID:{0} Msg:{1}", obj.ErrorID, obj.ErrorMsg));
        }

        void _apiTrader_OnServerDisconnected(int obj)
        {
            logger.Info("Server Disconnected:" + obj.ToString());
        }

        void _apiTrader_OnServerConnected()
        {
            logger.Info("Server Connected");
        }

        void DispError(ErrorField field)
        {
            if (field.ErrorID > 0)
            {
                logger.Info(string.Format("ID:{0} Msg", field.ErrorID, field.ErrorMsg));
            }
        }

        void btnStopEx_Click(object sender, EventArgs e)
        {
            if (_apiTrader == null) return;
            _apiTrader.Release();
            _apiTrader = null;
        }

    }
}
