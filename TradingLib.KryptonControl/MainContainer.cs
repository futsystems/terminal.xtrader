﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.MarketData;


namespace TradingLib.XTrader.Stock
{
    /// <summary>
    /// 交易插件设计
    /// 交易功能部分全部放到交易插件的dll中去实现,控件实现了接口ITraderAPI,主程序通过加载该接口来实现必须的功能
    /// 1.交易相关的 下单，查询交易记录等。不过这些功能相对设计简单 主要用于图标展现
    /// 2.功能区域的正常最大化，最小化，关闭等功能 相互之间的联动
    /// 3.合约选择等相关操作的联动 
    /// 以上操作需要设计一个双向联动接口 双方都通过该接口进行必要的数据访问。尽量减少操作暴露。实现低耦合，高内聚
    /// 
    /// 1.默认只创建登入控件，该控件简单轻便，在主程序调用时可以快速的加载并显示。
    /// 2.当客户执行登入时，当登入成功并执行初始化操作完毕后 在后台线程创建 交易控件，并对外暴露事件 如果在登入控件内的后台线程创建 则需要使用invoke 否则创建的控件无法在前段显示
    /// 3.在主控件中 获得该时间后 将登入控件隐藏同时将交易控件加入到当前主控件并显示 在接受控件注册，需要再次使用invoke才可以显示传递过来的控件
    /// 
    /// 后来通过实验 在登入后台线程直接触发事件，主控件监听后在函数内通过invoke 生成并显示控件，简化了代码逻辑
    /// 
    /// 
    /// </summary>
    public partial class MainContainer : UserControl,ITraderAPI
    {

        public event Action<EnumTraderWindowOperation> TraderWindowOpeartion;

        public MainContainer()
        {
            InitializeComponent();
            ctrlTraderLogin.BackColor = Color.White;
            ctrlTraderLogin.EntryTrader += new Action(ctrlTraderLogin_EntryTrader);

        }

        ctrlStockTrader _trader = null;
        void ctrlTraderLogin_EntryTrader()
        {
            if (InvokeRequired)
            {
                Invoke(new Action(ctrlTraderLogin_EntryTrader), new object[] { });
            }
            else
            {
                _trader = new ctrlStockTrader();
                _trader.Dock = DockStyle.Fill;
                _trader.TraderWindowOpeartion += new Action<EnumTraderWindowOperation>(tmp_TraderWindowOpeartion);
                ctrlTraderLogin.Visible = false;
                this.Controls.Add(_trader);
                _trader.Show();
            }
        }

        void tmp_TraderWindowOpeartion(EnumTraderWindowOperation obj)
        {
            if (TraderWindowOpeartion != null)
            {
                new System.Threading.Thread(delegate()
                {
                    TraderWindowOpeartion(obj);

                    if (obj == EnumTraderWindowOperation.Close)
                    {
                        //关闭交易系统
                        ctrlTraderLogin.Visible = true;
                        ctrlTraderLogin.StopTrader();
                        _trader.Visible = false;

                    }
                    
                }).Start();
            
            }
        }


    }
}
