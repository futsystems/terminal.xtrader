namespace TradingLib.XTrader.Future
{
    partial class ctrlOrderEntry
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageFlashOrder = new System.Windows.Forms.TabPage();
            this.lbShortCloseVol = new System.Windows.Forms.Label();
            this.lbLongCloseVol = new System.Windows.Forms.Label();
            this.lbShortOpenVol = new System.Windows.Forms.Label();
            this.lbLongOpenVol = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.inputFlagAuto = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.inputFlagCloseToday = new System.Windows.Forms.RadioButton();
            this.inputFlagClose = new System.Windows.Forms.RadioButton();
            this.inputFlagOpen = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageThreeBtn = new System.Windows.Forms.TabPage();
            this.panelFlashOrder = new System.Windows.Forms.Panel();
            this.tabPageTradition = new System.Windows.Forms.TabPage();
            this.holderPanel_Symbol = new System.Windows.Forms.Panel();
            this.panelThreeBtn = new System.Windows.Forms.Panel();
            this.panelTradition = new System.Windows.Forms.Panel();
            this.inputSymbol = new TradingLib.XTrader.Future.ctrlSymbolSelecter();
            this.inputArbFlag = new CSharpWin.ComboBoxEx();
            this.btnConditionOrder = new TradingLib.XTrader.FButton();
            this.inputSize = new TradingLib.XTrader.Future.FNumberInput();
            this.inputPrice = new TradingLib.XTrader.Future.FNumberInput();
            this.btnQryMaxVol = new TradingLib.XTrader.FButton();
            this.btnSell = new TradingLib.XTrader.FButton();
            this.btnReset = new TradingLib.XTrader.FButton();
            this.btnBuy = new TradingLib.XTrader.FButton();
            this.tabControl1.SuspendLayout();
            this.panelFlashOrder.SuspendLayout();
            this.holderPanel_Symbol.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageFlashOrder);
            this.tabControl1.Controls.Add(this.tabPageThreeBtn);
            this.tabControl1.Controls.Add(this.tabPageTradition);
            this.tabControl1.Location = new System.Drawing.Point(1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(334, 232);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageFlashOrder
            // 
            this.tabPageFlashOrder.Location = new System.Drawing.Point(4, 22);
            this.tabPageFlashOrder.Name = "tabPageFlashOrder";
            this.tabPageFlashOrder.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFlashOrder.Size = new System.Drawing.Size(326, 206);
            this.tabPageFlashOrder.TabIndex = 0;
            this.tabPageFlashOrder.Text = "闪电下单";
            this.tabPageFlashOrder.UseVisualStyleBackColor = true;
            // 
            // lbShortCloseVol
            // 
            this.lbShortCloseVol.AutoSize = true;
            this.lbShortCloseVol.Location = new System.Drawing.Point(215, 84);
            this.lbShortCloseVol.Name = "lbShortCloseVol";
            this.lbShortCloseVol.Size = new System.Drawing.Size(17, 12);
            this.lbShortCloseVol.TabIndex = 29;
            this.lbShortCloseVol.Text = "--";
            // 
            // lbLongCloseVol
            // 
            this.lbLongCloseVol.AutoSize = true;
            this.lbLongCloseVol.Location = new System.Drawing.Point(215, 68);
            this.lbLongCloseVol.Name = "lbLongCloseVol";
            this.lbLongCloseVol.Size = new System.Drawing.Size(17, 12);
            this.lbLongCloseVol.TabIndex = 28;
            this.lbLongCloseVol.Text = "--";
            // 
            // lbShortOpenVol
            // 
            this.lbShortOpenVol.AutoSize = true;
            this.lbShortOpenVol.Location = new System.Drawing.Point(150, 84);
            this.lbShortOpenVol.Name = "lbShortOpenVol";
            this.lbShortOpenVol.Size = new System.Drawing.Size(17, 12);
            this.lbShortOpenVol.TabIndex = 27;
            this.lbShortOpenVol.Text = "--";
            // 
            // lbLongOpenVol
            // 
            this.lbLongOpenVol.AutoSize = true;
            this.lbLongOpenVol.Location = new System.Drawing.Point(150, 68);
            this.lbLongOpenVol.Name = "lbLongOpenVol";
            this.lbLongOpenVol.Size = new System.Drawing.Size(17, 12);
            this.lbLongOpenVol.TabIndex = 26;
            this.lbLongOpenVol.Text = "--";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(249, 35);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(72, 16);
            this.checkBox2.TabIndex = 23;
            this.checkBox2.Text = "套利移仓";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.Visible = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.ForestGreen;
            this.label5.Location = new System.Drawing.Point(127, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "卖:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(127, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "买:";
            // 
            // inputFlagAuto
            // 
            this.inputFlagAuto.AutoSize = true;
            this.inputFlagAuto.Location = new System.Drawing.Point(184, 47);
            this.inputFlagAuto.Name = "inputFlagAuto";
            this.inputFlagAuto.Size = new System.Drawing.Size(48, 16);
            this.inputFlagAuto.TabIndex = 11;
            this.inputFlagAuto.Text = "自动";
            this.inputFlagAuto.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "价格";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "数量";
            // 
            // inputFlagCloseToday
            // 
            this.inputFlagCloseToday.AutoSize = true;
            this.inputFlagCloseToday.Location = new System.Drawing.Point(116, 47);
            this.inputFlagCloseToday.Name = "inputFlagCloseToday";
            this.inputFlagCloseToday.Size = new System.Drawing.Size(47, 16);
            this.inputFlagCloseToday.TabIndex = 4;
            this.inputFlagCloseToday.Text = "平今";
            this.inputFlagCloseToday.UseVisualStyleBackColor = true;
            // 
            // inputFlagClose
            // 
            this.inputFlagClose.AutoSize = true;
            this.inputFlagClose.Location = new System.Drawing.Point(63, 47);
            this.inputFlagClose.Name = "inputFlagClose";
            this.inputFlagClose.Size = new System.Drawing.Size(47, 16);
            this.inputFlagClose.TabIndex = 3;
            this.inputFlagClose.Text = "平仓";
            this.inputFlagClose.UseVisualStyleBackColor = true;
            // 
            // inputFlagOpen
            // 
            this.inputFlagOpen.AutoSize = true;
            this.inputFlagOpen.Checked = true;
            this.inputFlagOpen.Location = new System.Drawing.Point(10, 47);
            this.inputFlagOpen.Name = "inputFlagOpen";
            this.inputFlagOpen.Size = new System.Drawing.Size(47, 16);
            this.inputFlagOpen.TabIndex = 2;
            this.inputFlagOpen.TabStop = true;
            this.inputFlagOpen.Text = "开仓";
            this.inputFlagOpen.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "合约";
            // 
            // tabPageThreeBtn
            // 
            this.tabPageThreeBtn.Location = new System.Drawing.Point(4, 22);
            this.tabPageThreeBtn.Name = "tabPageThreeBtn";
            this.tabPageThreeBtn.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageThreeBtn.Size = new System.Drawing.Size(326, 206);
            this.tabPageThreeBtn.TabIndex = 1;
            this.tabPageThreeBtn.Text = "三键下单";
            this.tabPageThreeBtn.UseVisualStyleBackColor = true;
            // 
            // panelFlashOrder
            // 
            this.panelFlashOrder.Controls.Add(this.lbShortCloseVol);
            this.panelFlashOrder.Controls.Add(this.holderPanel_Symbol);
            this.panelFlashOrder.Controls.Add(this.lbLongCloseVol);
            this.panelFlashOrder.Controls.Add(this.checkBox2);
            this.panelFlashOrder.Controls.Add(this.lbShortOpenVol);
            this.panelFlashOrder.Controls.Add(this.inputFlagOpen);
            this.panelFlashOrder.Controls.Add(this.lbLongOpenVol);
            this.panelFlashOrder.Controls.Add(this.inputFlagClose);
            this.panelFlashOrder.Controls.Add(this.btnConditionOrder);
            this.panelFlashOrder.Controls.Add(this.inputFlagCloseToday);
            this.panelFlashOrder.Controls.Add(this.label2);
            this.panelFlashOrder.Controls.Add(this.inputSize);
            this.panelFlashOrder.Controls.Add(this.label3);
            this.panelFlashOrder.Controls.Add(this.inputPrice);
            this.panelFlashOrder.Controls.Add(this.btnQryMaxVol);
            this.panelFlashOrder.Controls.Add(this.btnSell);
            this.panelFlashOrder.Controls.Add(this.btnReset);
            this.panelFlashOrder.Controls.Add(this.btnBuy);
            this.panelFlashOrder.Controls.Add(this.inputFlagAuto);
            this.panelFlashOrder.Controls.Add(this.label5);
            this.panelFlashOrder.Controls.Add(this.label4);
            this.panelFlashOrder.Location = new System.Drawing.Point(5, 238);
            this.panelFlashOrder.Margin = new System.Windows.Forms.Padding(0);
            this.panelFlashOrder.Name = "panelFlashOrder";
            this.panelFlashOrder.Size = new System.Drawing.Size(320, 200);
            this.panelFlashOrder.TabIndex = 1;
            // 
            // tabPageTradition
            // 
            this.tabPageTradition.Location = new System.Drawing.Point(4, 22);
            this.tabPageTradition.Name = "tabPageTradition";
            this.tabPageTradition.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTradition.Size = new System.Drawing.Size(326, 206);
            this.tabPageTradition.TabIndex = 2;
            this.tabPageTradition.Text = "传统下单";
            this.tabPageTradition.UseVisualStyleBackColor = true;
            // 
            // holderPanel_Symbol
            // 
            this.holderPanel_Symbol.Controls.Add(this.inputSymbol);
            this.holderPanel_Symbol.Controls.Add(this.label1);
            this.holderPanel_Symbol.Controls.Add(this.inputArbFlag);
            this.holderPanel_Symbol.Dock = System.Windows.Forms.DockStyle.Top;
            this.holderPanel_Symbol.Location = new System.Drawing.Point(0, 0);
            this.holderPanel_Symbol.Name = "holderPanel_Symbol";
            this.holderPanel_Symbol.Size = new System.Drawing.Size(320, 32);
            this.holderPanel_Symbol.TabIndex = 0;
            // 
            // panelThreeBtn
            // 
            this.panelThreeBtn.Location = new System.Drawing.Point(5, 452);
            this.panelThreeBtn.Name = "panelThreeBtn";
            this.panelThreeBtn.Size = new System.Drawing.Size(320, 200);
            this.panelThreeBtn.TabIndex = 2;
            // 
            // panelTradition
            // 
            this.panelTradition.Location = new System.Drawing.Point(5, 658);
            this.panelTradition.Name = "panelTradition";
            this.panelTradition.Size = new System.Drawing.Size(320, 200);
            this.panelTradition.TabIndex = 3;
            // 
            // inputSymbol
            // 
            this.inputSymbol.AllowResizeDropDown = true;
            this.inputSymbol.ControlSize = new System.Drawing.Size(1, 1);
            this.inputSymbol.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.inputSymbol.DropDownControl = null;
            this.inputSymbol.DropSize = new System.Drawing.Size(121, 106);
            this.inputSymbol.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.inputSymbol.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(60)))), ((int)(((byte)(109)))));
            this.inputSymbol.Location = new System.Drawing.Point(44, 7);
            this.inputSymbol.Name = "inputSymbol";
            this.inputSymbol.Size = new System.Drawing.Size(151, 22);
            this.inputSymbol.TabIndex = 18;
            // 
            // inputArbFlag
            // 
            this.inputArbFlag.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.inputArbFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.inputArbFlag.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.inputArbFlag.ForeColor = System.Drawing.Color.Black;
            this.inputArbFlag.FormattingEnabled = true;
            this.inputArbFlag.ItemHeight = 14;
            this.inputArbFlag.Location = new System.Drawing.Point(239, 7);
            this.inputArbFlag.Name = "inputArbFlag";
            this.inputArbFlag.Size = new System.Drawing.Size(80, 22);
            this.inputArbFlag.TabIndex = 24;
            this.inputArbFlag.TabStop = false;
            // 
            // btnConditionOrder
            // 
            this.btnConditionOrder.BackColor = System.Drawing.Color.White;
            this.btnConditionOrder.CheckButton = false;
            this.btnConditionOrder.Checked = false;
            this.btnConditionOrder.Enabled = false;
            this.btnConditionOrder.IsPriceOn = false;
            this.btnConditionOrder.Location = new System.Drawing.Point(7, 177);
            this.btnConditionOrder.Name = "btnConditionOrder";
            this.btnConditionOrder.OrderEntryButton = false;
            this.btnConditionOrder.PriceStr = "";
            this.btnConditionOrder.Size = new System.Drawing.Size(50, 18);
            this.btnConditionOrder.TabIndex = 25;
            this.btnConditionOrder.Text = "条件单";
            this.btnConditionOrder.UseVisualStyleBackColor = true;
            // 
            // inputSize
            // 
            this.inputSize.DecimalPlace = 2;
            this.inputSize.DropDownControl = null;
            this.inputSize.DropDownSizeMode = TradingLib.XTrader.Future.SizeMode.UseComboSize;
            this.inputSize.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.inputSize.Location = new System.Drawing.Point(42, 72);
            this.inputSize.MaxVal = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.inputSize.MinVal = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.inputSize.Name = "inputSize";
            this.inputSize.PriceFormat = "{0:F2}";
            this.inputSize.ShowTop = false;
            this.inputSize.Size = new System.Drawing.Size(79, 20);
            this.inputSize.SymbolSelected = false;
            this.inputSize.TabIndex = 21;
            this.inputSize.Text = "fPriceInput2";
            this.inputSize.TxtValue = "1";
            // 
            // inputPrice
            // 
            this.inputPrice.DecimalPlace = 2;
            this.inputPrice.DropDownControl = null;
            this.inputPrice.DropDownSizeMode = TradingLib.XTrader.Future.SizeMode.UseComboSize;
            this.inputPrice.Increment = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.inputPrice.Location = new System.Drawing.Point(42, 104);
            this.inputPrice.MaxVal = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.inputPrice.MinVal = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.inputPrice.Name = "inputPrice";
            this.inputPrice.PriceFormat = "{0:F2}";
            this.inputPrice.ShowTop = false;
            this.inputPrice.Size = new System.Drawing.Size(119, 20);
            this.inputPrice.SymbolSelected = false;
            this.inputPrice.TabIndex = 20;
            this.inputPrice.Text = "fPriceInput1";
            this.inputPrice.TxtValue = "0";
            // 
            // btnQryMaxVol
            // 
            this.btnQryMaxVol.BackColor = System.Drawing.Color.White;
            this.btnQryMaxVol.CheckButton = false;
            this.btnQryMaxVol.Checked = false;
            this.btnQryMaxVol.IsPriceOn = false;
            this.btnQryMaxVol.Location = new System.Drawing.Point(7, 137);
            this.btnQryMaxVol.Name = "btnQryMaxVol";
            this.btnQryMaxVol.OrderEntryButton = false;
            this.btnQryMaxVol.PriceStr = "";
            this.btnQryMaxVol.Size = new System.Drawing.Size(50, 18);
            this.btnQryMaxVol.TabIndex = 9;
            this.btnQryMaxVol.Text = "查可开";
            this.btnQryMaxVol.UseVisualStyleBackColor = true;
            // 
            // btnSell
            // 
            this.btnSell.BackColor = System.Drawing.Color.Transparent;
            this.btnSell.CheckButton = false;
            this.btnSell.Checked = false;
            this.btnSell.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnSell.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(138)))), ((int)(((byte)(2)))));
            this.btnSell.IsPriceOn = false;
            this.btnSell.Location = new System.Drawing.Point(238, 137);
            this.btnSell.Name = "btnSell";
            this.btnSell.OrderEntryButton = true;
            this.btnSell.PriceStr = "";
            this.btnSell.Size = new System.Drawing.Size(80, 60);
            this.btnSell.TabIndex = 17;
            this.btnSell.Text = "卖出";
            this.btnSell.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.White;
            this.btnReset.CheckButton = false;
            this.btnReset.Checked = false;
            this.btnReset.IsPriceOn = false;
            this.btnReset.Location = new System.Drawing.Point(7, 157);
            this.btnReset.Name = "btnReset";
            this.btnReset.OrderEntryButton = false;
            this.btnReset.PriceStr = "";
            this.btnReset.Size = new System.Drawing.Size(50, 18);
            this.btnReset.TabIndex = 10;
            this.btnReset.Text = "复位";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // btnBuy
            // 
            this.btnBuy.BackColor = System.Drawing.Color.Transparent;
            this.btnBuy.CheckButton = false;
            this.btnBuy.Checked = false;
            this.btnBuy.Cursor = System.Windows.Forms.Cursors.Default;
            this.btnBuy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.btnBuy.IsPriceOn = false;
            this.btnBuy.Location = new System.Drawing.Point(152, 137);
            this.btnBuy.Name = "btnBuy";
            this.btnBuy.OrderEntryButton = true;
            this.btnBuy.PriceStr = "";
            this.btnBuy.Size = new System.Drawing.Size(80, 60);
            this.btnBuy.TabIndex = 16;
            this.btnBuy.Text = "买入";
            this.btnBuy.UseVisualStyleBackColor = true;
            // 
            // ctrlOrderEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.panelTradition);
            this.Controls.Add(this.panelThreeBtn);
            this.Controls.Add(this.panelFlashOrder);
            this.Controls.Add(this.tabControl1);
            this.Name = "ctrlOrderEntry";
            this.Size = new System.Drawing.Size(335, 877);
            this.tabControl1.ResumeLayout(false);
            this.panelFlashOrder.ResumeLayout(false);
            this.panelFlashOrder.PerformLayout();
            this.holderPanel_Symbol.ResumeLayout(false);
            this.holderPanel_Symbol.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageFlashOrder;
        private System.Windows.Forms.TabPage tabPageThreeBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton inputFlagOpen;
        private System.Windows.Forms.RadioButton inputFlagClose;
        private System.Windows.Forms.RadioButton inputFlagCloseToday;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private FButton btnQryMaxVol;
        private FButton btnReset;
        private System.Windows.Forms.CheckBox inputFlagAuto;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private FButton btnBuy;
        private FButton btnSell;
        private ctrlSymbolSelecter inputSymbol;
        private FNumberInput inputPrice;
        private FNumberInput inputSize;
        private System.Windows.Forms.TabPage tabPageTradition;
        private System.Windows.Forms.CheckBox checkBox2;
        private CSharpWin.ComboBoxEx inputArbFlag;
        private FButton btnConditionOrder;
        private System.Windows.Forms.Label lbShortOpenVol;
        private System.Windows.Forms.Label lbLongOpenVol;
        private System.Windows.Forms.Label lbLongCloseVol;
        private System.Windows.Forms.Label lbShortCloseVol;
        private System.Windows.Forms.Panel panelFlashOrder;
        private System.Windows.Forms.Panel holderPanel_Symbol;
        private System.Windows.Forms.Panel panelThreeBtn;
        private System.Windows.Forms.Panel panelTradition;
    }
}
