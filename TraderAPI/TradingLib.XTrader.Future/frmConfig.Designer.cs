namespace TradingLib.XTrader
{
    partial class frmConfig
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.cbExSendOrderDirect = new System.Windows.Forms.CheckBox();
            this.cbExSwitchToOpenWhenCloseOrderSubmit = new System.Windows.Forms.CheckBox();
            this.cbExDoubleOrderFilledEntryClosePosition = new System.Windows.Forms.CheckBox();
            this.cbExDoubleOrderCancelIfNotFilled = new System.Windows.Forms.CheckBox();
            this.cbExSwitchSymbolOfMarketDataView = new System.Windows.Forms.CheckBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.cbExPositionLine = new System.Windows.Forms.CheckBox();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(565, 301);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(565, 301);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.cbExPositionLine);
            this.tabPage1.Controls.Add(this.cbExSendOrderDirect);
            this.tabPage1.Controls.Add(this.cbExSwitchToOpenWhenCloseOrderSubmit);
            this.tabPage1.Controls.Add(this.cbExDoubleOrderFilledEntryClosePosition);
            this.tabPage1.Controls.Add(this.cbExDoubleOrderCancelIfNotFilled);
            this.tabPage1.Controls.Add(this.cbExSwitchSymbolOfMarketDataView);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(557, 275);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "交易界面";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // cbExSendOrderDirect
            // 
            this.cbExSendOrderDirect.AutoSize = true;
            this.cbExSendOrderDirect.Location = new System.Drawing.Point(9, 117);
            this.cbExSendOrderDirect.Name = "cbExSendOrderDirect";
            this.cbExSendOrderDirect.Size = new System.Drawing.Size(72, 16);
            this.cbExSendOrderDirect.TabIndex = 4;
            this.cbExSendOrderDirect.Text = "一键下单";
            this.cbExSendOrderDirect.UseVisualStyleBackColor = true;
            // 
            // cbExSwitchToOpenWhenCloseOrderSubmit
            // 
            this.cbExSwitchToOpenWhenCloseOrderSubmit.AutoSize = true;
            this.cbExSwitchToOpenWhenCloseOrderSubmit.Location = new System.Drawing.Point(9, 73);
            this.cbExSwitchToOpenWhenCloseOrderSubmit.Name = "cbExSwitchToOpenWhenCloseOrderSubmit";
            this.cbExSwitchToOpenWhenCloseOrderSubmit.Size = new System.Drawing.Size(222, 16);
            this.cbExSwitchToOpenWhenCloseOrderSubmit.TabIndex = 3;
            this.cbExSwitchToOpenWhenCloseOrderSubmit.Text = "发出平仓 平今委托后切换回开仓状态";
            this.cbExSwitchToOpenWhenCloseOrderSubmit.UseVisualStyleBackColor = true;
            // 
            // cbExDoubleOrderFilledEntryClosePosition
            // 
            this.cbExDoubleOrderFilledEntryClosePosition.AutoSize = true;
            this.cbExDoubleOrderFilledEntryClosePosition.Location = new System.Drawing.Point(9, 51);
            this.cbExDoubleOrderFilledEntryClosePosition.Name = "cbExDoubleOrderFilledEntryClosePosition";
            this.cbExDoubleOrderFilledEntryClosePosition.Size = new System.Drawing.Size(228, 16);
            this.cbExDoubleOrderFilledEntryClosePosition.TabIndex = 2;
            this.cbExDoubleOrderFilledEntryClosePosition.Text = "双击已完成的开仓委托时进入平仓界面";
            this.cbExDoubleOrderFilledEntryClosePosition.UseVisualStyleBackColor = true;
            // 
            // cbExDoubleOrderCancelIfNotFilled
            // 
            this.cbExDoubleOrderCancelIfNotFilled.AutoSize = true;
            this.cbExDoubleOrderCancelIfNotFilled.Location = new System.Drawing.Point(9, 29);
            this.cbExDoubleOrderCancelIfNotFilled.Name = "cbExDoubleOrderCancelIfNotFilled";
            this.cbExDoubleOrderCancelIfNotFilled.Size = new System.Drawing.Size(222, 16);
            this.cbExDoubleOrderCancelIfNotFilled.TabIndex = 1;
            this.cbExDoubleOrderCancelIfNotFilled.Text = "双击未完成的委托直接撤单,无需确认";
            this.cbExDoubleOrderCancelIfNotFilled.UseVisualStyleBackColor = true;
            // 
            // cbExSwitchSymbolOfMarketDataView
            // 
            this.cbExSwitchSymbolOfMarketDataView.AutoSize = true;
            this.cbExSwitchSymbolOfMarketDataView.Location = new System.Drawing.Point(9, 7);
            this.cbExSwitchSymbolOfMarketDataView.Name = "cbExSwitchSymbolOfMarketDataView";
            this.cbExSwitchSymbolOfMarketDataView.Size = new System.Drawing.Size(168, 16);
            this.cbExSwitchSymbolOfMarketDataView.TabIndex = 0;
            this.cbExSwitchSymbolOfMarketDataView.Text = "同步切换行情窗口中的合约";
            this.cbExSwitchSymbolOfMarketDataView.UseVisualStyleBackColor = true;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(478, 307);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "确 认";
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // cbExPositionLine
            // 
            this.cbExPositionLine.AutoSize = true;
            this.cbExPositionLine.Location = new System.Drawing.Point(9, 95);
            this.cbExPositionLine.Name = "cbExPositionLine";
            this.cbExPositionLine.Size = new System.Drawing.Size(156, 16);
            this.cbExPositionLine.TabIndex = 5;
            this.cbExPositionLine.Text = "行情窗口显示持仓成本线";
            this.cbExPositionLine.UseVisualStyleBackColor = true;
            // 
            // frmConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(565, 336);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.panel1);
            this.Name = "frmConfig";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "交易设置";
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.CheckBox cbExSwitchSymbolOfMarketDataView;
        private System.Windows.Forms.CheckBox cbExDoubleOrderCancelIfNotFilled;
        private System.Windows.Forms.CheckBox cbExDoubleOrderFilledEntryClosePosition;
        private System.Windows.Forms.CheckBox cbExSwitchToOpenWhenCloseOrderSubmit;
        private System.Windows.Forms.CheckBox cbExSendOrderDirect;
        private System.Windows.Forms.CheckBox cbExPositionLine;
    }
}