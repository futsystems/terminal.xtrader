namespace TradingLib.XTrader.Stock
{
    partial class MainContainer
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
            this.ctrlTraderLogin = new TradingLib.XTrader.Stock.ctrlTraderLogin();
            this.SuspendLayout();
            // 
            // ctrlTraderLogin
            // 
            this.ctrlTraderLogin.BackColor = System.Drawing.SystemColors.Control;
            this.ctrlTraderLogin.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctrlTraderLogin.Location = new System.Drawing.Point(0, 0);
            this.ctrlTraderLogin.Name = "ctrlTraderLogin";
            this.ctrlTraderLogin.Size = new System.Drawing.Size(878, 363);
            this.ctrlTraderLogin.TabIndex = 0;
            // 
            // MainContainer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ctrlTraderLogin);
            this.Name = "MainContainer";
            this.Size = new System.Drawing.Size(878, 363);
            this.ResumeLayout(false);

        }

        #endregion

        private ctrlTraderLogin ctrlTraderLogin;
    }
}
