namespace StockTrader
{
    partial class PageSTKOrderCancel
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
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.ctOrderViewSTK1 = new TradingLib.KryptonControl.ctOrderViewSTK();
            this.btnCancelSell = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancelBuy = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnCancelAll = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.ctOrderViewSTK1);
            this.kryptonPanel1.Controls.Add(this.btnCancelSell);
            this.kryptonPanel1.Controls.Add(this.btnCancelBuy);
            this.kryptonPanel1.Controls.Add(this.btnCancelAll);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(709, 341);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // ctOrderViewSTK1
            // 
            this.ctOrderViewSTK1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctOrderViewSTK1.Location = new System.Drawing.Point(0, 34);
            this.ctOrderViewSTK1.Name = "ctOrderViewSTK1";
            this.ctOrderViewSTK1.RealView = true;
            this.ctOrderViewSTK1.Size = new System.Drawing.Size(709, 307);
            this.ctOrderViewSTK1.TabIndex = 10;
            // 
            // btnCancelSell
            // 
            this.btnCancelSell.Location = new System.Drawing.Point(155, 3);
            this.btnCancelSell.Name = "btnCancelSell";
            this.btnCancelSell.Size = new System.Drawing.Size(70, 25);
            this.btnCancelSell.TabIndex = 9;
            this.btnCancelSell.Values.Text = "撤 卖";
            // 
            // btnCancelBuy
            // 
            this.btnCancelBuy.Location = new System.Drawing.Point(79, 3);
            this.btnCancelBuy.Name = "btnCancelBuy";
            this.btnCancelBuy.Size = new System.Drawing.Size(70, 25);
            this.btnCancelBuy.TabIndex = 8;
            this.btnCancelBuy.Values.Text = "撤 买";
            // 
            // btnCancelAll
            // 
            this.btnCancelAll.Location = new System.Drawing.Point(3, 3);
            this.btnCancelAll.Name = "btnCancelAll";
            this.btnCancelAll.Size = new System.Drawing.Size(70, 25);
            this.btnCancelAll.TabIndex = 7;
            this.btnCancelAll.Values.Text = "全 撤";
            // 
            // PageSTKOrderCancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "PageSTKOrderCancel";
            this.Size = new System.Drawing.Size(709, 341);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancelSell;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancelBuy;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancelAll;
        private TradingLib.KryptonControl.ctOrderViewSTK ctOrderViewSTK1;
    }
}
