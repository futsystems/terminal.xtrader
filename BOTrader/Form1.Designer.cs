namespace BOTrader
{
    partial class Form1
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            this.kryptonPalette1 = new ComponentFactory.Krypton.Toolkit.KryptonPalette(this.components);
            this.ctTradingInfo1 = new BOTrader.ctTradingInfo();
            this.ctCallPutSender1 = new BOTrader.ctCallPutSender();
            this.ctBOInfoInput1 = new BOTrader.ctBOInfoInput();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.ctTradingInfo1);
            this.kryptonPanel1.Controls.Add(this.ctCallPutSender1);
            this.kryptonPanel1.Controls.Add(this.ctBOInfoInput1);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(811, 443);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kryptonManager1
            // 
            this.kryptonManager1.GlobalPalette = this.kryptonPalette1;
            this.kryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Custom;
            // 
            // kryptonPalette1
            // 
            this.kryptonPalette1.BasePaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2010Silver;
            // 
            // ctTradingInfo1
            // 
            this.ctTradingInfo1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.ctTradingInfo1.Location = new System.Drawing.Point(0, 248);
            this.ctTradingInfo1.Name = "ctTradingInfo1";
            this.ctTradingInfo1.Size = new System.Drawing.Size(811, 195);
            this.ctTradingInfo1.TabIndex = 2;
            // 
            // ctCallPutSender1
            // 
            this.ctCallPutSender1.Location = new System.Drawing.Point(540, 40);
            this.ctCallPutSender1.Name = "ctCallPutSender1";
            this.ctCallPutSender1.Size = new System.Drawing.Size(268, 202);
            this.ctCallPutSender1.TabIndex = 1;
            // 
            // ctBOInfoInput1
            // 
            this.ctBOInfoInput1.Dock = System.Windows.Forms.DockStyle.Top;
            this.ctBOInfoInput1.Location = new System.Drawing.Point(0, 0);
            this.ctBOInfoInput1.Name = "ctBOInfoInput1";
            this.ctBOInfoInput1.Size = new System.Drawing.Size(811, 51);
            this.ctBOInfoInput1.TabIndex = 0;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(811, 443);
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ctBOInfoInput ctBOInfoInput1;
        private ctTradingInfo ctTradingInfo1;
        private ctCallPutSender ctCallPutSender1;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
        private ComponentFactory.Krypton.Toolkit.KryptonPalette kryptonPalette1;
    }
}

