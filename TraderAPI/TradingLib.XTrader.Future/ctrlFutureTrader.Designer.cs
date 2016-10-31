namespace TradingLib.XTrader.Future
{
    partial class ctrlFutureTrader
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lbAccount = new System.Windows.Forms.Label();
            this.panelHolder = new System.Windows.Forms.Panel();
            this.panelOrderEntry = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelPageHolder = new System.Windows.Forms.Panel();
            this.ctrlOrderEntry1 = new TradingLib.XTrader.Future.ctrlOrderEntry();
            this.btnHide = new TradingLib.XTrader.FButton();
            this.ctrlListMenu1 = new TradingLib.XTrader.Future.ctrlListMenu();
            this.panelTop.SuspendLayout();
            this.panelHolder.SuspendLayout();
            this.panelOrderEntry.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.button2);
            this.panelTop.Controls.Add(this.btnRefresh);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.lbAccount);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1080, 25);
            this.panelTop.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(943, 1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "锁定";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefresh.Location = new System.Drawing.Point(862, 1);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(75, 23);
            this.btnRefresh.TabIndex = 2;
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(119, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "可用资金:23232.99";
            // 
            // lbAccount
            // 
            this.lbAccount.AutoSize = true;
            this.lbAccount.Location = new System.Drawing.Point(3, 5);
            this.lbAccount.Name = "lbAccount";
            this.lbAccount.Size = new System.Drawing.Size(77, 12);
            this.lbAccount.TabIndex = 0;
            this.lbAccount.Text = "交易者，您好";
            // 
            // panelHolder
            // 
            this.panelHolder.Controls.Add(this.panelRight);
            this.panelHolder.Controls.Add(this.panelLeft);
            this.panelHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHolder.Location = new System.Drawing.Point(0, 25);
            this.panelHolder.Name = "panelHolder";
            this.panelHolder.Size = new System.Drawing.Size(1080, 234);
            this.panelHolder.TabIndex = 1;
            // 
            // panelOrderEntry
            // 
            this.panelOrderEntry.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.panelOrderEntry.Controls.Add(this.ctrlOrderEntry1);
            this.panelOrderEntry.Controls.Add(this.btnHide);
            this.panelOrderEntry.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelOrderEntry.Location = new System.Drawing.Point(0, 0);
            this.panelOrderEntry.Name = "panelOrderEntry";
            this.panelOrderEntry.Size = new System.Drawing.Size(344, 234);
            this.panelOrderEntry.TabIndex = 2;
            // 
            // panelLeft
            // 
            this.panelLeft.Controls.Add(this.ctrlListMenu1);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(120, 234);
            this.panelLeft.TabIndex = 4;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.panelPageHolder);
            this.panelRight.Controls.Add(this.panelOrderEntry);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelRight.Location = new System.Drawing.Point(120, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(960, 234);
            this.panelRight.TabIndex = 5;
            // 
            // panelPageHolder
            // 
            this.panelPageHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPageHolder.Location = new System.Drawing.Point(344, 0);
            this.panelPageHolder.Name = "panelPageHolder";
            this.panelPageHolder.Size = new System.Drawing.Size(616, 234);
            this.panelPageHolder.TabIndex = 3;
            // 
            // ctrlOrderEntry1
            // 
            this.ctrlOrderEntry1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlOrderEntry1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ctrlOrderEntry1.Location = new System.Drawing.Point(0, 0);
            this.ctrlOrderEntry1.Name = "ctrlOrderEntry1";
            this.ctrlOrderEntry1.Size = new System.Drawing.Size(335, 234);
            this.ctrlOrderEntry1.TabIndex = 3;
            // 
            // btnHide
            // 
            this.btnHide.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnHide.BackColor = System.Drawing.Color.White;
            this.btnHide.CheckButton = false;
            this.btnHide.Checked = false;
            this.btnHide.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnHide.IsPriceOn = false;
            this.btnHide.Location = new System.Drawing.Point(335, 0);
            this.btnHide.Name = "btnHide";
            this.btnHide.OrderEntryButton = false;
            this.btnHide.PriceStr = "";
            this.btnHide.Size = new System.Drawing.Size(7, 234);
            this.btnHide.TabIndex = 2;
            this.btnHide.Text = "<";
            this.btnHide.UseVisualStyleBackColor = false;
            // 
            // ctrlListMenu1
            // 
            this.ctrlListMenu1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlListMenu1.Location = new System.Drawing.Point(0, 0);
            this.ctrlListMenu1.Name = "ctrlListMenu1";
            this.ctrlListMenu1.Size = new System.Drawing.Size(120, 234);
            this.ctrlListMenu1.TabIndex = 1;
            this.ctrlListMenu1.Text = "ctrlListMenu1";
            // 
            // ctrlFutureTrader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelHolder);
            this.Controls.Add(this.panelTop);
            this.Name = "ctrlFutureTrader";
            this.Size = new System.Drawing.Size(1080, 259);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelHolder.ResumeLayout(false);
            this.panelOrderEntry.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lbAccount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnRefresh;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panelHolder;
        private ctrlListMenu ctrlListMenu1;
        private System.Windows.Forms.Panel panelOrderEntry;
        private FButton btnHide;
        private ctrlOrderEntry ctrlOrderEntry1;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelPageHolder;
    }
}
