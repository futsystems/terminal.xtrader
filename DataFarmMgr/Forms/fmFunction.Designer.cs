namespace TradingLib.DataFarmManager
{
    partial class fmFunction
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnResetAllSnapshot = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnResetAllSnapshot
            // 
            this.btnResetAllSnapshot.Location = new System.Drawing.Point(22, 12);
            this.btnResetAllSnapshot.Name = "btnResetAllSnapshot";
            this.btnResetAllSnapshot.Size = new System.Drawing.Size(91, 23);
            this.btnResetAllSnapshot.TabIndex = 0;
            this.btnResetAllSnapshot.Text = "重置快照";
            this.btnResetAllSnapshot.UseVisualStyleBackColor = true;
            // 
            // fmFunction
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(446, 344);
            this.Controls.Add(this.btnResetAllSnapshot);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmFunction";
            this.ShowIcon = false;
            this.Text = "工具";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnResetAllSnapshot;
    }
}