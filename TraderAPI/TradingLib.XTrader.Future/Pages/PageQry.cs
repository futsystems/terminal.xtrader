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
            btnQryPositionDetail.Click += new EventHandler(btnQryPositionDetail_Click);

            CoreService.EventQry.OnRspXQryAccountFinanceEvent += new Action<RspXQryAccountFinanceResponse>(EventQry_OnRspXQryAccountFinanceEvent);
            CoreService.EventQry.OnRspXQrySettlementResponse += new Action<RspXQrySettleInfoResponse>(EventQry_OnRspXQrySettlementResponse);
            CoreService.EventQry.OnRspXQryPositionDetailResponse += new Action<RspXQryPositionDetailResponse>(EventQry_OnRspXQryPositionDetailResponse);

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
                content.Add(string.Empty);
                content.Add(string.Empty);
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


        #region 持仓明细


        List<PositionDetail> positiondetaillist = new List<PositionDetail>();
        void EventQry_OnRspXQryPositionDetailResponse(RspXQryPositionDetailResponse obj)
        {
            if (obj.RequestID != qryID_PositionDetail) return;

            if (obj.PositionDetail != null)
                positiondetaillist.Add(obj.PositionDetail);


            if (obj.IsLast)
            {

                if (positiondetaillist.Count() > 0)
                {
                    List<string> settlelist = new List<string>();
                    int ln = 142;
                    string sline = Line(ln);

                    int i = 0;
                    int size = 0;
                    decimal unpl = 0;
                    decimal unplbydate = 0;
                    decimal hmargin = 0;

                    settlelist.Add(string.Empty);
                    settlelist.Add(SectionName("持仓明细"));
                    settlelist.Add(sline);
                    settlelist.Add(string.Format("|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|",
                        "交易所".PadCenterEx(len_EXCH),
                        "品种".PadCenterEx(len_SECURITY),
                        "合约".PadCenterEx( len_SYMBOL),
                        "开仓日期".PadCenterEx( len_DATE),
                        "投/保".PadCenterEx( len_TBMM),
                        "买/卖".PadCenterEx(len_TBMM),
                        "持仓量".PadCenterEx( len_SIZE),
                        "开仓价".PadCenterEx( len_PRICE),
                        "昨结算".PadCenterEx( len_PRICE),
                        "今结算".PadCenterEx( len_PRICE),
                        "浮动盈亏".PadCenterEx( len_PROFIT),
                        "盯市盈亏".PadCenterEx( len_PROFIT),
                        "保证金".PadCenterEx(len_MARGIN)
                        ));
                    settlelist.Add(sline);
                    foreach (PositionDetail pd in positiondetaillist)
                    {
                        SecurityFamily sym = CoreService.BasicInfoTracker.GetSecurity(pd.SecCode);
                        if (pd.Volume == 0) continue;
                        i++;
                        size += pd.Volume;
                        unpl += 0;
                        unplbydate += pd.PositionProfitByDate;
                        hmargin += pd.Margin;

                        settlelist.Add(string.Format("|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|",
                            pd.Exchange.PadCenterEx( len_EXCH),
                            sym.GetSecurityName().PadCenterEx(len_SECURITY),
                            pd.Symbol.PadCenterEx( len_SYMBOL),
                            pd.OpenDate.ToString().PadCenterEx( len_DATE),
                            "投".PadCenterEx(len_TBMM),
                            (pd.Side ? "买" : " 卖").PadLeftEx(len_TBMM),
                            pd.Volume.ToString().PadRightEx(len_SIZE),
                            pd.OpenPrice.ToFormatStr().PadCenterEx( len_PRICE),
                            pd.LastSettlementPrice.ToFormatStr().PadCenterEx( len_PRICE),
                            pd.SettlementPrice.ToFormatStr().PadCenterEx( len_PRICE),
                            "0".PadRightEx( len_PROFIT),
                            pd.PositionProfitByDate.ToFormatStr().PadRightEx( len_PROFIT),
                            pd.Margin.ToFormatStr().PadRightEx( len_MARGIN),
                            pd.Margin.ToFormatStr().PadRightEx( len_MARGIN)
                            ));
                    }
                    settlelist.Add(sline);
                    //settlelist.Add(string.Format("|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|",
                    //    padLeftEx("共" + i.ToString() + "条", len_EXCH),
                    //    padCenterEx("", len_SECURITY),
                    //    padCenterEx("", len_SYMBOL),
                    //    padCenterEx("", len_DATE),
                    //    padCenterEx("", len_TBMM),
                    //    padCenterEx("", len_TBMM),
                    //    padRightEx(size.ToString(), len_SIZE),
                    //    padCenterEx("", len_PRICE),
                    //    padCenterEx("", len_PRICE),
                    //    padCenterEx("", len_PRICE),
                    //    padCenterEx("", len_PROFIT),
                    //    padCenterEx("", len_PROFIT),
                    //    padCenterEx("", len_MARGIN)

                    //    //padRightEx(unpl.ToFormatStr(), len_PROFIT),
                    //    //padRightEx(unplbydate.ToFormatStr(), len_PROFIT),
                    //    //padRightEx(hmargin.ToFormatStr(), len_MARGIN)
                    //    ));
                    //settlelist.Add(sline);
                    //settlelist.Add(NewLine);
                    //settlelist.Add(NewLine);
                    rtPositionDetails.Text = string.Join("\r\n",settlelist.ToArray());
                }



                
               
                positiondetaillist.Clear();
            }
        }


        int qryID_PositionDetail = 0;
        void btnQryPositionDetail_Click(object sender, EventArgs e)
        {
            qryID_PositionDetail = CoreService.TLClient.ReqXQryPositionDetail();
        }
        #endregion

        const int len_EXCH = 10;
        const int len_SECURITY = 12;
        const int len_SYMBOL = 10;
        const int len_DATE = 8;
        const int len_TBMM = 5;
        const int len_SIZE = 6;
        const int len_PRICE = 12;
        const int len_MARGIN = 12;
        const int len_PROFIT = 12;
        const int len_TURNOVER = 13;
        const int len_COMMISSION = 12;
        const int len_SEQID = 8;

        static int section_location = 50;
        static string SectionName(string name)
        {
            return string.Format("{0," + section_location.ToString() + "}", name);
        }

        public static string Line(int num)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < num; i++)
            {
                sb.Append("-");
            }
            return sb.ToString();
        }
    }
}
