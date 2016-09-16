using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Common.Logging;
using TradingLib.KryptonControl;

namespace XTraderLite
{
    public partial class MainForm : ComponentFactory.Krypton.Toolkit.KryptonForm
    {
        ILog logger = LogManager.GetLogger("MainForm");

        private const int CS_DropSHADOW = 0x20000;
        private const int GCL_STYLE = (-26);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SetClassLong(IntPtr hwnd, int nIndex, int dwNewLong);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int GetClassLong(IntPtr hwnd, int nIndex);


        DataAPI.TDX.TDXDataAPI _dataAPI = null;
        public MainForm()
        {
            InitializeComponent();
            //将控件日志输出时间绑定到debug函数 用于输出到控件
            ControlLogFactoryAdapter.SendDebugEvent += new Action<string>(debug);


            SetClassLong(this.Handle, GCL_STYLE, GetClassLong(this.Handle, GCL_STYLE) | CS_DropSHADOW);
            WireEvent();

            InitControls();

            InitQuoteList();

            InitDataAPI();
        }

       

        void InitControls()
        {
            splitContainer.Panel2Collapsed = true;
            debugControl1.Dock = DockStyle.Fill;

        }
        void WireEvent()
        {

            btnClose.Click += new EventHandler(btnClose_Click);
            btnMax.Click += new EventHandler(btnMax_Click);
            btnMin.Click += new EventHandler(btnMin_Click);

            btnDemo3.Click += new EventHandler(btnDemo3_Click);
            btnDemo2.Click += new EventHandler(btnDemo2_Click);
            btnDemo1.Click += new EventHandler(btnDemo1_Click);

            topMenuPanel.MouseDown += new MouseEventHandler(TopMenuPanel_MouseDown);
            topMenuPanel.MouseUp += new MouseEventHandler(TopMenuPanel_MouseUp);
            topMenuPanel.MouseMove += new MouseEventHandler(TopMenuPanel_MouseMove);
            topMenuPanel.DoubleClick += new EventHandler(TopMenuPanel_DoubleClick);

            menuTrading.Click += new EventHandler(menuTrading_Click);

            splitContainer.SplitterMoved += new SplitterEventHandler(splitContainer_SplitterMoved);
            this.SizeChanged += new EventHandler(MainForm_SizeChanged);
        }

        

        void debug(string msg)
        {
            debugControl1.GotDebug(msg);
        }

        void MainForm_SizeChanged(object sender, EventArgs e)
        {
            //调节交易面板为最小值 如果移动的splitter则设置为当前值
            if (!_splitterMoved)
            {
                splitContainer.SplitterDistance = this.Height;
            }
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

            ControlPaint.DrawBorder(e.Graphics, ClientRectangle, Color.Gray, ButtonBorderStyle.Solid);
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
