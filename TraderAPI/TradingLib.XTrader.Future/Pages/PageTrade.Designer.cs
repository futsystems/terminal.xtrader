namespace TradingLib.XTrader.Future
{
    partial class PageTrade
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
            this.ctrlTrade1 = new TradingLib.XTrader.Future.ctrlTrade();
            this.SuspendLayout();
            // 
            // ctrlTrade1
            // 
            this.ctrlTrade1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlTrade1.Location = new System.Drawing.Point(0, 0);
            this.ctrlTrade1.Name = "ctrlTrade1";
            this.ctrlTrade1.Size = new System.Drawing.Size(930, 282);
            this.ctrlTrade1.TabIndex = 0;
            // 
            // PageTrade
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctrlTrade1);
            this.Name = "PageTrade";
            this.Size = new System.Drawing.Size(930, 282);
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlTrade ctrlTrade1;
    }
}
