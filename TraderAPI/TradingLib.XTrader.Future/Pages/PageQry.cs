using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;

namespace TradingLib.XTrader.Future
{
    public partial class PageQry : UserControl, IPage
    {
        string _pageName = PageTypes.PAGE_QRY;
        public string PageName { get { return _pageName; } }
    
        public PageQry()
        {
            InitializeComponent();
            WireEvent();

        }



        void WireEvent()
        {
            tabControl1.Selected += new TabControlEventHandler(tabControl1_Selected);
            btnQryAccountFinace.Click += new EventHandler(btnQryAccountFinace_Click);
            btnQrySettlement.Click += new EventHandler(btnQrySettlement_Click);

            CoreService.EventQry.OnRspXQryAccountFinanceEvent += new Action<RspXQryAccountFinanceResponse>(EventQry_OnRspXQryAccountFinanceEvent);
            CoreService.EventQry.OnRspXQrySettlementResponse += new Action<RspXQrySettleInfoResponse>(EventQry_OnRspXQrySettlementResponse);

            this.Load += new EventHandler(PageQry_Load);
        }

        void PageQry_Load(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;
        }

        void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPageIndex == 0)
            {
                p1TradingDay.Text = CoreService.TLClient.TradingDay.ToString();
            }
            if (e.TabPageIndex == 1)
            {
                p2TradingDay.Text = CoreService.TLClient.TradingDay.ToString();
            }
        }


        



        #region 查询账户财务信息
        void EventQry_OnRspXQryAccountFinanceEvent(RspXQryAccountFinanceResponse obj)
        {
            int columnWidth = 30;
            string split = "    ";
            if (obj.RequestID == qryID_AccountFinance)
            {
                AccountInfo report = obj.Report;
                List<string> content = new List<string>();
                content.Add(string.Format("币种:{0}", Util.GetEnumDescription(CoreService.TradingInfoTracker.Account.Currency).PadRightEx(columnWidth-5)));
                content.Add(string.Empty);
                content.Add(string.Format("当前权益:{0}", report.NowEquity.ToFormatStr().PadRightEx(columnWidth - 9)) + split + string.Format("昨日权益:{0}", report.LastEquity.ToFormatStr().PadRightEx(columnWidth - 9)));
                content.Add(string.Format("可用资金:{0}", report.AvabileFunds.ToFormatStr().PadRightEx(columnWidth - 9)) + split + string.Format("可取资金:{0}", report.AvabileFunds.ToFormatStr().PadRightEx(columnWidth - 9)));
                content.Add(string.Format("平仓盈亏:{0}", report.RealizedPL.ToFormatStr().PadRightEx(columnWidth - 9)) + split + string.Format("持仓盈亏:{0}", report.UnRealizedPL.ToFormatStr().PadRightEx(columnWidth - 9)));
                content.Add(string.Format("手续费:{0}", report.Commission.ToFormatStr().PadRightEx(columnWidth - 7)));
                content.Add(string.Format("保证金:{0}", report.Margin.ToFormatStr().PadRightEx(columnWidth - 7)) + split + string.Format("风险度:{0}", (report.Margin / report.NowEquity).ToFormatStr("{0:P}").PadRightEx(columnWidth - 7)));
                content.Add(string.Empty);
                content.Add(string.Format("冻结保证金:{0}", report.MarginFrozen.ToFormatStr().PadRightEx(columnWidth -10)) + split + string.Format("冻结手续费:{0}", report.CommissionFrozen.ToFormatStr().PadRightEx(columnWidth - 10)));
                content.Add(string.Format("冻结资金:{0}", (report.CommissionFrozen + report.MarginFrozen).ToFormatStr().PadRightEx(columnWidth - 9)));
                content.Add(string.Empty);
                content.Add(string.Format("入金金额:{0}", report.CashIn.ToFormatStr().PadRightEx(columnWidth - 9)) + split + string.Format("出金金额:{0}", report.CashOut.ToFormatStr().PadRightEx(columnWidth - 9)));
                content.Add(string.Format("信用额度:{0}", report.Credit.ToFormatStr().PadRightEx(columnWidth - 9)));
                
                rtAccountFinanceReport.Text = string.Join("\r\n",content.ToArray());
            }
        }

        void btnQryAccountFinace_Click(object sender, EventArgs e)
        {
            p1TradingDay.Text = CoreService.TLClient.TradingDay.ToString();
            qryID_AccountFinance = CoreService.TLClient.ReqXQryAccountFinance();

        }

        int qryID_AccountFinance = 0;
        #endregion


        #region 查询结算单


        List<string> settlementlist = new List<string>();
        void EventQry_OnRspXQrySettlementResponse(RspXQrySettleInfoResponse obj)
        {
            if (obj.RequestID != qryID_Settlement) return;
            settlementlist.Add(obj.SettlementContent);
            if (obj.IsLast)
            {
                rtSettlement.Text = string.Join("", settlementlist.ToArray());
                settlementlist.Clear();
            }
        }

        int qryID_Settlement = 0;

        void btnQrySettlement_Click(object sender, EventArgs e)
        {
            int tradingday = 0;
            if (!int.TryParse(p2TradingDay.Text, out tradingday))
            {
                MessageBox.Show("请输入正确日期");
                return;
            }

            qryID_Settlement = CoreService.TLClient.ReqXQrySettlement(tradingday);
        }
        #endregion




    }
}
