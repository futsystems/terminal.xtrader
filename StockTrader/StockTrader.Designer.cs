namespace StockTrader
{
    partial class StockTrader
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StockTrader));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbProgrameName = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.lbmessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mToolBar = new System.Windows.Forms.ToolStrip();
            this.menuTree = new ComponentFactory.Krypton.Toolkit.KryptonTreeView();
            this.mainPanel = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.lbConnectImg = new System.Windows.Forms.ToolStripStatusLabel();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.btnPass = new System.Windows.Forms.ToolStripButton();
            this.statusStrip1.SuspendLayout();
            this.mToolBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPanel)).BeginInit();
            this.SuspendLayout();
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbProgrameName,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.lbmessage,
            this.toolStripStatusLabel4,
            this.lbConnectImg});
            this.statusStrip1.Location = new System.Drawing.Point(0, 660);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(1126, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbProgrameName
            // 
            this.lbProgrameName.Name = "lbProgrameName";
            this.lbProgrameName.Size = new System.Drawing.Size(17, 17);
            this.lbProgrameName.Text = "--";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(17, 17);
            this.toolStripStatusLabel2.Text = "--";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(821, 17);
            this.toolStripStatusLabel3.Spring = true;
            // 
            // lbmessage
            // 
            this.lbmessage.AutoSize = false;
            this.lbmessage.Name = "lbmessage";
            this.lbmessage.Size = new System.Drawing.Size(200, 17);
            this.lbmessage.Text = "--";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(34, 17);
            this.toolStripStatusLabel4.Text = "连接:";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 70);
            // 
            // mToolBar
            // 
            this.mToolBar.AutoSize = false;
            this.mToolBar.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.mToolBar.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnExit,
            this.toolStripButton2,
            this.btnRefresh,
            this.toolStripSeparator1,
            this.btnPass});
            this.mToolBar.Location = new System.Drawing.Point(0, 0);
            this.mToolBar.Name = "mToolBar";
            this.mToolBar.Size = new System.Drawing.Size(1126, 70);
            this.mToolBar.TabIndex = 0;
            this.mToolBar.Text = "toolStrip1";
            // 
            // menuTree
            // 
            this.menuTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.menuTree.Location = new System.Drawing.Point(0, 70);
            this.menuTree.Name = "menuTree";
            this.menuTree.Size = new System.Drawing.Size(180, 590);
            this.menuTree.StateCommon.Back.Color1 = System.Drawing.Color.FromArgb(((int)(((byte)(191)))), ((int)(((byte)(219)))), ((int)(((byte)(255)))));
            this.menuTree.TabIndex = 0;
            // 
            // mainPanel
            // 
            this.mainPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.mainPanel.Location = new System.Drawing.Point(180, 70);
            this.mainPanel.Name = "mainPanel";
            this.mainPanel.Size = new System.Drawing.Size(946, 590);
            this.mainPanel.TabIndex = 2;
            // 
            // lbConnectImg
            // 
            this.lbConnectImg.AutoSize = false;
            this.lbConnectImg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.lbConnectImg.Image = global::StockTrader.Properties.Resources.connected;
            this.lbConnectImg.Margin = new System.Windows.Forms.Padding(0);
            this.lbConnectImg.Name = "lbConnectImg";
            this.lbConnectImg.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            this.lbConnectImg.Size = new System.Drawing.Size(22, 22);
            this.lbConnectImg.Text = "toolStripStatusLabel5";
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = false;
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(55, 65);
            this.btnExit.Text = "退 出";
            this.btnExit.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnExit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.AutoSize = false;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.toolStripButton2.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(55, 65);
            this.toolStripButton2.Text = "锁 屏";
            this.toolStripButton2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.toolStripButton2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnRefresh
            // 
            this.btnRefresh.AutoSize = false;
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnRefresh.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(55, 65);
            this.btnRefresh.Text = "刷 新";
            this.btnRefresh.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // btnPass
            // 
            this.btnPass.AutoSize = false;
            this.btnPass.Image = ((System.Drawing.Image)(resources.GetObject("btnPass.Image")));
            this.btnPass.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnPass.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPass.Name = "btnPass";
            this.btnPass.Size = new System.Drawing.Size(55, 65);
            this.btnPass.Text = "密 码";
            this.btnPass.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPass.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            // 
            // StockTrader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1126, 682);
            this.Controls.Add(this.mainPanel);
            this.Controls.Add(this.menuTree);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.mToolBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "StockTrader";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "终端";
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.mToolBar.ResumeLayout(false);
            this.mToolBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPanel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnPass;
        private System.Windows.Forms.ToolStrip mToolBar;
        private ComponentFactory.Krypton.Toolkit.KryptonTreeView menuTree;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel mainPanel;
        private System.Windows.Forms.ToolStripStatusLabel lbProgrameName;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel lbmessage;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel lbConnectImg;
    }
}

