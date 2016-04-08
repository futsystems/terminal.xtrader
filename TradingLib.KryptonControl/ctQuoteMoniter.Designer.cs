namespace TradingLib.KryptonControl
{
    partial class ctQuoteMoniter
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
            this.navigator = new ComponentFactory.Krypton.Navigator.KryptonNavigator();
            ((System.ComponentModel.ISupportInitialize)(this.navigator)).BeginInit();
            this.navigator.SuspendLayout();
            this.SuspendLayout();
            // 
            // navigator
            // 
            this.navigator.Bar.BarOrientation = ComponentFactory.Krypton.Toolkit.VisualOrientation.Bottom;
            this.navigator.Bar.TabStyle = ComponentFactory.Krypton.Toolkit.TabStyle.StandardProfile;
            this.navigator.Button.ButtonDisplayLogic = ComponentFactory.Krypton.Navigator.ButtonDisplayLogic.None;
            this.navigator.Button.CloseButtonAction = ComponentFactory.Krypton.Navigator.CloseButtonAction.None;
            this.navigator.Button.CloseButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.Hide;
            this.navigator.Button.ContextButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.Hide;
            this.navigator.Button.NextButtonDisplay = ComponentFactory.Krypton.Navigator.ButtonDisplay.Hide;
            this.navigator.Dock = System.Windows.Forms.DockStyle.Fill;
            this.navigator.Location = new System.Drawing.Point(0, 0);
            this.navigator.Name = "navigator";
            this.navigator.Size = new System.Drawing.Size(687, 352);
            this.navigator.TabIndex = 0;
            this.navigator.Text = "kryptonNavigator1";
            // 
            // ctQuoteMoniter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.navigator);
            this.Name = "ctQuoteMoniter";
            this.Size = new System.Drawing.Size(687, 352);
            ((System.ComponentModel.ISupportInitialize)(this.navigator)).EndInit();
            this.navigator.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Navigator.KryptonNavigator navigator;
    }
}
