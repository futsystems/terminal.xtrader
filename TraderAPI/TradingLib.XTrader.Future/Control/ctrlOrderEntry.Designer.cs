namespace TradingLib.XTrader.Future.Control
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.inputSize = new TradingLib.XTrader.Future.FNumberInput();
            this.inputPrice = new TradingLib.XTrader.Future.FNumberInput();
            this.ctrlSymbolSelecter1 = new TradingLib.XTrader.Future.ctrlSymbolSelecter();
            this.fButton2 = new TradingLib.XTrader.FButton();
            this.fButton1 = new TradingLib.XTrader.FButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(1, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(333, 231);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.inputSize);
            this.tabPage1.Controls.Add(this.inputPrice);
            this.tabPage1.Controls.Add(this.ctrlSymbolSelecter1);
            this.tabPage1.Controls.Add(this.fButton2);
            this.tabPage1.Controls.Add(this.fButton1);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.checkBox1);
            this.tabPage1.Controls.Add(this.button2);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.radioButton3);
            this.tabPage1.Controls.Add(this.radioButton2);
            this.tabPage1.Controls.Add(this.radioButton1);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(325, 205);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "闪电下单";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // inputSize
            // 
            this.inputSize.DecimalPlace = 2;
            this.inputSize.DropDownControl = null;
            this.inputSize.DropDownSizeMode = TradingLib.XTrader.Future.SizeMode.UseComboSize;
            this.inputSize.Location = new System.Drawing.Point(41, 69);
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
            this.inputSize.ShowTop = false;
            this.inputSize.Size = new System.Drawing.Size(79, 20);
            this.inputSize.TabIndex = 21;
            this.inputSize.Text = "fPriceInput2";
            this.inputSize.TxtValue = "1";
            // 
            // inputPrice
            // 
            this.inputPrice.DecimalPlace = 2;
            this.inputPrice.DropDownControl = null;
            this.inputPrice.DropDownSizeMode = TradingLib.XTrader.Future.SizeMode.UseComboSize;
            this.inputPrice.Location = new System.Drawing.Point(41, 104);
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
            this.inputPrice.ShowTop = false;
            this.inputPrice.Size = new System.Drawing.Size(119, 20);
            this.inputPrice.TabIndex = 20;
            this.inputPrice.Text = "fPriceInput1";
            this.inputPrice.TxtValue = "0";
            // 
            // ctrlSymbolSelecter1
            // 
            this.ctrlSymbolSelecter1.AllowResizeDropDown = true;
            this.ctrlSymbolSelecter1.ControlSize = new System.Drawing.Size(1, 1);
            this.ctrlSymbolSelecter1.DropDownControl = null;
            this.ctrlSymbolSelecter1.DropSize = new System.Drawing.Size(121, 106);
            this.ctrlSymbolSelecter1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold);
            this.ctrlSymbolSelecter1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(60)))), ((int)(((byte)(109)))));
            this.ctrlSymbolSelecter1.Location = new System.Drawing.Point(42, 11);
            this.ctrlSymbolSelecter1.Name = "ctrlSymbolSelecter1";
            this.ctrlSymbolSelecter1.Size = new System.Drawing.Size(151, 20);
            this.ctrlSymbolSelecter1.TabIndex = 18;
            // 
            // fButton2
            // 
            this.fButton2.BackColor = System.Drawing.Color.Transparent;
            this.fButton2.CheckButton = false;
            this.fButton2.Checked = false;
            this.fButton2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fButton2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(8)))), ((int)(((byte)(138)))), ((int)(((byte)(2)))));
            this.fButton2.Location = new System.Drawing.Point(239, 137);
            this.fButton2.Name = "fButton2";
            this.fButton2.OrderEntryButton = true;
            this.fButton2.Price = new decimal(new int[] {
            122400,
            0,
            0,
            131072});
            this.fButton2.Size = new System.Drawing.Size(80, 60);
            this.fButton2.TabIndex = 17;
            this.fButton2.Text = "卖出";
            this.fButton2.UseVisualStyleBackColor = true;
            // 
            // fButton1
            // 
            this.fButton1.BackColor = System.Drawing.Color.Transparent;
            this.fButton1.CheckButton = false;
            this.fButton1.Checked = false;
            this.fButton1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.fButton1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(189)))), ((int)(((byte)(9)))), ((int)(((byte)(9)))));
            this.fButton1.Location = new System.Drawing.Point(153, 137);
            this.fButton1.Name = "fButton1";
            this.fButton1.OrderEntryButton = true;
            this.fButton1.Price = new decimal(new int[] {
            122400,
            0,
            0,
            131072});
            this.fButton1.Size = new System.Drawing.Size(80, 60);
            this.fButton1.TabIndex = 16;
            this.fButton1.Text = "买入";
            this.fButton1.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.ForestGreen;
            this.label5.Location = new System.Drawing.Point(126, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(23, 12);
            this.label5.TabIndex = 13;
            this.label5.Text = "卖:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(126, 65);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(23, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "买:";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(186, 45);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(48, 16);
            this.checkBox1.TabIndex = 11;
            this.checkBox1.Text = "自动";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(6, 162);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(50, 23);
            this.button2.TabIndex = 10;
            this.button2.Text = "复位";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(50, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "查可开";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "价格";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "数量";
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(115, 46);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(47, 16);
            this.radioButton3.TabIndex = 4;
            this.radioButton3.TabStop = true;
            this.radioButton3.Text = "平今";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(62, 46);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 3;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "平仓";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(9, 46);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 2;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "开仓";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "合约";
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(325, 205);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "三键下单";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ctrlOrderEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.Controls.Add(this.tabControl1);
            this.Name = "ctrlOrderEntry";
            this.Size = new System.Drawing.Size(335, 233);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private FButton fButton1;
        private FButton fButton2;
        private ctrlSymbolSelecter ctrlSymbolSelecter1;
        private FNumberInput inputPrice;
        private FNumberInput inputSize;
    }
}
