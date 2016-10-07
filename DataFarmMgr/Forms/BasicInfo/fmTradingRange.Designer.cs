namespace TradingLib.DataFarmManager
{
    partial class fmTradingRange
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
            this.startDay = new System.Windows.Forms.ComboBox();
            this.startTime = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.endTime = new System.Windows.Forms.DateTimePicker();
            this.endDay = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.settleFlag = new System.Windows.Forms.ComboBox();
            this.marketClose = new System.Windows.Forms.CheckBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "开始日期";
            // 
            // startDay
            // 
            this.startDay.FormattingEnabled = true;
            this.startDay.Location = new System.Drawing.Point(24, 29);
            this.startDay.Name = "startDay";
            this.startDay.Size = new System.Drawing.Size(76, 20);
            this.startDay.TabIndex = 1;
            // 
            // startTime
            // 
            this.startTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.startTime.Location = new System.Drawing.Point(106, 28);
            this.startTime.Name = "startTime";
            this.startTime.ShowUpDown = true;
            this.startTime.Size = new System.Drawing.Size(86, 21);
            this.startTime.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "开始时间";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(312, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "结束时间";
            // 
            // endTime
            // 
            this.endTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.endTime.Location = new System.Drawing.Point(314, 29);
            this.endTime.Name = "endTime";
            this.endTime.ShowUpDown = true;
            this.endTime.Size = new System.Drawing.Size(86, 21);
            this.endTime.TabIndex = 6;
            // 
            // endDay
            // 
            this.endDay.FormattingEnabled = true;
            this.endDay.Location = new System.Drawing.Point(232, 30);
            this.endDay.Name = "endDay";
            this.endDay.Size = new System.Drawing.Size(76, 20);
            this.endDay.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(230, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "结束日期";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(22, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "结算标识";
            // 
            // settleFlag
            // 
            this.settleFlag.FormattingEnabled = true;
            this.settleFlag.Location = new System.Drawing.Point(24, 78);
            this.settleFlag.Name = "settleFlag";
            this.settleFlag.Size = new System.Drawing.Size(76, 20);
            this.settleFlag.TabIndex = 9;
            // 
            // marketClose
            // 
            this.marketClose.AutoSize = true;
            this.marketClose.Location = new System.Drawing.Point(236, 78);
            this.marketClose.Name = "marketClose";
            this.marketClose.Size = new System.Drawing.Size(72, 16);
            this.marketClose.TabIndex = 10;
            this.marketClose.Text = "收盘小节";
            this.marketClose.UseVisualStyleBackColor = true;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(325, 110);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 11;
            this.btnSubmit.Text = "提 交";
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // fmTradingRange
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(416, 145);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.marketClose);
            this.Controls.Add(this.settleFlag);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.endTime);
            this.Controls.Add(this.endDay);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.startTime);
            this.Controls.Add(this.startDay);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmTradingRange";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "交易小节";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox startDay;
        private System.Windows.Forms.DateTimePicker startTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker endTime;
        private System.Windows.Forms.ComboBox endDay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox settleFlag;
        private System.Windows.Forms.CheckBox marketClose;
        private System.Windows.Forms.Button btnSubmit;
    }
}