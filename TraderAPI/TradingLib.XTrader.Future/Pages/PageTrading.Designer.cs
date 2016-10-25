namespace TradingLib.XTrader.Future
{
    partial class PageTrading
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
            this.ctrlPosition1 = new TradingLib.XTrader.Future.ctrlPosition();
            this.ctrlOrder1 = new TradingLib.XTrader.Future.ctrlOrder();
            this.SuspendLayout();
            // 
            // ctrlPosition1
            // 
            this.ctrlPosition1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlPosition1.Location = new System.Drawing.Point(0, 0);
            this.ctrlPosition1.Name = "ctrlPosition1";
            this.ctrlPosition1.Size = new System.Drawing.Size(1054, 160);
            this.ctrlPosition1.TabIndex = 0;
            // 
            // ctrlOrder1
            // 
            this.ctrlOrder1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctrlOrder1.Location = new System.Drawing.Point(0, 162);
            this.ctrlOrder1.Name = "ctrlOrder1";
            this.ctrlOrder1.Size = new System.Drawing.Size(1054, 160);
            this.ctrlOrder1.TabIndex = 1;
            // 
            // PageTrading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctrlOrder1);
            this.Controls.Add(this.ctrlPosition1);
            this.Name = "PageTrading";
            this.Size = new System.Drawing.Size(1054, 322);
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlPosition ctrlPosition1;
        private ctrlOrder ctrlOrder1;
    }
}
