﻿namespace XTraderPro
{
    partial class UserControl1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControl1));
            this.axStockChartX1 = new AxSTOCKCHARTXLib.AxStockChartX();
            ((System.ComponentModel.ISupportInitialize)(this.axStockChartX1)).BeginInit();
            this.SuspendLayout();
            // 
            // axStockChartX1
            // 
            this.axStockChartX1.Enabled = true;
            this.axStockChartX1.Location = new System.Drawing.Point(84, 50);
            this.axStockChartX1.Name = "axStockChartX1";
            this.axStockChartX1.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("axStockChartX1.OcxState")));
            this.axStockChartX1.Size = new System.Drawing.Size(376, 232);
            this.axStockChartX1.TabIndex = 0;
            // 
            // UserControl1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.axStockChartX1);
            this.Name = "UserControl1";
            this.Size = new System.Drawing.Size(579, 364);
            ((System.ComponentModel.ISupportInitialize)(this.axStockChartX1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxSTOCKCHARTXLib.AxStockChartX axStockChartX1;
    }
}
