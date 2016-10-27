using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.MarketData;
using System.Drawing;


namespace TradingLib.XTrader.Control
{
    public partial class ctrlSymbolHighLight : System.Windows.Forms.Control
    {
        public ctrlSymbolHighLight()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
            this.MouseDoubleClick += new MouseEventHandler(ctrlSymbolHighLight_MouseDoubleClick);
        }


        public event Action<MDSymbol> SymbolDoubleClick;
        void ctrlSymbolHighLight_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //MessageBox.Show(string.Format("mouse x:{0} Symbol width:{1}", e.X, string.Join(",", symbolWidth.Select(s => s.ToString()).ToArray())));
            int location = -1;
            for (int i = 0; i < symbolWidth.Count; i++)
            {
                if (e.X < symbolWidth[i])
                {
                    location = i;
                    break;
                }
            }
            if (location >= 0 && SymbolDoubleClick!= null)
            {
                SymbolDoubleClick(symbolList[location].Symbol);
                //MessageBox.Show(symbolList[location].Symbol.Symbol);
            }
        }



        bool update = false;

        
        public void AddSymbol(SymbolHighLight sym)
        {
            symbolList.Add(sym);
            Invalidate();
        }

        public void Clear()
        {
            symbolList.Clear();
            symbolWidth.Clear();
            Invalidate();
        }

        List<SymbolHighLight> symbolList = new List<SymbolHighLight>();
        List<int> symbolWidth = new List<int>();

        Font titleFont = new Font("宋体", 10, FontStyle.Bold);
        Font priceFont = new Font("Arial", 9, FontStyle.Bold);
        SolidBrush brush = new SolidBrush(Color.Silver);
        Pen sPen = new Pen(Color.Gray, 1);

        /// <summary>
        /// 返回合约集
        /// </summary>
        public IEnumerable<MDSymbol> Symbols
        {
            get { return symbolList.Select(s => s.Symbol); }
        }


        public void Update(MDSymbol symbol)
        {
            if (symbolList.Select(s => s.Symbol).Any(sym => sym.UniqueKey == sym.UniqueKey))
            {
                Invalidate();
            }
        }

        void GDI_Paint(object sender, System.Windows.Forms.PaintEventArgs e)
        {

            Graphics g = e.Graphics;

            
            string t = string.Empty;
            int space =0;
            symbolWidth.Clear();

            if (symbolList.Count == 0)
            {
                return;
            }
            foreach (var h in symbolList)
            {
                brush.Color = Color.DimGray;
                g.DrawString(h.Title, titleFont, brush, space += 2, (this.Height - titleFont.Height) / 2);



                if (h.Symbol.TickSnapshot.Price > h.Symbol.GetYdPrice())
                {
                    brush.Color = Color.Red;
                }
                else
                {
                    brush.Color = Color.Green;
                }
                space += (int)g.MeasureString(h.Title, titleFont).Width;
                t = string.Format("{0:F2}", h.Symbol.TickSnapshot.Price);
                g.DrawString(t, priceFont, brush, space += 5, (this.Height - titleFont.Height) / 2);

                space += (int)g.MeasureString(t, titleFont).Width;
                t = string.Format("{0:F2}", Math.Abs(h.Symbol.TickSnapshot.Price - h.Symbol.GetYdPrice()));
                g.DrawString(t, priceFont, brush, space += 10, (this.Height - priceFont.Height) / 2);


                space += (int)g.MeasureString(t, titleFont).Width;
                t = string.Format("{0:F2}", h.Symbol.TickSnapshot.Amount / 100000000) + "亿";
                brush.Color = Color.DimGray;
                g.DrawString(t, priceFont, brush, space += 10, (this.Height - priceFont.Height) / 2);

                space += (int)g.MeasureString(t, titleFont).Width;
                g.DrawLine(sPen, space += 2, 0 + 4, space, this.Height - 4);
                symbolWidth.Add(space);
            }
        }
    }

    
}
