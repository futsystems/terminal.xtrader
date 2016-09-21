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
            btnFreqYear.Click += new EventHandler(btnFreq_Click);
            btnFreqM1.Click += new EventHandler(btnFreq_Click);
            btnFreqM5.Click += new EventHandler(btnFreq_Click);
            btnFreqM15.Click += new EventHandler(btnFreq_Click);
            btnFreqM30.Click += new EventHandler(btnFreq_Click);
            btnFreqM60.Click += new EventHandler(btnFreq_Click);

            btnDrawBox.Click += new EventHandler(btnDrawBox_Click);
            btnF10.Click += new EventHandler(btnF10_Click);
            btnTickList.Click += new EventHandler(btnTickList_Click);
            btnPriceVolList.Click += new EventHandler(btnPriceVolList_Click);
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
