namespace CStock
{
    partial class TStock
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>


        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TStock));
            this.StockMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.Hint = new System.Windows.Forms.Label();
            this.DataHint = new System.Windows.Forms.Panel();
            this.phint = new System.Windows.Forms.Panel();
            this.Board = new System.Windows.Forms.Panel();
            this.ctDetailsBoard1 = new CStock.ctDetailsBoard();
            this.SP1 = new System.Windows.Forms.Splitter();
            this.ofd = new System.Windows.Forms.OpenFileDialog();
            this.sfd = new System.Windows.Forms.SaveFileDialog();
            this.DrawBoard = new System.Windows.Forms.Panel();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton16 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton18 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton19 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton20 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton21 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton22 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton23 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton24 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton25 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton26 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton27 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton28 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton29 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.bt1 = new System.Windows.Forms.ToolStripButton();
            this.bt2 = new System.Windows.Forms.ToolStripButton();
            this.bt3 = new System.Windows.Forms.ToolStripButton();
            this.bt4 = new System.Windows.Forms.ToolStripButton();
            this.bt5 = new System.Windows.Forms.ToolStripButton();
            this.bt6 = new System.Windows.Forms.ToolStripButton();
            this.bt7 = new System.Windows.Forms.ToolStripButton();
            this.bt8 = new System.Windows.Forms.ToolStripButton();
            this.bt9 = new System.Windows.Forms.ToolStripButton();
            this.bt10 = new System.Windows.Forms.ToolStripButton();
            this.bt11 = new System.Windows.Forms.ToolStripButton();
            this.bt12 = new System.Windows.Forms.ToolStripButton();
            this.bt13 = new System.Windows.Forms.ToolStripButton();
            this.bt14 = new System.Windows.Forms.ToolStripButton();
            this.Tab = new System.Windows.Forms.PictureBox();
            this.debugBox = new System.Windows.Forms.Panel();
            this.DataHint.SuspendLayout();
            this.Board.SuspendLayout();
            this.DrawBoard.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tab)).BeginInit();
            this.SuspendLayout();
            // 
            // StockMenu
            // 
            this.StockMenu.Name = "StockMenu";
            this.StockMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.StockMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // Hint
            // 
            this.Hint.AutoSize = true;
            this.Hint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Hint.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Hint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Hint.Location = new System.Drawing.Point(159, 19);
            this.Hint.Name = "Hint";
            this.Hint.Size = new System.Drawing.Size(79, 14);
            this.Hint.TabIndex = 4;
            this.Hint.Text = "信息地雷提示";
            this.Hint.Visible = false;
            // 
            // DataHint
            // 
            this.DataHint.BackColor = System.Drawing.Color.White;
            this.DataHint.Controls.Add(this.phint);
            this.DataHint.Location = new System.Drawing.Point(75, 18);
            this.DataHint.Name = "DataHint";
            this.DataHint.Padding = new System.Windows.Forms.Padding(1);
            this.DataHint.Size = new System.Drawing.Size(78, 95);
            this.DataHint.TabIndex = 7;
            this.DataHint.Visible = false;
            // 
            // phint
            // 
            this.phint.BackColor = System.Drawing.Color.Black;
            this.phint.Dock = System.Windows.Forms.DockStyle.Fill;
            this.phint.Location = new System.Drawing.Point(1, 1);
            this.phint.Name = "phint";
            this.phint.Size = new System.Drawing.Size(76, 93);
            this.phint.TabIndex = 0;
            this.phint.MouseDown += new System.Windows.Forms.MouseEventHandler(this.phint_MouseDown);
            this.phint.MouseMove += new System.Windows.Forms.MouseEventHandler(this.phint_MouseMove);
            this.phint.MouseUp += new System.Windows.Forms.MouseEventHandler(this.phint_MouseUp);
            // 
            // Board
            // 
            this.Board.BackColor = System.Drawing.Color.Black;
            this.Board.Controls.Add(this.ctDetailsBoard1);
            this.Board.Dock = System.Windows.Forms.DockStyle.Right;
            this.Board.Location = new System.Drawing.Point(596, 0);
            this.Board.MinimumSize = new System.Drawing.Size(160, 0);
            this.Board.Name = "Board";
            this.Board.Size = new System.Drawing.Size(246, 660);
            this.Board.TabIndex = 8;
            this.Board.Resize += new System.EventHandler(this.Board_Resize);
            // 
            // ctDetailsBoard1
            // 
            this.ctDetailsBoard1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctDetailsBoard1.Location = new System.Drawing.Point(0, 0);
            this.ctDetailsBoard1.Name = "ctDetailsBoard1";
            this.ctDetailsBoard1.Size = new System.Drawing.Size(246, 660);
            this.ctDetailsBoard1.StockLabel = "00001 深发展A";
            this.ctDetailsBoard1.TabIndex = 25;
            this.ctDetailsBoard1.TabStop = false;
            // 
            // SP1
            // 
            this.SP1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.SP1.Dock = System.Windows.Forms.DockStyle.Right;
            this.SP1.Location = new System.Drawing.Point(595, 0);
            this.SP1.Name = "SP1";
            this.SP1.Size = new System.Drawing.Size(1, 660);
            this.SP1.TabIndex = 19;
            this.SP1.TabStop = false;
            // 
            // ofd
            // 
            this.ofd.FileName = "*.txt";
            // 
            // DrawBoard
            // 
            this.DrawBoard.AutoSize = true;
            this.DrawBoard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.DrawBoard.Controls.Add(this.toolStrip2);
            this.DrawBoard.Controls.Add(this.toolStrip1);
            this.DrawBoard.Dock = System.Windows.Forms.DockStyle.Left;
            this.DrawBoard.Location = new System.Drawing.Point(0, 0);
            this.DrawBoard.Name = "DrawBoard";
            this.DrawBoard.Size = new System.Drawing.Size(46, 660);
            this.DrawBoard.TabIndex = 21;
            this.DrawBoard.Visible = false;
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip2.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStrip2.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton16,
            this.toolStripButton18,
            this.toolStripButton19,
            this.toolStripButton20,
            this.toolStripButton21,
            this.toolStripButton22,
            this.toolStripButton23,
            this.toolStripButton24,
            this.toolStripButton25,
            this.toolStripButton26,
            this.toolStripButton27,
            this.toolStripButton28,
            this.toolStripButton29,
            this.toolStripButton1,
            this.toolStripButton2});
            this.toolStrip2.Location = new System.Drawing.Point(23, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip2.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip2.Size = new System.Drawing.Size(23, 660);
            this.toolStrip2.TabIndex = 1;
            // 
            // toolStripButton16
            // 
            this.toolStripButton16.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton16.Image = global::TradingLib.XTrader.Properties.Resources.K15;
            this.toolStripButton16.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton16.Name = "toolStripButton16";
            this.toolStripButton16.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton16.Tag = "14";
            this.toolStripButton16.Text = "toolStripButton1";
            this.toolStripButton16.ToolTipText = "线形回归线";
            this.toolStripButton16.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton18
            // 
            this.toolStripButton18.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton18.Image = global::TradingLib.XTrader.Properties.Resources.K16;
            this.toolStripButton18.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton18.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton18.Name = "toolStripButton18";
            this.toolStripButton18.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton18.Tag = "15";
            this.toolStripButton18.Text = "toolStripButton3";
            this.toolStripButton18.ToolTipText = "周期线";
            this.toolStripButton18.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton19
            // 
            this.toolStripButton19.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton19.Image = global::TradingLib.XTrader.Properties.Resources.K17;
            this.toolStripButton19.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton19.Name = "toolStripButton19";
            this.toolStripButton19.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton19.Tag = "16";
            this.toolStripButton19.Text = "toolStripButton4";
            this.toolStripButton19.ToolTipText = "周期线";
            this.toolStripButton19.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton20
            // 
            this.toolStripButton20.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton20.Image = global::TradingLib.XTrader.Properties.Resources.K18;
            this.toolStripButton20.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton20.Name = "toolStripButton20";
            this.toolStripButton20.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton20.Tag = "17";
            this.toolStripButton20.Text = "toolStripButton5";
            this.toolStripButton20.ToolTipText = "费波拉契线";
            this.toolStripButton20.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton21
            // 
            this.toolStripButton21.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton21.Image = global::TradingLib.XTrader.Properties.Resources.K19;
            this.toolStripButton21.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton21.Name = "toolStripButton21";
            this.toolStripButton21.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton21.Tag = "18";
            this.toolStripButton21.Text = "toolStripButton6";
            this.toolStripButton21.ToolTipText = "江恩时间序列";
            this.toolStripButton21.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton22
            // 
            this.toolStripButton22.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton22.Image = global::TradingLib.XTrader.Properties.Resources.K20;
            this.toolStripButton22.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton22.Name = "toolStripButton22";
            this.toolStripButton22.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton22.Tag = "19";
            this.toolStripButton22.Text = "toolStripButton7";
            this.toolStripButton22.ToolTipText = "阻速线";
            this.toolStripButton22.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton23
            // 
            this.toolStripButton23.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton23.Image = global::TradingLib.XTrader.Properties.Resources.K21;
            this.toolStripButton23.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton23.Name = "toolStripButton23";
            this.toolStripButton23.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton23.Tag = "20";
            this.toolStripButton23.Text = "toolStripButton8";
            this.toolStripButton23.ToolTipText = "江恩角度线";
            this.toolStripButton23.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton24
            // 
            this.toolStripButton24.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton24.Image = global::TradingLib.XTrader.Properties.Resources.K22;
            this.toolStripButton24.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton24.Name = "toolStripButton24";
            this.toolStripButton24.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton24.Tag = "21";
            this.toolStripButton24.Text = "toolStripButton9";
            this.toolStripButton24.ToolTipText = "矩形";
            this.toolStripButton24.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton25
            // 
            this.toolStripButton25.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton25.Image = global::TradingLib.XTrader.Properties.Resources.k23;
            this.toolStripButton25.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton25.Name = "toolStripButton25";
            this.toolStripButton25.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton25.Tag = "22";
            this.toolStripButton25.Text = "toolStripButton10";
            this.toolStripButton25.ToolTipText = "涨标记";
            this.toolStripButton25.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton26
            // 
            this.toolStripButton26.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton26.Image = global::TradingLib.XTrader.Properties.Resources.k24;
            this.toolStripButton26.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton26.Name = "toolStripButton26";
            this.toolStripButton26.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton26.Tag = "23";
            this.toolStripButton26.Text = "toolStripButton11";
            this.toolStripButton26.ToolTipText = "跌标记";
            this.toolStripButton26.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton27
            // 
            this.toolStripButton27.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton27.Image = global::TradingLib.XTrader.Properties.Resources.K25;
            this.toolStripButton27.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton27.Name = "toolStripButton27";
            this.toolStripButton27.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton27.Tag = "24";
            this.toolStripButton27.Text = "toolStripButton12";
            this.toolStripButton27.ToolTipText = "文字注释";
            this.toolStripButton27.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton28
            // 
            this.toolStripButton28.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton28.Image = global::TradingLib.XTrader.Properties.Resources.K26;
            this.toolStripButton28.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton28.Name = "toolStripButton28";
            this.toolStripButton28.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton28.Tag = "25";
            this.toolStripButton28.Text = "toolStripButton13";
            this.toolStripButton28.ToolTipText = "删除画线";
            this.toolStripButton28.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton29
            // 
            this.toolStripButton29.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton29.Image = global::TradingLib.XTrader.Properties.Resources.K27;
            this.toolStripButton29.ImageTransparentColor = System.Drawing.Color.White;
            this.toolStripButton29.Name = "toolStripButton29";
            this.toolStripButton29.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton29.Tag = "26";
            this.toolStripButton29.Text = "toolStripButton14";
            this.toolStripButton29.ToolTipText = "隐藏自画线";
            this.toolStripButton29.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::TradingLib.XTrader.Properties.Resources.K28;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton1.Tag = "27";
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.ToolTipText = "画线信息浏览";
            this.toolStripButton1.Click += new System.EventHandler(this.Draw);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(22, 20);
            this.toolStripButton2.Tag = "28";
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Visible = false;
            this.toolStripButton2.Click += new System.EventHandler(this.Draw);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Left;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.toolStrip1.GripMargin = new System.Windows.Forms.Padding(0);
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.bt1,
            this.bt2,
            this.bt3,
            this.bt4,
            this.bt5,
            this.bt6,
            this.bt7,
            this.bt8,
            this.bt9,
            this.bt10,
            this.bt11,
            this.bt12,
            this.bt13,
            this.bt14});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Padding = new System.Windows.Forms.Padding(0);
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(23, 660);
            this.toolStrip1.TabIndex = 0;
            // 
            // bt1
            // 
            this.bt1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt1.Image = global::TradingLib.XTrader.Properties.Resources.K1;
            this.bt1.ImageTransparentColor = System.Drawing.Color.White;
            this.bt1.Name = "bt1";
            this.bt1.Size = new System.Drawing.Size(22, 20);
            this.bt1.Tag = "0";
            this.bt1.Text = "toolStripButton1";
            this.bt1.ToolTipText = "请选定";
            // 
            // bt2
            // 
            this.bt2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt2.Image = global::TradingLib.XTrader.Properties.Resources.K2;
            this.bt2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.bt2.ImageTransparentColor = System.Drawing.Color.White;
            this.bt2.Name = "bt2";
            this.bt2.Size = new System.Drawing.Size(22, 20);
            this.bt2.Tag = "1";
            this.bt2.Text = "toolStripButton3";
            this.bt2.ToolTipText = "线段";
            this.bt2.Click += new System.EventHandler(this.Draw);
            // 
            // bt3
            // 
            this.bt3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt3.Image = global::TradingLib.XTrader.Properties.Resources.K3;
            this.bt3.ImageTransparentColor = System.Drawing.Color.White;
            this.bt3.Name = "bt3";
            this.bt3.Size = new System.Drawing.Size(22, 20);
            this.bt3.Tag = "2";
            this.bt3.Text = "toolStripButton4";
            this.bt3.ToolTipText = "直线";
            this.bt3.Click += new System.EventHandler(this.Draw);
            // 
            // bt4
            // 
            this.bt4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt4.Image = global::TradingLib.XTrader.Properties.Resources.K4;
            this.bt4.ImageTransparentColor = System.Drawing.Color.White;
            this.bt4.Name = "bt4";
            this.bt4.Size = new System.Drawing.Size(22, 20);
            this.bt4.Tag = "3";
            this.bt4.Text = "toolStripButton5";
            this.bt4.ToolTipText = "箭头";
            this.bt4.Click += new System.EventHandler(this.Draw);
            // 
            // bt5
            // 
            this.bt5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt5.Image = global::TradingLib.XTrader.Properties.Resources.K5;
            this.bt5.ImageTransparentColor = System.Drawing.Color.White;
            this.bt5.Name = "bt5";
            this.bt5.Size = new System.Drawing.Size(22, 20);
            this.bt5.Tag = "4";
            this.bt5.Text = "toolStripButton6";
            this.bt5.ToolTipText = "射线";
            this.bt5.Click += new System.EventHandler(this.Draw);
            // 
            // bt6
            // 
            this.bt6.BackColor = System.Drawing.Color.Gray;
            this.bt6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt6.Image = global::TradingLib.XTrader.Properties.Resources.K6;
            this.bt6.ImageTransparentColor = System.Drawing.Color.Maroon;
            this.bt6.Name = "bt6";
            this.bt6.Size = new System.Drawing.Size(22, 20);
            this.bt6.Tag = "5";
            this.bt6.Text = "toolStripButton7";
            this.bt6.ToolTipText = "价格通道线";
            this.bt6.Click += new System.EventHandler(this.Draw);
            // 
            // bt7
            // 
            this.bt7.BackColor = System.Drawing.Color.Gray;
            this.bt7.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt7.Image = global::TradingLib.XTrader.Properties.Resources.K7;
            this.bt7.ImageTransparentColor = System.Drawing.Color.White;
            this.bt7.Name = "bt7";
            this.bt7.Size = new System.Drawing.Size(22, 20);
            this.bt7.Tag = "6";
            this.bt7.Text = "toolStripButton8";
            this.bt7.ToolTipText = "平行直线";
            this.bt7.Click += new System.EventHandler(this.Draw);
            // 
            // bt8
            // 
            this.bt8.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt8.Image = global::TradingLib.XTrader.Properties.Resources.K8;
            this.bt8.ImageTransparentColor = System.Drawing.Color.White;
            this.bt8.Name = "bt8";
            this.bt8.Size = new System.Drawing.Size(22, 20);
            this.bt8.Tag = "7";
            this.bt8.Text = "toolStripButton9";
            this.bt8.ToolTipText = "圆弧";
            this.bt8.Click += new System.EventHandler(this.Draw);
            // 
            // bt9
            // 
            this.bt9.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt9.Image = global::TradingLib.XTrader.Properties.Resources.K9;
            this.bt9.ImageTransparentColor = System.Drawing.Color.White;
            this.bt9.Name = "bt9";
            this.bt9.Size = new System.Drawing.Size(22, 20);
            this.bt9.Tag = "8";
            this.bt9.Text = "toolStripButton10";
            this.bt9.ToolTipText = "黄金价位线";
            this.bt9.Click += new System.EventHandler(this.Draw);
            // 
            // bt10
            // 
            this.bt10.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt10.Image = global::TradingLib.XTrader.Properties.Resources.K10;
            this.bt10.ImageTransparentColor = System.Drawing.Color.White;
            this.bt10.Name = "bt10";
            this.bt10.Size = new System.Drawing.Size(22, 20);
            this.bt10.Tag = "9";
            this.bt10.Text = "toolStripButton11";
            this.bt10.ToolTipText = "黄金目标线";
            this.bt10.Click += new System.EventHandler(this.Draw);
            // 
            // bt11
            // 
            this.bt11.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt11.Image = global::TradingLib.XTrader.Properties.Resources.K11;
            this.bt11.ImageTransparentColor = System.Drawing.Color.White;
            this.bt11.Name = "bt11";
            this.bt11.Size = new System.Drawing.Size(22, 20);
            this.bt11.Tag = "10";
            this.bt11.Text = "toolStripButton12";
            this.bt11.ToolTipText = "黄金分割";
            this.bt11.Click += new System.EventHandler(this.Draw);
            // 
            // bt12
            // 
            this.bt12.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt12.Image = global::TradingLib.XTrader.Properties.Resources.K12;
            this.bt12.ImageTransparentColor = System.Drawing.Color.White;
            this.bt12.Name = "bt12";
            this.bt12.Size = new System.Drawing.Size(22, 20);
            this.bt12.Tag = "11";
            this.bt12.Text = "toolStripButton13";
            this.bt12.ToolTipText = "百分比线";
            this.bt12.Click += new System.EventHandler(this.Draw);
            // 
            // bt13
            // 
            this.bt13.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt13.Image = global::TradingLib.XTrader.Properties.Resources.K13;
            this.bt13.ImageTransparentColor = System.Drawing.Color.White;
            this.bt13.Name = "bt13";
            this.bt13.Size = new System.Drawing.Size(22, 20);
            this.bt13.Tag = "12";
            this.bt13.Text = "toolStripButton14";
            this.bt13.ToolTipText = "波段线";
            this.bt13.Click += new System.EventHandler(this.Draw);
            // 
            // bt14
            // 
            this.bt14.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.bt14.Image = global::TradingLib.XTrader.Properties.Resources.K14;
            this.bt14.ImageTransparentColor = System.Drawing.Color.White;
            this.bt14.Name = "bt14";
            this.bt14.Size = new System.Drawing.Size(22, 20);
            this.bt14.Tag = "13";
            this.bt14.Text = "toolStripButton15";
            this.bt14.ToolTipText = "线形回归带";
            this.bt14.Click += new System.EventHandler(this.Draw);
            // 
            // Tab
            // 
            this.Tab.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Tab.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.Tab.Location = new System.Drawing.Point(46, 643);
            this.Tab.Name = "Tab";
            this.Tab.Size = new System.Drawing.Size(549, 17);
            this.Tab.TabIndex = 20;
            this.Tab.TabStop = false;
            this.Tab.Visible = false;
            this.Tab.Paint += new System.Windows.Forms.PaintEventHandler(this.Tab_Paint);
            this.Tab.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Tab_MouseClick);
            // 
            // debugBox
            // 
            this.debugBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.debugBox.BackColor = System.Drawing.Color.WhiteSmoke;
            this.debugBox.Location = new System.Drawing.Point(369, 18);
            this.debugBox.Name = "debugBox";
            this.debugBox.Size = new System.Drawing.Size(207, 159);
            this.debugBox.TabIndex = 22;
            this.debugBox.Visible = false;
            // 
            // TStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.debugBox);
            this.Controls.Add(this.Tab);
            this.Controls.Add(this.DrawBoard);
            this.Controls.Add(this.DataHint);
            this.Controls.Add(this.SP1);
            this.Controls.Add(this.Board);
            this.Controls.Add(this.Hint);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "TStock";
            this.Size = new System.Drawing.Size(842, 660);
            this.Load += new System.EventHandler(this.TStock_Load);
            this.Click += new System.EventHandler(this.TStock_Click);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TStock_Paint);
            this.DoubleClick += new System.EventHandler(this.TStock_DoubleClick);
            this.Enter += new System.EventHandler(this.TStock_Enter);
            this.Leave += new System.EventHandler(this.TStock_Leave);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.TStock_MouseDown);
            this.MouseLeave += new System.EventHandler(this.TStock_MouseLeave);
            this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.TStock_MouseMove);
            this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.TStock_MouseUp);
            this.Resize += new System.EventHandler(this.TStock_Resize);
            this.DataHint.ResumeLayout(false);
            this.Board.ResumeLayout(false);
            this.DrawBoard.ResumeLayout(false);
            this.DrawBoard.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Tab)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip StockMenu;
        private System.Windows.Forms.Label Hint;
        private System.Windows.Forms.Panel DataHint;
        private System.Windows.Forms.Panel phint;
        private System.Windows.Forms.Panel Board;
        private System.Windows.Forms.Splitter SP1;
        private System.Windows.Forms.OpenFileDialog ofd;
        private System.Windows.Forms.SaveFileDialog sfd;
        private System.Windows.Forms.PictureBox Tab;
        private System.Windows.Forms.Panel DrawBoard;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton bt1;
        private System.Windows.Forms.ToolStripButton bt2;
        private System.Windows.Forms.ToolStripButton bt3;
        private System.Windows.Forms.ToolStripButton bt4;
        private System.Windows.Forms.ToolStripButton bt5;
        private System.Windows.Forms.ToolStripButton bt6;
        private System.Windows.Forms.ToolStripButton bt7;
        private System.Windows.Forms.ToolStripButton bt8;
        private System.Windows.Forms.ToolStripButton bt9;
        private System.Windows.Forms.ToolStripButton bt10;
        private System.Windows.Forms.ToolStripButton bt11;
        private System.Windows.Forms.ToolStripButton bt12;
        private System.Windows.Forms.ToolStripButton bt13;
        private System.Windows.Forms.ToolStripButton bt14;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton toolStripButton16;
        private System.Windows.Forms.ToolStripButton toolStripButton18;
        private System.Windows.Forms.ToolStripButton toolStripButton19;
        private System.Windows.Forms.ToolStripButton toolStripButton20;
        private System.Windows.Forms.ToolStripButton toolStripButton21;
        private System.Windows.Forms.ToolStripButton toolStripButton22;
        private System.Windows.Forms.ToolStripButton toolStripButton23;
        private System.Windows.Forms.ToolStripButton toolStripButton24;
        private System.Windows.Forms.ToolStripButton toolStripButton25;
        private System.Windows.Forms.ToolStripButton toolStripButton26;
        private System.Windows.Forms.ToolStripButton toolStripButton27;
        private System.Windows.Forms.ToolStripButton toolStripButton28;
        private System.Windows.Forms.ToolStripButton toolStripButton29;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private ctDetailsBoard ctDetailsBoard1;
        private System.Windows.Forms.Panel debugBox;
    }


}