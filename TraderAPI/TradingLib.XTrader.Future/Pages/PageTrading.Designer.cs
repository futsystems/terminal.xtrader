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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.ctrlPosition1 = new TradingLib.XTrader.Future.ctrlPosition();
            this.ctrlOrder1 = new TradingLib.XTrader.Future.ctrlOrder();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.White;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.ctrlPosition1);
            this.splitContainer1.Panel1MinSize = 50;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ctrlOrder1);
            this.splitContainer1.Panel2MinSize = 50;
            this.splitContainer1.Size = new System.Drawing.Size(1079, 244);
            this.splitContainer1.SplitterDistance = 108;
            this.splitContainer1.TabIndex = 2;
            // 
            // ctrlPosition1
            // 
            this.ctrlPosition1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlPosition1.Location = new System.Drawing.Point(0, 0);
            this.ctrlPosition1.Name = "ctrlPosition1";
            this.ctrlPosition1.RealView = true;
            this.ctrlPosition1.Size = new System.Drawing.Size(1079, 108);
            this.ctrlPosition1.TabIndex = 0;
            // 
            // ctrlOrder1
            // 
            this.ctrlOrder1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlOrder1.Location = new System.Drawing.Point(0, 0);
            this.ctrlOrder1.Name = "ctrlOrder1";
            this.ctrlOrder1.RealView = true;
            this.ctrlOrder1.Size = new System.Drawing.Size(1079, 132);
            this.ctrlOrder1.TabIndex = 1;
            // 
            // PageTrading
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitContainer1);
            this.Name = "PageTrading";
            this.Size = new System.Drawing.Size(1079, 244);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlPosition ctrlPosition1;
        private ctrlOrder ctrlOrder1;
        private System.Windows.Forms.SplitContainer splitContainer1;
    }
}
