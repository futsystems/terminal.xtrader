namespace XTraderPro
{
    partial class frmMain
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartClient = new Nevron.UI.WinForm.Controls.NButton();
            this.symbol = new Nevron.UI.WinForm.Controls.NTextBox();
            this.maxCount = new Nevron.UI.WinForm.Controls.NNumericUpDown();
            this.fromEnd = new Nevron.UI.WinForm.Controls.NCheckBox();
            this.interval = new Nevron.UI.WinForm.Controls.NNumericUpDown();
            this.start = new Nevron.UI.WinForm.Controls.NDateTimePicker();
            this.end = new Nevron.UI.WinForm.Controls.NDateTimePicker();
            this.btnQryBar = new Nevron.UI.WinForm.Controls.NButton();
            this.btnResetChart = new Nevron.UI.WinForm.Controls.NButton();
            this.btnScrollLeft = new Nevron.UI.WinForm.Controls.NButton();
            this.nScrollRight = new Nevron.UI.WinForm.Controls.NButton();
            this.btnZoomIn = new Nevron.UI.WinForm.Controls.NButton();
            this.btnZoomOut = new Nevron.UI.WinForm.Controls.NButton();
            this.ctlChart1 = new TradingLib.TraderControl.ctlChart();
            ((System.ComponentModel.ISupportInitialize)(this.maxCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.interval)).BeginInit();
            this.SuspendLayout();
            // 
            // btnStartClient
            // 
            this.btnStartClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStartClient.Location = new System.Drawing.Point(611, 12);
            this.btnStartClient.Name = "btnStartClient";
            this.btnStartClient.Size = new System.Drawing.Size(75, 23);
            this.btnStartClient.TabIndex = 1;
            this.btnStartClient.Text = "StartClient";
            // 
            // symbol
            // 
            this.symbol.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.symbol.Location = new System.Drawing.Point(611, 52);
            this.symbol.Name = "symbol";
            this.symbol.Size = new System.Drawing.Size(64, 19);
            this.symbol.TabIndex = 3;
            this.symbol.Text = "IF1511";
            // 
            // maxCount
            // 
            this.maxCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.maxCount.Location = new System.Drawing.Point(739, 52);
            this.maxCount.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.maxCount.Name = "maxCount";
            this.maxCount.Size = new System.Drawing.Size(60, 20);
            this.maxCount.TabIndex = 4;
            this.maxCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // fromEnd
            // 
            this.fromEnd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.fromEnd.AutoSize = true;
            this.fromEnd.ButtonProperties.BorderOffset = 2;
            this.fromEnd.Location = new System.Drawing.Point(805, 53);
            this.fromEnd.Name = "fromEnd";
            this.fromEnd.Size = new System.Drawing.Size(66, 16);
            this.fromEnd.TabIndex = 5;
            this.fromEnd.Text = "FromEnd";
            // 
            // interval
            // 
            this.interval.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.interval.Location = new System.Drawing.Point(681, 52);
            this.interval.Name = "interval";
            this.interval.Size = new System.Drawing.Size(40, 20);
            this.interval.TabIndex = 6;
            this.interval.Value = new decimal(new int[] {
            60,
            0,
            0,
            0});
            // 
            // start
            // 
            this.start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.start.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.start.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.start.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.start.CalendarTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.start.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.start.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.start.Location = new System.Drawing.Point(611, 79);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(188, 22);
            this.start.TabIndex = 7;
            // 
            // end
            // 
            this.end.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.end.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this.end.CalendarForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.end.CalendarMonthBackground = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(235)))), ((int)(((byte)(235)))));
            this.end.CalendarTitleBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(76)))));
            this.end.CalendarTitleForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(242)))), ((int)(((byte)(242)))), ((int)(((byte)(242)))));
            this.end.CalendarTrailingForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(153)))), ((int)(((byte)(153)))), ((int)(((byte)(153)))));
            this.end.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.end.Location = new System.Drawing.Point(611, 107);
            this.end.Name = "end";
            this.end.Size = new System.Drawing.Size(188, 22);
            this.end.TabIndex = 8;
            // 
            // btnQryBar
            // 
            this.btnQryBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQryBar.Location = new System.Drawing.Point(805, 79);
            this.btnQryBar.Name = "btnQryBar";
            this.btnQryBar.Size = new System.Drawing.Size(75, 42);
            this.btnQryBar.TabIndex = 9;
            this.btnQryBar.Text = "QryBar";
            // 
            // btnResetChart
            // 
            this.btnResetChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnResetChart.Location = new System.Drawing.Point(611, 159);
            this.btnResetChart.Name = "btnResetChart";
            this.btnResetChart.Size = new System.Drawing.Size(75, 23);
            this.btnResetChart.TabIndex = 11;
            this.btnResetChart.Text = "ResetChart";
            this.btnResetChart.Click += new System.EventHandler(this.btnResetChart_Click);
            // 
            // btnScrollLeft
            // 
            this.btnScrollLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnScrollLeft.Location = new System.Drawing.Point(611, 198);
            this.btnScrollLeft.Name = "btnScrollLeft";
            this.btnScrollLeft.Size = new System.Drawing.Size(75, 23);
            this.btnScrollLeft.TabIndex = 12;
            this.btnScrollLeft.Text = "ScrollLeft";
            this.btnScrollLeft.Click += new System.EventHandler(this.btnScrollLeft_Click);
            // 
            // nScrollRight
            // 
            this.nScrollRight.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nScrollRight.Location = new System.Drawing.Point(701, 198);
            this.nScrollRight.Name = "nScrollRight";
            this.nScrollRight.Size = new System.Drawing.Size(75, 23);
            this.nScrollRight.TabIndex = 13;
            this.nScrollRight.Text = "ScrollRight";
            this.nScrollRight.Click += new System.EventHandler(this.nScrollRight_Click);
            // 
            // btnZoomIn
            // 
            this.btnZoomIn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomIn.Location = new System.Drawing.Point(611, 227);
            this.btnZoomIn.Name = "btnZoomIn";
            this.btnZoomIn.Size = new System.Drawing.Size(75, 23);
            this.btnZoomIn.TabIndex = 14;
            this.btnZoomIn.Text = "ZoomIn";
            this.btnZoomIn.Click += new System.EventHandler(this.btnZoomIn_Click);
            // 
            // btnZoomOut
            // 
            this.btnZoomOut.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZoomOut.Location = new System.Drawing.Point(701, 227);
            this.btnZoomOut.Name = "btnZoomOut";
            this.btnZoomOut.Size = new System.Drawing.Size(75, 23);
            this.btnZoomOut.TabIndex = 15;
            this.btnZoomOut.Text = "ZoomOut";
            this.btnZoomOut.Click += new System.EventHandler(this.btnZoomOut_Click);
            // 
            // ctlChart1
            // 
            this.ctlChart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctlChart1.ColorDownBody = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ctlChart1.ColorDownBox = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ctlChart1.ColorSeperator = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(60)))), ((int)(((byte)(57)))));
            this.ctlChart1.ColorUpBody = System.Drawing.Color.Black;
            this.ctlChart1.ColorUpBox = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(60)))), ((int)(((byte)(57)))));
            this.ctlChart1.DefaultVisibleRecordCount = 150;
            this.ctlChart1.Location = new System.Drawing.Point(12, 0);
            this.ctlChart1.Name = "ctlChart1";
            this.ctlChart1.RightDrawingSpace = 10;
            this.ctlChart1.ScaleAlignment = TradingLib.TraderControl.EnumScaleAlignment.Left;
            this.ctlChart1.ScaleDecimalPlace = 2;
            this.ctlChart1.ScaleType = TradingLib.TraderControl.EnumScaleType.Linear;
            this.ctlChart1.ShowCrossHairs = false;
            this.ctlChart1.ShowPanelSeperator = true;
            this.ctlChart1.ShowTitle = true;
            this.ctlChart1.ShowXGrid = false;
            this.ctlChart1.ShowYGrid = false;
            this.ctlChart1.Size = new System.Drawing.Size(595, 430);
            this.ctlChart1.TabIndex = 16;
            this.ctlChart1.ThreeD = false;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(888, 426);
            this.Controls.Add(this.ctlChart1);
            this.Controls.Add(this.btnZoomOut);
            this.Controls.Add(this.btnZoomIn);
            this.Controls.Add(this.nScrollRight);
            this.Controls.Add(this.btnScrollLeft);
            this.Controls.Add(this.btnResetChart);
            this.Controls.Add(this.btnQryBar);
            this.Controls.Add(this.end);
            this.Controls.Add(this.start);
            this.Controls.Add(this.interval);
            this.Controls.Add(this.fromEnd);
            this.Controls.Add(this.maxCount);
            this.Controls.Add(this.symbol);
            this.Controls.Add(this.btnStartClient);
            this.Name = "frmMain";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.maxCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.interval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Nevron.UI.WinForm.Controls.NButton btnStartClient;
        private Nevron.UI.WinForm.Controls.NTextBox symbol;
        private Nevron.UI.WinForm.Controls.NNumericUpDown maxCount;
        private Nevron.UI.WinForm.Controls.NCheckBox fromEnd;
        private Nevron.UI.WinForm.Controls.NNumericUpDown interval;
        private Nevron.UI.WinForm.Controls.NDateTimePicker start;
        private Nevron.UI.WinForm.Controls.NDateTimePicker end;
        private Nevron.UI.WinForm.Controls.NButton btnQryBar;
        private Nevron.UI.WinForm.Controls.NButton btnResetChart;
        private Nevron.UI.WinForm.Controls.NButton btnScrollLeft;
        private Nevron.UI.WinForm.Controls.NButton nScrollRight;
        private Nevron.UI.WinForm.Controls.NButton btnZoomIn;
        private Nevron.UI.WinForm.Controls.NButton btnZoomOut;
        private TradingLib.TraderControl.ctlChart ctlChart1;

    }
}

