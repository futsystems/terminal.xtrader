namespace XTraderLite
{
    partial class frmDataFarmList
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
            this.lbCurrentServer = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前站点:";
            // 
            // lbCurrentServer
            // 
            this.lbCurrentServer.AutoSize = true;
            this.lbCurrentServer.Location = new System.Drawing.Point(78, 13);
            this.lbCurrentServer.Name = "lbCurrentServer";
            this.lbCurrentServer.Size = new System.Drawing.Size(17, 12);
            this.lbCurrentServer.TabIndex = 1;
            this.lbCurrentServer.Text = "--";
            // 
            // fmDataFarmList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 215);
            this.Controls.Add(this.lbCurrentServer);
            this.Controls.Add(this.label1);
            this.Name = "fmDataFarmList";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "行情服务器站点列表";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbCurrentServer;
    }
}