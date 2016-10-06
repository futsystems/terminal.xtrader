namespace TradingLib.DataFarmManager
{
    partial class fmMarketTimeEdit
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
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnDel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lbDesp = new System.Windows.Forms.Label();
            this.lbMTName = new System.Windows.Forms.Label();
            this.kryptonLabel3 = new System.Windows.Forms.Label();
            this.kryptonLabel2 = new System.Windows.Forms.Label();
            this.kryptonLabel1 = new System.Windows.Forms.Label();
            this.kryptonLabel7 = new System.Windows.Forms.Label();
            this.closetime = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(328, 378);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(70, 25);
            this.btnSubmit.TabIndex = 10;
            this.btnSubmit.Text = "更 新";
            // 
            // btnDel
            // 
            this.btnDel.Location = new System.Drawing.Point(328, 328);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(70, 25);
            this.btnDel.TabIndex = 7;
            this.btnDel.Text = "删 除";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(248, 328);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(70, 25);
            this.btnAdd.TabIndex = 6;
            this.btnAdd.Text = "添 加";
            // 
            // lbDesp
            // 
            this.lbDesp.AutoSize = true;
            this.lbDesp.Location = new System.Drawing.Point(70, 37);
            this.lbDesp.Name = "lbDesp";
            this.lbDesp.Size = new System.Drawing.Size(17, 12);
            this.lbDesp.TabIndex = 4;
            this.lbDesp.Text = "--";
            // 
            // lbMTName
            // 
            this.lbMTName.AutoSize = true;
            this.lbMTName.Location = new System.Drawing.Point(70, 13);
            this.lbMTName.Name = "lbMTName";
            this.lbMTName.Size = new System.Drawing.Size(17, 12);
            this.lbMTName.TabIndex = 3;
            this.lbMTName.Text = "--";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(9, 61);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(51, 20);
            this.kryptonLabel3.TabIndex = 2;
            this.kryptonLabel3.Text = "时间段:";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(23, 37);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(38, 20);
            this.kryptonLabel2.TabIndex = 1;
            this.kryptonLabel2.Text = "描述:";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(23, 13);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(38, 20);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Text = "名称:";
            // 
            // kryptonLabel7
            // 
            this.kryptonLabel7.Location = new System.Drawing.Point(189, 14);
            this.kryptonLabel7.Name = "kryptonLabel7";
            this.kryptonLabel7.Size = new System.Drawing.Size(63, 20);
            this.kryptonLabel7.TabIndex = 16;
            this.kryptonLabel7.Text = "收盘时间:";
            // 
            // closetime
            // 
            this.closetime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.closetime.Location = new System.Drawing.Point(259, 13);
            this.closetime.Name = "closetime";
            this.closetime.ShowUpDown = true;
            this.closetime.Size = new System.Drawing.Size(100, 21);
            this.closetime.TabIndex = 15;
            // 
            // fmMarketTimeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(410, 416);
            this.Controls.Add(this.kryptonLabel7);
            this.Controls.Add(this.closetime);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.lbDesp);
            this.Controls.Add(this.lbMTName);
            this.Controls.Add(this.kryptonLabel3);
            this.Controls.Add(this.kryptonLabel2);
            this.Controls.Add(this.kryptonLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmMarketTimeEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "交易时间段";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbDesp;
        private System.Windows.Forms.Label lbMTName;
        private System.Windows.Forms.Label kryptonLabel3;
        private System.Windows.Forms.Label kryptonLabel2;
        private System.Windows.Forms.Label kryptonLabel1;
        private System.Windows.Forms.Button btnDel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label kryptonLabel7;
        private System.Windows.Forms.DateTimePicker closetime;
    }
}