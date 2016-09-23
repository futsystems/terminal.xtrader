namespace TradingLib.KryptonControl
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
            this.ctOrderViewSTK1 = new TradingLib.KryptonControl.ctOrderViewSTK();
            this.btnCancelAll = new System.Windows.Forms.Button();
            this.btnCancelSell = new System.Windows.Forms.Button();
            this.btnCancelBuy = new System.Windows.Forms.Button();
            this.fPanel1 = new TradingLib.KryptonControl.FPanel();
            this.fPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctOrderViewSTK1
            // 
            this.ctOrderViewSTK1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctOrderViewSTK1.Location = new System.Drawing.Point(0, 30);
            this.ctOrderViewSTK1.Name = "ctOrderViewSTK1";
            this.ctOrderViewSTK1.RealView = true;
            this.ctOrderViewSTK1.Size = new System.Drawing.Size(855, 234);
            this.ctOrderViewSTK1.TabIndex = 10;
            // 
            // btnCancelAll
            // 
            this.btnCancelAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelAll.Location = new System.Drawing.Point(615, 3);
            this.btnCancelAll.Name = "btnCancelAll";
            this.btnCancelAll.Size = new System.Drawing.Size(75, 23);
            this.btnCancelAll.TabIndex = 11;
            this.btnCancelAll.Text = "全 撤";
            this.btnCancelAll.UseVisualStyleBackColor = true;
            // 
            // btnCancelSell
            // 
            this.btnCancelSell.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelSell.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelSell.Location = new System.Drawing.Point(777, 3);
            this.btnCancelSell.Name = "btnCancelSell";
            this.btnCancelSell.Size = new System.Drawing.Size(75, 23);
            this.btnCancelSell.TabIndex = 12;
            this.btnCancelSell.Text = "全撤";
            this.btnCancelSell.UseVisualStyleBackColor = true;
            // 
            // btnCancelBuy
            // 
            this.btnCancelBuy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancelBuy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelBuy.Location = new System.Drawing.Point(696, 3);
            this.btnCancelBuy.Name = "btnCancelBuy";
            this.btnCancelBuy.Size = new System.Drawing.Size(75, 23);
            this.btnCancelBuy.TabIndex = 13;
            this.btnCancelBuy.Text = "撤 买";
            this.btnCancelBuy.UseVisualStyleBackColor = true;
            // 
            // fPanel1
            // 
            this.fPanel1.Controls.Add(this.ctOrderViewSTK1);
            this.fPanel1.Controls.Add(this.btnCancelSell);
            this.fPanel1.Controls.Add(this.btnCancelBuy);
            this.fPanel1.Controls.Add(this.btnCancelAll);
            this.fPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fPanel1.Location = new System.Drawing.Point(0, 0);
            this.fPanel1.Name = "fPanel1";
            this.fPanel1.Size = new System.Drawing.Size(855, 264);
            this.fPanel1.TabIndex = 1;
            // 
            // PageSTKOrderCancel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fPanel1);
            this.Name = "PageSTKOrderCancel";
            this.Size = new System.Drawing.Size(855, 264);
            this.fPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private TradingLib.KryptonControl.ctOrderViewSTK ctOrderViewSTK1;
        private System.Windows.Forms.Button btnCancelAll;
        private System.Windows.Forms.Button btnCancelBuy;
        private System.Windows.Forms.Button btnCancelSell;
        private FPanel fPanel1;
    }
}
