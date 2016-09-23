namespace TradingLib.XTrader.Control
{
    partial class ctrlQuoteList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctrlQuoteList));
            this.scrollBar = new System.Windows.Forms.VScrollBar();
            this.quotelist = new TradingLib.XTrader.Control.ViewQuoteList();
            this.blockTab = new TradingLib.XTrader.Control.BlockTab();
            this.blockPanel = new System.Windows.Forms.Panel();
            this.blockPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // scrollBar
            // 
            this.scrollBar.Dock = System.Windows.Forms.DockStyle.Right;
            this.scrollBar.Location = new System.Drawing.Point(907, 0);
            this.scrollBar.Name = "scrollBar";
            this.scrollBar.Size = new System.Drawing.Size(17, 408);
            this.scrollBar.TabIndex = 1;
            this.scrollBar.Visible = false;
            // 
            // quotelist
            // 
            this.quotelist.BackColor = System.Drawing.Color.Black;
            this.quotelist.DNColor = System.Drawing.Color.Green;
            this.quotelist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.quotelist.HeaderBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.quotelist.HeaderFont = new System.Drawing.Font("Microsoft Sans Serif", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quotelist.HeaderFontColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.quotelist.Location = new System.Drawing.Point(0, 0);
            this.quotelist.MenuEnable = false;
            this.quotelist.Name = "quotelist";
            this.quotelist.QuoteBackColor1 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.quotelist.QuoteBackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.quotelist.QuoteFont = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.quotelist.QuoteType = TradingLib.XTrader.Control.EnumQuoteListType.STOCK_CN;
            this.quotelist.SelectedColor = System.Drawing.Color.FromArgb(((int)(((byte)(75)))), ((int)(((byte)(75)))), ((int)(((byte)(75)))));
            this.quotelist.Size = new System.Drawing.Size(907, 408);
            this.quotelist.StartIndex = 0;
            this.quotelist.SymbolFont = new System.Drawing.Font("Microsoft Sans Serif", 10.5F);
            this.quotelist.SymbolFontColor = System.Drawing.Color.Green;
            this.quotelist.TabIndex = 0;
            this.quotelist.TableLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.quotelist.Text = "viewQuoteList1";
            this.quotelist.UPColor = System.Drawing.Color.Red;
            // 
            // blockTab
            // 
            this.blockTab.BackColor = System.Drawing.Color.Transparent;
            this.blockTab.Dock = System.Windows.Forms.DockStyle.Left;
            this.blockTab.Location = new System.Drawing.Point(0, 0);
            this.blockTab.Name = "blockTab";
            this.blockTab.Size = new System.Drawing.Size(237, 20);
            this.blockTab.TabIndex = 0;
            this.blockTab.TabStop = false;
            this.blockTab.Text = "blockButton1";
            // 
            // blockPanel
            // 
            this.blockPanel.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("blockPanel.BackgroundImage")));
            this.blockPanel.Controls.Add(this.blockTab);
            this.blockPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.blockPanel.Location = new System.Drawing.Point(0, 408);
            this.blockPanel.Name = "blockPanel";
            this.blockPanel.Size = new System.Drawing.Size(924, 20);
            this.blockPanel.TabIndex = 2;
            // 
            // ctrlQuoteList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.quotelist);
            this.Controls.Add(this.scrollBar);
            this.Controls.Add(this.blockPanel);
            this.Name = "ctrlQuoteList";
            this.Size = new System.Drawing.Size(924, 428);
            this.blockPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.VScrollBar scrollBar;
        private TradingLib.XTrader.Control.ViewQuoteList quotelist;
        private BlockTab blockTab;
        private System.Windows.Forms.Panel blockPanel;


    }
}
