using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TradingLib.XTrader.Control;
using TradingLib.MarketData;

namespace XTraderLite
{
    public partial class MainForm
    {

        Dictionary<EnumViewType, IView> viewMap = new Dictionary<EnumViewType, IView>();
        LinkedList<IView> viewLink = new LinkedList<IView>();
        //IView _curView = null;

        /// <summary>
        /// 设置当前有效视图
        /// </summary>
        /// <param name="type"></param>
        void SetCurrentViewType(EnumViewType type,bool enter=true)
        {
            //logger.Info("set ?????????????:" + type.ToString());
            IView target = null;
            if(!viewMap.TryGetValue(type,out target))
            {
                logger.Warn(string.Format("ViewType:{0} not supported",type));
                return;
            }

            if (target.Visible) return;
            
            foreach (var v in viewMap.Values)
            {
                if (v.Visible) v.Hide();
            }
            if(enter)
            {
                viewLink.AddLast(target);
            }
            target.Show();
            target.Focus();
        }

        /// <summary>
        /// 回退
        /// </summary>
        void RollBackView(bool all=false)
        {
            if (viewLink.Count>=2)
            {
                if (!all)
                {
                    SetCurrentViewType(viewLink.Last.Previous.Value.ViewType, false);//返回前一个
                    viewLink.Remove(viewLink.Last);
                }
                else
                {
                    LinkedListNode<IView> first = viewLink.First;
                    SetCurrentViewType(first.Value.ViewType,false);//返回首页
                    
                    viewLink = new LinkedList<IView>();
                    viewLink.AddFirst(first.Value);
                }
            }
        }


        void SwitchTradingBox()
        {
            panelBroker.Visible ^= true;
            //隐藏交易面板时 将当前行情视图获取焦点
            if (!panelBroker.Visible)
            {
                if (viewLink.Last != null) viewLink.Last.Value.Focus();
            }
        }


        void SwitchMainView()
        {
            //当前为报价表状态 则进入分时
            if (ctrlQuoteList.Visible)
            {
                ViewKChart();
                return;
            }

            if (ctrlKChart.Visible)//如果K线图可视 则切换显示模式 否则直接进入该视图
            {
                switch (ctrlKChart.KChartViewType)
                {
                    case CStock.KChartViewType.TimeView:
                        ctrlKChart.KChartViewType = CStock.KChartViewType.KView;
                        break;
                    case CStock.KChartViewType.KView:
                        ctrlKChart.KChartViewType = CStock.KChartViewType.TimeView;
                        break;
                    default:
                        break;
                }
            }
            ViewKChart();
        }


        MDSymbol GetAvabileSymbol()
        {
            //判定报价列表选中的合约
            MDSymbol tmp = null;
            //当前处于报价列表 则通过报价选中行来获得合约
            if (viewLink.Last.Value.ViewType == EnumViewType.QuoteList)
            {
                tmp = ctrlQuoteList.SymbolSelected;
            }
            else
            {
                tmp = CurrentKChartSymbol;
            }
            return tmp;
        }

        /// <summary>
        /// 查看报价列表
        /// </summary>
        void ViewQuoteList()
        {
            SetCurrentViewType(EnumViewType.QuoteList);
            UpdateToolBarStatus();
        }

        /// <summary>
        /// 查看K线图
        /// </summary>
        void ViewKChart(MDSymbol symbol=null)
        {
            MDSymbol tmp = symbol;
            if (tmp == null)
            {
                tmp = GetAvabileSymbol();
                if (tmp == null) return;
            }

            SetCurrentViewType(EnumViewType.KChart);
            SetKChartSymbol(tmp);
            UpdateToolBarStatus();
        }


        /// <summary>
        /// 查看分笔明细
        /// </summary>
        void ViewTickList()
        {
            MDSymbol tmp = GetAvabileSymbol();
            if (tmp == null) return;

            SetCurrentViewType(EnumViewType.TradeSplit);
            ctrlTickList.Clear();
            ctrlTickList.SetSymbol(tmp);
            int reqId = MDService.DataAPI.QryTradeSplitData(tmp.Exchange, tmp.Symbol, 0, 2000);
            tickListLoadRequest.TryAdd(reqId, ctrlTickList);

        }

        /// <summary>
        /// 查看粉价明细
        /// </summary>
        void ViewPriceVolList()
        {
            MDSymbol tmp = GetAvabileSymbol();
            if (tmp == null) return;

            SetCurrentViewType(EnumViewType.PriceVol);
            ctrlPriceVolList.Clear();
            ctrlPriceVolList.SetSymbol(tmp);
            int reqId = MDService.DataAPI.QryPriceVol(tmp.Exchange, tmp.Symbol);
            priceVolListRequest.TryAdd(reqId, this);
        }

        /// <summary>
        /// 查看基础信息
        /// </summary>
        void ViewSymbolInfo()
        {
            MDSymbol tmp = GetAvabileSymbol();
            if (tmp == null) return;

            SetCurrentViewType(EnumViewType.BasicInfo);
            ctrlSymbolInfo.Clear();
            ctrlSymbolInfo.SetSymbol(tmp);
            MDService.DataAPI.QrySymbolInfoType(tmp.Exchange, tmp.Symbol);

        }
    }
}
