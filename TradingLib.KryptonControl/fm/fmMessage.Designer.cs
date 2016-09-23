namespace TradingLib.XTrader.Stock
{
    partial class fmMessage
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
            this.kryptonPanel1 = new System.Windows.Forms.Panel();
            this.kryptonPanel2 = new  System.Windows.Forms.Panel();
            this.btnSubmit = new System.Windows.Forms.Button();
            this._label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).BeginInit();
            this.kryptonPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.btnSubmit);
            this.kryptonPanel1.Controls.Add(this.kryptonPanel2);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(334, 181);
            this.kryptonPanel1.TabIndex = 0;
            // 
            // kryptonPanel2
            // 
            this.kryptonPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.kryptonPanel2.Controls.Add(this._label);
            this.kryptonPanel2.Location = new System.Drawing.Point(12, 12);
            this.kryptonPanel2.Name = "kryptonPanel2";
            this.kryptonPanel2.Size = new System.Drawing.Size(310, 128);
            this.kryptonPanel2.TabIndex = 1;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.Location = new System.Drawing.Point(260, 146);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(62, 25);
            this.btnSubmit.TabIndex = 1;
            this.btnSubmit.Text = "确 定";
            // 
            // _label
            // 
            this._label.AutoSize = false;
            this._label.Dock = System.Windows.Forms.DockStyle.Fill;
            this._label.Font = new System.Drawing.Font("Segoe UI", 9F);
            this._label.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(57)))), ((int)(((byte)(91)))));
            this._label.Location = new System.Drawing.Point(0, 0);
            this._label.Name = "_label";
            this._label.Size = new System.Drawing.Size(310, 128);
            this._label.Text = "messsage";
            // 
            // fmMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(334, 181);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "fmMessage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "fmMessage";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel2)).EndInit();
            this.kryptonPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel kryptonPanel1;
        private System.Windows.Forms.Panel kryptonPanel2;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label _label;
    }
}