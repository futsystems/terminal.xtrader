namespace TradingLib.TraderControl
{
    partial class fmChangePassword
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
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel3 = new Telerik.WinControls.UI.RadLabel();
            this.btnConfirm = new Telerik.WinControls.UI.RadButton();
            this.oldpass = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.pass1 = new Telerik.WinControls.UI.RadMaskedEditBox();
            this.pass2 = new Telerik.WinControls.UI.RadMaskedEditBox();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConfirm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.oldpass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pass1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pass2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            this.SuspendLayout();
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(2, 12);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(43, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "旧密码:";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(2, 36);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(43, 18);
            this.radLabel2.TabIndex = 1;
            this.radLabel2.Text = "新密码:";
            // 
            // radLabel3
            // 
            this.radLabel3.Location = new System.Drawing.Point(12, 60);
            this.radLabel3.Name = "radLabel3";
            this.radLabel3.Size = new System.Drawing.Size(31, 18);
            this.radLabel3.TabIndex = 2;
            this.radLabel3.Text = "重复:";
            // 
            // btnConfirm
            // 
            this.btnConfirm.Location = new System.Drawing.Point(153, 93);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(70, 24);
            this.btnConfirm.TabIndex = 3;
            this.btnConfirm.Text = "提 交";
            // 
            // oldpass
            // 
            this.oldpass.Location = new System.Drawing.Point(62, 10);
            this.oldpass.Name = "oldpass";
            this.oldpass.PasswordChar = '*';
            this.oldpass.Size = new System.Drawing.Size(125, 20);
            this.oldpass.TabIndex = 4;
            this.oldpass.TabStop = false;
            // 
            // pass1
            // 
            this.pass1.Location = new System.Drawing.Point(62, 36);
            this.pass1.Name = "pass1";
            this.pass1.PasswordChar = '*';
            this.pass1.Size = new System.Drawing.Size(125, 20);
            this.pass1.TabIndex = 5;
            this.pass1.TabStop = false;
            // 
            // pass2
            // 
            this.pass2.Location = new System.Drawing.Point(62, 60);
            this.pass2.Mask = "*";
            this.pass2.Name = "pass2";
            this.pass2.PasswordChar = '*';
            this.pass2.Size = new System.Drawing.Size(125, 20);
            this.pass2.TabIndex = 6;
            this.pass2.TabStop = false;
            // 
            // fmChangePassword
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(235, 127);
            this.Controls.Add(this.pass2);
            this.Controls.Add(this.pass1);
            this.Controls.Add(this.oldpass);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.radLabel3);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "fmChangePassword";
            // 
            // 
            // 
            this.RootElement.ApplyShapeToControl = true;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "修改密码";
            this.ThemeName = "ControlDefault";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnConfirm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.oldpass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pass1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pass2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private Telerik.WinControls.UI.RadLabel radLabel3;
        private Telerik.WinControls.UI.RadButton btnConfirm;
        private Telerik.WinControls.UI.RadMaskedEditBox oldpass;
        private Telerik.WinControls.UI.RadMaskedEditBox pass1;
        private Telerik.WinControls.UI.RadMaskedEditBox pass2;
    }
}
