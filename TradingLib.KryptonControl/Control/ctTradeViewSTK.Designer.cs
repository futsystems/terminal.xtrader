﻿namespace TradingLib.NevronControl.Control
{
    partial class ctTradeViewSTK
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
            this.tradeGrid = new ComponentFactory.Krypton.Toolkit.KryptonDataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.tradeGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // tradeGrid
            // 
            this.tradeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tradeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tradeGrid.Location = new System.Drawing.Point(0, 0);
            this.tradeGrid.Name = "tradeGrid";
            this.tradeGrid.RowTemplate.Height = 23;
            this.tradeGrid.Size = new System.Drawing.Size(734, 368);
            this.tradeGrid.TabIndex = 0;
            // 
            // ctTradeViewSTK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tradeGrid);
            this.Name = "ctTradeViewSTK";
            this.Size = new System.Drawing.Size(734, 368);
            ((System.ComponentModel.ISupportInitialize)(this.tradeGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonDataGridView tradeGrid;

    }
}
