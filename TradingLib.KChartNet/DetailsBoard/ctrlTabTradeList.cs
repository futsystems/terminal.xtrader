using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.MarketData;
using Common.Logging;
using System.Drawing;

using CStock;

namespace TradingLib.KryptonControl
{
    public partial class ctrlTabTradeList : System.Windows.Forms.Control
    {
        ILog logger = LogManager.GetLogger("ctrlTabTradeList");
        public ctrlTabTradeList()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(ctrlTradeListTab_Paint);
            this.Resize += new EventHandler(ctrlTabTradeList_Resize);
        }

        void ctrlTabTradeList_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
        }

        /// <summary>
        /// 返回分笔成交明细可显示行数
        /// </summary>
        public int RowCount
        {
            get
            {
                return (this.Height - 2) / lineHeight;
            }
        }


        /// <summary>
        /// 清空数据
        /// </summary>
        public void Clear()
        {
            tradeList.Clear();
            Invalidate();
        }

        /// <summary>
        /// 添加分笔数据
        /// </summary>
        /// <param name="trade"></param>
        public void AddTrade(TradeSplit trade,bool update=true)
        {
            tradeList.Add(trade);
            if (update) Invalidate();
        }

        public void AddTrade(List<TradeSplit> trades,bool update=true)
        {
            tradeList.AddRange(trades);
            if (update) Invalidate();
        }



        MDSymbol symbol = null;
        public MDSymbol Symbol
        {
            get { return symbol; }
            set { symbol = value; }
        }

        int lineHeight = 18;
        List<TradeSplit> tradeList = new List<TradeSplit>();
        void ctrlTradeListTab_Paint(object sender, PaintEventArgs e)
        {
            //logger.Info("paint .....");
            Graphics cv = e.Graphics;
            Rectangle r1 = this.ClientRectangle;
            Brush br = new SolidBrush(Color.Black);
            Pen p = new Pen(Color.Maroon);
            cv.FillRectangle(br, r1);
            br.Dispose();
            p.Dispose();

            if (tradeList.Count == 0)
                return;
            int i = 0;
            int h = (this.Height - 2) / lineHeight;
            if (tradeList.Count > h)
                i = tradeList.Count - h;

            string ss;
            int time = -1, jj = -1;
            float lw;
            SizeF si;
            TradeSplit tk = tradeList[0];
            System.Drawing.Font font = Constants.QuoteFont;
            if (Symbol.BlockType == "7")// tk.value > 300) //为指数
            {
                lw = (this.Width - 52) / 2;
                for (int j = i; j < tradeList.Count; j++)
                {
                    tk = tradeList[j];
                    ss = "";
                    if (time == -1)
                    {
                        jj = 1;
                        ss = string.Format("{0:D2}:{1:D2}:{2:D2}", tk.Time / 100, tk.Time % 100, jj);
                        time = tk.Time;
                    }
                    else
                    {
                        if (tk.Time == time)
                        {
                            jj += 1;
                            ss = string.Format(":{0:D2}", jj);
                        }

                        if (tk.Time > time)// (tk.time - time) > 100)
                        {
                            jj = 1;
                            ss = string.Format("{0:D2}:{1:D2}:{2:D2}", tk.Time / 100, tk.Time % 100, jj);
                            time = tk.Time;
                        }
                    }
                    r1.Y = (j - i) * lineHeight + 2;
                    si = cv.MeasureString(ss, font);
                    if (jj == 1)
                        cv.DrawString(ss, font, Brushes.Gray, (int)(60 - si.Width), r1.Top);
                    else
                        cv.DrawString(ss, font, Brushes.Gray, (int)(60 - si.Width - 1), r1.Top);

                    ss = string.Format("{0:F2}", tk.Price);
                    si = cv.MeasureString(ss, font);
                    cv.DrawString(ss, font, Brushes.Red, (int)(50 + lw - si.Width), r1.Top);

                    ss = string.Format("{0:D}", tk.Vol);
                    si = cv.MeasureString(ss, font);
                    cv.DrawString(ss, font, Brushes.YellowGreen, (int)(50 + 2 * lw - si.Width), r1.Top);
                }
            }
            else
            {
                lw = (this.Width - 92) / 2;
                for (int j = i; j < tradeList.Count; j++)
                {
                    tk = tradeList[j];
                    ss = "";
                    if (time == -1)
                    {
                        jj = 1;
                        ss = string.Format("{0:D2}:{1:D2}:{2:D2}", tk.Time / 100, tk.Time % 100, jj);
                        time = tk.Time;
                    }
                    else
                    {
                        if (tk.Time == time)
                        {
                            jj += 1;
                            ss = string.Format(":{0:D2}", jj);
                        }

                        if (tk.Time > time)// (tk.time - time) > 100)
                        {
                            jj = 1;
                            ss = string.Format("{0:D2}:{1:D2}:{2:D2}", tk.Time / 100, tk.Time % 100, jj);
                            time = tk.Time;
                        }
                    }
                    r1.Y = (j - i) * lineHeight + 2;
                    si = cv.MeasureString(ss, font);
                    if (jj == 1)
                        cv.DrawString(ss, font, Brushes.White, (int)(60 - si.Width), r1.Top);
                    else
                        cv.DrawString(ss, font, Brushes.White, (int)(60 - si.Width - 1), r1.Top);

                    ss = string.Format("{0:F2}", tk.Price);
                    si = cv.MeasureString(ss, font);
                    cv.DrawString(ss, font, Brushes.Red, (int)(50 + lw - si.Width), r1.Top);

                    ss = string.Format("{0:D}", tk.Vol);
                    si = cv.MeasureString(ss, font);
                    cv.DrawString(ss, font, Brushes.Yellow, (int)(50 + 2 * lw - si.Width), r1.Top);

                    if (tk.Flag == 1)
                        ss = "B";
                    else
                        ss = "S";
                    si = cv.MeasureString(ss, font);
                    if (tk.Flag == 1)
                        cv.DrawString(ss, font, Brushes.Red, this.Width - 40, r1.Top);
                    else
                        cv.DrawString(ss, font, Brushes.Lime, this.Width - 40, r1.Top);
                    ss = tk.TradeCount.ToString();
                    si = cv.MeasureString(ss, font);
                    cv.DrawString(ss, font, Brushes.Gray, this.Width - si.Width, r1.Top);
                }
            }
        }


    }
}
