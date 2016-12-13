﻿using System;
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
            exAddress.Text = "121.40.201.40";
            exAddress.Text = "127.0.0.1";
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
            btnExUpdatePass.Click += new EventHandler(btnExUpdatePass_Click);
            btnExQrySymbol.Click += new EventHandler(btnExQrySymbol_Click);
            btnExQryOrder.Click += new EventHandler(btnExQryOrder_Click);
            btnExQryTrade.Click += new EventHandler(btnExQryTrade_Click);
            btnExQryPosition.Click += new EventHandler(btnExQryPosition_Click);
            btnExQryTradingAccount.Click += new EventHandler(btnExQryTradingAccount_Click);
            btnExQryMaxOrderVol.Click += new EventHandler(btnExQryMaxOrderVol_Click);
            btnExPlaceOrder.Click += new EventHandler(btnExPlaceOrder_Click);
            btnExCancelOrder.Click += new EventHandler(btnExCancelOrder_Click);
        }

        void btnExCancelOrder_Click(object sender, EventArgs e)
        {
            if (_apiTrader == null) return;
            frm.fmOrderAction frm = new frm.fmOrderAction(_apiTrader);
            frm.ShowDialog();
            frm.Close();
        }

        void btnExPlaceOrder_Click(object sender, EventArgs e)
        {
            if (_apiTrader == null) return;
            frm.fmReqOrderInsert frm = new frm.fmReqOrderInsert(_apiTrader);
            frm.ShowDialog();
            frm.Close();
        }

        void btnExQryMaxOrderVol_Click(object sender, EventArgs e)
        {
            if (_apiTrader == null) return;
            frm.fmQryMaxOrderVol frm = new frm.fmQryMaxOrderVol(_apiTrader);
            frm.ShowDialog();
            frm.Close();
        }

        void btnExQryTradingAccount_Click(object sender, EventArgs e)
        {
            if (_apiTrader == null) return;
            XLQryTradingAccountField req = new XLQryTradingAccountField();
            bool ret = _apiTrader.QryTradingAccount(req, ++_requestId);
            logger.Info(string.Format("QryTradingAccount Send Success:{0}", ret));
        }

        void btnExQryPosition_Click(object sender, EventArgs e)
        {
            if (_apiTrader == null) return;
            XLQryPositionField req = new XLQryPositionField();
            bool ret = _apiTrader.QryPosition(req, ++_requestId);
            logger.Info(string.Format("QryPosition Send Success:{0}", ret));
        }

        void btnExQryTrade_Click(object sender, EventArgs e)
        {
            if (_apiTrader == null) return;
            XLQryTradeField req = new XLQryTradeField();
            bool ret = _apiTrader.QryTrade(req, ++_requestId);
            logger.Info(string.Format("QryTrade Send Success:{0}", ret));
        }

        void btnExQryOrder_Click(object sender, EventArgs e)
        {
            if (_apiTrader == null) return;
            XLQryOrderField req = new XLQryOrderField();
            bool ret = _apiTrader.QryOrder(req, ++_requestId);
            logger.Info(string.Format("QryOrder Send Success:{0}", ret));
        }

        void btnExQrySymbol_Click(object sender, EventArgs e)
        {
            if (_apiTrader == null) return;
            XLQrySymbolField req = new XLQrySymbolField();
            bool ret = _apiTrader.QrySymbol(req, ++_requestId);
            logger.Info(string.Format("QrySymbol Send Success:{0}", ret));
        }

        void btnExUpdatePass_Click(object sender, EventArgs e)
        {
            if (_apiTrader == null) return;
            frm.fmReqUserPasswordUpdate frm = new frm.fmReqUserPasswordUpdate(_apiTrader);
            frm.ShowDialog();
            frm.Close();
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
            _apiTrader.OnRspUserPasswordUpdate += new Action<XLRspUserPasswordUpdateField, ErrorField, uint, bool>(_apiTrader_OnRspUserPasswordUpdate);
            _apiTrader.OnRspQrySymbol += new Action<XLSymbolField, ErrorField, uint, bool>(_apiTrader_OnRspQrySymbol);
            _apiTrader.OnRspQryOrder += new Action<XLOrderField, ErrorField, uint, bool>(_apiTrader_OnRspQryOrder);
            _apiTrader.OnRspQryTrade += new Action<XLTradeField, ErrorField, uint, bool>(_apiTrader_OnRspQryTrade);
            _apiTrader.OnRspQryPosition += new Action<XLPositionField, ErrorField, uint, bool>(_apiTrader_OnRspQryPosition);
            _apiTrader.OnRspQryTradingAccount += new Action<XLTradingAccountField, ErrorField, uint, bool>(_apiTrader_OnRspQryTradingAccount);
            _apiTrader.OnRspQryMaxOrderVol += new Action<XLQryMaxOrderVolumeField, ErrorField, uint, bool>(_apiTrader_OnRspQryMaxOrderVol);
            _apiTrader.OnRspOrderInsert += new Action<XLInputOrderField, ErrorField, uint, bool>(_apiTrader_OnRspOrderInsert);
            _apiTrader.OnRspOrderAction += new Action<XLInputOrderActionField, ErrorField, uint, bool>(_apiTrader_OnRspOrderAction);

            _apiTrader.OnRtnOrder += new Action<XLOrderField>(_apiTrader_OnRtnOrder);
            _apiTrader.OnRtnTrade += new Action<XLTradeField>(_apiTrader_OnRtnTrade);
            _apiTrader.OnRtnPosition += new Action<XLPositionField>(_apiTrader_OnRtnPosition);
            System.Globalization.CultureInfo info = new System.Globalization.CultureInfo("en");
            
            System.Threading.Thread.CurrentThread.CurrentCulture = info;
            logger.Info("Encoding:" + Encoding.Default.ToString());
            new Thread(() =>
            {

                _apiTrader.Init();
                _apiTrader.Join();
                logger.Info("API Thread Stopped");
            }).Start();
        }

        void _apiTrader_OnRspOrderAction(XLInputOrderActionField arg1, ErrorField arg2, uint arg3, bool arg4)
        {
            logger.Info(string.Format("Field:{0} Rsp:{1} RequestID:{2} IsLast:{3}", JsonConvert.SerializeObject(arg1), JsonConvert.SerializeObject(arg2), arg3, arg4));
        }

        void _apiTrader_OnRspOrderInsert(XLInputOrderField arg1, ErrorField arg2, uint arg3, bool arg4)
        {
            logger.Info(string.Format("Field:{0} Rsp:{1} RequestID:{2} IsLast:{3}", JsonConvert.SerializeObject(arg1), JsonConvert.SerializeObject(arg2), arg3, arg4));
        }

        void _apiTrader_OnRspQryMaxOrderVol(XLQryMaxOrderVolumeField arg1, ErrorField arg2, uint arg3, bool arg4)
        {
            logger.Info(string.Format("Field:{0} Rsp:{1} RequestID:{2} IsLast:{3}", JsonConvert.SerializeObject(arg1), JsonConvert.SerializeObject(arg2), arg3, arg4));
        }

        void _apiTrader_OnRspQryTradingAccount(XLTradingAccountField arg1, ErrorField arg2, uint arg3, bool arg4)
        {
            logger.Info(string.Format("Field:{0} Rsp:{1} RequestID:{2} IsLast:{3}", JsonConvert.SerializeObject(arg1), JsonConvert.SerializeObject(arg2), arg3, arg4));
        }

        void _apiTrader_OnRtnPosition(XLPositionField obj)
        {
            logger.Info(string.Format("PositionNotify:{0}", JsonConvert.SerializeObject(obj)));
        }

        void _apiTrader_OnRspQryPosition(XLPositionField arg1, ErrorField arg2, uint arg3, bool arg4)
        {
            logger.Info(string.Format("Field:{0} Rsp:{1} RequestID:{2} IsLast:{3}", JsonConvert.SerializeObject(arg1), JsonConvert.SerializeObject(arg2), arg3, arg4));
        }

        void _apiTrader_OnRtnTrade(XLTradeField obj)
        {
            logger.Info(string.Format("TradeNotify:{0}", JsonConvert.SerializeObject(obj)));
        }

        void _apiTrader_OnRtnOrder(XLOrderField obj)
        {
            logger.Info(string.Format("OrderNotify:{0}", JsonConvert.SerializeObject(obj)));
        }


        void _apiTrader_OnRspQryTrade(XLTradeField arg1, ErrorField arg2, uint arg3, bool arg4)
        {
            logger.Info(string.Format("Field:{0} Rsp:{1} RequestID:{2} IsLast:{3}", JsonConvert.SerializeObject(arg1), JsonConvert.SerializeObject(arg2), arg3, arg4));
        }

        void _apiTrader_OnRspQryOrder(XLOrderField arg1, ErrorField arg2, uint arg3, bool arg4)
        {
            logger.Info(string.Format("Field:{0} Rsp:{1} RequestID:{2} IsLast:{3}", JsonConvert.SerializeObject(arg1), JsonConvert.SerializeObject(arg2), arg3, arg4));
        }

        /// <summary>
        /// 服务端封送结构体 字符串通过UTF8编码转换成byte数组
        /// c#默认使用GB2312编码来接收字符串因此导致出现乱码
        /// 将字符串通过GGB2312还原成byte数组然后用UTF8解码即可得到正确的字符串
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <param name="arg3"></param>
        /// <param name="arg4"></param>
        void _apiTrader_OnRspQrySymbol(XLSymbolField arg1, ErrorField arg2, uint arg3, bool arg4)
        {
            string s = arg1.SymbolName;
            byte[] b = Encoding.Default.GetBytes(s);
            string s2 = Encoding.UTF8.GetString(b);

            logger.Info(string.Format("Field:{0} Rsp:{1} RequestID:{2} IsLast:{3}", JsonConvert.SerializeObject(arg1), JsonConvert.SerializeObject(arg2), arg3, arg4));
        }

        void _apiTrader_OnRspUserPasswordUpdate(XLRspUserPasswordUpdateField arg1, ErrorField arg2, uint arg3, bool arg4)
        {
            logger.Info(string.Format("Field:{0} Rsp:{1} RequestID:{2} IsLast:{3}", JsonConvert.SerializeObject(arg1), JsonConvert.SerializeObject(arg2), arg3, arg4));
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
