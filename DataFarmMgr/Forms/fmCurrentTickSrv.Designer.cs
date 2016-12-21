namespace TradingLib.DataFarmManager
{
    partial class fmCurrentTickSrv
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
            this.label1 = new System.Windows.Forms.Label();
            this.lbCurrentTickSrv = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前TickSrv:";
            // 
            // lbCurrentTickSrv
            // 
            this.lbCurrentTickSrv.AutoSize = true;
            this.lbCurrentTickSrv.Location = new System.Drawing.Point(95, 21);
            this.lbCurrentTickSrv.Name = "lbCurrentTickSrv";
            this.lbCurrentTickSrv.Size = new System.Drawing.Size(17, 12);
            this.lbCurrentTickSrv.TabIndex = 1;
            this.lbCurrentTickSrv.Text = "--";
            // 
            // fmCurrentTickSrv
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(295, 97);
            this.Controls.Add(this.lbCurrentTickSrv);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "fmCurrentTickSrv";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "查询当前TickSrv";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbCurrentTickSrv;
    }
}