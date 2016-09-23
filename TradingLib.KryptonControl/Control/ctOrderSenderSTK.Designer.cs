namespace TradingLib.KryptonControl
{
    partial class ctOrderSenderSTK
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.symbol = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.price = new System.Windows.Forms.NumericUpDown();
            this.lbMoneyAvabile = new System.Windows.Forms.Label();
            this.lbMaxOrderVol = new System.Windows.Forms.Label();
            this.size = new System.Windows.Forms.NumericUpDown();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnReset = new System.Windows.Forms.Button();
            this.lbSymbolName = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.price)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.size)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 26;
            this.label1.Text = "股东代码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(3, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 27;
            this.label2.Text = "证券代码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(3, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 28;
            this.label3.Text = "买入价格";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Location = new System.Drawing.Point(3, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 29;
            this.label4.Text = "可用资金";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Location = new System.Drawing.Point(3, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 30;
            this.label5.Text = "最大可买";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Location = new System.Drawing.Point(3, 134);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 31;
            this.label6.Text = "买入数量";
            // 
            // symbol
            // 
            this.symbol.Location = new System.Drawing.Point(63, 28);
            this.symbol.Name = "symbol";
            this.symbol.Size = new System.Drawing.Size(107, 21);
            this.symbol.TabIndex = 32;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(63, 5);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(106, 20);
            this.comboBox1.TabIndex = 33;
            // 
            // price
            // 
            this.price.Location = new System.Drawing.Point(62, 55);
            this.price.Name = "price";
            this.price.Size = new System.Drawing.Size(107, 21);
            this.price.TabIndex = 34;
            // 
            // lbMoneyAvabile
            // 
            this.lbMoneyAvabile.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbMoneyAvabile.Location = new System.Drawing.Point(63, 79);
            this.lbMoneyAvabile.Name = "lbMoneyAvabile";
            this.lbMoneyAvabile.Size = new System.Drawing.Size(106, 20);
            this.lbMoneyAvabile.TabIndex = 35;
            this.lbMoneyAvabile.Text = "label7";
            // 
            // lbMaxOrderVol
            // 
            this.lbMaxOrderVol.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbMaxOrderVol.Location = new System.Drawing.Point(63, 101);
            this.lbMaxOrderVol.Name = "lbMaxOrderVol";
            this.lbMaxOrderVol.Size = new System.Drawing.Size(106, 20);
            this.lbMaxOrderVol.TabIndex = 36;
            this.lbMaxOrderVol.Text = "label8";
            // 
            // size
            // 
            this.size.Location = new System.Drawing.Point(63, 132);
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(107, 21);
            this.size.TabIndex = 37;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(95, 157);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 38;
            this.btnSubmit.Text = "买入下单";
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(5, 157);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 39;
            this.btnReset.Text = "重置";
            this.btnReset.UseVisualStyleBackColor = true;
            // 
            // lbSymbolName
            // 
            this.lbSymbolName.AutoSize = true;
            this.lbSymbolName.BackColor = System.Drawing.Color.Transparent;
            this.lbSymbolName.Location = new System.Drawing.Point(175, 13);
            this.lbSymbolName.Name = "lbSymbolName";
            this.lbSymbolName.Size = new System.Drawing.Size(17, 12);
            this.lbSymbolName.TabIndex = 40;
            this.lbSymbolName.Text = "--";
            // 
            // ctOrderSenderSTK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.lbSymbolName);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.size);
            this.Controls.Add(this.lbMaxOrderVol);
            this.Controls.Add(this.lbMoneyAvabile);
            this.Controls.Add(this.price);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.symbol);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "ctOrderSenderSTK";
            this.Size = new System.Drawing.Size(211, 187);
            ((System.ComponentModel.ISupportInitialize)(this.price)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.size)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox symbol;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown price;
        private System.Windows.Forms.Label lbMoneyAvabile;
        private System.Windows.Forms.Label lbMaxOrderVol;
        private System.Windows.Forms.NumericUpDown size;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Label lbSymbolName;
    }
}
