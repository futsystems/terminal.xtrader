namespace TradingLib.KryptonControl
{
    partial class PageSTKOrderHist
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
            this.ctOrderViewSTK1 = new TradingLib.KryptonControl.ctOrderViewSTK();
            this.fPanel1 = new TradingLib.KryptonControl.FPanel();
            this.end = new System.Windows.Forms.DateTimePicker();
            this.btnQry = new System.Windows.Forms.Button();
            this.start = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.fPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ctOrderViewSTK1
            // 
            this.ctOrderViewSTK1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ctOrderViewSTK1.Location = new System.Drawing.Point(0, 30);
            this.ctOrderViewSTK1.Name = "ctOrderViewSTK1";
            this.ctOrderViewSTK1.RealView = false;
            this.ctOrderViewSTK1.Size = new System.Drawing.Size(791, 255);
            this.ctOrderViewSTK1.TabIndex = 3;
            // 
            // fPanel1
            // 
            this.fPanel1.Controls.Add(this.end);
            this.fPanel1.Controls.Add(this.btnQry);
            this.fPanel1.Controls.Add(this.ctOrderViewSTK1);
            this.fPanel1.Controls.Add(this.start);
            this.fPanel1.Controls.Add(this.label2);
            this.fPanel1.Controls.Add(this.label1);
            this.fPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fPanel1.Location = new System.Drawing.Point(0, 0);
            this.fPanel1.Name = "fPanel1";
            this.fPanel1.Size = new System.Drawing.Size(791, 285);
            this.fPanel1.TabIndex = 1;
            // 
            // end
            // 
            this.end.Location = new System.Drawing.Point(254, 5);
            this.end.Name = "end";
            this.end.Size = new System.Drawing.Size(127, 21);
            this.end.TabIndex = 18;
            // 
            // btnQry
            // 
            this.btnQry.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnQry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnQry.Location = new System.Drawing.Point(713, 3);
            this.btnQry.Name = "btnQry";
            this.btnQry.Size = new System.Drawing.Size(75, 23);
            this.btnQry.TabIndex = 17;
            this.btnQry.Text = "全 撤";
            this.btnQry.UseVisualStyleBackColor = true;
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(62, 5);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(127, 21);
            this.start.TabIndex = 16;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(195, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 15;
            this.label2.Text = "终止日期";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(3, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "起始日期";
            // 
            // PageSTKOrderHist
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fPanel1);
            this.Name = "PageSTKOrderHist";
            this.Size = new System.Drawing.Size(791, 285);
            this.fPanel1.ResumeLayout(false);
            this.fPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TradingLib.KryptonControl.ctOrderViewSTK ctOrderViewSTK1;
        private FPanel fPanel1;
        private System.Windows.Forms.DateTimePicker end;
        private System.Windows.Forms.Button btnQry;
        private System.Windows.Forms.DateTimePicker start;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
