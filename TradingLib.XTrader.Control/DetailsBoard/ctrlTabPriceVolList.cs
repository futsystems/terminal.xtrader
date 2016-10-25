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
    public partial class ctrlTabPriceVolList : System.Windows.Forms.Control
    {
        ILog logger = LogManager.GetLogger("ctrlPriceVolList");
        public ctrlTabPriceVolList()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.Paint += new PaintEventHandler(ctrlPriceVolList_Paint);
            this.Resize += new EventHandler(ctrlPriceVolList_Resize);
        }

        void ctrlPriceVolList_Resize(object sender, EventArgs e)
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
            pvList.Clear();
            Invalidate();
        }

        /// <summary>
        /// 添加分价数据
        /// </summary>
        /// <param name="trade"></param>
        public void AddPriceVol(PriceVolPair pv,bool update=true)
        {
            pvList.Add(pv);
            if (update) Invalidate();
        }

        public void AddPriceVol(List<PriceVolPair> pvs, bool update = true)
        {
            pvList.AddRange(pvs);
            if (update) Invalidate();
        }



        MDSymbol symbol = null;
        string _priceFormat = "{0:F2}";
        public void SetSymbol(MDSymbol sym)
        {
            symbol = sym;
            _priceFormat = sym.GetFormat();
        }


        int lineHeight = 18;
        List<PriceVolPair> pvList = new List<PriceVolPair>();
        void ctrlPriceVolList_Paint(object sender, PaintEventArgs e)
        {
            Graphics cv = e.Graphics;
            Rectangle r1 = this.ClientRectangle;
            cv.FillRectangle(Brushes.Black, r1);

            if (pvList.Count == 0)
                return;

            string ss;
            float lw = (this.Width - 52) / 2; ;
            double pr = 0;
            if (symbol != null)
                pr = symbol.GetYdPrice();

            SizeF si;
            int maxvol = 1;
            for (int j = 0; j < pvList.Count; j++)
            {
                PriceVolPair tk = pvList[j];
                if (tk.Vol > maxvol)
                    maxvol = tk.Vol;
            }
            for (int j = pvList.Count - 1; j > -1; j--)
            {
                PriceVolPair tk = pvList[j];
                r1.Y = (pvList.Count - 1 - j) * lineHeight + 2;

                ss = string.Format(_priceFormat, tk.Price);
                si = cv.MeasureString(ss, Constants.QuoteFont);
                if (tk.Price > pr)
                    cv.DrawString(ss, Constants.QuoteFont, Brushes.Red, (int)(50 - si.Width), r1.Top);
                else
                    cv.DrawString(ss, Constants.QuoteFont, Brushes.Green, (int)(50 - si.Width), r1.Top);

                ss = string.Format("{0:D}", tk.Vol);
                si = cv.MeasureString(ss, Constants.QuoteFont);
                cv.DrawString(ss, Constants.QuoteFont, Brushes.Yellow, (int)(50 + 1 * lw - si.Width), r1.Top);
                int ww = (int)(tk.Vol * (lw - 4) / maxvol);
                if (ww == 0)
                    ww = 1;
                cv.FillRectangle(Brushes.Aqua, (50 + lw + 2), r1.Top + 2, ww, lineHeight - 4);

                if (r1.Y + lineHeight > this.Height)
                    break;
            }
        }


    }
}
