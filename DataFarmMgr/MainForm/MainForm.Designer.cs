namespace TradingLib.DataFarmManager
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnConnect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDebug1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRegister = new System.Windows.Forms.ToolStripButton();
            this.btnUnregister = new System.Windows.Forms.ToolStripButton();
            this.btnDebugForm = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnStartFeedTick = new System.Windows.Forms.ToolStripButton();
            this.btnStopFeedTick = new System.Windows.Forms.ToolStripButton();
            this.mainPanel = new System.Windows.Forms.Panel();
            this.ctrlQuoteList = new TradingLib.XTrader.Control.ctrlQuoteList();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSecurity = new System.Windows.Forms.ToolStripMenuItem();
            this.menuSymbol = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.menuMarketTime = new System.Windows.Forms.ToolStripMenuItem();
            this.menuExchange = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.menuBarData = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnFunctionForm = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.mainPanel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 667);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1274, 22);
            this.statusStrip1.TabIndex = 0;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.World);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnConnect,
            this.toolStripSeparator1,
            this.btnDebug1,
            this.toolStripSeparator2,
            this.btnRegister,
            this.btnUnregister,
            this.btnDebugForm,
            this.toolStripSeparator4,
            this.btnStartFeedTick,
            this.btnStopFeedTick,
            this.btnFunctionForm});
            this.toolStrip1.Location = new System.Drawing.Point(0, 25);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1274, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnConnect
            // 
            this.btnConnect.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnConnect.Image = ((System.Drawing.Image)(resources.GetObject("btnConnect.Image")));
            this.btnConnect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(36, 22);
            this.btnConnect.Text = "连接";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDebug1
            // 
            this.btnDebug1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDebug1.Image = ((System.Drawing.Image)(resources.GetObject("btnDebug1.Image")));
            this.btnDebug1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDebug1.Name = "btnDebug1";
            this.btnDebug1.Size = new System.Drawing.Size(28, 22);
            this.btnDebug1.Text = "D1";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRegister
            // 
            this.btnRegister.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnRegister.Image = ((System.Drawing.Image)(resources.GetObject("btnRegister.Image")));
            this.btnRegister.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRegister.Name = "btnRegister";
            this.btnRegister.Size = new System.Drawing.Size(60, 22);
            this.btnRegister.Text = "订阅行情";
            // 
            // btnUnregister
            // 
            this.btnUnregister.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUnregister.Image = ((System.Drawing.Image)(resources.GetObject("btnUnregister.Image")));
            this.btnUnregister.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUnregister.Name = "btnUnregister";
            this.btnUnregister.Size = new System.Drawing.Size(84, 22);
            this.btnUnregister.Text = "取消所有订阅";
            // 
            // btnDebugForm
            // 
            this.btnDebugForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnDebugForm.Image = ((System.Drawing.Image)(resources.GetObject("btnDebugForm.Image")));
            this.btnDebugForm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDebugForm.Name = "btnDebugForm";
            this.btnDebugForm.Size = new System.Drawing.Size(60, 22);
            this.btnDebugForm.Text = "日志窗口";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnStartFeedTick
            // 
            this.btnStartFeedTick.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnStartFeedTick.Image = ((System.Drawing.Image)(resources.GetObject("btnStartFeedTick.Image")));
            this.btnStartFeedTick.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStartFeedTick.Name = "btnStartFeedTick";
            this.btnStartFeedTick.Size = new System.Drawing.Size(74, 22);
            this.btnStartFeedTick.Text = "AcceptTick";
            // 
            // btnStopFeedTick
            // 
            this.btnStopFeedTick.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnStopFeedTick.Image = ((System.Drawing.Image)(resources.GetObject("btnStopFeedTick.Image")));
            this.btnStopFeedTick.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStopFeedTick.Name = "btnStopFeedTick";
            this.btnStopFeedTick.Size = new System.Drawing.Size(70, 22);
            this.btnStopFeedTick.Text = "RejectTick";
            // 
            // mainPanel
            // 
            this.mainPanel.Controls.Add(this.ctrlQuoteList);
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(0, 50);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(1274, 617);
            this.mainPanel.TabIndex = 2;
            // 
            // ctrlQuoteList
            // 
            this.ctrlQuoteList.Location = new System.Drawing.Point(3, 3);
            this.ctrlQuoteList.Name = "ctrlQuoteList";
            this.ctrlQuoteList.Size = new System.Drawing.Size(351, 167);
            this.ctrlQuoteList.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1274, 25);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuSecurity,
            this.menuSymbol,
            this.toolStripSeparator3,
            this.menuMarketTime,
            this.menuExchange});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(92, 21);
            this.toolStripMenuItem1.Text = "基础数据管理";
            // 
            // menuSecurity
            // 
            this.menuSecurity.Name = "menuSecurity";
            this.menuSecurity.Size = new System.Drawing.Size(136, 22);
            this.menuSecurity.Text = "品种数据";
            // 
            // menuSymbol
            // 
            this.menuSymbol.Name = "menuSymbol";
            this.menuSymbol.Size = new System.Drawing.Size(136, 22);
            this.menuSymbol.Text = "合约数据";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(133, 6);
            // 
            // menuMarketTime
            // 
            this.menuMarketTime.Name = "menuMarketTime";
            this.menuMarketTime.Size = new System.Drawing.Size(136, 22);
            this.menuMarketTime.Text = "交易时间段";
            // 
            // menuExchange
            // 
            this.menuExchange.Name = "menuExchange";
            this.menuExchange.Size = new System.Drawing.Size(136, 22);
            this.menuExchange.Text = "交易所";
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuBarData,
            this.toolStripMenuItem4});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(92, 21);
            this.toolStripMenuItem2.Text = "行情数据管理";
            // 
            // menuBarData
            // 
            this.menuBarData.Name = "menuBarData";
            this.menuBarData.Size = new System.Drawing.Size(192, 22);
            this.menuBarData.Text = "Bar数据";
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(192, 22);
            this.toolStripMenuItem4.Text = "toolStripMenuItem4";
            // 
            // btnFunctionForm
            // 
            this.btnFunctionForm.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnFunctionForm.Image = ((System.Drawing.Image)(resources.GetObject("btnFunctionForm.Image")));
            this.btnFunctionForm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFunctionForm.Name = "btnFunctionForm";
            this.btnFunctionForm.Size = new System.Drawing.Size(36, 22);
            this.btnFunctionForm.Text = "工具";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1274, 689);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "行情服务器管理端";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.mainPanel.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnConnect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnDebugForm;
        private System.Windows.Forms.Panel mainPanel;
        private XTrader.Control.ctrlQuoteList ctrlQuoteList;
        private System.Windows.Forms.ToolStripButton btnDebug1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnRegister;
        private System.Windows.Forms.ToolStripButton btnUnregister;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem menuSecurity;
        private System.Windows.Forms.ToolStripMenuItem menuSymbol;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem menuMarketTime;
        private System.Windows.Forms.ToolStripMenuItem menuExchange;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem menuBarData;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnStartFeedTick;
        private System.Windows.Forms.ToolStripButton btnStopFeedTick;
        private System.Windows.Forms.ToolStripButton btnFunctionForm;
    }
}