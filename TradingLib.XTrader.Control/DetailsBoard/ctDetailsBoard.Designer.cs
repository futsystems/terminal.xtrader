namespace CStock
{
    partial class ctDetailsBoard
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
            this.QuoteInfoBox = new System.Windows.Forms.Panel();
            this.pbox5 = new System.Windows.Forms.Panel();
            this.pbox4 = new System.Windows.Forms.Panel();
            this.pbox3 = new System.Windows.Forms.Panel();
            this.TabBox = new System.Windows.Forms.PictureBox();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.DetailTabBox = new System.Windows.Forms.Panel();
            this.pbox2 = new TradingLib.KryptonControl.ctrlTabPriceVolList();
            this.pbox1 = new TradingLib.KryptonControl.ctrlTabTradeList();
            this.ctrlQuoteInfo1 = new TradingLib.XTrader.ctrlQuoteInfo();
            this.ctrlStockQuoteInfo1 = new CStock.ctrlStockQuoteInfo();
            this.QuoteInfoBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabBox)).BeginInit();
            this.DetailTabBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // QuoteInfoBox
            // 
            this.QuoteInfoBox.BackColor = System.Drawing.Color.Black;
            this.QuoteInfoBox.Controls.Add(this.ctrlQuoteInfo1);
            this.QuoteInfoBox.Controls.Add(this.ctrlStockQuoteInfo1);
            this.QuoteInfoBox.Controls.Add(this.splitter1);
            this.QuoteInfoBox.Dock = System.Windows.Forms.DockStyle.Top;
            this.QuoteInfoBox.Location = new System.Drawing.Point(0, 0);
            this.QuoteInfoBox.MinimumSize = new System.Drawing.Size(160, 0);
            this.QuoteInfoBox.Name = "QuoteInfoBox";
            this.QuoteInfoBox.Size = new System.Drawing.Size(302, 437);
            this.QuoteInfoBox.TabIndex = 9;
            // 
            // pbox5
            // 
            this.pbox5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(255)))));
            this.pbox5.Location = new System.Drawing.Point(68, 57);
            this.pbox5.Margin = new System.Windows.Forms.Padding(0);
            this.pbox5.Name = "pbox5";
            this.pbox5.Size = new System.Drawing.Size(46, 36);
            this.pbox5.TabIndex = 5;
            this.pbox5.Visible = false;
            // 
            // pbox4
            // 
            this.pbox4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.pbox4.Location = new System.Drawing.Point(14, 57);
            this.pbox4.Margin = new System.Windows.Forms.Padding(0);
            this.pbox4.Name = "pbox4";
            this.pbox4.Size = new System.Drawing.Size(46, 36);
            this.pbox4.TabIndex = 4;
            this.pbox4.Visible = false;
            // 
            // pbox3
            // 
            this.pbox3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.pbox3.Location = new System.Drawing.Point(120, 15);
            this.pbox3.Margin = new System.Windows.Forms.Padding(0);
            this.pbox3.Name = "pbox3";
            this.pbox3.Size = new System.Drawing.Size(46, 36);
            this.pbox3.TabIndex = 3;
            this.pbox3.Visible = false;
            // 
            // TabBox
            // 
            this.TabBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            this.TabBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.TabBox.Location = new System.Drawing.Point(0, 236);
            this.TabBox.Name = "TabBox";
            this.TabBox.Size = new System.Drawing.Size(302, 18);
            this.TabBox.TabIndex = 0;
            this.TabBox.TabStop = false;
            this.TabBox.Paint += new System.Windows.Forms.PaintEventHandler(this.TabBox_Paint);
            this.TabBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.TabBox_MouseClick);
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Enabled = false;
            this.splitter1.Location = new System.Drawing.Point(0, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(302, 1);
            this.splitter1.TabIndex = 24;
            this.splitter1.TabStop = false;
            // 
            // DetailTabBox
            // 
            this.DetailTabBox.Controls.Add(this.pbox2);
            this.DetailTabBox.Controls.Add(this.TabBox);
            this.DetailTabBox.Controls.Add(this.pbox1);
            this.DetailTabBox.Controls.Add(this.pbox5);
            this.DetailTabBox.Controls.Add(this.pbox3);
            this.DetailTabBox.Controls.Add(this.pbox4);
            this.DetailTabBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DetailTabBox.Location = new System.Drawing.Point(0, 437);
            this.DetailTabBox.Name = "DetailTabBox";
            this.DetailTabBox.Size = new System.Drawing.Size(302, 254);
            this.DetailTabBox.TabIndex = 10;
            // 
            // pbox2
            // 
            this.pbox2.Location = new System.Drawing.Point(68, 15);
            this.pbox2.Name = "pbox2";
            this.pbox2.Size = new System.Drawing.Size(46, 36);
            this.pbox2.TabIndex = 7;
            this.pbox2.Text = "ctrlPriceVolList1";
            this.pbox2.Visible = false;
            this.pbox2.DoubleClick += new System.EventHandler(this.pbox2_DoubleClick);
            // 
            // pbox1
            // 
            this.pbox1.Location = new System.Drawing.Point(13, 15);
            this.pbox1.Name = "pbox1";
            this.pbox1.Size = new System.Drawing.Size(46, 36);
            this.pbox1.TabIndex = 6;
            this.pbox1.Text = "ctrlTabTradeList1";
            this.pbox1.DoubleClick += new System.EventHandler(this.pbox1_DoubleClick);
            // 
            // ctrlQuoteInfo1
            // 
            this.ctrlQuoteInfo1.Location = new System.Drawing.Point(13, 120);
            this.ctrlQuoteInfo1.Name = "ctrlQuoteInfo1";
            this.ctrlQuoteInfo1.Size = new System.Drawing.Size(272, 202);
            this.ctrlQuoteInfo1.TabIndex = 26;
            this.ctrlQuoteInfo1.Text = "ctrlQuoteInfo1";
            // 
            // ctrlStockQuoteInfo1
            // 
            this.ctrlStockQuoteInfo1.Location = new System.Drawing.Point(3, 3);
            this.ctrlStockQuoteInfo1.Name = "ctrlStockQuoteInfo1";
            this.ctrlStockQuoteInfo1.Size = new System.Drawing.Size(176, 97);
            this.ctrlStockQuoteInfo1.TabIndex = 25;
            // 
            // ctDetailsBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DetailTabBox);
            this.Controls.Add(this.QuoteInfoBox);
            this.Name = "ctDetailsBoard";
            this.Size = new System.Drawing.Size(302, 691);
            this.QuoteInfoBox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TabBox)).EndInit();
            this.DetailTabBox.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel QuoteInfoBox;
        private System.Windows.Forms.Panel pbox5;
        private System.Windows.Forms.Panel pbox4;
        private System.Windows.Forms.Panel pbox3;
        private System.Windows.Forms.PictureBox TabBox;
        private System.Windows.Forms.Splitter splitter1;
        private TradingLib.KryptonControl.ctrlTabTradeList pbox1;
        private TradingLib.KryptonControl.ctrlTabPriceVolList pbox2;
        private ctrlStockQuoteInfo ctrlStockQuoteInfo1;
        private TradingLib.XTrader.ctrlQuoteInfo ctrlQuoteInfo1;
        private System.Windows.Forms.Panel DetailTabBox;
    }
}
