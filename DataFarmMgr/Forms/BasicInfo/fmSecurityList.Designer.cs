namespace TradingLib.DataFarmManager
{
    partial class fmSecurityList
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
            this.cbExchange = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnAddSecurity = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "交易所";
            // 
            // cbExchange
            // 
            this.cbExchange.FormattingEnabled = true;
            this.cbExchange.Location = new System.Drawing.Point(57, 8);
            this.cbExchange.Name = "cbExchange";
            this.cbExchange.Size = new System.Drawing.Size(121, 20);
            this.cbExchange.TabIndex = 1;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnAddSecurity);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.cbExchange);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(864, 35);
            this.panel1.TabIndex = 5;
            // 
            // btnAddSecurity
            // 
            this.btnAddSecurity.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddSecurity.Location = new System.Drawing.Point(781, 6);
            this.btnAddSecurity.Name = "btnAddSecurity";
            this.btnAddSecurity.Size = new System.Drawing.Size(75, 23);
            this.btnAddSecurity.TabIndex = 4;
            this.btnAddSecurity.Text = "添加合约";
            this.btnAddSecurity.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 35);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(864, 417);
            this.panel2.TabIndex = 6;
            // 
            // fmSecurityList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(864, 452);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmSecurityList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "合约列表";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbExchange;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnAddSecurity;
        private System.Windows.Forms.Panel panel2;
    }
}