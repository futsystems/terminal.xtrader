namespace TradingLib.XTrader.Future
{
    partial class ctrlTrade
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnBySymbol = new System.Windows.Forms.RadioButton();
            this.btnByOrder = new System.Windows.Forms.RadioButton();
            this.btnDetail = new System.Windows.Forms.RadioButton();
            this.button6 = new TradingLib.XTrader.FButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.btnBySymbol);
            this.panel1.Controls.Add(this.btnByOrder);
            this.panel1.Controls.Add(this.btnDetail);
            this.panel1.Controls.Add(this.button6);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(802, 25);
            this.panel1.TabIndex = 2;
            // 
            // btnBySymbol
            // 
            this.btnBySymbol.AutoSize = true;
            this.btnBySymbol.Location = new System.Drawing.Point(170, 2);
            this.btnBySymbol.Name = "btnBySymbol";
            this.btnBySymbol.Size = new System.Drawing.Size(83, 16);
            this.btnBySymbol.TabIndex = 7;
            this.btnBySymbol.TabStop = true;
            this.btnBySymbol.Text = "按合约汇总";
            this.btnBySymbol.UseVisualStyleBackColor = true;
            // 
            // btnByOrder
            // 
            this.btnByOrder.AutoSize = true;
            this.btnByOrder.Location = new System.Drawing.Point(81, 2);
            this.btnByOrder.Name = "btnByOrder";
            this.btnByOrder.Size = new System.Drawing.Size(83, 16);
            this.btnByOrder.TabIndex = 6;
            this.btnByOrder.TabStop = true;
            this.btnByOrder.Text = "按委托汇总";
            this.btnByOrder.UseVisualStyleBackColor = true;
            // 
            // btnDetail
            // 
            this.btnDetail.AutoSize = true;
            this.btnDetail.Checked = true;
            this.btnDetail.Location = new System.Drawing.Point(4, 2);
            this.btnDetail.Name = "btnDetail";
            this.btnDetail.Size = new System.Drawing.Size(71, 16);
            this.btnDetail.TabIndex = 5;
            this.btnDetail.TabStop = true;
            this.btnDetail.Text = "成交明细";
            this.btnDetail.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button6.BackColor = System.Drawing.Color.White;
            this.button6.CheckButton = false;
            this.button6.Checked = false;
            this.button6.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button6.Location = new System.Drawing.Point(742, 2);
            this.button6.Name = "button6";
            this.button6.OrderEntryButton = false;
            this.button6.Price = new decimal(new int[] {
            122400,
            0,
            0,
            131072});
            this.button6.Size = new System.Drawing.Size(47, 20);
            this.button6.TabIndex = 4;
            this.button6.Text = "导出";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Cursor = System.Windows.Forms.Cursors.Default;
            this.panel2.Location = new System.Drawing.Point(1, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(800, 208);
            this.panel2.TabIndex = 3;
            // 
            // ctrlTrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ctrlTrade";
            this.Size = new System.Drawing.Size(802, 233);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private FButton button6;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton btnDetail;
        private System.Windows.Forms.RadioButton btnByOrder;
        private System.Windows.Forms.RadioButton btnBySymbol;
    }
}
