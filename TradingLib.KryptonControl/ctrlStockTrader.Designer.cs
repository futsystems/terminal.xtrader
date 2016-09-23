namespace TradingLib.KryptonControl
{
    partial class ctrlStockTrader
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

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctrlStockTrader));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.menuTree = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.topPanel = new System.Windows.Forms.Panel();
            this.cbAccount = new System.Windows.Forms.ComboBox();
            this.panelControlBox = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.btnMax = new System.Windows.Forms.PictureBox();
            this.btnMin = new System.Windows.Forms.PictureBox();
            this.btnPosition = new System.Windows.Forms.CheckBox();
            this.btnTodayTrade = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.CheckBox();
            this.btnSell = new System.Windows.Forms.CheckBox();
            this.btnBuy = new System.Windows.Forms.CheckBox();
            this.mainPanel = new TradingLib.KryptonControl.FPanel();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.topPanel.SuspendLayout();
            this.panelControlBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMin)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "toolbar_exit.png");
            this.imageList1.Images.SetKeyName(1, "01_buy.png");
            this.imageList1.Images.SetKeyName(2, "02_sell.png");
            this.imageList1.Images.SetKeyName(3, "03_cancel.png");
            this.imageList1.Images.SetKeyName(4, "04_buysell.png");
            this.imageList1.Images.SetKeyName(5, "05_qry.png");
            this.imageList1.Images.SetKeyName(6, "06_qry_item.png");
            this.imageList1.Images.SetKeyName(7, "07_changepass.png");
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.menuTree);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(200, 307);
            this.panel1.TabIndex = 0;
            // 
            // menuTree
            // 
            this.menuTree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.menuTree.Location = new System.Drawing.Point(0, 0);
            this.menuTree.Name = "menuTree";
            this.menuTree.Size = new System.Drawing.Size(200, 307);
            this.menuTree.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(200, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 307);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // topPanel
            // 
            this.topPanel.BackgroundImage = global::TradingLib.KryptonControl.Properties.Resources.topbarbg;
            this.topPanel.Controls.Add(this.btnRefresh);
            this.topPanel.Controls.Add(this.cbAccount);
            this.topPanel.Controls.Add(this.panelControlBox);
            this.topPanel.Controls.Add(this.btnPosition);
            this.topPanel.Controls.Add(this.btnTodayTrade);
            this.topPanel.Controls.Add(this.btnCancel);
            this.topPanel.Controls.Add(this.btnSell);
            this.topPanel.Controls.Add(this.btnBuy);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(203, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(803, 30);
            this.topPanel.TabIndex = 2;
            // 
            // cbAccount
            // 
            this.cbAccount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cbAccount.FormattingEnabled = true;
            this.cbAccount.Location = new System.Drawing.Point(598, 4);
            this.cbAccount.Name = "cbAccount";
            this.cbAccount.Size = new System.Drawing.Size(142, 20);
            this.cbAccount.TabIndex = 7;
            // 
            // panelControlBox
            // 
            this.panelControlBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panelControlBox.BackColor = System.Drawing.Color.Transparent;
            this.panelControlBox.Controls.Add(this.btnClose);
            this.panelControlBox.Controls.Add(this.btnMax);
            this.panelControlBox.Controls.Add(this.btnMin);
            this.panelControlBox.Location = new System.Drawing.Point(746, 6);
            this.panelControlBox.Name = "panelControlBox";
            this.panelControlBox.Size = new System.Drawing.Size(54, 18);
            this.panelControlBox.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.BackgroundImage = global::TradingLib.KryptonControl.Properties.Resources.close;
            this.btnClose.Location = new System.Drawing.Point(36, 0);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(18, 18);
            this.btnClose.TabIndex = 2;
            this.btnClose.TabStop = false;
            // 
            // btnMax
            // 
            this.btnMax.BackgroundImage = global::TradingLib.KryptonControl.Properties.Resources.max;
            this.btnMax.Location = new System.Drawing.Point(18, 0);
            this.btnMax.Name = "btnMax";
            this.btnMax.Size = new System.Drawing.Size(18, 18);
            this.btnMax.TabIndex = 1;
            this.btnMax.TabStop = false;
            // 
            // btnMin
            // 
            this.btnMin.BackgroundImage = global::TradingLib.KryptonControl.Properties.Resources.min;
            this.btnMin.Location = new System.Drawing.Point(0, 0);
            this.btnMin.Name = "btnMin";
            this.btnMin.Size = new System.Drawing.Size(18, 18);
            this.btnMin.TabIndex = 0;
            this.btnMin.TabStop = false;
            // 
            // btnPosition
            // 
            this.btnPosition.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnPosition.Location = new System.Drawing.Point(186, 3);
            this.btnPosition.Name = "btnPosition";
            this.btnPosition.Size = new System.Drawing.Size(39, 24);
            this.btnPosition.TabIndex = 5;
            this.btnPosition.Text = "持仓";
            this.btnPosition.UseVisualStyleBackColor = true;
            // 
            // btnTodayTrade
            // 
            this.btnTodayTrade.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnTodayTrade.Location = new System.Drawing.Point(141, 3);
            this.btnTodayTrade.Name = "btnTodayTrade";
            this.btnTodayTrade.Size = new System.Drawing.Size(39, 24);
            this.btnTodayTrade.TabIndex = 4;
            this.btnTodayTrade.Text = "成交";
            this.btnTodayTrade.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnCancel.Location = new System.Drawing.Point(96, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(39, 24);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "撤单";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSell
            // 
            this.btnSell.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnSell.Location = new System.Drawing.Point(51, 3);
            this.btnSell.Name = "btnSell";
            this.btnSell.Size = new System.Drawing.Size(39, 24);
            this.btnSell.TabIndex = 2;
            this.btnSell.Text = "卖出";
            this.btnSell.UseVisualStyleBackColor = true;
            // 
            // btnBuy
            // 
            this.btnBuy.Appearance = System.Windows.Forms.Appearance.Button;
            this.btnBuy.Location = new System.Drawing.Point(6, 3);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.Size = new System.Drawing.Size(39, 24);
            this.btnBuy.TabIndex = 1;
            this.btnBuy.Text = "买入";
            this.btnBuy.UseVisualStyleBackColor = true;
            // 
            // mainPanel
            // 
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(203, 30);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(803, 277);
            this.mainPanel.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(231, 3);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(39, 24);
            this.btnRefresh.TabIndex = 8;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // ctrlStockTrader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.topPanel);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Name = "ctrlStockTrader";
            this.Size = new System.Drawing.Size(1006, 307);
            this.panel1.ResumeLayout(false);
            this.topPanel.ResumeLayout(false);
            this.panelControlBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMin)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.TreeView menuTree;
        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.CheckBox btnBuy;
        private System.Windows.Forms.CheckBox btnSell;
        private System.Windows.Forms.CheckBox btnCancel;
        private System.Windows.Forms.CheckBox btnTodayTrade;
        private System.Windows.Forms.CheckBox btnPosition;
        private System.Windows.Forms.Panel panelControlBox;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.PictureBox btnMax;
        private System.Windows.Forms.PictureBox btnMin;
        private System.Windows.Forms.ComboBox cbAccount;
        private FPanel mainPanel;
        private System.Windows.Forms.Button btnRefresh;
    }
}
