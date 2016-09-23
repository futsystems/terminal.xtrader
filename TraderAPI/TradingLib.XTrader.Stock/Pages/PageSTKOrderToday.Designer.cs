namespace TradingLib.XTrader.Stock
{
    partial class PageSTKOrderToday
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
            this.ctOrderViewSTK1 = new TradingLib.XTrader.Stock.ctOrderViewSTK();
            this.SuspendLayout();
            // 
            // ctOrderViewSTK1
            // 
            this.ctOrderViewSTK1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctOrderViewSTK1.Location = new System.Drawing.Point(0, 0);
            this.ctOrderViewSTK1.Name = "ctOrderViewSTK1";
            this.ctOrderViewSTK1.Size = new System.Drawing.Size(650, 343);
            this.ctOrderViewSTK1.TabIndex = 0;
            // 
            // PageSTKOrderToday
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctOrderViewSTK1);
            this.Name = "PageSTKOrderToday";
            this.Size = new System.Drawing.Size(650, 343);
            this.ResumeLayout(false);

        }

        #endregion

        private TradingLib.XTrader.Stock.ctOrderViewSTK ctOrderViewSTK1;
    }
}
