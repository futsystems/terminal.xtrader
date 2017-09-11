namespace TradingLib.XTrader.Future
{
    partial class PageBankWebSite
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnWithdraw = new TradingLib.XTrader.FButton();
            this.btnDeposit = new TradingLib.XTrader.FButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.btnWithdraw);
            this.panel1.Controls.Add(this.btnDeposit);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(870, 319);
            this.panel1.TabIndex = 26;
            // 
            // btnWithdraw
            // 
            this.btnWithdraw.BackColor = System.Drawing.Color.White;
            this.btnWithdraw.CheckButton = false;
            this.btnWithdraw.Checked = false;
            this.btnWithdraw.IsPriceOn = false;
            this.btnWithdraw.Location = new System.Drawing.Point(181, 25);
            this.btnWithdraw.Name = "btnWithdraw";
            this.btnWithdraw.OrderEntryButton = false;
            this.btnWithdraw.PriceStr = "";
            this.btnWithdraw.Size = new System.Drawing.Size(147, 23);
            this.btnWithdraw.TabIndex = 3;
            this.btnWithdraw.Text = "出金(账户资金转银行)";
            this.btnWithdraw.UseVisualStyleBackColor = false;
            // 
            // btnDeposit
            // 
            this.btnDeposit.BackColor = System.Drawing.Color.White;
            this.btnDeposit.CheckButton = false;
            this.btnDeposit.Checked = false;
            this.btnDeposit.IsPriceOn = false;
            this.btnDeposit.Location = new System.Drawing.Point(14, 25);
            this.btnDeposit.Name = "btnDeposit";
            this.btnDeposit.OrderEntryButton = false;
            this.btnDeposit.PriceStr = "";
            this.btnDeposit.Size = new System.Drawing.Size(147, 23);
            this.btnDeposit.TabIndex = 2;
            this.btnDeposit.Text = "入金(银行资金转账户)";
            this.btnDeposit.UseVisualStyleBackColor = false;
            // 
            // PageBankWebSite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Name = "PageBankWebSite";
            this.Size = new System.Drawing.Size(872, 319);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FButton btnWithdraw;
        private FButton btnDeposit;
    }
}
