namespace TradingLib.XTrader.Future
{
    partial class PageQry
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.rtAccountFinanceReport = new System.Windows.Forms.RichTextBox();
            this.p1TradingDay = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.p2TradingDay = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rtSettlement = new System.Windows.Forms.RichTextBox();
            this.btnQryAccountFinace = new TradingLib.XTrader.FButton();
            this.btnQrySettlement = new TradingLib.XTrader.FButton();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(951, 316);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.btnQryAccountFinace);
            this.tabPage1.Controls.Add(this.p1TradingDay);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(943, 290);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "资金状况";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.rtAccountFinanceReport);
            this.panel1.Location = new System.Drawing.Point(165, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(779, 280);
            this.panel1.TabIndex = 4;
            // 
            // rtAccountFinanceReport
            // 
            this.rtAccountFinanceReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtAccountFinanceReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtAccountFinanceReport.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtAccountFinanceReport.Location = new System.Drawing.Point(0, 0);
            this.rtAccountFinanceReport.Name = "rtAccountFinanceReport";
            this.rtAccountFinanceReport.Size = new System.Drawing.Size(777, 278);
            this.rtAccountFinanceReport.TabIndex = 0;
            this.rtAccountFinanceReport.Text = "";
            // 
            // p1TradingDay
            // 
            this.p1TradingDay.Enabled = false;
            this.p1TradingDay.Location = new System.Drawing.Point(67, 10);
            this.p1TradingDay.Name = "p1TradingDay";
            this.p1TradingDay.Size = new System.Drawing.Size(92, 21);
            this.p1TradingDay.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "查询日期";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Controls.Add(this.btnQrySettlement);
            this.tabPage2.Controls.Add(this.p2TradingDay);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(943, 290);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "结算单";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(943, 290);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "持仓明细";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(943, 290);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "历史成交";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(943, 290);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "出入金";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // p2TradingDay
            // 
            this.p2TradingDay.Location = new System.Drawing.Point(67, 10);
            this.p2TradingDay.Name = "p2TradingDay";
            this.p2TradingDay.Size = new System.Drawing.Size(92, 21);
            this.p2TradingDay.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "结算日期";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.rtSettlement);
            this.panel2.Location = new System.Drawing.Point(165, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(779, 281);
            this.panel2.TabIndex = 7;
            // 
            // rtSettlement
            // 
            this.rtSettlement.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtSettlement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtSettlement.Font = new System.Drawing.Font("宋体", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rtSettlement.Location = new System.Drawing.Point(0, 0);
            this.rtSettlement.Name = "rtSettlement";
            this.rtSettlement.Size = new System.Drawing.Size(777, 279);
            this.rtSettlement.TabIndex = 1;
            this.rtSettlement.Text = "";
            // 
            // btnQryAccountFinace
            // 
            this.btnQryAccountFinace.BackColor = System.Drawing.Color.White;
            this.btnQryAccountFinace.CheckButton = false;
            this.btnQryAccountFinace.Checked = false;
            this.btnQryAccountFinace.IsPriceOn = false;
            this.btnQryAccountFinace.Location = new System.Drawing.Point(5, 103);
            this.btnQryAccountFinace.Name = "btnQryAccountFinace";
            this.btnQryAccountFinace.OrderEntryButton = false;
            this.btnQryAccountFinace.PriceStr = "";
            this.btnQryAccountFinace.Size = new System.Drawing.Size(75, 23);
            this.btnQryAccountFinace.TabIndex = 3;
            this.btnQryAccountFinace.Text = "查询";
            this.btnQryAccountFinace.UseVisualStyleBackColor = false;
            // 
            // btnQrySettlement
            // 
            this.btnQrySettlement.BackColor = System.Drawing.Color.White;
            this.btnQrySettlement.CheckButton = false;
            this.btnQrySettlement.Checked = false;
            this.btnQrySettlement.IsPriceOn = false;
            this.btnQrySettlement.Location = new System.Drawing.Point(5, 103);
            this.btnQrySettlement.Name = "btnQrySettlement";
            this.btnQrySettlement.OrderEntryButton = false;
            this.btnQrySettlement.PriceStr = "";
            this.btnQrySettlement.Size = new System.Drawing.Size(75, 23);
            this.btnQrySettlement.TabIndex = 6;
            this.btnQrySettlement.Text = "查询";
            this.btnQrySettlement.UseVisualStyleBackColor = false;
            // 
            // PageQry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "PageQry";
            this.Size = new System.Drawing.Size(951, 316);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox p1TradingDay;
        private FButton btnQryAccountFinace;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox rtAccountFinanceReport;
        private FButton btnQrySettlement;
        private System.Windows.Forms.TextBox p2TradingDay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RichTextBox rtSettlement;
    }
}
