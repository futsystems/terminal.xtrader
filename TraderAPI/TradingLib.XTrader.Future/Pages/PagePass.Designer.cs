namespace TradingLib.XTrader.Future
{
    partial class PagePass
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
            this.btnChange = new TradingLib.XTrader.FButton();
            this.cbPassType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pass = new System.Windows.Forms.MaskedTextBox();
            this.newpass1 = new System.Windows.Forms.MaskedTextBox();
            this.newpass2 = new System.Windows.Forms.MaskedTextBox();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.newpass2);
            this.panel1.Controls.Add(this.newpass1);
            this.panel1.Controls.Add(this.pass);
            this.panel1.Controls.Add(this.btnChange);
            this.panel1.Controls.Add(this.cbPassType);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(1, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(870, 319);
            this.panel1.TabIndex = 26;
            // 
            // btnChange
            // 
            this.btnChange.BackColor = System.Drawing.Color.White;
            this.btnChange.CheckButton = false;
            this.btnChange.Checked = false;
            this.btnChange.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChange.IsPriceOn = false;
            this.btnChange.Location = new System.Drawing.Point(184, 124);
            this.btnChange.Name = "btnChange";
            this.btnChange.OrderEntryButton = false;
            this.btnChange.PriceStr = "";
            this.btnChange.Size = new System.Drawing.Size(75, 23);
            this.btnChange.TabIndex = 8;
            this.btnChange.Text = "修改";
            this.btnChange.UseVisualStyleBackColor = false;
            // 
            // cbPassType
            // 
            this.cbPassType.FormattingEnabled = true;
            this.cbPassType.Items.AddRange(new object[] {
            "交易密码"});
            this.cbPassType.Location = new System.Drawing.Point(129, 7);
            this.cbPassType.Name = "cbPassType";
            this.cbPassType.Size = new System.Drawing.Size(130, 20);
            this.cbPassType.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 100);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(95, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "再次输入新密码:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 70);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "新密码:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "当前密码:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "密码类型:";
            // 
            // pass
            // 
            this.pass.Location = new System.Drawing.Point(129, 37);
            this.pass.Name = "pass";
            this.pass.PasswordChar = '*';
            this.pass.Size = new System.Drawing.Size(130, 21);
            this.pass.TabIndex = 9;
            // 
            // newpass1
            // 
            this.newpass1.Location = new System.Drawing.Point(129, 67);
            this.newpass1.Name = "newpass1";
            this.newpass1.PasswordChar = '*';
            this.newpass1.Size = new System.Drawing.Size(130, 21);
            this.newpass1.TabIndex = 10;
            // 
            // newpass2
            // 
            this.newpass2.Location = new System.Drawing.Point(129, 97);
            this.newpass2.Name = "newpass2";
            this.newpass2.PasswordChar = '*';
            this.newpass2.Size = new System.Drawing.Size(130, 21);
            this.newpass2.TabIndex = 11;
            // 
            // PagePass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel1);
            this.Name = "PagePass";
            this.Size = new System.Drawing.Size(872, 319);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbPassType;
        private FButton btnChange;
        private System.Windows.Forms.MaskedTextBox newpass2;
        private System.Windows.Forms.MaskedTextBox newpass1;
        private System.Windows.Forms.MaskedTextBox pass;
    }
}
