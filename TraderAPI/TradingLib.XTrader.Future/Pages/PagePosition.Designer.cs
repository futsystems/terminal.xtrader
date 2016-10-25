namespace TradingLib.XTrader.Future
{
    partial class PagePosition
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
            this.SuspendLayout();
            // 
            // ctrlPosition1
            // 
            this.ctrlPosition1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlPosition1.Location = new System.Drawing.Point(0, 0);
            this.ctrlPosition1.Name = "ctrlPosition1";
            this.ctrlPosition1.Size = new System.Drawing.Size(837, 271);
            this.ctrlPosition1.TabIndex = 0;
            // 
            // PagePosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctrlPosition1);
            this.Name = "PagePosition";
            this.Size = new System.Drawing.Size(837, 271);
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlPosition ctrlPosition1;
    }
}
