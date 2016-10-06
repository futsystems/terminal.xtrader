namespace TradingLib.DataFarmManager
{
    partial class fmExchangeEdit
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
            this.label7 = new System.Windows.Forms.Label();
            this.code = new System.Windows.Forms.TextBox();
            this.name = new System.Windows.Forms.TextBox();
            this.title = new System.Windows.Forms.TextBox();
            this.cbCountry = new System.Windows.Forms.ComboBox();
            this.cbTimeZone = new System.Windows.Forms.ComboBox();
            this.calendar = new System.Windows.Forms.ComboBox();
            this.closeTime = new System.Windows.Forms.DateTimePicker();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(45, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "交易所编号:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(45, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "交易所名称:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "交易所简称:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(81, 98);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "国家:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(81, 123);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "时区:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(57, 148);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(59, 12);
            this.label6.TabIndex = 5;
            this.label6.Text = "交易日历:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(57, 176);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 6;
            this.label7.Text = "结算时间:";
            // 
            // code
            // 
            this.code.Location = new System.Drawing.Point(130, 20);
            this.code.Name = "code";
            this.code.Size = new System.Drawing.Size(131, 21);
            this.code.TabIndex = 7;
            // 
            // name
            // 
            this.name.Location = new System.Drawing.Point(130, 45);
            this.name.Name = "name";
            this.name.Size = new System.Drawing.Size(131, 21);
            this.name.TabIndex = 8;
            // 
            // title
            // 
            this.title.Location = new System.Drawing.Point(130, 70);
            this.title.Name = "title";
            this.title.Size = new System.Drawing.Size(131, 21);
            this.title.TabIndex = 9;
            // 
            // cbCountry
            // 
            this.cbCountry.FormattingEnabled = true;
            this.cbCountry.Location = new System.Drawing.Point(130, 95);
            this.cbCountry.Name = "cbCountry";
            this.cbCountry.Size = new System.Drawing.Size(131, 20);
            this.cbCountry.TabIndex = 10;
            // 
            // cbTimeZone
            // 
            this.cbTimeZone.FormattingEnabled = true;
            this.cbTimeZone.Location = new System.Drawing.Point(130, 120);
            this.cbTimeZone.Name = "cbTimeZone";
            this.cbTimeZone.Size = new System.Drawing.Size(131, 20);
            this.cbTimeZone.TabIndex = 11;
            // 
            // calendar
            // 
            this.calendar.FormattingEnabled = true;
            this.calendar.Location = new System.Drawing.Point(130, 145);
            this.calendar.Name = "calendar";
            this.calendar.Size = new System.Drawing.Size(131, 20);
            this.calendar.TabIndex = 12;
            // 
            // closeTime
            // 
            this.closeTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.closeTime.Location = new System.Drawing.Point(130, 170);
            this.closeTime.Name = "closeTime";
            this.closeTime.ShowUpDown = true;
            this.closeTime.Size = new System.Drawing.Size(131, 21);
            this.closeTime.TabIndex = 13;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Location = new System.Drawing.Point(247, 246);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 14;
            this.btnSubmit.Text = "提 交";
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // fmExchangeEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 281);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.closeTime);
            this.Controls.Add(this.calendar);
            this.Controls.Add(this.cbTimeZone);
            this.Controls.Add(this.cbCountry);
            this.Controls.Add(this.title);
            this.Controls.Add(this.name);
            this.Controls.Add(this.code);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmExchangeEdit";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "fmExchangeEdit";
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
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox code;
        private System.Windows.Forms.TextBox name;
        private System.Windows.Forms.TextBox title;
        private System.Windows.Forms.ComboBox cbCountry;
        private System.Windows.Forms.ComboBox cbTimeZone;
        private System.Windows.Forms.ComboBox calendar;
        private System.Windows.Forms.DateTimePicker closeTime;
        private System.Windows.Forms.Button btnSubmit;
    }
}