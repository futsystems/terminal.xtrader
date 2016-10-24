using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.MarketData;
using Common.Logging;
using TradingLib.XTrader;

namespace CStock
{
    public partial class ctrlStockQuoteInfo : UserControl,IQuoteInfo
    {
        MDSymbol _symbol = null;

        public MDSymbol Symbol { get { return _symbol; } }

        Label[] SellValue = new Label[10];
        Label[] SellVol = new Label[10];
        Label[] BuyValue = new Label[10];
        Label[] BuyVol = new Label[10];

        Label[] Cell = new Label[28];
        Color volc = Color.FromArgb(192, 192, 0);
        string _format = "{0:F2}";

        EnumQuoteInfoType _quoteinfotype = EnumQuoteInfoType.StockCN;

        public EnumQuoteInfoType QuoteInfoType { get { return _quoteinfotype; } }

        public int DefaultHeight { get { return 415; } }

        
        public ctrlStockQuoteInfo()
        {
            InitializeComponent();

            weibi.Font = Constants.QuoteFont;
            weica.Font = Constants.QuoteFont;
            int i = 0;
            //添加买盘label到layoutpanel
            for (i = 0; i < 5; i++)
            {
                Label l = new Label();
                l.AutoSize = false;
                l.TextAlign = ContentAlignment.MiddleRight;
                l.Dock = DockStyle.Top;
                l.Height = 18;
                l.ForeColor = Color.Silver;
                l.Font = Constants.QuoteFont;
                l.Text = i.ToString();// "";
                SellValue[i] = l;
                Sell.Controls.Add(l, 1, 4 - i);

                Label l11 = new Label();
                l11.AutoSize = false;
                l11.TextAlign = ContentAlignment.MiddleRight;
                l11.Dock = DockStyle.Top;
                l11.Height = 18;
                l11.ForeColor = Constants.ColorSize;
                l11.Font = Constants.QuoteFont;
                l11.Text = i.ToString();
                SellVol[i] = l11;
                Sell.Controls.Add(l11, 2, 4 - i);
            }

            //添加卖盘label到layoutpanel
            for (i = 0; i < 5; i++)
            {
                Label l = new Label();
                l.AutoSize = false;
                l.TextAlign = ContentAlignment.MiddleRight;
                l.Dock = DockStyle.Top;
                l.Height = 18;
                l.ForeColor = Color.Silver;
                l.Font = Constants.QuoteFont;
                l.Text = i.ToString();// "";
                BuyValue[i] = l;
                Buy.Controls.Add(l, 1, i);

                Label l11 = new Label();
                l11.AutoSize = false;
                l11.TextAlign = ContentAlignment.MiddleRight;
                l11.Dock = DockStyle.Top;
                l11.Height = 18;
                l11.ForeColor = Constants.ColorSize;
                l11.Font = Constants.QuoteFont;
                l11.Text = i.ToString();
                BuyVol[i] = l11;
                Buy.Controls.Add(l11, 2, i);
            }

            //初始化其他参数标签并加入到layoutpanel
            for (i = 0; i < 28; i++)
            {
                Label l = new Label();
                l.AutoSize = false;
                l.TextAlign = ContentAlignment.MiddleRight;
                l.Dock = DockStyle.Top;
                l.Height = 18;
                l.ForeColor = Color.Silver;
                l.Font = Constants.QuoteFont;
                l.Text = i.ToString();
                Cell[i] = l;
            }
            cell4.Controls.Add(Cell[0], 1, 0);
            cell4.Controls.Add(Cell[1], 1, 1);
            cell4.Controls.Add(Cell[2], 1, 2);
            cell4.Controls.Add(Cell[3], 1, 3);
            cell4.Controls.Add(Cell[4], 1, 4);
            cell4.Controls.Add(Cell[5], 1, 5);
            cell4.Controls.Add(Cell[6], 1, 6);

            cell4.Controls.Add(Cell[7], 3, 0);
            cell4.Controls.Add(Cell[8], 3, 1);
            cell4.Controls.Add(Cell[9], 3, 2);
            cell4.Controls.Add(Cell[10], 3, 3);
            cell4.Controls.Add(Cell[11], 3, 4);
            cell4.Controls.Add(Cell[12], 3, 5);
            cell4.Controls.Add(Cell[13], 3, 6);


            cell5.Controls.Add(Cell[14], 1, 0);
            cell5.Controls.Add(Cell[15], 1, 1);
            cell5.Controls.Add(Cell[16], 1, 2);
            cell5.Controls.Add(Cell[17], 3, 0);
            cell5.Controls.Add(Cell[18], 3, 1);
            cell5.Controls.Add(Cell[19], 3, 2);

            Cell[21].ForeColor = volc;
            Cell[23].ForeColor = volc;

        }

