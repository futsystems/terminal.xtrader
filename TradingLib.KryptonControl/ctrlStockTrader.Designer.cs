namespace TradingLib.KryptonControl
{
    partial class ctrlStockTrader
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctrlStockTrader));
            this.Holder = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.menuTree = new ComponentFactory.Krypton.Toolkit.KryptonTreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.mainPanel = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            ((System.ComponentModel.ISupportInitialize)(this.Holder)).BeginInit();
            this.Holder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // Holder
            // 
            this.Holder.Controls.Add(this.mainPanel);
            this.Holder.Controls.Add(this.menuTree);
            this.Holder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Holder.Location = new System.Drawing.Point(0, 0);
            this.Holder.Name = "Holder";
            this.Holder.Size = new System.Drawing.Size(1006, 307);
            this.Holder.TabIndex = 0;
            // 
            // menuTree
            // 
            this.menuTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.menuTree.Location = new System.Drawing.Point(0, 0);
            this.menuTree.Name = "menuTree";
            this.menuTree.Size = new System.Drawing.Size(160, 307);
            this.menuTree.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(227)))), ((int)(((byte)(230)))), ((int)(((byte)(232)))));
            this.menuTree.TabIndex = 1;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "toolbar_exit.png");
            this.imageList1.Images.SetKeyName(1, "01_buy.png");
            this.imageList1.Images.SetKeyName(2, "02_sell.png");
            this.imageList1.Images.SetKeyName(3, "03_cancel.png");
            this.imageList1.Images.SetKeyName(4, "04_buysell.png");
            this.imageList1.Images.SetKeyName(5, "05_qry.png");
            this.imageList1.Images.SetKeyName(6, "06_qry_item.png");
            this.imageList1.Images.SetKeyName(7, "07_changepass.png");
            // 
            // mainPanel
            // 
            this.mainPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPanel.Location = new System.Drawing.Point(160, 0);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(846, 307);
            this.mainPanel.TabIndex = 2;
            // 
            // ctrlStockTrader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Holder);
            this.Name = "ctrlStockTrader";
            this.Size = new System.Drawing.Size(1006, 307);
            ((System.ComponentModel.ISupportInitialize)(this.Holder)).EndInit();
            this.Holder.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainPanel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel Holder;
        private ComponentFactory.Krypton.Toolkit.KryptonTreeView menuTree;
        private System.Windows.Forms.ImageList imageList1;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel mainPanel;
    }
}
