using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.MarketData;

namespace TradingLib.XTrader.Control
{
    public partial class ctrlTraderLogin : UserControl
    {

        public event Action ExitTrader;


        public ctrlTraderLogin()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SizeChanged += new EventHandler(ctrlTraderLogin_SizeChanged);
            //this.Paint += new PaintEventHandler(ctrlTraderLogin_Paint);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            //this.Update();
            //this.Resize += new EventHandler(ctrlTraderLogin_Resize);

            //InitControl();

            //WireEvent();
        }

        void WireEvent()
        {
            this.Load +=new EventHandler(ctrlTraderLogin_Load);

            btnLogin.Click += new EventHandler(btnLogin_Click);
            btnExit.Click += new EventHandler(btnExit_Click);


            
            //this.AcceptButton = this.btnLogin;
        }


        void ctrlTraderLogin_Load(object sender, EventArgs e)
        {
            //加载交易服务器地址
            foreach (var v in (new ServerConfig("broker.cfg")).GetServerNodes())
            {
                serverList.Items.Add(v);
            }
            if (serverList.Items.Count > 0)
            {
                serverList.SelectedIndex = 0;
            }

            //加载席位
            foreach (var v in (new ServerConfig("seat.cfg")).GetServerNodes())
            {
                seat.Items.Add(v);
            }
            if (seat.Items.Count > 0)
            {
                seat.SelectedIndex = 0;
            }
        }


        void InitControl()
        {

            encrypt.SelectedIndex = 0;
        }



        void btnExit_Click(object sender, EventArgs e)
        {
            if (ExitTrader != null)
            {
                ExitTrader();
            }
        }

        //加载交易插件并执行初始化过程
        void btnLogin_Click(object sender, EventArgs e)
        {

        }


        //Bitmap bm = null;
        //void ctrlTraderLogin_Resize(object sender, EventArgs e)
        //{
        //    Graphics g = textBox1.CreateGraphics();

        //    if (g.VisibleClipBounds.IsEmpty == false)
        //    {
        //        bm = new Bitmap((int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height);

        //        textBox1.DrawToBitmap(bm, new Rectangle(0, 0, (int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height));
        //        maskedTextBox1.DrawToBitmap(bm, new Rectangle(0, 0, (int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height));
        //        comboBox1.DrawToBitmap(bm, new Rectangle(0, 0, (int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height));
        //        maskedTextBox2.DrawToBitmap(bm, new Rectangle(0, 0, (int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height));
        //        //button1.DrawToBitmap(bm, new Rectangle(0, 0, (int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height));
        //        //button2.DrawToBitmap(bm, new Rectangle(0, 0, (int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height));
        //        //listBox1.DrawToBitmap(bm, new Rectangle(0, 0, (int)g.VisibleClipBounds.Width, (int)g.VisibleClipBounds.Height));
        //    }

        //    g.Dispose();
        //}

        //void ctrlTraderLogin_Paint(object sender, PaintEventArgs e)
        //{
        //    if (bm != null)
        //    {
        //        e.Graphics.DrawImageUnscaled(bm, 0, 0, bm.Width, bm.Height);
        //    }
        //}

        void ctrlTraderLogin_SizeChanged(object sender, EventArgs e)
        {
            holder.Location = new Point((this.Width - holder.Width) / 2, holder.Location.Y);
        }
    }
}