        public void SetSymbol(MDSymbol symbol)
        {
            if (BuyValue[0] == null)
                return;
            _symbol = symbol;

            StkLabel.Text = string.Format("{0} {1}", _symbol.Symbol, _symbol.Name);

            if (symbol.BlockType == "7")
            {
                if (Buy.Visible)
                {
                    Buy.Visible = false;
                    BuySp.Visible = false;
                    Sell.Visible = false;
                    SellSp.Visible = false;
                }

            }
            else
            {
                if (!Buy.Visible)
                {
                    Buy.Visible = true;
                    Sell.Visible = true;
                    BuySp.Visible = true;
                    SellSp.Visible = true;
                }
            }
        }

        public void ClearData()
        {
            if (SellValue[0] != null)
            {
                for (int i = 0; i < 5; i++)
                {
                    SellValue[i].Text = "";
                    SellVol[i].Text = "";
                    BuyValue[i].Text = "";
                    BuyVol[i].Text = "";

                }
            }
        }
        /// <summary>
        /// 更新盘口数据
        /// </summary>
        /// <param name="symbol"></param>
        public void OnTick(MDSymbol symbol)
        {
            double f1 = symbol.TickSnapshot.SellQTY1 + symbol.TickSnapshot.SellQTY2 + symbol.TickSnapshot.SellQTY3 + symbol.TickSnapshot.SellQTY4 + symbol.TickSnapshot.SellQTY5;
            double f2 = symbol.TickSnapshot.BuyQTY1 + symbol.TickSnapshot.BuyQTY2 + symbol.TickSnapshot.BuyQTY3 + symbol.TickSnapshot.BuyQTY4 + symbol.TickSnapshot.BuyQTY5;
            weibi.Text = "";
            weica.Text = "";

            if ((f1 + f2) > 0)
            {
                weibi.Text = string.Format("{0:F2}%", (f2 - f1) * 100 / (f1 + f2));
                weibi.ForeColor = f2 > f1 ? Constants.ColorUp : Constants.ColorDown;
            }
            else
            {
                weibi.Text = "0%";
                weibi.ForeColor = Color.White;
            }
            if ((f2 + f1) > 0)
            {
                weica.Text = string.Format("{0:F0}", f2 - f1);
                weica.ForeColor = f2 > f1 ? Constants.ColorUp : Constants.ColorDown;
            }
            else
            {
                weica.Text = "";
            }


            for (int i = 0; i < 5; i++)
            {
                SellValue[i].Text = "";
                SellVol[i].Text = "";
                BuyValue[i].Text = "";
                BuyVol[i].Text = "";

            }
            for (int i = 0; i < 24; i++)
                Cell[i].Text = "";
            if (symbol.TickSnapshot.BuyQTY1 > 0)
            {
                BuyValue[0].Text = String.Format(_format, symbol.TickSnapshot.Buy1);
                BuyVol[0].Text = String.Format("{0:F0}", symbol.TickSnapshot.BuyQTY1);
                BuyValue[0].ForeColor = symbol.TickSnapshot.Buy1 > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            }

            if (symbol.TickSnapshot.BuyQTY2 > 0)
            {
                BuyValue[1].Text = String.Format(_format, symbol.TickSnapshot.Buy2);
                BuyVol[1].Text = String.Format("{0:F0}", symbol.TickSnapshot.BuyQTY2);
                BuyValue[1].ForeColor = symbol.TickSnapshot.Buy2 > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            }

            if (symbol.TickSnapshot.BuyQTY3 > 0)
            {
                BuyValue[2].Text = String.Format(_format, symbol.TickSnapshot.Buy3);
                BuyVol[2].Text = String.Format("{0:F0}", symbol.TickSnapshot.BuyQTY3);
                BuyValue[2].ForeColor = symbol.TickSnapshot.Buy3 > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            }

            if (symbol.TickSnapshot.BuyQTY4 > 0)
            {
                BuyValue[3].Text = String.Format(_format, symbol.TickSnapshot.Buy4);
                BuyVol[3].Text = String.Format("{0:F0}", symbol.TickSnapshot.BuyQTY4);
                BuyValue[3].ForeColor = symbol.TickSnapshot.Buy4 > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            }
            if (symbol.TickSnapshot.BuyQTY5 > 0)
            {
                BuyValue[4].Text = String.Format(_format, symbol.TickSnapshot.Buy5);
                BuyVol[4].Text = String.Format("{0:F0}", symbol.TickSnapshot.BuyQTY5);
                BuyValue[4].ForeColor = symbol.TickSnapshot.Buy5 > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            }


            if (symbol.TickSnapshot.SellQTY1 > 0)
            {
                SellValue[0].Text = String.Format(_format, symbol.TickSnapshot.Sell1);
                SellVol[0].Text = String.Format("{0:F0}", symbol.TickSnapshot.SellQTY1);
                SellValue[0].ForeColor = symbol.TickSnapshot.Sell1 > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            }

            if (symbol.TickSnapshot.SellQTY2 > 0)
            {
                SellValue[1].Text = String.Format(_format, symbol.TickSnapshot.Sell2);
                SellVol[1].Text = String.Format("{0:F0}", symbol.TickSnapshot.SellQTY2);
                SellValue[1].ForeColor = symbol.TickSnapshot.Sell2 > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            }

            if (symbol.TickSnapshot.SellQTY3 > 0)
            {
                SellValue[2].Text = String.Format(_format, symbol.TickSnapshot.Sell3);
                SellVol[2].Text = String.Format("{0:F0}", symbol.TickSnapshot.SellQTY3);
                SellValue[2].ForeColor = symbol.TickSnapshot.Sell3 > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            }

            if (symbol.TickSnapshot.SellQTY4 > 0)
            {
                SellValue[3].Text = String.Format(_format, symbol.TickSnapshot.Sell4);
                SellVol[3].Text = String.Format("{0:F0}", symbol.TickSnapshot.SellQTY4);
                SellValue[3].ForeColor = symbol.TickSnapshot.Sell4 > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            }
            if (symbol.TickSnapshot.SellQTY5 > 0)
            {
                SellValue[4].Text = String.Format(_format, symbol.TickSnapshot.Sell5);
                SellVol[4].Text = String.Format("{0:F0}", symbol.TickSnapshot.SellQTY5);
                SellValue[4].ForeColor = symbol.TickSnapshot.Sell5 > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            }


            Cell[0].Text = String.Format(_format, symbol.TickSnapshot.Price);
            Cell[0].ForeColor = symbol.TickSnapshot.Price > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;

            Cell[7].Text = String.Format(_format, symbol.TickSnapshot.Open);
            Cell[7].ForeColor = symbol.TickSnapshot.Open > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;

            Cell[1].Text = String.Format(_format, symbol.TickSnapshot.Price - symbol.TickSnapshot.PreClose);
            Cell[1].ForeColor = symbol.TickSnapshot.Price > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;

            Cell[8].Text = String.Format(_format, symbol.TickSnapshot.High);
            Cell[8].ForeColor = symbol.TickSnapshot.High > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;

            if (symbol.TickSnapshot.PreClose != 0)
            {
                Cell[2].Text = String.Format("{0:F2}%", (symbol.TickSnapshot.Price - symbol.TickSnapshot.PreClose) * 100 / symbol.TickSnapshot.PreClose);
                Cell[2].ForeColor = symbol.TickSnapshot.Price > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            }
            Cell[9].Text = String.Format(_format, symbol.TickSnapshot.Low);
            Cell[9].ForeColor = symbol.TickSnapshot.Low > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;

            Cell[3].Text = String.Format("{0:F0}", symbol.TickSnapshot.Size);
            double d1 = symbol.TickSnapshot.Volume;//均价
            if (d1 > 0)
            {
                double d2 = (symbol.TickSnapshot.Amount / symbol.TickSnapshot.Volume) / 100.0;
                Cell[10].Text = String.Format(_format, d2);
                Cell[10].ForeColor = d2 > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            }
            double d3 = symbol.TickSnapshot.Amount; //总额
            if (d3 > 100000000)
            {
                Cell[4].Text = String.Format("{0:F2}亿", symbol.TickSnapshot.Amount / 100000000);
            }
            else
                Cell[4].Text = String.Format("{0:F0}万", symbol.TickSnapshot.Amount / 10000);
            if (d1 > 1000000)//总量
            {
                d1 = d1 / 10000;
                Cell[11].Text = String.Format("{0:F1}万", d1);
            }
            else
                Cell[11].Text = String.Format("{0:F0}", d1);

            f2 = 0.1;
            if (symbol.Symbol.IndexOf("ST") > -1)
                f2 = 0.05;
            Cell[5].Text = String.Format(_format, symbol.TickSnapshot.PreClose * (1 + f2));//涨停
            Cell[5].ForeColor = Constants.ColorUp;
            Cell[12].Text = String.Format(_format, symbol.TickSnapshot.PreClose * (1 - f2));//跌停
            Cell[12].ForeColor = Constants.ColorDown;
            Cell[6].Text = String.Format("{0:F0}", symbol.TickSnapshot.S);
            Cell[6].ForeColor = Color.Red;
            Cell[13].Text = String.Format("{0:F0}", symbol.TickSnapshot.B);
            Cell[13].ForeColor = Constants.ColorDown;
            if (symbol.FinanceData.LTG > 0)
            {
                Cell[14].Text = String.Format("{0:F2}%", symbol.TickSnapshot.Volume / symbol.FinanceData.LTG);
                Cell[18].Text = String.Format("{0:F1}亿", symbol.FinanceData.LTG / 10000);
            }
            else
            {
                Cell[14].Text = "";
                Cell[18].Text = "";
            }

            if (symbol.FinanceData.zl != null)
            {
                Cell[17].Text = String.Format("{0:F1}亿", symbol.FinanceData.zl[0] / 10000);
                Cell[15].Text = String.Format("{0:F2}", symbol.FinanceData.zl[15] / symbol.FinanceData.zl[0] / 10);
                f2 = 0;
                if (symbol.FinanceData.zl[26] > 0)
                    f2 = symbol.FinanceData.zl[26] / symbol.FinanceData.zl[0] / 10;
                Cell[16].Text = String.Format(_format, f2);

                if ((f2 > 0) && (symbol.FinanceData.zl[29] > 0))
                {
                    f2 = symbol.TickSnapshot.Price / (f2 / symbol.FinanceData.zl[29] * 12);
                    Cell[19].Text = String.Format("{0:F1}", f2);
                }
            }
            else
            {
                Cell[15].Text = "";
                Cell[16].Text = "";
                Cell[17].Text = "";
                Cell[19].Text = "";
            }


            Cell[20].Text = String.Format(_format, symbol.TickSnapshot.sellall);
            Cell[20].ForeColor = symbol.TickSnapshot.sellall > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            Cell[21].Text = String.Format("{0:F0}", symbol.TickSnapshot.sellQTYall);

            Cell[22].Text = String.Format(_format, symbol.TickSnapshot.buyall);
            Cell[22].ForeColor = symbol.TickSnapshot.buyall > symbol.TickSnapshot.PreClose ? Constants.ColorUp : Constants.ColorDown;
            Cell[23].Text = String.Format("{0:F0}", symbol.TickSnapshot.buyQTYall);
            //FSGS[0].PreClose = symbol.TickSnapshot.PreClose;
            this.Invalidate();
        }
    }
}
