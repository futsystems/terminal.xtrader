namespace StockTrader
{
    partial class PageSTKOrderEntry
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
            this.kryptonNavigator1 = new ComponentFactory.Krypton.Navigator.KryptonNavigator();
            this.kryptonPage1 = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kryptonPage2 = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.kryptonPage3 = new ComponentFactory.Krypton.Navigator.KryptonPage();
            this.ctOrderSenderSTK2 = new TradingLib.KryptonControl.ctOrderSenderSTK();
            this.ctOrderSenderSTK1 = new TradingLib.KryptonControl.ctOrderSenderSTK();
            this.ctQuoteViewSTK1 = new TradingLib.KryptonControl.ctQuoteViewSTK();
            this.ctPositionViewSTK1 = new TradingLib.KryptonControl.ctPositionViewSTK();
            this.ctTradeViewSTK1 = new TradingLib.KryptonControl.ctTradeViewSTK();
            this.ctOrderViewSTK1 = new TradingLib.KryptonControl.ctOrderViewSTK();
            this.kryptonButton1 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton2 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton3 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonButton4 = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigator1)).BeginInit();
            this.kryptonNavigator1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage1)).BeginInit();
            this.kryptonPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage2)).BeginInit();
            this.kryptonPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage3)).BeginInit();
            this.kryptonPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonButton4);
            this.kryptonPanel1.Controls.Add(this.kryptonButton3);
            this.kryptonPanel1.Controls.Add(this.kryptonButton2);
            this.kryptonPanel1.Controls.Add(this.kryptonButton1);
            this.kryptonPanel1.Controls.Add(this.ctOrderSenderSTK2);
            this.kryptonPanel1.Controls.Add(this.ctOrderSenderSTK1);
            this.kryptonPanel1.Controls.Add(this.ctQuoteViewSTK1);
            this.kryptonPanel1.Controls.Add(this.kryptonNavigator1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(765, 457);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kryptonNavigator1
            // 
            this.kryptonNavigator1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonNavigator1.Bar.TabStyle = ComponentFactory.Krypton.Toolkit.TabStyle.LowProfile;
            this.kryptonNavigator1.Button.CloseButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonNavigator1.Button.ContextButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.Hide;
            this.kryptonNavigator1.Location = new System.Drawing.Point(0, 256);
            this.kryptonNavigator1.Name = "kryptonNavigator1";
            this.kryptonNavigator1.Pages.AddRange(new ComponentFactory.Krypton.Navigator.KryptonPage[] {
            this.kryptonPage1,
            this.kryptonPage2,
            this.kryptonPage3});
            this.kryptonNavigator1.SelectedIndex = 0;
            this.kryptonNavigator1.Size = new System.Drawing.Size(765, 201);
            this.kryptonNavigator1.TabIndex = 0;
            this.kryptonNavigator1.Text = "kryptonNavigator1";
            // 
            // kryptonPage1
            // 
            this.kryptonPage1.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage1.Controls.Add(this.ctPositionViewSTK1);
            this.kryptonPage1.Flags = 65534;
            this.kryptonPage1.LastVisibleSet = true;
            this.kryptonPage1.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage1.Name = "kryptonPage1";
            this.kryptonPage1.Size = new System.Drawing.Size(763, 174);
            this.kryptonPage1.Text = "持 仓";
            this.kryptonPage1.ToolTipTitle = "Page ToolTip";
            this.kryptonPage1.UniqueName = "FBEC13F020E24647C28421D19CAEED92";
            // 
            // kryptonPage2
            // 
            this.kryptonPage2.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage2.Controls.Add(this.ctTradeViewSTK1);
            this.kryptonPage2.Flags = 65534;
            this.kryptonPage2.LastVisibleSet = true;
            this.kryptonPage2.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage2.Name = "kryptonPage2";
            this.kryptonPage2.Size = new System.Drawing.Size(673, 190);
            this.kryptonPage2.Text = "成 交";
            this.kryptonPage2.ToolTipTitle = "Page ToolTip";
            this.kryptonPage2.UniqueName = "C4515BBC7621474309ABB010533562CD";
            // 
            // kryptonPage3
            // 
            this.kryptonPage3.AutoHiddenSlideSize = new System.Drawing.Size(200, 200);
            this.kryptonPage3.Controls.Add(this.ctOrderViewSTK1);
            this.kryptonPage3.Flags = 65534;
            this.kryptonPage3.LastVisibleSet = true;
            this.kryptonPage3.MinimumSize = new System.Drawing.Size(50, 50);
            this.kryptonPage3.Name = "kryptonPage3";
            this.kryptonPage3.Size = new System.Drawing.Size(673, 190);
            this.kryptonPage3.Text = "委 托";
            this.kryptonPage3.ToolTipTitle = "Page ToolTip";
            this.kryptonPage3.UniqueName = "5BEDEEF8AB6F4F204F917E95F24B1A0E";
            // 
            // ctOrderSenderSTK2
            // 
            this.ctOrderSenderSTK2.Location = new System.Drawing.Point(454, 3);
            this.ctOrderSenderSTK2.Name = "ctOrderSenderSTK2";
            this.ctOrderSenderSTK2.Side = false;
            this.ctOrderSenderSTK2.Size = new System.Drawing.Size(220, 212);
            this.ctOrderSenderSTK2.TabIndex = 2;
            // 
            // ctOrderSenderSTK1
            // 
            this.ctOrderSenderSTK1.Location = new System.Drawing.Point(3, 3);
            this.ctOrderSenderSTK1.Name = "ctOrderSenderSTK1";
            this.ctOrderSenderSTK1.Side = true;
            this.ctOrderSenderSTK1.Size = new System.Drawing.Size(220, 212);
            this.ctOrderSenderSTK1.TabIndex = 1;
            // 
            // ctQuoteViewSTK1
            // 
            this.ctQuoteViewSTK1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.ctQuoteViewSTK1.Location = new System.Drawing.Point(226, 3);
            this.ctQuoteViewSTK1.Margin = new System.Windows.Forms.Padding(0);
            this.ctQuoteViewSTK1.Name = "ctQuoteViewSTK1";
            this.ctQuoteViewSTK1.Size = new System.Drawing.Size(220, 250);
            this.ctQuoteViewSTK1.TabIndex = 1;
            // 
            // ctPositionViewSTK1
            // 
            this.ctPositionViewSTK1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctPositionViewSTK1.Location = new System.Drawing.Point(0, 0);
            this.ctPositionViewSTK1.Name = "ctPositionViewSTK1";
            this.ctPositionViewSTK1.Size = new System.Drawing.Size(763, 174);
            this.ctPositionViewSTK1.TabIndex = 0;
            // 
            // ctTradeViewSTK1
            // 
            this.ctTradeViewSTK1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctTradeViewSTK1.Location = new System.Drawing.Point(0, 0);
            this.ctTradeViewSTK1.Name = "ctTradeViewSTK1";
            this.ctTradeViewSTK1.Size = new System.Drawing.Size(673, 190);
            this.ctTradeViewSTK1.TabIndex = 0;
            // 
            // ctOrderViewSTK1
            // 
            this.ctOrderViewSTK1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctOrderViewSTK1.Location = new System.Drawing.Point(0, 0);
            this.ctOrderViewSTK1.Name = "ctOrderViewSTK1";
            this.ctOrderViewSTK1.Size = new System.Drawing.Size(673, 190);
            this.ctOrderViewSTK1.TabIndex = 0;
            // 
            // kryptonButton1
            // 
            this.kryptonButton1.Location = new System.Drawing.Point(26, 219);
            this.kryptonButton1.Name = "kryptonButton1";
            this.kryptonButton1.Size = new System.Drawing.Size(70, 25);
            this.kryptonButton1.TabIndex = 3;
            this.kryptonButton1.Values.Text = "刷 新";
            // 
            // kryptonButton2
            // 
            this.kryptonButton2.Location = new System.Drawing.Point(465, 219);
            this.kryptonButton2.Name = "kryptonButton2";
            this.kryptonButton2.Size = new System.Drawing.Size(70, 25);
            this.kryptonButton2.TabIndex = 4;
            this.kryptonButton2.Values.Text = "全 撤";
            // 
            // kryptonButton3
            // 
            this.kryptonButton3.Location = new System.Drawing.Point(541, 219);
            this.kryptonButton3.Name = "kryptonButton3";
            this.kryptonButton3.Size = new System.Drawing.Size(70, 25);
            this.kryptonButton3.TabIndex = 5;
            this.kryptonButton3.Values.Text = "撤 买";
            // 
            // kryptonButton4
            // 
            this.kryptonButton4.Location = new System.Drawing.Point(617, 219);
            this.kryptonButton4.Name = "kryptonButton4";
            this.kryptonButton4.Size = new System.Drawing.Size(70, 25);
            this.kryptonButton4.TabIndex = 6;
            this.kryptonButton4.Values.Text = "撤 卖";
            // 
            // PageSTKOrderEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "PageSTKOrderEntry";
            this.Size = new System.Drawing.Size(765, 457);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonNavigator1)).EndInit();
            this.kryptonNavigator1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage1)).EndInit();
            this.kryptonPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage2)).EndInit();
            this.kryptonPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPage3)).EndInit();
            this.kryptonPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Navigator.KryptonNavigator kryptonNavigator1;
        private ComponentFactory.Krypton.Navigator.KryptonPage kryptonPage1;
        private ComponentFactory.Krypton.Navigator.KryptonPage kryptonPage2;
        private ComponentFactory.Krypton.Navigator.KryptonPage kryptonPage3;
        private TradingLib.KryptonControl.ctPositionViewSTK ctPositionViewSTK1;
        private TradingLib.KryptonControl.ctTradeViewSTK ctTradeViewSTK1;
        private TradingLib.KryptonControl.ctOrderViewSTK ctOrderViewSTK1;
        private TradingLib.KryptonControl.ctQuoteViewSTK ctQuoteViewSTK1;
        private TradingLib.KryptonControl.ctOrderSenderSTK ctOrderSenderSTK1;
        private TradingLib.KryptonControl.ctOrderSenderSTK ctOrderSenderSTK2;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton4;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton3;
        private ComponentFactory.Krypton.Toolkit.KryptonButton kryptonButton2;

    }
}
