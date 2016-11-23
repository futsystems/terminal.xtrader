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
    public partial class MainForm
    {


        void WireToolBar()
        {
            //toolbar
            btnBack.Click += new EventHandler(btnBack_Click);
            btnHome.Click += new EventHandler(btnHome_Click);
            btnRefresh.Click += new EventHandler(btnRefresh_Click);
            btnQuoteView.Click += new EventHandler(btnQuoteView_Click);
            btnIntraView.Click += new EventHandler(btnIntraView_Click);
            btnBarView.Click += new EventHandler(btnBarView_Click);

            btnFreqDay.Click += new EventHandler(btnFreq_Click);
            btnFreqWeek.Click += new EventHandler(btnFreq_Click);
            btnFreqMonth.Click += new EventHandler(btnFreq_Click);
            btnFreqQuarter.Click += new EventHandler(btnFreq_Click);
            //btnFreqYear.Click += new EventHandler(btnFreq_Click);
            btnFreqM1.Click += new EventHandler(btnFreq_Click);
            btnFreqM5.Click += new EventHandler(btnFreq_Click);
            btnFreqM3.Click += new EventHandler(btnFreq_Click);
            btnFreqM15.Click += new EventHandler(btnFreq_Click);
            btnFreqM30.Click += new EventHandler(btnFreq_Click);
            btnFreqM60.Click += new EventHandler(btnFreq_Click);

            btnDrawBox.Click += new EventHandler(btnDrawBox_Click);
            btnF10.Click += new EventHandler(btnF10_Click);
            btnTickList.Click += new EventHandler(btnTickList_Click);
            btnPriceVolList.Click += new EventHandler(btnPriceVolList_Click);


            btnDebugForm.Click += new EventHandler(btnDebugForm_Click);

#if  DEBUG
            
#else
            btnDemo1.Visible = false;
            btnDemo2.Visible = false;
            btnDemo3.Visible = false;
            btnDebugForm.Visible = false;
            toolStripSeparator10.Visible = false;
#endif

        }

        void btnDebugForm_Click(object sender, EventArgs e)
        {
            _debugform.Show();
        }
        List<ToolStripButton> checkedButtons = new List<ToolStripButton>();
        List<ToolStripButton> hiddenButtons = new List<ToolStripButton>();
        List<ToolStripButton> freqButtons = new List<ToolStripButton>();

        /// <summary>
        /// 更新工具栏按钮状态
        /// 根据当前显示视图 更新工具栏可视状态与选中状态
        /// </summary>
        void UpdateToolBarStatus()
        {
            ClearToolButtonCheckedStatus();
            ClearFreqButtonCheckStatus();

            if (ctrlQuoteList.Visible)
            {
                CheckToolButton(btnQuoteView);
            }
            if (ctrlKChart.Visible)
            {
                if (ctrlKChart.IsIntraView)
                {
                    CheckToolButton(btnIntraView);
                }
                if (ctrlKChart.IsBarView)
                {
                    CheckToolButton(btnBarView);
                    CheckFreqButton(_currentFreq);
                    
                }
            }
        }

        void CheckFreqButton(string freq)
        {
            foreach (var btn in freqButtons)
            {
                if (btn.Tag.ToString() == freq)
                {
                    btn.CheckState = CheckState.Checked;
                    return;
                }
            }
        }

        void CheckToolButton(ToolStripButton btn)
        {
            btn.CheckState = CheckState.Checked ;
            checkedButtons.Add(btn);
        }

        /// <summary>
        /// 清空所有按下去的按钮
        /// </summary>
        void ClearToolButtonCheckedStatus()
        {
            foreach (var btn in checkedButtons)
            {
                btn.CheckState = CheckState.Unchecked;
            }
        }

        void ClearFreqButtonCheckStatus()
        {
            foreach (var btn in freqButtons)
            {
                btn.CheckState = CheckState.Unchecked;
            }
        }



        /// <summary>
        /// 返回首页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnHome_Click(object sender, EventArgs e)
        {
            RollBackView(true);//返回到第一个视图
        }

        /// <summary>
        /// 返回上一个视图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnBack_Click(object sender, EventArgs e)
        {
            RollBackView();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnRefresh_Click(object sender, EventArgs e)
        {
            logger.Info("refresh data");

            //刷新K线控件
            RefreshKChart();

            //报价列表
            if (ctrlQuoteList.Visible)
            {
                //重新订阅实时行情
                MDService.DataAPI.RegisterSymbol(ctrlQuoteList.SymbolVisible.ToArray());
            
            }
        }

        void RefreshKChart()
        {
            if (ctrlKChart.Visible)
            {
                ctrlKChart.ClearData();
                ctrlKChart.SetSymbol(_currentSymbol);
                ctrlKChart.SetCycle(_currentFreq);

                if (_currentSymbol != null && !string.IsNullOrEmpty(_currentFreq))
                {
                    //盘口面板信息与频率和合约无关 直接查询获得更新
                    if (ctrlKChart.ShowDetailPanel)
                    {
                        if (ctrlKChart.TabValue == 0)
                        {
                            int reqId = MDService.DataAPI.QryTradeSplitData(_currentSymbol.Exchange, _currentSymbol.Symbol, 0, ctrlKChart.TabHigh);
                            kChartLoadTradeRequest.TryAdd(reqId, this);
                        }
                        if (ctrlKChart.TabValue == 1)
                        {
                            MDService.DataAPI.QryPriceVol(_currentSymbol.Exchange, _currentSymbol.Symbol);
                        }
                    }

                    //分时数据查询
                    if (ctrlKChart.DaysForIntradayView > 1)
                    {
                        minuteData.Clear();
                        int reqid = MDService.DataAPI.QrySecurityBars(CurrentKChartSymbol.Exchange, CurrentKChartSymbol.Symbol, ConstFreq.Freq_Day, 1, 10);//获得最近10日K线 当天日新不请求 该日分时通过日内分时查询
                        kChartIntraViewDayBarRequest.TryAdd(reqid, this);
                    }
                    else
                    {
                        MDService.DataAPI.QryMinuteDate(_currentSymbol.Exchange, _currentSymbol.Symbol, 0);
                    }

                    //Bar数据查询
                    MDService.DataAPI.QrySecurityBars(_currentSymbol.Exchange, _currentSymbol.Symbol, _currentFreq, 0, 800);
                }
            }
        }


        /// <summary>
        /// 查看报价列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnQuoteView_Click(object sender, EventArgs e)
        {
            ViewQuoteList();
        }

        /// <summary>
        /// 查看K线图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnBarView_Click(object sender, EventArgs e)
        {
            //判定报价列表选中的合约
            MDSymbol tmp = ctrlQuoteList.SymbolSelected;
            if (tmp == null) return;

            ctrlKChart.KChartViewType = CStock.KChartViewType.KView;
            ViewKChart();

        }

        /// <summary>
        /// 查看分时图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnIntraView_Click(object sender, EventArgs e)
        {
            MDSymbol tmp = ctrlQuoteList.SymbolSelected;
            if (tmp == null) return;

            ctrlKChart.KChartViewType = CStock.KChartViewType.TimeView;
            ViewKChart();
        }

        /// <summary>
        /// 切换频率
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnFreq_Click(object sender, EventArgs e)
        {
            MDSymbol tmp = GetAvabileSymbol();
            if (tmp == null) return;

            ToolStripButton btn = sender as ToolStripButton;
            if (btn == null || !freqButtons.Any(b=>b==btn)) return;
            //设定当前频率
            _currentFreq = btn.Tag.ToString();
            ctrlKChart.KChartViewType = CStock.KChartViewType.KView;
            //设定当前显示视图
            ViewKChart();
        }

        /// <summary>
        /// 循环切换频率
        /// </summary>
        void SwitchFreq()
        {
            //为空设置为默认 日线
            if (_currentFreq == string.Empty)
                _currentFreq = ConstFreq.Freq_Day;
            //最后一个频率 返回第一个频率
            if (_currentFreq == freqLink.Last.Value)
            {
                _currentFreq = freqLink.First.Value;
            }
            else
            {
                LinkedListNode<string> node = freqLink.Find(_currentFreq);
                _currentFreq = node.Next.Value;
            }

            ctrlKChart.KChartViewType = CStock.KChartViewType.KView;
            //设定当前显示视图
            ViewKChart();

        }

        /// <summary>
        /// 打开自绘工具栏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnDrawBox_Click(object sender, EventArgs e)
        {
            
            if (ctrlKChart.Visible && ctrlKChart.IsBarView)
            { 
                ctrlKChart.ShowDrawToolBox = !btnDrawBox.Checked;
                btnDrawBox.Checked = !btnDrawBox.Checked;
            }
        }

        /// <summary>
        /// 查看F10资料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnF10_Click(object sender, EventArgs e)
        {
            ViewSymbolInfo();
        }

        void btnPriceVolList_Click(object sender, EventArgs e)
        {
            ViewPriceVolList();
        }

        void btnTickList_Click(object sender, EventArgs e)
        {
            ViewTickList();
        }



    }
}
