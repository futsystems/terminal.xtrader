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

namespace TradingLib.XTrader
{
    public partial class ctrlQuoteInfo : System.Windows.Forms.Control, IQuoteInfo
    {
        ILog logger = LogManager.GetLogger("ctrlQuoteInfo");
        public ctrlQuoteInfo()
        {
            InitializeComponent();
            this.DoubleBuffered = true;

            this.Paint += new PaintEventHandler(ctrlQuoteInfo_Paint);
            this.Resize += new EventHandler(ctrlPriceVolList_Resize);

            rformat.Alignment = StringAlignment.Far;
        }

        void ctrlPriceVolList_Resize(object sender, EventArgs e)
        {
            this.Invalidate();
            
            
        }

        public int DefaultHeight { get { return 202; } }

        EnumQuoteInfoType _quoteinfotype = EnumQuoteInfoType.FutureOverSea;

        public EnumQuoteInfoType QuoteInfoType { get { return _quoteinfotype; } }


        MDSymbol _symbol = null;
        string _priceFormat = "{0:F2}";
        public void SetSymbol(MDSymbol sym)
        {
            _symbol = sym;
            _symTitle = string.Format("{0}({1})", _symbol.Name, _symbol.Symbol);
            _priceFormat = sym.GetFormat();
        }

        string _symTitle = "美原油(CLX6)";
        SolidBrush _brush = new SolidBrush(Color.Yellow);
        SizeF fsize;
        SizeF locatioin;
        Pen pen = new Pen(Color.White);
        int symbolTitleHieght = 30;
        int quoteRateHight = 5;
        const string SELL = "卖出";
        const string BUY = "买入";
        const string LASTPRICE = "最新";
        const string LASTSIZE = "现手";
        const string AVGPRICE = "均价";
        const string CHANGE = "涨跌";
        const string PRESETTLE = "昨结";
        const string CHANGEPECT = "幅度";
        const string OPEN = "开盘";
        const string VOL = "总手";
        const string HIGH = "最高";
        const string PREOI = "持仓";
        const string LOW = "最低";

        Point location = new Point(0, 0);

        Color GetPriceColor(double price)
        { 
            if(_symbol == null) return Color.Red;
            if (price == _symbol.GetYdPrice()) return Constants.ColorEq;
            if (price > _symbol.GetYdPrice()) return Constants.ColorUp;
            return Constants.ColorDown;
        }

        Color GetColor(double val)
        {
            if (val==0) return Constants.ColorEq;
            if (val >0 ) return Constants.ColorUp;
            return Constants.ColorDown;
        }

        StringFormat rformat = new StringFormat();

        public void OnTick(MDSymbol symbol)
        {
            this.Invalidate();
        }


