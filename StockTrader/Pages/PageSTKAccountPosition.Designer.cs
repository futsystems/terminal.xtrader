namespace StockTrader
{
    partial class PageSTKAccountPosition
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
            this.kryptonLabel6 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel5 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lbCash = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lbFrozenFund = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lbAvabileFund = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel10 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lbSTKMarketValue = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lbTotalEquity = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.ctPositionViewSTK1 = new TradingLib.KryptonControl.ctPositionViewSTK();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.lbTotalEquity);
            this.kryptonPanel1.Controls.Add(this.lbSTKMarketValue);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel10);
            this.kryptonPanel1.Controls.Add(this.lbAvabileFund);
            this.kryptonPanel1.Controls.Add(this.lbFrozenFund);
            this.kryptonPanel1.Controls.Add(this.lbCash);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel6);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel5);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel4);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.ctPositionViewSTK1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(685, 413);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kryptonLabel6
            // 
            this.kryptonLabel6.Location = new System.Drawing.Point(273, 65);
            this.kryptonLabel6.Name = "kryptonLabel6";
            this.kryptonLabel6.Size = new System.Drawing.Size(48, 20);
            this.kryptonLabel6.TabIndex = 6;
            this.kryptonLabel6.Values.Text = "总资产";
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(273, 39);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(60, 20);
            this.kryptonLabel5.TabIndex = 5;
            this.kryptonLabel5.Values.Text = "股票市值";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(273, 13);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(60, 20);
            this.kryptonLabel4.TabIndex = 4;
            this.kryptonLabel4.Values.Text = "可取资金";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(17, 65);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(60, 20);
            this.kryptonLabel3.TabIndex = 3;
            this.kryptonLabel3.Values.Text = "可用资金";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(17, 39);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(60, 20);
            this.kryptonLabel2.TabIndex = 2;
            this.kryptonLabel2.Values.Text = "冻结资金";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(17, 13);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(60, 20);
            this.kryptonLabel1.TabIndex = 1;
            this.kryptonLabel1.Values.Text = "资金余额";
            // 
            // lbCash
            // 
            this.lbCash.Location = new System.Drawing.Point(83, 13);
            this.lbCash.Name = "lbCash";
            this.lbCash.Size = new System.Drawing.Size(20, 20);
            this.lbCash.TabIndex = 7;
            this.lbCash.Values.Text = "--";
            // 
            // lbFrozenFund
            // 
            this.lbFrozenFund.Location = new System.Drawing.Point(83, 39);
            this.lbFrozenFund.Name = "lbFrozenFund";
            this.lbFrozenFund.Size = new System.Drawing.Size(20, 20);
            this.lbFrozenFund.TabIndex = 8;
            this.lbFrozenFund.Values.Text = "--";
            // 
            // lbAvabileFund
            // 
            this.lbAvabileFund.Location = new System.Drawing.Point(83, 65);
            this.lbAvabileFund.Name = "lbAvabileFund";
            this.lbAvabileFund.Size = new System.Drawing.Size(20, 20);
            this.lbAvabileFund.TabIndex = 9;
            this.lbAvabileFund.Values.Text = "--";
            // 
            // kryptonLabel10
            // 
            this.kryptonLabel10.Location = new System.Drawing.Point(339, 13);
            this.kryptonLabel10.Name = "kryptonLabel10";
            this.kryptonLabel10.Size = new System.Drawing.Size(20, 20);
            this.kryptonLabel10.TabIndex = 10;
            this.kryptonLabel10.Values.Text = "--";
            // 
            // lbSTKMarketValue
            // 
            this.lbSTKMarketValue.Location = new System.Drawing.Point(339, 39);
            this.lbSTKMarketValue.Name = "lbSTKMarketValue";
            this.lbSTKMarketValue.Size = new System.Drawing.Size(20, 20);
            this.lbSTKMarketValue.TabIndex = 11;
            this.lbSTKMarketValue.Values.Text = "--";
            // 
            // lbTotalEquity
            // 
            this.lbTotalEquity.Location = new System.Drawing.Point(339, 65);
            this.lbTotalEquity.Name = "lbTotalEquity";
            this.lbTotalEquity.Size = new System.Drawing.Size(20, 20);
            this.lbTotalEquity.TabIndex = 12;
            this.lbTotalEquity.Values.Text = "--";
            // 
            // ctPositionViewSTK1
            // 
            this.ctPositionViewSTK1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.ctPositionViewSTK1.Location = new System.Drawing.Point(0, 103);
            this.ctPositionViewSTK1.Name = "ctPositionViewSTK1";
            this.ctPositionViewSTK1.Size = new System.Drawing.Size(685, 310);
            this.ctPositionViewSTK1.TabIndex = 0;
            // 
            // PageSTKAccountPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "PageSTKAccountPosition";
            this.Size = new System.Drawing.Size(685, 413);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private TradingLib.KryptonControl.ctPositionViewSTK ctPositionViewSTK1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel6;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lbCash;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lbTotalEquity;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lbSTKMarketValue;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel10;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lbAvabileFund;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lbFrozenFund;
    }
}
