namespace TradingLib.XTrader.Future
{
    partial class PageBank1
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
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.depositNormal = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.depositLeverageDeposit = new System.Windows.Forms.RadioButton();
            this.amountDeposit = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDeposit = new TradingLib.XTrader.FButton();
            this.cbCurrency = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.withdrawAvabile = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.withdrawCreditWithdraw = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.withdrawNormal = new System.Windows.Forms.RadioButton();
            this.amountWithdraw = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cbCurrency2 = new System.Windows.Forms.ComboBox();
            this.btnWithdraw = new TradingLib.XTrader.FButton();
            this.btnSetBankInfo = new TradingLib.XTrader.FButton();
            this.account = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amountDeposit)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amountWithdraw)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Controls.Add(this.btnSetBankInfo);
            this.panel1.Controls.Add(this.account);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1007, 244);
            this.panel1.TabIndex = 26;
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(362, 132);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(424, 69);
            this.textBox2.TabIndex = 14;
            this.textBox2.Text = "1.出金手续费是每笔2人民币，请客户出金后耐心等待！出金时间：每周 一至周五，9:00-17:00\r\n2.出金提现：提取当前权益部分及全部资金，随后请查收银行到账" +
    "信息。\r\n3.减少配资：通过减少配资金额可增加交易安全度，出金资金归属公司。 ";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(360, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(57, 12);
            this.label7.TabIndex = 13;
            this.label7.Text = "了解出金";
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(362, 30);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(424, 69);
            this.textBox1.TabIndex = 12;
            this.textBox1.Text = "1.入金手续费是打款金额的0.3%无上限，入金直接到账\r\n2.配资入金 ：杠杆比例为1:10，系统默认追加配资资金；例：入金10000人民币（1388.8美金），" +
    "配资金额为:13888美金,根据杠杆比例该账户可用资金为15276.8美金。\r\n3.追加资金 ：增加当前权益部分，系统不再进行增加配资额度。 ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(360, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "了解入金";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(5, 33);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(305, 197);
            this.tabControl1.TabIndex = 10;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.depositNormal);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.depositLeverageDeposit);
            this.tabPage1.Controls.Add(this.amountDeposit);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.btnDeposit);
            this.tabPage1.Controls.Add(this.cbCurrency);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(297, 171);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "充值";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // depositNormal
            // 
            this.depositNormal.AutoSize = true;
            this.depositNormal.Location = new System.Drawing.Point(115, 83);
            this.depositNormal.Name = "depositNormal";
            this.depositNormal.Size = new System.Drawing.Size(71, 16);
            this.depositNormal.TabIndex = 10;
            this.depositNormal.TabStop = true;
            this.depositNormal.Text = "追加资金";
            this.depositNormal.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "充值金额";
            // 
            // depositLeverageDeposit
            // 
            this.depositLeverageDeposit.AutoSize = true;
            this.depositLeverageDeposit.Location = new System.Drawing.Point(27, 83);
            this.depositLeverageDeposit.Name = "depositLeverageDeposit";
            this.depositLeverageDeposit.Size = new System.Drawing.Size(71, 16);
            this.depositLeverageDeposit.TabIndex = 9;
            this.depositLeverageDeposit.TabStop = true;
            this.depositLeverageDeposit.Text = "配资入金";
            this.depositLeverageDeposit.UseVisualStyleBackColor = true;
            // 
            // amountDeposit
            // 
            this.amountDeposit.DecimalPlaces = 2;
            this.amountDeposit.Location = new System.Drawing.Point(97, 11);
            this.amountDeposit.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.amountDeposit.Name = "amountDeposit";
            this.amountDeposit.Size = new System.Drawing.Size(89, 21);
            this.amountDeposit.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "币种";
            // 
            // btnDeposit
            // 
            this.btnDeposit.BackColor = System.Drawing.Color.White;
            this.btnDeposit.CheckButton = false;
            this.btnDeposit.Checked = false;
            this.btnDeposit.IsPriceOn = false;
            this.btnDeposit.Location = new System.Drawing.Point(39, 123);
            this.btnDeposit.Name = "btnDeposit";
            this.btnDeposit.OrderEntryButton = false;
            this.btnDeposit.PriceStr = "";
            this.btnDeposit.Size = new System.Drawing.Size(147, 23);
            this.btnDeposit.TabIndex = 2;
            this.btnDeposit.Text = "充值(银行资金转账户)";
            this.btnDeposit.UseVisualStyleBackColor = false;
            // 
            // cbCurrency
            // 
            this.cbCurrency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrency.FormattingEnabled = true;
            this.cbCurrency.Location = new System.Drawing.Point(97, 43);
            this.cbCurrency.Name = "cbCurrency";
            this.cbCurrency.Size = new System.Drawing.Size(89, 20);
            this.cbCurrency.TabIndex = 5;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.withdrawAvabile);
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.withdrawCreditWithdraw);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.withdrawNormal);
            this.tabPage2.Controls.Add(this.amountWithdraw);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.cbCurrency2);
            this.tabPage2.Controls.Add(this.btnWithdraw);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(297, 171);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "提现";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // withdrawAvabile
            // 
            this.withdrawAvabile.AutoSize = true;
            this.withdrawAvabile.Location = new System.Drawing.Point(227, 46);
            this.withdrawAvabile.Name = "withdrawAvabile";
            this.withdrawAvabile.Size = new System.Drawing.Size(17, 12);
            this.withdrawAvabile.TabIndex = 19;
            this.withdrawAvabile.Text = "--";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(192, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "可提:";
            // 
            // withdrawCreditWithdraw
            // 
            this.withdrawCreditWithdraw.AutoSize = true;
            this.withdrawCreditWithdraw.Location = new System.Drawing.Point(115, 83);
            this.withdrawCreditWithdraw.Name = "withdrawCreditWithdraw";
            this.withdrawCreditWithdraw.Size = new System.Drawing.Size(71, 16);
            this.withdrawCreditWithdraw.TabIndex = 17;
            this.withdrawCreditWithdraw.TabStop = true;
            this.withdrawCreditWithdraw.Text = "减少配资";
            this.withdrawCreditWithdraw.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 11;
            this.label4.Text = "提现金额";
            // 
            // withdrawNormal
            // 
            this.withdrawNormal.AutoSize = true;
            this.withdrawNormal.Location = new System.Drawing.Point(27, 83);
            this.withdrawNormal.Name = "withdrawNormal";
            this.withdrawNormal.Size = new System.Drawing.Size(71, 16);
            this.withdrawNormal.TabIndex = 16;
            this.withdrawNormal.TabStop = true;
            this.withdrawNormal.Text = "出金提现";
            this.withdrawNormal.UseVisualStyleBackColor = true;
            // 
            // amountWithdraw
            // 
            this.amountWithdraw.DecimalPlaces = 2;
            this.amountWithdraw.Location = new System.Drawing.Point(97, 11);
            this.amountWithdraw.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.amountWithdraw.Name = "amountWithdraw";
            this.amountWithdraw.Size = new System.Drawing.Size(89, 21);
            this.amountWithdraw.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 14;
            this.label5.Text = "币种";
            // 
            // cbCurrency2
            // 
            this.cbCurrency2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCurrency2.FormattingEnabled = true;
            this.cbCurrency2.Location = new System.Drawing.Point(97, 43);
            this.cbCurrency2.Name = "cbCurrency2";
            this.cbCurrency2.Size = new System.Drawing.Size(89, 20);
            this.cbCurrency2.TabIndex = 15;
            // 
            // btnWithdraw
            // 
            this.btnWithdraw.BackColor = System.Drawing.Color.White;
            this.btnWithdraw.CheckButton = false;
            this.btnWithdraw.Checked = false;
            this.btnWithdraw.IsPriceOn = false;
            this.btnWithdraw.Location = new System.Drawing.Point(39, 123);
            this.btnWithdraw.Name = "btnWithdraw";
            this.btnWithdraw.OrderEntryButton = false;
            this.btnWithdraw.PriceStr = "";
            this.btnWithdraw.Size = new System.Drawing.Size(147, 23);
            this.btnWithdraw.TabIndex = 3;
            this.btnWithdraw.Text = "提现(账户资金转银行)";
            this.btnWithdraw.UseVisualStyleBackColor = false;
            // 
            // btnSetBankInfo
            // 
            this.btnSetBankInfo.BackColor = System.Drawing.Color.White;
            this.btnSetBankInfo.CheckButton = false;
            this.btnSetBankInfo.Checked = false;
            this.btnSetBankInfo.IsPriceOn = false;
            this.btnSetBankInfo.Location = new System.Drawing.Point(190, 4);
            this.btnSetBankInfo.Name = "btnSetBankInfo";
            this.btnSetBankInfo.OrderEntryButton = false;
            this.btnSetBankInfo.PriceStr = "";
            this.btnSetBankInfo.Size = new System.Drawing.Size(116, 23);
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
            // PageBank1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Name = "PageBank1";
            this.Size = new System.Drawing.Size(1009, 244);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amountDeposit)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.amountWithdraw)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown amountDeposit;
        private FButton btnWithdraw;
        private FButton btnDeposit;
        private System.Windows.Forms.ComboBox cbCurrency;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label account;
        private FButton btnSetBankInfo;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.RadioButton depositLeverageDeposit;
        private System.Windows.Forms.RadioButton depositNormal;
        private System.Windows.Forms.RadioButton withdrawCreditWithdraw;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton withdrawNormal;
        private System.Windows.Forms.NumericUpDown amountWithdraw;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbCurrency2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label withdrawAvabile;
    }
}
