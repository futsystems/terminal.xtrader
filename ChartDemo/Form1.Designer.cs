namespace ChartDemo
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.btnQryBar = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnClearData = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.ctlChart1 = new TradingLib.Chart.ctlChart();
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonPalette1 = new ComponentFactory.Krypton.Toolkit.KryptonPalette(this.components);
            this.btnUpdate = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.btnQryBar)).BeginInit();
            this.btnQryBar.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnQryBar
            // 
            this.btnQryBar.Controls.Add(this.btnUpdate);
            this.btnQryBar.Controls.Add(this.kryptonButton1);
            this.btnQryBar.Controls.Add(this.btnClearData);
            this.btnQryBar.Controls.Add(this.ctlChart1);
            this.btnQryBar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnQryBar.Location = new System.Drawing.Point(0, 0);
            this.btnQryBar.Name = "btnQryBar";
            this.btnQryBar.Size = new System.Drawing.Size(826, 389);
            this.btnQryBar.TabIndex = 0;
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonButton1.Location = new System.Drawing.Point(724, 12);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(90, 25);
            this.kryptonButton1.TabIndex = 2;
            this.kryptonButton1.Values.Text = "查询历史数据";
            this.kryptonButton1.Click += new System.EventHandler(this.kryptonButton1_Click);
            // 
            // btnClearData
            // 
            this.btnClearData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClearData.Location = new System.Drawing.Point(579, 12);
            this.btnClearData.Name = "btnClearData";
            this.btnClearData.Size = new System.Drawing.Size(90, 25);
            this.btnClearData.TabIndex = 1;
            this.btnClearData.Values.Text = "清空数据";
            this.btnClearData.Click += new System.EventHandler(this.btnClearData_Click);
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
            this.ctlChart1.Location = new System.Drawing.Point(0, 0);
            this.ctlChart1.Name = "ctlChart1";
            this.ctlChart1.RightDrawingSpace = 10;
            this.ctlChart1.ScaleAlignment = TradingLib.Chart.EnumScaleAlignment.Left;
            this.ctlChart1.ScaleDecimalPlace = 2;
            this.ctlChart1.ScaleType = TradingLib.Chart.EnumScaleType.Linear;
            this.ctlChart1.ShowCrossHairs = false;
            this.ctlChart1.ShowPanelSeperator = true;
            this.ctlChart1.ShowTitle = true;
            this.ctlChart1.ShowXGrid = false;
            this.ctlChart1.ShowYGrid = false;
            this.ctlChart1.Size = new System.Drawing.Size(573, 389);
            this.ctlChart1.Symbol = null;
            this.ctlChart1.TabIndex = 0;
            this.ctlChart1.ThreeD = true;
            // 
            // kryptonManager1
            // 
            this.kryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Office2010Silver;
            // 
            // btnUpdate
            // 
            this.btnUpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdate.Location = new System.Drawing.Point(724, 88);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(90, 25);
            this.btnUpdate.TabIndex = 3;
            this.btnUpdate.Values.Text = "Update";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(826, 389);
            this.Controls.Add(this.btnQryBar);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.btnQryBar)).EndInit();
            this.btnQryBar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel btnQryBar;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
        private ComponentFactory.Krypton.Toolkit.KryptonPalette kryptonPalette1;
        private TradingLib.Chart.ctlChart ctlChart1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnClearData;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnUpdate;
    }
}

