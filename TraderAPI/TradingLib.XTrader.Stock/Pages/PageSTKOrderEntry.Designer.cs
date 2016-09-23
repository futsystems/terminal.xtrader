namespace TradingLib.XTrader.Stock
{
    partial class PageSTKOrderEntry
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
            this.ctQuoteViewSTK1 = new TradingLib.XTrader.Stock.ctQuoteViewSTK();
            this.ctOrderSenderSTK1 = new TradingLib.XTrader.Stock.ctOrderSenderSTK();
            this.ctPositionViewSTK1 = new TradingLib.XTrader.Stock.ctPositionViewSTK();
            this.SuspendLayout();
            // 
            // ctQuoteViewSTK1
            // 
            this.ctQuoteViewSTK1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctQuoteViewSTK1.Location = new System.Drawing.Point(200, 0);
            this.ctQuoteViewSTK1.Margin = new System.Windows.Forms.Padding(0);
            this.ctQuoteViewSTK1.Name = "ctQuoteViewSTK1";
            this.ctQuoteViewSTK1.Size = new System.Drawing.Size(220, 250);
            this.ctQuoteViewSTK1.TabIndex = 1;
            // 
            // ctOrderSenderSTK1
            // 
            this.ctOrderSenderSTK1.BackColor = System.Drawing.Color.Transparent;
            this.ctOrderSenderSTK1.Location = new System.Drawing.Point(0, 0);
            this.ctOrderSenderSTK1.Name = "ctOrderSenderSTK1";
            this.ctOrderSenderSTK1.Side = true;
            this.ctOrderSenderSTK1.Size = new System.Drawing.Size(200, 212);
            this.ctOrderSenderSTK1.TabIndex = 1;
            // 
            // ctPositionViewSTK1
            // 
            this.ctPositionViewSTK1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctPositionViewSTK1.Location = new System.Drawing.Point(423, 0);
            this.ctPositionViewSTK1.Name = "ctPositionViewSTK1";
            this.ctPositionViewSTK1.RealView = true;
            this.ctPositionViewSTK1.Size = new System.Drawing.Size(764, 301);
            this.ctPositionViewSTK1.TabIndex = 2;
            // 
            // PageSTKOrderEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.ctPositionViewSTK1);
            this.Controls.Add(this.ctQuoteViewSTK1);
            this.Controls.Add(this.ctOrderSenderSTK1);
            this.Name = "PageSTKOrderEntry";
            this.Size = new System.Drawing.Size(1190, 301);
            this.ResumeLayout(false);

        }

        #endregion

        private TradingLib.XTrader.Stock.ctQuoteViewSTK ctQuoteViewSTK1;
        private TradingLib.XTrader.Stock.ctOrderSenderSTK ctOrderSenderSTK1;
        private ctPositionViewSTK ctPositionViewSTK1;

    }
}
