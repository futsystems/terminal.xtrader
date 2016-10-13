using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TradingLib.MarketData;


namespace XTraderLite
{
    /// <summary>
    /// 交易整合
    /// 在panelBroker中放置的是一个交易控件
    /// 所有交易相关的内容都封装在该控件中，从登入到交易以及退出
    /// 通过配置不同的交易API可以实现不同品种的交易
    /// 同时在交易过程中 行情部分可能会与交易部分有相关交叉，因此我们再这里通过接口来进行调用
    /// 行情部分通过交易接口实现 合约选择，交易数据读取等，下单等相关操作
    /// 交易部分通过交易接口实现 合约逆向选择，等互动操作
    /// 
    /// 原则：降低耦合，实现不同功能的自我内聚
    /// </summary>
    public partial class MainForm
    {

        ITraderAPI _traderApi = null;
        Control _traderCtrl = null;

        /// <summary>
        /// 初始化交易插件
        /// </summary>
        void InitTrader()
        {
            try
            {
                //
                //从配置文件设定的dll初始化交易插件
                string dllname = new ConfigFileBase("apitrader.cfg").GetFirstLine();
                _traderApi = Utils.LoadTraderAPI(dllname);//此处可以设定类名 这样就可以提供多个插件 通过配置文件来实现加载哪个交易或行情插件

                if (_traderApi != null)
                {
                    _traderCtrl = _traderApi as Control;
                    if (_traderCtrl != null)
                    {
                        panelBroker.Controls.Add(_traderCtrl);
                        _traderCtrl.Dock = DockStyle.Fill;
                        _traderApi.Show();
                    }

                    _traderApi.TraderWindowOpeartion += new Action<EnumTraderWindowOperation>(_traderApi_TraderWindowOpeartion);
                    _traderApi.ViewKChart += new Action<string, string, int>(_traderApi_ViewKChart);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("交易插件加载异常,请检查配置文件");
            }
        }

        void ctrlTraderLogin1_ExitTrader()
        {
            panelBroker.Visible = false;
        }
        void SwitchTradingBox()
        {
            if (_traderApi == null)
            {
                MessageBox.Show("交易插件未加载");
                return;
            }

            panelBroker.Visible ^= true;
            //隐藏交易面板时 将当前行情视图获取焦点
            if (!panelBroker.Visible)
            {
                if (viewLink.Last != null) viewLink.Last.Value.Focus();
            }
        }


        /// <summary>
        /// 交易插件接口
        /// </summary>
        ITraderAPI TraderAPI
        {
            get { return _traderApi; }
        }

        /// <summary>
        /// 进入委托面板
        /// 用于调用交易插件接口 用于切换到买入或卖出状态 并设定对应合约
        /// </summary>
        /// <param name="side"></param>
        /// <param name="symbol"></param>
        void EntryOrderPanel(bool side, MDSymbol symbol)
        {
            
            if (_traderApi != null)
            {
                logger.Info(string.Format("Entry Order Panel, Size:{0} Symbol:{1}", side, symbol.UniqueKey));
                _traderApi.EntryOrder(side, symbol.Exchange, symbol.Symbol);
            }
            
        }

        

        void _traderApi_ViewKChart(string arg1, string arg2,int arg3)
        {
            if (InvokeRequired)
            {
                //logger.Info("_traderApi_SymbolSelected invoked");
                //交易插件出发合约选择事件 由于交易插件与当前行情控件在不同线程创建 因此进行界面操作时需要统一进行InvokeRequired判断
                Invoke(new Action<string, string,int>(_traderApi_ViewKChart), new object[] { arg1, arg2,arg3 });
            }
            else
            {
                logger.Info(string.Format("Symbol {0}-{1} Selected", arg1, arg2));
                MDSymbol symbol = MDService.DataAPI.GetSymbol(arg1, arg2);
                if (symbol != null)
                {
                    ctrlKChart.KChartViewType = (arg3 == 0 ? CStock.KChartViewType.TimeView : CStock.KChartViewType.KView);
                    ViewKChart(symbol);
                }
            }
        }

        int _oldPanelBrokerHeight = 0;
        bool _panelBrokerMax = false;
        void _traderApi_TraderWindowOpeartion(EnumTraderWindowOperation obj)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<EnumTraderWindowOperation>(_traderApi_TraderWindowOpeartion), new object[] { obj });
            }
            else
            {
                switch (obj)
                {
                    case EnumTraderWindowOperation.Min:
                        _panelBrokerMax = false;
                        panelBroker.Hide();
                        break;
                    case EnumTraderWindowOperation.Max:
                        {
                            if (!_panelBrokerMax)
                            {
                                _panelBrokerMax = true;
                                _oldPanelBrokerHeight = panelBroker.Height;
                                //splitter.SplitPosition = 0;
                                panelBroker.Height = panelBroker.Height + panelMarket.Height;
                                //splitter.Enabled = false;
                                //panelMarket.Visible = false;
                                //panelBroker.Dock = DockStyle.Fill;
                            }
                            else
                            {
                                //panelMarket.Visible = true;
                                //panelBroker.Dock = DockStyle.Bottom;
                                _panelBrokerMax = false;
                                panelBroker.Height = _oldPanelBrokerHeight;
                                //splitter.Enabled = true;


                            }
                        }
                        break;
                    case EnumTraderWindowOperation.Close:
                        {
                            _panelBrokerMax = false;
                            SwitchTradingBox();
                        }
                        break;
                    default:
                        break;
                }
            }

        }
    }
}
