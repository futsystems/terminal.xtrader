﻿using System;
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


        void InitTrader()
        {
            
        }

        void ctrlTraderLogin1_ExitTrader()
        {
            panelBroker.Visible = false;
        }
        void SwitchTradingBox()
        {
            panelBroker.Visible ^= true;
            //隐藏交易面板时 将当前行情视图获取焦点
            if (!panelBroker.Visible)
            {
                if (viewLink.Last != null) viewLink.Last.Value.Focus();
            }

            //加载交易控件
            if (panelBroker.Visible && _traderApi == null)
            {
                new System.Threading.Thread(LoadTrader).Start(); 

            }
        }

        ITraderAPI _traderApi = null;
        Control _traderCtrl = null;
        void LoadTrader()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(LoadTrader), new object[] { });
            }
            else
            {
                _traderApi = Utils.LoadTraderAPI("TradingLib.MarketData.ITraderAPI");

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
                }
            }
        }

        int _oldPanelBrokerHeight = 0;
        bool _panelBrokerMax = false;
        void _traderApi_TraderWindowOpeartion(EnumTraderWindowOperation obj)
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
                    _panelBrokerMax = false;
                    break;
                default:
                    break;
            }
        }
    }
}
