using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;

using TradingLib.API;
using TradingLib.Common;
using TradingLib.MarketData;
using TradingLib.XTrader.Control;
using TradingLib.DataCore;

namespace TradingLib.DataFarmManager
{
    public partial class MainForm : Form
    {
        ILog logger = LogManager.GetLogger("MainForm");

       

        DebugForm debugForm = null;
        public MainForm()
        {
            InitializeComponent();
            debugForm = new DebugForm();
            //将控件日志输出时间绑定到debug函数 用于输出到控件
            ControlLogFactoryAdapter.SendDebugEvent += new Action<string>(Debug);

            WireEvent();

            InitMenu();

            InitControl();

            InitQuoteLists();

            InitToolBar();

            //初始化弹窗提醒
            InitPopBW();
        }

        void WireEvent()
        {
            //绑定基础数据修改操作 用于更新TLClient维护的基础数据集
            DataCoreService.EventContrib.RegisterCallback(Modules.DATACORE, Method_DataCore.UPDATE_INFO_MARKETTIME, OnRspUpdateMarketTime);
            DataCoreService.EventContrib.RegisterCallback(Modules.DATACORE, Method_DataCore.UPDATE_INFO_EXCHANGE, OnRspUpdateExchange);
            DataCoreService.EventContrib.RegisterCallback(Modules.DATACORE, Method_DataCore.UPDATE_INFO_SECURITY, OnRspUpdateSecurity);
            DataCoreService.EventContrib.RegisterCallback(Modules.DATACORE, Method_DataCore.UPDATE_INFO_SYMBOL, OnRspUpdateSymbol);
            
        }

        void OnRspUpdateMarketTime(string json, bool isLast)
        {
            string message = json.DeserializeObject<string>();
            var mt = MarketTimeImpl.Deserialize(message);
            DataCoreService.DataClient.GotMarketTime(mt);
        }
        void OnRspUpdateExchange(string json, bool isLast)
        {
            string message = json.DeserializeObject<string>();
            var ex = ExchangeImpl.Deserialize(message);
            DataCoreService.DataClient.GotExchange(ex);
        }
        void OnRspUpdateSecurity(string json, bool isLast)
        {
            string message = json.DeserializeObject<string>();
            var sec = SecurityFamilyImpl.Deserialize(message);
            DataCoreService.DataClient.GotSecurity(sec);
        }
        void OnRspUpdateSymbol(string json, bool isLast)
        {
            string message = json.DeserializeObject<string>();
            var sym = SymbolImpl.Deserialize(message);
            DataCoreService.DataClient.GotSymbol(sym);
        }

        void InitControl()
        {
            ctrlQuoteList.Dock = DockStyle.Fill;

        }


        Dictionary<string, MDSymbol> mdsymbolmap = new Dictionary<string, MDSymbol>();

        MDSymbol GetMDSymbol(string key)
        {
            MDSymbol target = null;
            if (mdsymbolmap.TryGetValue(key, out target))
                return target;
            return null;
        }


        


        void Debug(string msg)
        {
            debugForm.Debug(msg);
        }

        #region pop message


        fmPopMessage popwindow = new fmPopMessage();
        System.ComponentModel.BackgroundWorker bg;

        TradingLib.Common.RingBuffer<TradingLib.API.RspInfo> infobuffer = new TradingLib.Common.RingBuffer<TradingLib.API.RspInfo>(1000);


        /// <summary>
        /// 将需要弹出的消息放入缓存
        /// </summary>
        /// <param name="info"></param>
        void OnRspInfo(TradingLib.API.RspInfo info)
        {
            //将RspInfo写入缓存 等待后台线程进行处理
            infobuffer.Write(info);
        }

        void InitPopBW()
        {
            bg = new BackgroundWorker();
            bg.WorkerReportsProgress = true;
            bg.DoWork += new DoWorkEventHandler(bg_DoWork);
            bg.ProgressChanged += new ProgressChangedEventHandler(bg_ProgressChanged);
            bg.RunWorkerAsync();

            DataCoreService.EventContrib.OnRspInfoEvent += new Action<TradingLib.API.RspInfo>(OnRspInfo);
        }

        /// <summary>
        /// 当后台线程有触发时 调用显示窗口
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bg_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            TradingLib.API.RspInfo info = e.UserState as TradingLib.API.RspInfo;
            System.Drawing.Point p = PointToScreen(status.Location);
            p = new System.Drawing.Point(p.X, p.Y - popwindow.Height + status.Height);

            popwindow.Location = p;
            popwindow.PopMessage(info);
        }

        /// <summary>
        /// 后台工作流程 当缓存中有数据是通过ReportProgress进行触发
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void bg_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                //检查变量 然后对外触发 
                while (infobuffer.hasItems)
                {
                    TradingLib.API.RspInfo info = infobuffer.Read();
                    bg.ReportProgress(1, info);
                    Util.sleep(1000);
                }
                Util.sleep(50);
            }
        }

        #endregion
    }
}
