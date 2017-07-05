namespace TradingLib.XTrader.Future
{
    partial class PageBankFZ
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
            this.btnSetBankInfo = new TradingLib.XTrader.FButton();
            this.account = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbCurrency = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnWithdraw = new TradingLib.XTrader.FButton();
            this.btnDeposit = new TradingLib.XTrader.FButton();
            this.amount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.cbBank = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amount)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.cbBank);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.btnSetBankInfo);
            this.panel1.Controls.Add(this.account);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbCurrency);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.btnWithdraw);
            this.panel1.Controls.Add(this.btnDeposit);
            this.panel1.Controls.Add(this.amount);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(870, 319);
            this.panel1.TabIndex = 26;
            // 
            // btnSetBankInfo
            // 
            this.btnSetBankInfo.BackColor = System.Drawing.Color.White;
            this.btnSetBankInfo.CheckButton = false;
            this.btnSetBankInfo.Checked = false;
            this.btnSetBankInfo.IsPriceOn = false;
            this.btnSetBankInfo.Location = new System.Drawing.Point(207, 4);
            this.btnSetBankInfo.Name = "btnSetBankInfo";
            this.btnSetBankInfo.OrderEntryButton = false;
            this.btnSetBankInfo.PriceStr = "";
            this.btnSetBankInfo.Size = new System.Drawing.Size(147, 23);
            this.btnSetBankInfo.TabIndex = 8;
            this.btnSetBankInfo.Text = "设定签约银行";
            this.btnSetBankInfo.UseVisualStyleBackColor = false;
            // 
            // account
            // 
            this.account.AutoSize = true;
            this.account.Location = new System.Drawing.Point(75, 9);
            this.account.Name = "account";
            this.account.Size = new System.Drawing.Size(17, 12);
            this.account.TabIndex = 7;
            this.account.Text = "--";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 6;
            this.label3.Text = "交易账户";
            // 
            // cbCurrency
            // 
            this.cbCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrency.FormattingEnabled = true;
            this.cbCurrency.Location = new System.Drawing.Point(75, 81);
            this.cbCurrency.Name = "cbCurrency";
            this.cbCurrency.Size = new System.Drawing.Size(89, 20);
            this.cbCurrency.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "币种";
            // 
            // btnWithdraw
            // 
            this.btnWithdraw.BackColor = System.Drawing.Color.White;
            this.btnWithdraw.CheckButton = false;
            this.btnWithdraw.Checked = false;
            this.btnWithdraw.IsPriceOn = false;
            this.btnWithdraw.Location = new System.Drawing.Point(207, 168);
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
            this.btnDeposit.Location = new System.Drawing.Point(5, 168);
            this.btnDeposit.Name = "btnDeposit";
            this.btnDeposit.OrderEntryButton = false;
            this.btnDeposit.PriceStr = "";
            this.btnDeposit.Size = new System.Drawing.Size(147, 23);
            this.btnDeposit.TabIndex = 2;
            this.btnDeposit.Text = "入金(银行资金转账户)";
            this.btnDeposit.UseVisualStyleBackColor = false;
            // 
            // amount
            // 
            this.amount.DecimalPlaces = 2;
            this.amount.Location = new System.Drawing.Point(75, 41);
            this.amount.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.amount.Name = "amount";
            this.amount.Size = new System.Drawing.Size(89, 21);
            this.amount.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "转账金额";
            // 
            // cbBank
            // 
            this.cbBank.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBank.FormattingEnabled = true;
            this.cbBank.Location = new System.Drawing.Point(75, 119);
            this.cbBank.Name = "cbBank";
            this.cbBank.Size = new System.Drawing.Size(89, 20);
            this.cbBank.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "入金渠道";
            // 
            // PageBankFZ
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Name = "PageBankFZ";
            this.Size = new System.Drawing.Size(872, 319);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown amount;
        private FButton btnWithdraw;
        private FButton btnDeposit;
        private System.Windows.Forms.ComboBox cbCurrency;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label account;
        private FButton btnSetBankInfo;
        private System.Windows.Forms.ComboBox cbBank;
        private System.Windows.Forms.Label label4;
    }
}
