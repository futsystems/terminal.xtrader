namespace StockTrader
{
    partial class PageSTKTradeHist
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
            this.btnQry = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.end = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.start = new ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.ctTradeViewSTK1 = new TradingLib.KryptonControl.ctTradeViewSTK();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.ctTradeViewSTK1);
            this.kryptonPanel1.Controls.Add(this.btnQry);
            this.kryptonPanel1.Controls.Add(this.end);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.start);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(578, 342);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // btnQry
            // 
            this.btnQry.Location = new System.Drawing.Point(343, 3);
            this.btnQry.Name = "btnQry";
            this.btnQry.Size = new System.Drawing.Size(70, 25);
            this.btnQry.TabIndex = 10;
            this.btnQry.Values.Text = "查 询";
            // 
            // end
            // 
            this.end.Location = new System.Drawing.Point(221, 4);
            this.end.Name = "end";
            this.end.Size = new System.Drawing.Size(116, 21);
            this.end.TabIndex = 9;
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(192, 5);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(23, 20);
            this.kryptonLabel2.TabIndex = 8;
            this.kryptonLabel2.Values.Text = "至";
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(70, 4);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(116, 21);
            this.start.TabIndex = 7;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(4, 4);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(60, 20);
            this.kryptonLabel1.TabIndex = 6;
            this.kryptonLabel1.Values.Text = "查询日期";
            // 
            // ctTradeViewSTK1
            // 
            this.ctTradeViewSTK1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctTradeViewSTK1.Location = new System.Drawing.Point(0, 30);
            this.ctTradeViewSTK1.Name = "ctTradeViewSTK1";
            this.ctTradeViewSTK1.RealView = true;
            this.ctTradeViewSTK1.Size = new System.Drawing.Size(578, 312);
            this.ctTradeViewSTK1.TabIndex = 11;
            // 
            // PageSTKTradeHist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "PageSTKTradeHist";
            this.Size = new System.Drawing.Size(578, 342);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnQry;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker end;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonDateTimePicker start;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private TradingLib.KryptonControl.ctTradeViewSTK ctTradeViewSTK1;
    }
}