        void ctrlQuoteInfo_Paint(object sender, PaintEventArgs e)
        {
            Graphics cv = e.Graphics;
            Rectangle r1 = this.ClientRectangle;
            cv.FillRectangle(Brushes.Black, r1);
            string pricestr= string.Empty;
            string sizestr = string.Empty;

            Color color = Color.Red;

            locatioin.Height = 0;
            locatioin.Width = 0;

            //绘制合约标题
            _brush.Color = Color.Yellow;
            fsize = cv.MeasureString(_symTitle, Constants.Font_QuoteInfo_SymbolTitle);
            cv.DrawString(_symTitle, Constants.Font_QuoteInfo_SymbolTitle, _brush,(this.Width-fsize.Width)/2, 2);
            locatioin.Height += symbolTitleHieght;

            pen.Color = Constants.Color_TableLine;
            cv.DrawLine(pen, 0, locatioin.Height, this.Width, locatioin.Height);
            locatioin.Height += 3;

            double quotrate = 0.4;
            if (_symbol != null)
            {
                
                double f1 = _symbol.TickSnapshot.SellQTY1;
                double f2 = _symbol.TickSnapshot.BuyQTY1;
                if (f1 + f2 > 0)
                {
                    quotrate = f2 / (f2 + f1);
                }
            }

            pen.Width = quoteRateHight;
            int buylength = (int)(this.Width * quotrate);
           
            pen.Color = Color.Red;
            cv.DrawLine(pen, 0, locatioin.Height, buylength, locatioin.Height);
            pen.Color = Constants.ColorDown;
            cv.DrawLine(pen, buylength, locatioin.Height, this.Width, locatioin.Height);
            locatioin.Height += 3;


            //卖出栏
            pen.Color = Constants.Color_TableLine;
            pen.Width = 1;
            cv.DrawLine(pen, 0, locatioin.Height, this.Width, locatioin.Height);


            if (_symbol != null)
            {
                pricestr = string.Format(_priceFormat, _symbol.TickSnapshot.Sell1);
                color = GetPriceColor(_symbol.TickSnapshot.Sell1);
                sizestr = ((int)_symbol.TickSnapshot.SellQTY1).ToString();
            }
            else
            {
                pricestr = "234.0";
                sizestr = "44";
            }
            fsize = cv.MeasureString(pricestr, Constants.Font_QuoteInfo_BigQuote);

            _brush.Color = Constants.ColorLabel;
            cv.DrawString(SELL, Constants.Font_QuoteInfo_FieldTitle, _brush, 0, locatioin.Height +10);

            _brush.Color = color;
            
            cv.DrawString(pricestr, Constants.Font_QuoteInfo_BigQuote, _brush, 50, locatioin.Height +2);
            _brush.Color = Color.Yellow;
            cv.DrawString(sizestr, Constants.Font_QuoteInfo_BigQuote, _brush, 50 + fsize.Width + 10, locatioin.Height + 2);

            locatioin.Height += fsize.Height +2;

            //买入栏
            if (_symbol != null)
            {
                pricestr = string.Format(_priceFormat, _symbol.TickSnapshot.Buy1);
                color = GetPriceColor(_symbol.TickSnapshot.Buy1);
                sizestr = ((int)_symbol.TickSnapshot.BuyQTY1).ToString();
            }
            else
            {
                pricestr = "224.8";
                sizestr = "15";
            }

            pen.Color = Constants.Color_TableLine;
            pen.Width = 1;
            cv.DrawLine(pen, 0, locatioin.Height, this.Width, locatioin.Height);

            _brush.Color = Constants.ColorLabel;
            cv.DrawString(BUY, Constants.Font_QuoteInfo_FieldTitle, _brush, 0, locatioin.Height+10);

           
            _brush.Color = color;
            cv.DrawString(pricestr, Constants.Font_QuoteInfo_BigQuote, _brush, 50, locatioin.Height + 2);
            _brush.Color = Color.Yellow;
            cv.DrawString(sizestr, Constants.Font_QuoteInfo_BigQuote, _brush, 50 + fsize.Width + 10, locatioin.Height + 2);

            locatioin.Height += fsize.Height + 2;
            pen.Color = Constants.Color_TableLine;
            pen.Width = 1;
            cv.DrawLine(pen, 0, locatioin.Height, this.Width, locatioin.Height);

            locatioin.Height += 1;

            //绘制表格
            //line1
            _brush.Color = Constants.ColorLabel;
            cv.DrawString(LASTPRICE, Constants.Font_QuoteInfo_FieldTitle, _brush, 0, locatioin.Height + 3);

            if (_symbol != null)
            {
                pricestr = string.Format(_priceFormat, _symbol.TickSnapshot.Price);
                color = GetPriceColor(_symbol.TickSnapshot.Price);
            }
            _brush.Color = color;
            fsize = cv.MeasureString(pricestr,Constants.Font_QuoteInfo_FieldPrice);
            cv.DrawString(pricestr, Constants.Font_QuoteInfo_FieldPrice, _brush, this.Width / 2, locatioin.Height + 3, rformat);

            _brush.Color = Constants.ColorLabel;
            
            cv.DrawString(LASTSIZE, Constants.Font_QuoteInfo_FieldTitle, _brush, this.Width/2, locatioin.Height + 3);
            if (_symbol != null)
            {
                pricestr = _symbol.TickSnapshot.Size.ToString();
            }
            _brush.Color = Color.Yellow;
            fsize = cv.MeasureString(pricestr, Constants.Font_QuoteInfo_FieldPrice);
            cv.DrawString(pricestr, Constants.Font_QuoteInfo_FieldPrice, _brush, this.Width, locatioin.Height + 3, rformat);

            locatioin.Height += (fsize.Height + 3);

            //line2 涨跌 昨结
            _brush.Color = Constants.ColorLabel;
            cv.DrawString(CHANGE, Constants.Font_QuoteInfo_FieldTitle, _brush, 0, locatioin.Height + 3);

            if (_symbol != null)
            {
                pricestr = string.Format(_priceFormat, _symbol.TickSnapshot.Price - _symbol.GetYdPrice());
                color = GetColor(_symbol.TickSnapshot.Price - _symbol.GetYdPrice());
            }
            _brush.Color = color;
            fsize = cv.MeasureString(pricestr, Constants.Font_QuoteInfo_FieldPrice);
            cv.DrawString(pricestr, Constants.Font_QuoteInfo_FieldPrice, _brush, this.Width / 2, locatioin.Height + 3, rformat);

            _brush.Color = Constants.ColorLabel;
            cv.DrawString(PRESETTLE, Constants.Font_QuoteInfo_FieldTitle, _brush, this.Width / 2, locatioin.Height + 3);

            if (_symbol != null)
            {
                pricestr = string.Format(_priceFormat, _symbol.GetYdPrice());
            }
            _brush.Color = Color.Silver;
            fsize = cv.MeasureString(pricestr, Constants.Font_QuoteInfo_FieldPrice);
            cv.DrawString(pricestr, Constants.Font_QuoteInfo_FieldPrice, _brush, this.Width, locatioin.Height + 3, rformat);

            locatioin.Height += (fsize.Height + 3);

            //line3 幅度 开盘
            _brush.Color = Constants.ColorLabel;
            cv.DrawString(CHANGEPECT, Constants.Font_QuoteInfo_FieldTitle, _brush, 0, locatioin.Height + 3);

            if (_symbol != null)
            {
                pricestr = string.Format("{0:F2}%", 100*(_symbol.TickSnapshot.Price - _symbol.GetYdPrice()) / _symbol.GetYdPrice());
                color = GetPriceColor(_symbol.TickSnapshot.Price);
            }
            else
            {
                pricestr = "-0.20%";
            }
            _brush.Color = color;
            fsize = cv.MeasureString(pricestr, Constants.Font_QuoteInfo_FieldPrice);
            cv.DrawString(pricestr, Constants.Font_QuoteInfo_FieldPrice, _brush, this.Width / 2, locatioin.Height + 3, rformat);

            _brush.Color = Constants.ColorLabel;
            cv.DrawString(OPEN, Constants.Font_QuoteInfo_FieldTitle, _brush, this.Width / 2, locatioin.Height + 3);

            if (_symbol != null)
            {
                pricestr = string.Format(_priceFormat, _symbol.TickSnapshot.Open);
                color = GetPriceColor(_symbol.TickSnapshot.Open);
            }
            else
            {
                pricestr = "50.85";
            }
            _brush.Color = Color.Silver;
            fsize = cv.MeasureString(pricestr, Constants.Font_QuoteInfo_FieldPrice);
            cv.DrawString(pricestr, Constants.Font_QuoteInfo_FieldPrice, _brush, this.Width, locatioin.Height + 3, rformat);

            locatioin.Height += (fsize.Height + 3);


            //line4 总手 最高
            _brush.Color = Constants.ColorLabel;
            cv.DrawString(VOL, Constants.Font_QuoteInfo_FieldTitle, _brush, 0, locatioin.Height + 3);

            if (_symbol != null)
            {
                pricestr = ((int)_symbol.TickSnapshot.Volume).ToString();
                color = GetPriceColor(_symbol.TickSnapshot.Price);
            }
            else
            {
                pricestr = "333333";
            }
            _brush.Color = Color.Yellow;
            fsize = cv.MeasureString(pricestr, Constants.Font_QuoteInfo_FieldPrice);
            cv.DrawString(pricestr, Constants.Font_QuoteInfo_FieldPrice, _brush, this.Width / 2, locatioin.Height + 3, rformat);

            _brush.Color = Constants.ColorLabel;
            cv.DrawString(HIGH, Constants.Font_QuoteInfo_FieldTitle, _brush, this.Width / 2, locatioin.Height + 3);

            if (_symbol != null)
            {
                pricestr = string.Format(_priceFormat, _symbol.TickSnapshot.High);
                color = GetPriceColor(_symbol.TickSnapshot.High);
            }
            else
            {
                pricestr = "52.85";
            }
            _brush.Color = color;
            fsize = cv.MeasureString(pricestr, Constants.Font_QuoteInfo_FieldPrice);
            cv.DrawString(pricestr, Constants.Font_QuoteInfo_FieldPrice, _brush, this.Width, locatioin.Height + 3, rformat);

            locatioin.Height += (fsize.Height + 3);


            //line5 持仓 最低
            _brush.Color = Constants.ColorLabel;
            cv.DrawString(PREOI, Constants.Font_QuoteInfo_FieldTitle, _brush, 0, locatioin.Height + 3);

            if (_symbol != null)
            {
                pricestr = ((int)_symbol.TickSnapshot.PreOI).ToString();
            }
            else
            {
                pricestr = "3423";
            }
            _brush.Color = Color.Yellow;
            fsize = cv.MeasureString(pricestr, Constants.Font_QuoteInfo_FieldPrice);
            cv.DrawString(pricestr, Constants.Font_QuoteInfo_FieldPrice, _brush, this.Width / 2, locatioin.Height + 3, rformat);

            _brush.Color = Constants.ColorLabel;
            cv.DrawString(LOW, Constants.Font_QuoteInfo_FieldTitle, _brush, this.Width / 2, locatioin.Height + 3);

            if (_symbol != null)
            {
                pricestr = string.Format(_priceFormat, _symbol.TickSnapshot.Low);
                color = GetPriceColor(_symbol.TickSnapshot.Low);
            }
            else
            {
                pricestr = "49.85";
            }
            _brush.Color = color;
            fsize = cv.MeasureString(pricestr, Constants.Font_QuoteInfo_FieldPrice);
            cv.DrawString(pricestr, Constants.Font_QuoteInfo_FieldPrice, _brush, this.Width, locatioin.Height + 3, rformat);

            locatioin.Height += (fsize.Height + 5);

            pen.Color = Constants.Color_TableLine;
            pen.Width = 1;
            cv.DrawLine(pen, 0, locatioin.Height, this.Width, locatioin.Height);
        }


    }
}
