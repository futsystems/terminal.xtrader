using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace XTraderLite
{
    public partial class MainForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        private const int CS_DropSHADOW = 0x20000;
        private const int GCL_STYLE = (-26);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);

        public MainForm()
        {
            InitializeComponent();

            SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW);
            WireEvent();
        }

        void WireEvent()
        {
            btnClose.Click += new EventHandler(btnClose_Click);
            btnMax.Click += new EventHandler(btnMax_Click);
            btnMin.Click += new EventHandler(btnMin_Click);

            topMenuPanel.MouseDown += new MouseEventHandler(TopMenuPanel_MouseDown);
            topMenuPanel.MouseUp += new MouseEventHandler(TopMenuPanel_MouseUp);
            topMenuPanel.MouseMove += new MouseEventHandler(TopMenuPanel_MouseMove);
            topMenuPanel.DoubleClick += new EventHandler(TopMenuPanel_DoubleClick);
        }


        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        int WS_CAPTION = 0xC00000;
        //        int WS_BORDER = 0x800000;
        //        CreateParams CP = base.CreateParams;
        //        CP.Style &= ~WS_CAPTION | WS_BORDER;
        //        return CP;
        //    }
        //}

        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);
            //int borderWidth = 2;
            //Color borderColor = Color.Blue;
            //ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, borderColor,
            //  borderWidth, ButtonBorderStyle.Solid, borderColor, borderWidth,
            //  ButtonBorderStyle.Solid, borderColor, borderWidth, ButtonBorderStyle.Solid,
            //  borderColor, borderWidth, ButtonBorderStyle.Solid);
            //e.Graphics.DrawRectangle(Pens.Black, this.Bounds);

            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Black, ButtonBorderStyle.Solid);
        }

        private bool m_isMouseDown = false;
        private Point m_mousePos = new Point();

        void TopMenuPanel_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_isMouseDown)
            {
                Point tempPos = Cursor.Position;
                this.Location = new Point(Location.X + (tempPos.X - m_mousePos.X), Location.Y + (tempPos.Y - m_mousePos.Y));
                m_mousePos = Cursor.Position;
            }
        }

        void TopMenuPanel_MouseUp(object sender, MouseEventArgs e)
        {
            m_isMouseDown = false;
        }

        void TopMenuPanel_MouseDown(object sender, MouseEventArgs e)
        {
            m_mousePos = Cursor.Position;
            m_isMouseDown = true;

            
        }

        void TopMenuPanel_DoubleClick(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                //max11.Image = stock2.Properties.Resources.C2;

            }
            else
            {
                WindowState = FormWindowState.Normal;
                //max11.Image = stock2.Properties.Resources.C3;
            }
        }


        void btnMin_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
            //this.btnMin.Enabled = false;
        }

        void btnMax_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Maximized;
                //max11.Image = stock2.Properties.Resources.C2;

            }
            else
            {
                WindowState = FormWindowState.Normal;
                //max11.Image = stock2.Properties.Resources.C3;
            }
            
            //this.max11.Enabled = false;
            //this.max11.Enabled = true;
        }

        void btnClose_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
            Environment.Exit(0);
        }
    }
}
