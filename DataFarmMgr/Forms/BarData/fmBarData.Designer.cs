namespace TradingLib.DataFarmManager
{
    partial class fmBarData
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.btnUpload = new System.Windows.Forms.Button();
            this.havePartial = new System.Windows.Forms.CheckBox();
            this.fromEnd = new System.Windows.Forms.CheckBox();
            this.startIndex = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.maxCount = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.cbBarFrequency = new System.Windows.Forms.ComboBox();
            this.btnQry = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbSymbol = new System.Windows.Forms.ComboBox();
            this.cbSecurity = new System.Windows.Forms.ComboBox();
            this.cbExchange = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxCount)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnUpload);
            this.panel1.Controls.Add(this.havePartial);
            this.panel1.Controls.Add(this.fromEnd);
            this.panel1.Controls.Add(this.startIndex);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.maxCount);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.dateTimePicker1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.cbBarFrequency);
            this.panel1.Controls.Add(this.btnQry);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cbSymbol);
            this.panel1.Controls.Add(this.cbSecurity);
            this.panel1.Controls.Add(this.cbExchange);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(748, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(304, 574);
            this.panel1.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 335);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 17;
            this.label7.Text = "返回最大数量";
            // 
            // btnUpload
            // 
            this.btnUpload.Location = new System.Drawing.Point(190, 463);
            this.btnUpload.Name = "btnUpload";
            this.btnUpload.Size = new System.Drawing.Size(75, 23);
            this.btnUpload.TabIndex = 16;
            this.btnUpload.Text = "上 传";
            this.btnUpload.UseVisualStyleBackColor = true;
            // 
            // havePartial
            // 
            this.havePartial.AutoSize = true;
            this.havePartial.Location = new System.Drawing.Point(213, 147);
            this.havePartial.Name = "havePartial";
            this.havePartial.Size = new System.Drawing.Size(90, 16);
            this.havePartial.TabIndex = 15;
            this.havePartial.Text = "HavePartial";
            this.havePartial.UseVisualStyleBackColor = true;
            // 
            // fromEnd
            // 
            this.fromEnd.AutoSize = true;
            this.fromEnd.Location = new System.Drawing.Point(213, 125);
            this.fromEnd.Name = "fromEnd";
            this.fromEnd.Size = new System.Drawing.Size(66, 16);
            this.fromEnd.TabIndex = 14;
            this.fromEnd.Text = "FromEnd";
            this.fromEnd.UseVisualStyleBackColor = true;
            // 
            // startIndex
            // 
            this.startIndex.Location = new System.Drawing.Point(114, 125);
            this.startIndex.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.startIndex.Name = "startIndex";
            this.startIndex.Size = new System.Drawing.Size(70, 21);
            this.startIndex.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(112, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "起始位置";
            // 
            // maxCount
            // 
            this.maxCount.Location = new System.Drawing.Point(14, 125);
            this.maxCount.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.maxCount.Name = "maxCount";
            this.maxCount.Size = new System.Drawing.Size(81, 21);
            this.maxCount.TabIndex = 11;
            this.maxCount.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "返回最大数量";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimePicker1.Location = new System.Drawing.Point(8, 71);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.ShowCheckBox = true;
            this.dateTimePicker1.ShowUpDown = true;
            this.dateTimePicker1.Size = new System.Drawing.Size(189, 21);
            this.dateTimePicker1.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(203, 57);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "周期";
            // 
            // cbBarFrequency
            // 
            this.cbBarFrequency.FormattingEnabled = true;
            this.cbBarFrequency.Location = new System.Drawing.Point(203, 72);
            this.cbBarFrequency.Name = "cbBarFrequency";
            this.cbBarFrequency.Size = new System.Drawing.Size(89, 20);
            this.cbBarFrequency.TabIndex = 7;
            // 
            // btnQry
            // 
            this.btnQry.Location = new System.Drawing.Point(214, 196);
            this.btnQry.Name = "btnQry";
            this.btnQry.Size = new System.Drawing.Size(75, 23);
            this.btnQry.TabIndex = 6;
            this.btnQry.Text = "查 询";
            this.btnQry.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(203, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "合约";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(110, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "品种";
            // 
            // cbSymbol
            // 
            this.cbSymbol.FormattingEnabled = true;
            this.cbSymbol.Location = new System.Drawing.Point(205, 25);
            this.cbSymbol.Name = "cbSymbol";
            this.cbSymbol.Size = new System.Drawing.Size(87, 20);
            this.cbSymbol.TabIndex = 3;
            // 
            // cbSecurity
            // 
            this.cbSecurity.FormattingEnabled = true;
            this.cbSecurity.Location = new System.Drawing.Point(112, 25);
            this.cbSecurity.Name = "cbSecurity";
            this.cbSecurity.Size = new System.Drawing.Size(87, 20);
            this.cbSecurity.TabIndex = 2;
            // 
            // cbExchange
            // 
            this.cbExchange.FormattingEnabled = true;
            this.cbExchange.Location = new System.Drawing.Point(8, 25);
            this.cbExchange.Name = "cbExchange";
            this.cbExchange.Size = new System.Drawing.Size(98, 20);
            this.cbExchange.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "交易所";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(748, 574);
            this.panel2.TabIndex = 1;
            // 
            // fmBarData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1052, 574);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "fmBarData";
            this.Text = "fmBarData";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.startIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.maxCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cbSymbol;
        private System.Windows.Forms.ComboBox cbSecurity;
        private System.Windows.Forms.ComboBox cbExchange;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbBarFrequency;
        private System.Windows.Forms.Button btnQry;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.NumericUpDown maxCount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown startIndex;
        private System.Windows.Forms.CheckBox fromEnd;
        private System.Windows.Forms.CheckBox havePartial;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnUpload;
    }
}