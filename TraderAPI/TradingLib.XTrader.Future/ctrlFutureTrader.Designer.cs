﻿namespace TradingLib.XTrader.Future
{
    partial class ctrlFutureTrader
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
            this.panelTop = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelHolder = new System.Windows.Forms.Panel();
            this.orderEntryPanel = new System.Windows.Forms.Panel();
            this.ctrlPosition1 = new TradingLib.XTrader.Future.ctrlPosition();
            this.ctrlOrderEntry1 = new TradingLib.XTrader.Future.Control.ctrlOrderEntry();
            this.ctrlListMenu1 = new TradingLib.XTrader.Future.ctrlListMenu();
            this.panelTop.SuspendLayout();
            this.panelHolder.SuspendLayout();
            this.orderEntryPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.button2);
            this.panelTop.Controls.Add(this.button1);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1080, 25);
            this.panelTop.TabIndex = 0;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(943, 1);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 3;
            this.button2.Text = "锁定";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(862, 1);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "刷新";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(119, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "可用资金:23232.99";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "交易者，您好";
            // 
            // panelHolder
            // 
            this.panelHolder.Controls.Add(this.ctrlPosition1);
            this.panelHolder.Controls.Add(this.orderEntryPanel);
            this.panelHolder.Controls.Add(this.ctrlListMenu1);
            this.panelHolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelHolder.Location = new System.Drawing.Point(0, 25);
            this.panelHolder.Name = "panelHolder";
            this.panelHolder.Size = new System.Drawing.Size(1080, 234);
            this.panelHolder.TabIndex = 1;
            // 
            // orderEntryPanel
            // 
            this.orderEntryPanel.Controls.Add(this.ctrlOrderEntry1);
            this.orderEntryPanel.Dock = System.Windows.Forms.DockStyle.Left;
            this.orderEntryPanel.Location = new System.Drawing.Point(120, 0);
            this.orderEntryPanel.Name = "orderEntryPanel";
            this.orderEntryPanel.Size = new System.Drawing.Size(353, 234);
            this.orderEntryPanel.TabIndex = 2;
            // 
            // ctrlPosition1
            // 
            this.ctrlPosition1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlPosition1.Location = new System.Drawing.Point(473, 0);
            this.ctrlPosition1.Name = "ctrlPosition1";
            this.ctrlPosition1.Size = new System.Drawing.Size(607, 234);
            this.ctrlPosition1.TabIndex = 3;
            // 
            // ctrlOrderEntry1
            // 
            this.ctrlOrderEntry1.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ctrlOrderEntry1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlOrderEntry1.Location = new System.Drawing.Point(0, 0);
            this.ctrlOrderEntry1.Name = "ctrlOrderEntry1";
            this.ctrlOrderEntry1.Size = new System.Drawing.Size(353, 234);
            this.ctrlOrderEntry1.TabIndex = 0;
            // 
            // ctrlListMenu1
            // 
            this.ctrlListMenu1.Dock = System.Windows.Forms.DockStyle.Left;
            this.ctrlListMenu1.Location = new System.Drawing.Point(0, 0);
            this.ctrlListMenu1.Name = "ctrlListMenu1";
            this.ctrlListMenu1.Size = new System.Drawing.Size(120, 234);
            this.ctrlListMenu1.TabIndex = 1;
            this.ctrlListMenu1.Text = "ctrlListMenu1";
            // 
            // ctrlFutureTrader
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelHolder);
            this.Controls.Add(this.panelTop);
            this.Name = "ctrlFutureTrader";
            this.Size = new System.Drawing.Size(1080, 259);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelHolder.ResumeLayout(false);
            this.orderEntryPanel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Panel panelHolder;
        private ctrlListMenu ctrlListMenu1;
        private System.Windows.Forms.Panel orderEntryPanel;
        private Control.ctrlOrderEntry ctrlOrderEntry1;
        private ctrlPosition ctrlPosition1;
    }
}
