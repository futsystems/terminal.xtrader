namespace FutsTrader
{
    partial class TradeLite
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TradeLite));
            this.radStatusStrip1 = new Telerik.WinControls.UI.RadStatusStrip();
            this.authStatus = new Telerik.WinControls.UI.RadLabelElement();
            this.radLabelElement1 = new Telerik.WinControls.UI.RadLabelElement();
            this.radLabelElement2 = new Telerik.WinControls.UI.RadLabelElement();
            this.message = new Telerik.WinControls.UI.RadLabelElement();
            this.radLabelElement4 = new Telerik.WinControls.UI.RadLabelElement();
            this.timeStatus = new Telerik.WinControls.UI.RadLabelElement();
            this.exStatus = new Telerik.WinControls.UI.RadLabelElement();
            this.mdStatus = new Telerik.WinControls.UI.RadLabelElement();
            this.btnConfig = new Telerik.WinControls.UI.RadButtonElement();
            this.radThemeManager1 = new Telerik.WinControls.RadThemeManager();
            this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme();
            this.SplitContainer = new Telerik.WinControls.UI.RadSplitContainer();
            this.splitPanel1 = new Telerik.WinControls.UI.SplitPanel();
            this.viewPanel = new Telerik.WinControls.UI.RadPanel();
            this.mainPageView = new Telerik.WinControls.UI.RadPageView();
            this.pageQuote = new Telerik.WinControls.UI.RadPageViewPage();
            this.viewQuoteList1 = new TradingLib.TraderControl.ViewQuoteList();
            this.pageOrder = new Telerik.WinControls.UI.RadPageViewPage();
            this.ctOrderView1 = new TradingLib.TraderControl.ctOrderView();
            this.pagePosition = new Telerik.WinControls.UI.RadPageViewPage();
            this.ctPositionView1 = new TradingLib.TraderControl.ctPositionView();
            this.pageTrade = new Telerik.WinControls.UI.RadPageViewPage();
            this.ctTradeView1 = new TradingLib.TraderControl.ctTradeView();
            this.pageAccount = new Telerik.WinControls.UI.RadPageViewPage();
            this.ctAccountInfo1 = new TradingLib.TraderControl.ctAccountInfo();
            this.pageSecurity = new Telerik.WinControls.UI.RadPageViewPage();
            this.ctSymbolSelect1 = new TradingLib.TraderControl.ctSymbolSelect();
            this.pageWeb = new Telerik.WinControls.UI.RadPageViewPage();
            this.pageDebug = new Telerik.WinControls.UI.RadPageViewPage();
            this.ctDebug1 = new TradingLib.TraderControl.ctDebug();
            this.splitPanel2 = new Telerik.WinControls.UI.SplitPanel();
            this.ctOrderSender1 = new TradingLib.TraderControl.ctOrderSender();
            this.radLabelElement5 = new Telerik.WinControls.UI.RadLabelElement();
            this.radLabelElement3 = new Telerik.WinControls.UI.RadLabelElement();
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).BeginInit();
            this.SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel1)).BeginInit();
            this.splitPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.viewPanel)).BeginInit();
            this.viewPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainPageView)).BeginInit();
            this.mainPageView.SuspendLayout();
            this.pageQuote.SuspendLayout();
            this.pageOrder.SuspendLayout();
            this.pagePosition.SuspendLayout();
            this.pageTrade.SuspendLayout();
            this.pageAccount.SuspendLayout();
            this.pageSecurity.SuspendLayout();
            this.pageDebug.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel2)).BeginInit();
            this.splitPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radStatusStrip1
            // 
            this.radStatusStrip1.Items.AddRange(new Telerik.WinControls.RadItem[] {
            this.authStatus,
            this.radLabelElement1,
            this.radLabelElement2,
            this.message,
            this.radLabelElement4,
            this.timeStatus,
            this.exStatus,
            this.mdStatus,
            this.btnConfig});
            this.radStatusStrip1.Location = new System.Drawing.Point(0, 297);
            this.radStatusStrip1.Name = "radStatusStrip1";
            this.radStatusStrip1.Size = new System.Drawing.Size(604, 24);
            this.radStatusStrip1.TabIndex = 0;
            this.radStatusStrip1.Text = "radStatusStrip1";
            this.radStatusStrip1.ThemeName = "Windows8";
            // 
            // authStatus
            // 
            this.authStatus.AccessibleDescription = "登入";
            this.authStatus.AccessibleName = "登入";
            this.authStatus.Name = "authStatus";
            this.radStatusStrip1.SetSpring(this.authStatus, false);
            this.authStatus.Text = "";
            this.authStatus.TextAlignment = System.Drawing.ContentAlignment.MiddleRight;
            this.authStatus.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.authStatus.TextWrap = true;
            this.authStatus.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radLabelElement1
            // 
            this.radLabelElement1.DefaultSize = new System.Drawing.Size(10, 0);
            this.radLabelElement1.Name = "radLabelElement1";
            this.radStatusStrip1.SetSpring(this.radLabelElement1, false);
            this.radLabelElement1.Text = "";
            this.radLabelElement1.TextWrap = true;
            this.radLabelElement1.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radLabelElement2
            // 
            this.radLabelElement2.AccessibleDescription = "状态:";
            this.radLabelElement2.AccessibleName = "状态:";
            this.radLabelElement2.Name = "radLabelElement2";
            this.radStatusStrip1.SetSpring(this.radLabelElement2, false);
            this.radLabelElement2.Text = "消息:";
            this.radLabelElement2.TextWrap = true;
            this.radLabelElement2.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // message
            // 
            this.message.DefaultSize = new System.Drawing.Size(250, 0);
            this.message.Name = "message";
            this.radStatusStrip1.SetSpring(this.message, false);
            this.message.Text = "";
            this.message.TextWrap = true;
            this.message.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radLabelElement4
            // 
            this.radLabelElement4.AccessibleDescription = "radLabelElement4";
            this.radLabelElement4.AccessibleName = "radLabelElement4";
            this.radLabelElement4.Name = "radLabelElement4";
            this.radStatusStrip1.SetSpring(this.radLabelElement4, true);
            this.radLabelElement4.Text = "";
            this.radLabelElement4.TextWrap = true;
            this.radLabelElement4.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // timeStatus
            // 
            this.timeStatus.AccessibleDescription = "00:00:00";
            this.timeStatus.AccessibleName = "00:00:00";
            this.timeStatus.Name = "timeStatus";
            this.radStatusStrip1.SetSpring(this.timeStatus, false);
            this.timeStatus.Text = "00:00:00";
            this.timeStatus.TextWrap = true;
            this.timeStatus.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // exStatus
            // 
            this.exStatus.AccessibleDescription = "交易";
            this.exStatus.AccessibleName = "交易";
            this.exStatus.Image = global::FutsTrader.Properties.Resources.disconnected;
            this.exStatus.Name = "exStatus";
            this.radStatusStrip1.SetSpring(this.exStatus, false);
            this.exStatus.Text = "交易:";
            this.exStatus.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.exStatus.TextWrap = true;
            this.exStatus.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // mdStatus
            // 
            this.mdStatus.AccessibleDescription = "行情";
            this.mdStatus.AccessibleName = "行情";
            this.mdStatus.Image = global::FutsTrader.Properties.Resources.disconnected;
            this.mdStatus.Name = "mdStatus";
            this.radStatusStrip1.SetSpring(this.mdStatus, false);
            this.mdStatus.Text = "行情:";
            this.mdStatus.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.mdStatus.TextWrap = true;
            this.mdStatus.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // btnConfig
            // 
            this.btnConfig.AccessibleDescription = "设置";
            this.btnConfig.AccessibleName = "设置";
            this.btnConfig.Margin = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.btnConfig.Name = "btnConfig";
            this.radStatusStrip1.SetSpring(this.btnConfig, false);
            this.btnConfig.Text = "设置";
            this.btnConfig.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            this.btnConfig.Click += new System.EventHandler(this.btnConfig_Click);
            // 
            // SplitContainer
            // 
            this.SplitContainer.Controls.Add(this.splitPanel1);
            this.SplitContainer.Controls.Add(this.splitPanel2);
            this.SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.SplitContainer.Location = new System.Drawing.Point(0, 0);
            this.SplitContainer.Name = "SplitContainer";
            this.SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // 
            // 
            this.SplitContainer.RootElement.MinSize = new System.Drawing.Size(25, 25);
            this.SplitContainer.Size = new System.Drawing.Size(604, 297);
            this.SplitContainer.SplitterWidth = 4;
            this.SplitContainer.TabIndex = 2;
            this.SplitContainer.TabStop = false;
            this.SplitContainer.Text = "radSplitContainer1";
            this.SplitContainer.Click += new System.EventHandler(this.SplitContainer_Click);
            ((Telerik.WinControls.UI.SplitPanelElement)(this.SplitContainer.GetChildAt(0))).BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(239)))), ((int)(((byte)(239)))), ((int)(((byte)(239)))));
            ((Telerik.WinControls.UI.SplitContainerElement)(this.SplitContainer.GetChildAt(1))).SplitterWidth = 4;
            // 
            // splitPanel1
            // 
            this.splitPanel1.Controls.Add(this.viewPanel);
            this.splitPanel1.Location = new System.Drawing.Point(0, 0);
            this.splitPanel1.Name = "splitPanel1";
            // 
            // 
            // 
            this.splitPanel1.RootElement.MinSize = new System.Drawing.Size(0, 0);
            this.splitPanel1.Size = new System.Drawing.Size(604, 205);
            this.splitPanel1.SizeInfo.AutoSizeScale = new System.Drawing.SizeF(0.2534418F, 0.1883562F);
            this.splitPanel1.SizeInfo.MinimumSize = new System.Drawing.Size(0, 0);
            this.splitPanel1.SizeInfo.SplitterCorrection = new System.Drawing.Size(202, 55);
            this.splitPanel1.TabIndex = 0;
            this.splitPanel1.TabStop = false;
            // 
            // viewPanel
            // 
            this.viewPanel.Controls.Add(this.mainPageView);
            this.viewPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewPanel.Location = new System.Drawing.Point(0, 0);
            this.viewPanel.Name = "viewPanel";
            this.viewPanel.Size = new System.Drawing.Size(604, 205);
            this.viewPanel.TabIndex = 2;
            this.viewPanel.Text = "radPanel1";
            // 
            // mainPageView
            // 
            this.mainPageView.Controls.Add(this.pageQuote);
            this.mainPageView.Controls.Add(this.pageOrder);
            this.mainPageView.Controls.Add(this.pagePosition);
            this.mainPageView.Controls.Add(this.pageTrade);
            this.mainPageView.Controls.Add(this.pageAccount);
            this.mainPageView.Controls.Add(this.pageSecurity);
            this.mainPageView.Controls.Add(this.pageWeb);
            this.mainPageView.Controls.Add(this.pageDebug);
            this.mainPageView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainPageView.Location = new System.Drawing.Point(0, 0);
            this.mainPageView.Margin = new System.Windows.Forms.Padding(0);
            this.mainPageView.Name = "mainPageView";
            this.mainPageView.SelectedPage = this.pageQuote;
            this.mainPageView.Size = new System.Drawing.Size(604, 205);
            this.mainPageView.TabIndex = 0;
            this.mainPageView.ThemeName = "Windows8";
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.mainPageView.GetChildAt(0))).StripButtons = Telerik.WinControls.UI.StripViewButtons.None;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.mainPageView.GetChildAt(0))).ItemAlignment = Telerik.WinControls.UI.StripViewItemAlignment.Near;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.mainPageView.GetChildAt(0))).ItemFitMode = Telerik.WinControls.UI.StripViewItemFitMode.None;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.mainPageView.GetChildAt(0))).StripAlignment = Telerik.WinControls.UI.StripViewAlignment.Right;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.mainPageView.GetChildAt(0))).ItemDragMode = Telerik.WinControls.UI.PageViewItemDragMode.Preview;
            ((Telerik.WinControls.UI.RadPageViewStripElement)(this.mainPageView.GetChildAt(0))).ItemContentOrientation = Telerik.WinControls.UI.PageViewContentOrientation.Horizontal;
            ((Telerik.WinControls.UI.RadPageViewContentAreaElement)(this.mainPageView.GetChildAt(0).GetChildAt(1))).Padding = new System.Windows.Forms.Padding(0);
            // 
            // pageQuote
            // 
            this.pageQuote.Controls.Add(this.viewQuoteList1);
            this.pageQuote.Location = new System.Drawing.Point(1, 1);
            this.pageQuote.Name = "pageQuote";
            this.pageQuote.Size = new System.Drawing.Size(566, 203);
            this.pageQuote.Text = "报价";
            // 
            // viewQuoteList1
            // 
            this.viewQuoteList1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.viewQuoteList1.DNColor = System.Drawing.Color.Green;
            this.viewQuoteList1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.viewQuoteList1.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.viewQuoteList1.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.viewQuoteList1.HeaderFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(86)))), ((int)(((byte)(157)))), ((int)(((byte)(229)))));
            this.viewQuoteList1.Location = new System.Drawing.Point(0, 0);
            this.viewQuoteList1.MenuEnable = false;
            this.viewQuoteList1.Name = "viewQuoteList1";
            this.viewQuoteList1.QuoteBackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.viewQuoteList1.QuoteBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.viewQuoteList1.QuoteFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.viewQuoteList1.QuoteViewWidth = 1030;
            this.viewQuoteList1.SelectedQuoteRow = -1;
            this.viewQuoteList1.Size = new System.Drawing.Size(566, 203);
            this.viewQuoteList1.SymbolFont = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold);
            this.viewQuoteList1.SymbolFontColor = System.Drawing.Color.Gold;
            this.viewQuoteList1.TabIndex = 0;
            this.viewQuoteList1.TableLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.viewQuoteList1.Text = "viewQuoteList1";
            this.viewQuoteList1.UPColor = System.Drawing.Color.Red;
            // 
            // pageOrder
            // 
            this.pageOrder.Controls.Add(this.ctOrderView1);
            this.pageOrder.Location = new System.Drawing.Point(1, 1);
            this.pageOrder.Name = "pageOrder";
            this.pageOrder.Size = new System.Drawing.Size(566, 203);
            this.pageOrder.Text = "委托";
            // 
            // ctOrderView1
            // 
            this.ctOrderView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctOrderView1.Location = new System.Drawing.Point(0, 0);
            this.ctOrderView1.Name = "ctOrderView1";
            this.ctOrderView1.Size = new System.Drawing.Size(566, 203);
            this.ctOrderView1.TabIndex = 0;
            // 
            // pagePosition
            // 
            this.pagePosition.Controls.Add(this.ctPositionView1);
            this.pagePosition.Location = new System.Drawing.Point(1, 1);
            this.pagePosition.Name = "pagePosition";
            this.pagePosition.Size = new System.Drawing.Size(564, 203);
            this.pagePosition.Text = "持仓";
            // 
            // ctPositionView1
            // 
            this.ctPositionView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctPositionView1.Location = new System.Drawing.Point(0, 0);
            this.ctPositionView1.Name = "ctPositionView1";
            this.ctPositionView1.Size = new System.Drawing.Size(564, 203);
            this.ctPositionView1.TabIndex = 0;
            // 
            // pageTrade
            // 
            this.pageTrade.Controls.Add(this.ctTradeView1);
            this.pageTrade.Location = new System.Drawing.Point(1, 1);
            this.pageTrade.Name = "pageTrade";
            this.pageTrade.Size = new System.Drawing.Size(564, 203);
            this.pageTrade.Text = "成交";
            // 
            // ctTradeView1
            // 
            this.ctTradeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctTradeView1.Location = new System.Drawing.Point(0, 0);
            this.ctTradeView1.Name = "ctTradeView1";
            this.ctTradeView1.Size = new System.Drawing.Size(564, 203);
            this.ctTradeView1.TabIndex = 0;
            // 
            // pageAccount
            // 
            this.pageAccount.Controls.Add(this.ctAccountInfo1);
            this.pageAccount.Location = new System.Drawing.Point(1, 1);
            this.pageAccount.Name = "pageAccount";
            this.pageAccount.Size = new System.Drawing.Size(566, 203);
            this.pageAccount.Text = "帐户";
            // 
            // ctAccountInfo1
            // 
            this.ctAccountInfo1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctAccountInfo1.Location = new System.Drawing.Point(0, 0);
            this.ctAccountInfo1.Name = "ctAccountInfo1";
            this.ctAccountInfo1.Size = new System.Drawing.Size(566, 203);
            this.ctAccountInfo1.TabIndex = 0;
            // 
            // pageSecurity
            // 
            this.pageSecurity.Controls.Add(this.ctSymbolSelect1);
            this.pageSecurity.Location = new System.Drawing.Point(1, 1);
            this.pageSecurity.Name = "pageSecurity";
            this.pageSecurity.Size = new System.Drawing.Size(564, 203);
            this.pageSecurity.Text = "合约";
            // 
            // ctSymbolSelect1
            // 
            this.ctSymbolSelect1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctSymbolSelect1.Location = new System.Drawing.Point(0, 0);
            this.ctSymbolSelect1.Name = "ctSymbolSelect1";
            this.ctSymbolSelect1.Size = new System.Drawing.Size(564, 203);
            this.ctSymbolSelect1.TabIndex = 0;
            // 
            // pageWeb
            // 
            this.pageWeb.Location = new System.Drawing.Point(1, 1);
            this.pageWeb.Name = "pageWeb";
            this.pageWeb.Size = new System.Drawing.Size(564, 203);
            this.pageWeb.Text = "快讯";
            // 
            // pageDebug
            // 
            this.pageDebug.Controls.Add(this.ctDebug1);
            this.pageDebug.Location = new System.Drawing.Point(1, 1);
            this.pageDebug.Name = "pageDebug";
            this.pageDebug.Size = new System.Drawing.Size(564, 203);
            this.pageDebug.Text = "日志";
            // 
            // ctDebug1
            // 
            this.ctDebug1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctDebug1.EnableSearching = true;
            this.ctDebug1.ExternalTimeStamp = 0;
            this.ctDebug1.Location = new System.Drawing.Point(0, 0);
            this.ctDebug1.Margin = new System.Windows.Forms.Padding(2);
            this.ctDebug1.Name = "ctDebug1";
            this.ctDebug1.Size = new System.Drawing.Size(564, 203);
            this.ctDebug1.TabIndex = 0;
            this.ctDebug1.TimeStamps = true;
            this.ctDebug1.UseExternalTimeStamp = false;
            // 
            // splitPanel2
            // 
            this.splitPanel2.Controls.Add(this.ctOrderSender1);
            this.splitPanel2.Location = new System.Drawing.Point(0, 209);
            this.splitPanel2.Name = "splitPanel2";
            // 
            // 
            // 
            this.splitPanel2.RootElement.MaxSize = new System.Drawing.Size(0, 88);
            this.splitPanel2.RootElement.MinSize = new System.Drawing.Size(0, 88);
            this.splitPanel2.Size = new System.Drawing.Size(604, 88);
            this.splitPanel2.SizeInfo.AutoSizeScale = new System.Drawing.SizeF(-0.2534418F, -0.1883562F);
            this.splitPanel2.SizeInfo.MaximumSize = new System.Drawing.Size(0, 88);
            this.splitPanel2.SizeInfo.MinimumSize = new System.Drawing.Size(0, 88);
            this.splitPanel2.SizeInfo.SplitterCorrection = new System.Drawing.Size(-202, -55);
            this.splitPanel2.TabIndex = 1;
            this.splitPanel2.TabStop = false;
            // 
            // ctOrderSender1
            // 
            this.ctOrderSender1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctOrderSender1.Location = new System.Drawing.Point(0, 0);
            this.ctOrderSender1.Margin = new System.Windows.Forms.Padding(0);
            this.ctOrderSender1.Name = "ctOrderSender1";
            this.ctOrderSender1.OrderTracker = null;
            this.ctOrderSender1.OrderType = TradingLib.API.QSEnumOrderType.Market;
            this.ctOrderSender1.PositionTracker = null;
            this.ctOrderSender1.Size = new System.Drawing.Size(604, 88);
            this.ctOrderSender1.TabIndex = 0;
            // 
            // radLabelElement5
            // 
            this.radLabelElement5.AccessibleDescription = "行情:";
            this.radLabelElement5.AccessibleName = "行情:";
            this.radLabelElement5.Name = "radLabelElement5";
            this.radLabelElement5.Text = "行情:";
            this.radLabelElement5.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.radLabelElement5.TextWrap = true;
            this.radLabelElement5.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // radLabelElement3
            // 
            this.radLabelElement3.AccessibleDescription = "交易:";
            this.radLabelElement3.AccessibleName = "交易:";
            this.radLabelElement3.Name = "radLabelElement3";
            this.radLabelElement3.Text = "交易:";
            this.radLabelElement3.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.radLabelElement3.TextWrap = true;
            this.radLabelElement3.Visibility = Telerik.WinControls.ElementVisibility.Visible;
            // 
            // TradeLite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(604, 321);
            this.Controls.Add(this.SplitContainer);
            this.Controls.Add(this.radStatusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(334, 149);
            this.Name = "TradeLite";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "";
            this.ThemeName = "Windows8";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.radStatusStrip1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SplitContainer)).EndInit();
            this.SplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel1)).EndInit();
            this.splitPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.viewPanel)).EndInit();
            this.viewPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainPageView)).EndInit();
            this.mainPageView.ResumeLayout(false);
            this.pageQuote.ResumeLayout(false);
            this.pageOrder.ResumeLayout(false);
            this.pagePosition.ResumeLayout(false);
            this.pageTrade.ResumeLayout(false);
            this.pageAccount.ResumeLayout(false);
            this.pageSecurity.ResumeLayout(false);
            this.pageDebug.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitPanel2)).EndInit();
            this.splitPanel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadStatusStrip radStatusStrip1;
        private Telerik.WinControls.RadThemeManager radThemeManager1;
        //private Telerik.WinControls.Themes.Office2010BlackTheme office2010BlackTheme1;
        //private Telerik.WinControls.Themes.TelerikMetroTheme telerikMetroTheme1;
        private Telerik.WinControls.Themes.Windows8Theme windows8Theme1;
        private Telerik.WinControls.UI.RadSplitContainer SplitContainer;
        private Telerik.WinControls.UI.SplitPanel splitPanel1;
        private Telerik.WinControls.UI.SplitPanel splitPanel2;
        private Telerik.WinControls.UI.RadPanel viewPanel;
        private Telerik.WinControls.UI.RadButtonElement btnConfig;
        private Telerik.WinControls.UI.RadLabelElement radLabelElement1;
        private Telerik.WinControls.UI.RadLabelElement radLabelElement2;
        private Telerik.WinControls.UI.RadLabelElement message;
        private Telerik.WinControls.UI.RadLabelElement radLabelElement5;
        private Telerik.WinControls.UI.RadLabelElement radLabelElement3;
        private Telerik.WinControls.UI.RadLabelElement radLabelElement4;
        private Telerik.WinControls.UI.RadLabelElement timeStatus;
        private Telerik.WinControls.UI.RadLabelElement exStatus;
        private Telerik.WinControls.UI.RadLabelElement mdStatus;
        private Telerik.WinControls.UI.RadLabelElement authStatus;
        //private Telerik.WinControls.Themes.Office2010SilverTheme office2010SilverTheme1;
        private TradingLib.TraderControl.ctOrderSender ctOrderSender1;
        private Telerik.WinControls.UI.RadPageView mainPageView;
        private Telerik.WinControls.UI.RadPageViewPage pageQuote;
        private TradingLib.TraderControl.ViewQuoteList viewQuoteList1;
        private Telerik.WinControls.UI.RadPageViewPage pageOrder;
        private TradingLib.TraderControl.ctOrderView ctOrderView1;
        private Telerik.WinControls.UI.RadPageViewPage pagePosition;
        private TradingLib.TraderControl.ctPositionView ctPositionView1;
        private Telerik.WinControls.UI.RadPageViewPage pageTrade;
        private TradingLib.TraderControl.ctTradeView ctTradeView1;
        private Telerik.WinControls.UI.RadPageViewPage pageSecurity;
        private TradingLib.TraderControl.ctSymbolSelect ctSymbolSelect1;
        private Telerik.WinControls.UI.RadPageViewPage pageAccount;
        private TradingLib.TraderControl.ctAccountInfo ctAccountInfo1;
        private Telerik.WinControls.UI.RadPageViewPage pageDebug;
        private TradingLib.TraderControl.ctDebug ctDebug1;
        private Telerik.WinControls.UI.RadPageViewPage pageWeb;
    }
}
