using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TradingLib.XTrader.Control
{
    public partial class ctrlTraderLogin : UserControl
    {
        public ctrlTraderLogin()
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.SizeChanged += new EventHandler(ctrlTraderLogin_SizeChanged);
            //this.Paint += new PaintEventHandler(ctrlTraderLogin_Paint);
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
            //this.Update();
            //this.Resize += new EventHandler(ctrlTraderLogin_Resize);
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
