namespace TradingLib.DataFarmManager
{
    partial class fmSymbolEdit
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbSymbol = new System.Windows.Forms.Label();
            this.cbExchange = new System.Windows.Forms.ComboBox();
            this.cbSecurity = new System.Windows.Forms.ComboBox();
            this.cbMonth = new System.Windows.Forms.ComboBox();
            this.expiredate = new System.Windows.Forms.DateTimePicker();
            this.cbSymbolType = new System.Windows.Forms.ComboBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "合约:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "交易所:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(40, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "品种:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 77);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "月份:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(28, 98);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "到期日:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(40, 121);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "类别:";
            // 
            // lbSymbol
            // 
            this.lbSymbol.AutoSize = true;
            this.lbSymbol.Location = new System.Drawing.Point(94, 9);
            this.lbSymbol.Name = "lbSymbol";
            this.lbSymbol.Size = new System.Drawing.Size(17, 12);
            this.lbSymbol.TabIndex = 6;
            this.lbSymbol.Text = "--";
            // 
            // cbExchange
            // 
            this.cbExchange.FormattingEnabled = true;
            this.cbExchange.Location = new System.Drawing.Point(96, 32);
            this.cbExchange.Name = "cbExchange";
            this.cbExchange.Size = new System.Drawing.Size(121, 20);
            this.cbExchange.TabIndex = 7;
            // 
            // cbSecurity
            // 
            this.cbSecurity.FormattingEnabled = true;
            this.cbSecurity.Location = new System.Drawing.Point(96, 58);
            this.cbSecurity.Name = "cbSecurity";
            this.cbSecurity.Size = new System.Drawing.Size(121, 20);
            this.cbSecurity.TabIndex = 8;
            // 
            // cbMonth
            // 
            this.cbMonth.FormattingEnabled = true;
            this.cbMonth.Location = new System.Drawing.Point(96, 84);
            this.cbMonth.Name = "cbMonth";
            this.cbMonth.Size = new System.Drawing.Size(121, 20);
            this.cbMonth.TabIndex = 9;
            // 
            // expiredate
            // 
            this.expiredate.Location = new System.Drawing.Point(96, 111);
            this.expiredate.Name = "expiredate";
            this.expiredate.Size = new System.Drawing.Size(121, 21);
            this.expiredate.TabIndex = 10;
            // 
            // cbSymbolType
            // 
            this.cbSymbolType.FormattingEnabled = true;
            this.cbSymbolType.Location = new System.Drawing.Point(96, 138);
            this.cbSymbolType.Name = "cbSymbolType";
            this.cbSymbolType.Size = new System.Drawing.Size(121, 20);
            this.cbSymbolType.TabIndex = 11;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(176, 202);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 12;
            this.btnSubmit.Text = "提 交";
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // fmSymbolEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(263, 237);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.cbSymbolType);
            this.Controls.Add(this.expiredate);
            this.Controls.Add(this.cbMonth);
            this.Controls.Add(this.cbSecurity);
            this.Controls.Add(this.cbExchange);
            this.Controls.Add(this.lbSymbol);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmSymbolEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "添加合约";
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
        private System.Windows.Forms.Label lbSymbol;
        private System.Windows.Forms.ComboBox cbExchange;
        private System.Windows.Forms.ComboBox cbSecurity;
        private System.Windows.Forms.ComboBox cbMonth;
        private System.Windows.Forms.DateTimePicker expiredate;
        private System.Windows.Forms.ComboBox cbSymbolType;
        private System.Windows.Forms.Button btnSubmit;
    }
}