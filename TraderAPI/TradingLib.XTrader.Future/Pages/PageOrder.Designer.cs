namespace TradingLib.XTrader.Future
{
    partial class PageOrder
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
            this.ctrlOrder1 = new TradingLib.XTrader.Future.ctrlOrder();
            this.SuspendLayout();
            // 
            // ctrlOrder1
            // 
            this.ctrlOrder1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlOrder1.Location = new System.Drawing.Point(0, 0);
            this.ctrlOrder1.Name = "ctrlOrder1";
            this.ctrlOrder1.Size = new System.Drawing.Size(1055, 273);
            this.ctrlOrder1.TabIndex = 0;
            // 
            // PageOrder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctrlOrder1);
            this.Name = "PageOrder";
            this.Size = new System.Drawing.Size(1055, 273);
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlOrder ctrlOrder1;
    }
}
