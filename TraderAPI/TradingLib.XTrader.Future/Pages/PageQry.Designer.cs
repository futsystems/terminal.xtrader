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
            this.rtAccountFinanceReport = new ICSharpCode.TextEditor.TextEditorControl();
            this.btnQryAccountFinace = new TradingLib.XTrader.FButton();
            this.p1TradingDay = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rtSettlement = new ICSharpCode.TextEditor.TextEditorControl();
            this.p2TradingDay = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnQrySettlement = new TradingLib.XTrader.FButton();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rtPositionDetails = new ICSharpCode.TextEditor.TextEditorControl();
            this.btnQryPositionDetail = new TradingLib.XTrader.FButton();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
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
            this.rtAccountFinanceReport.AutoScroll = true;
            this.rtAccountFinanceReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtAccountFinanceReport.Enabled = false;
            this.rtAccountFinanceReport.IndentStyle = ICSharpCode.TextEditor.Document.IndentStyle.None;
            this.rtAccountFinanceReport.IsReadOnly = false;
            this.rtAccountFinanceReport.Location = new System.Drawing.Point(0, 0);
            this.rtAccountFinanceReport.Margin = new System.Windows.Forms.Padding(0);
            this.rtAccountFinanceReport.Name = "rtAccountFinanceReport";
            this.rtAccountFinanceReport.ShowLineNumbers = false;
            this.rtAccountFinanceReport.ShowVRuler = false;
            this.rtAccountFinanceReport.Size = new System.Drawing.Size(777, 278);
            this.rtAccountFinanceReport.TabIndex = 9;
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
            this.tabPage2.Controls.Add(this.p2TradingDay);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.btnQrySettlement);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(943, 290);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "结算单";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.rtSettlement);
            this.panel2.Location = new System.Drawing.Point(165, 10);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(778, 280);
            this.panel2.TabIndex = 7;
            // 
            // rtSettlement
            // 
            this.rtSettlement.AutoScroll = true;
            this.rtSettlement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtSettlement.Enabled = false;
            this.rtSettlement.IndentStyle = ICSharpCode.TextEditor.Document.IndentStyle.None;
            this.rtSettlement.IsReadOnly = false;
            this.rtSettlement.Location = new System.Drawing.Point(0, 0);
            this.rtSettlement.Margin = new System.Windows.Forms.Padding(0);
            this.rtSettlement.Name = "rtSettlement";
            this.rtSettlement.ShowLineNumbers = false;
            this.rtSettlement.ShowVRuler = false;
            this.rtSettlement.Size = new System.Drawing.Size(776, 278);
            this.rtSettlement.TabIndex = 8;
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
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel3);
            this.tabPage3.Controls.Add(this.btnQryPositionDetail);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(943, 290);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "持仓明细";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.rtPositionDetails);
            this.panel3.Location = new System.Drawing.Point(165, 10);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(778, 280);
            this.panel3.TabIndex = 8;
            // 
            // rtPositionDetails
            // 
            this.rtPositionDetails.AutoScroll = true;
            this.rtPositionDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtPositionDetails.Enabled = false;
            this.rtPositionDetails.IndentStyle = ICSharpCode.TextEditor.Document.IndentStyle.None;
            this.rtPositionDetails.IsReadOnly = false;
            this.rtPositionDetails.Location = new System.Drawing.Point(0, 0);
            this.rtPositionDetails.Margin = new System.Windows.Forms.Padding(0);
            this.rtPositionDetails.Name = "rtPositionDetails";
            this.rtPositionDetails.ShowLineNumbers = false;
            this.rtPositionDetails.ShowVRuler = false;
            this.rtPositionDetails.Size = new System.Drawing.Size(776, 278);
            this.rtPositionDetails.TabIndex = 9;
            // 
            // btnQryPositionDetail
            // 
            this.btnQryPositionDetail.BackColor = System.Drawing.Color.White;
            this.btnQryPositionDetail.CheckButton = false;
            this.btnQryPositionDetail.Checked = false;
            this.btnQryPositionDetail.IsPriceOn = false;
            this.btnQryPositionDetail.Location = new System.Drawing.Point(5, 103);
            this.btnQryPositionDetail.Name = "btnQryPositionDetail";
            this.btnQryPositionDetail.OrderEntryButton = false;
            this.btnQryPositionDetail.PriceStr = "";
            this.btnQryPositionDetail.Size = new System.Drawing.Size(75, 23);
            this.btnQryPositionDetail.TabIndex = 7;
            this.btnQryPositionDetail.Text = "查询";
            this.btnQryPositionDetail.UseVisualStyleBackColor = false;
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
            this.tabPage3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox p1TradingDay;
        private FButton btnQryAccountFinace;
        private System.Windows.Forms.Panel panel1;
        private FButton btnQrySettlement;
        private System.Windows.Forms.TextBox p2TradingDay;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel2;
        private FButton btnQryPositionDetail;
        private System.Windows.Forms.Panel panel3;
        private ICSharpCode.TextEditor.TextEditorControl rtSettlement;
        private ICSharpCode.TextEditor.TextEditorControl rtAccountFinanceReport;
        private ICSharpCode.TextEditor.TextEditorControl rtPositionDetails;
    }
}
