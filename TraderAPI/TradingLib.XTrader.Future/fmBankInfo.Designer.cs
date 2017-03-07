namespace TradingLib.XTrader.Future
{
    partial class fmBankInfo
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
            this.name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.idcard = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.branch = new System.Windows.Forms.TextBox();
            this.acno = new System.Windows.Forms.TextBox();
            this.cbbank = new System.Windows.Forms.ComboBox();
            this.btnSubmit = new TradingLib.XTrader.FButton();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "姓名";
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(91, 9);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(100, 21);
            this.name.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 42);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "身份证";
            // 
            // idcard
            // 
            this.idcard.Location = new System.Drawing.Point(91, 39);
            this.idcard.Name = "idcard";
            this.idcard.Size = new System.Drawing.Size(189, 21);
            this.idcard.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "银行";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(25, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 5;
            this.label4.Text = "分行";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(25, 132);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "账号";
            // 
            // branch
            // 
            this.branch.Location = new System.Drawing.Point(91, 99);
            this.branch.Name = "branch";
            this.branch.Size = new System.Drawing.Size(189, 21);
            this.branch.TabIndex = 7;
            // 
            // acno
            // 
            this.acno.Location = new System.Drawing.Point(91, 129);
            this.acno.Name = "acno";
            this.acno.Size = new System.Drawing.Size(189, 21);
            this.acno.TabIndex = 8;
            // 
            // cbbank
            // 
            this.cbbank.FormattingEnabled = true;
            this.cbbank.Location = new System.Drawing.Point(91, 69);
            this.cbbank.Name = "cbbank";
            this.cbbank.Size = new System.Drawing.Size(121, 20);
            this.cbbank.TabIndex = 9;
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.White;
            this.btnSubmit.CheckButton = false;
            this.btnSubmit.Checked = false;
            this.btnSubmit.IsPriceOn = false;
            this.btnSubmit.Location = new System.Drawing.Point(191, 178);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.OrderEntryButton = false;
            this.btnSubmit.PriceStr = "";
            this.btnSubmit.Size = new System.Drawing.Size(89, 23);
            this.btnSubmit.TabIndex = 10;
            this.btnSubmit.Text = "提 交";
            this.btnSubmit.UseVisualStyleBackColor = false;
            // 
            // fmBankInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(339, 213);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.cbbank);
            this.Controls.Add(this.acno);
            this.Controls.Add(this.branch);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.idcard);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.name);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmBankInfo";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设定签约银行";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox idcard;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox branch;
        private System.Windows.Forms.TextBox acno;
        private System.Windows.Forms.ComboBox cbbank;
        private FButton btnSubmit;
    }
}