﻿namespace TradingLib.TraderControl
{
    partial class ctlChart
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ctlChart));
            this.ctmRight = new Nevron.UI.WinForm.Controls.NContextMenu();
            this.mnuBuy = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuSell = new Nevron.UI.WinForm.Controls.NCommand();
            this.mnuIndicator = new Nevron.UI.WinForm.Controls.NCommand();
            this.StockChartX1 = new AxSTOCKCHARTXLib.AxStockChartX();
            ((System.ComponentModel.ISupportInitialize)(this.StockChartX1)).BeginInit();
            this.SuspendLayout();
            // 
            // ctmRight
            // 
            this.ctmRight.Commands.AddRange(new Nevron.UI.WinForm.Controls.NCommand[] {
            this.mnuBuy,
            this.mnuSell,
            this.mnuIndicator});
            this.ctmRight.MenuOptions.FitInWorkingArea = true;
            // 
            // mnuBuy
            // 
            this.mnuBuy.Properties.Text = "买入";
            // 
            // mnuSell
            // 
            this.mnuSell.Properties.Text = "卖出";
            // 
            // mnuIndicator
            // 
            this.mnuIndicator.Properties.Text = "技术指标分析";
            // 
            // StockChartX1
            // 
            this.StockChartX1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StockChartX1.Enabled = true;
            this.StockChartX1.Location = new System.Drawing.Point(0, 0);
            this.StockChartX1.Name = "StockChartX1";
            this.StockChartX1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("StockChartX1.OcxState")));
            this.StockChartX1.Size = new System.Drawing.Size(735, 386);
            this.StockChartX1.TabIndex = 0;
            // 
            // ctlChart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.StockChartX1);
            this.Name = "ctlChart";
            this.Size = new System.Drawing.Size(735, 386);
            ((System.ComponentModel.ISupportInitialize)(this.StockChartX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Nevron.UI.WinForm.Controls.NContextMenu ctmRight;
        private Nevron.UI.WinForm.Controls.NCommand mnuBuy;
        private Nevron.UI.WinForm.Controls.NCommand mnuSell;
        private Nevron.UI.WinForm.Controls.NCommand mnuIndicator;
        private AxSTOCKCHARTXLib.AxStockChartX StockChartX1;


    }
}
