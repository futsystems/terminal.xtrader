namespace FutsTrader
{
    partial class LoginForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LoginForm));
            this.account = new Telerik.WinControls.UI.RadTextBox();
            this.btnLogin = new Telerik.WinControls.UI.RadButton();
            this.lbLoginStatus = new Telerik.WinControls.UI.RadLabel();
            this.savepassword = new Telerik.WinControls.UI.RadCheckBox();
            this.serverlist = new Telerik.WinControls.UI.RadListControl();
            this.radLabel1 = new Telerik.WinControls.UI.RadLabel();
            this.radLabel2 = new Telerik.WinControls.UI.RadLabel();
            this.btnExit = new System.Windows.Forms.LinkLabel();
            this.password = new System.Windows.Forms.MaskedTextBox();
            this.windows8Theme1 = new Telerik.WinControls.Themes.Windows8Theme();
            ((System.ComponentModel.ISupportInitialize)(this.account)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbLoginStatus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.savepassword)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.serverlist)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).BeginInit();
            this.SuspendLayout();
            // 
            // account
            // 
            this.account.AutoSize = false;
            this.account.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.account.Location = new System.Drawing.Point(302, 160);
            this.account.Multiline = true;
            this.account.Name = "account";
            this.account.Size = new System.Drawing.Size(145, 26);
            this.account.TabIndex = 0;
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogin.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(92)))), ((int)(((byte)(127)))));
            this.btnLogin.Location = new System.Drawing.Point(460, 160);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(87, 58);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "登 入";
            // 
            // lbLoginStatus
            // 
            this.lbLoginStatus.BackColor = System.Drawing.Color.Transparent;
            this.lbLoginStatus.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbLoginStatus.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lbLoginStatus.Location = new System.Drawing.Point(35, 262);
            this.lbLoginStatus.Name = "lbLoginStatus";
            this.lbLoginStatus.Size = new System.Drawing.Size(18, 21);
            this.lbLoginStatus.TabIndex = 3;
            this.lbLoginStatus.Text = "--";
            // 
            // savepassword
            // 
            this.savepassword.BackColor = System.Drawing.Color.Transparent;
            this.savepassword.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.savepassword.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.savepassword.Location = new System.Drawing.Point(317, 225);
            this.savepassword.Name = "savepassword";
            this.savepassword.Size = new System.Drawing.Size(74, 21);
            this.savepassword.TabIndex = 4;
            this.savepassword.Text = "记住密码";
            // 
            // serverlist
            // 
            this.serverlist.Location = new System.Drawing.Point(12, 160);
            this.serverlist.Name = "serverlist";
            this.serverlist.Size = new System.Drawing.Size(194, 86);
            this.serverlist.TabIndex = 5;
            this.serverlist.Text = "radListControl1";
            // 
            // radLabel1
            // 
            this.radLabel1.BackColor = System.Drawing.Color.Transparent;
            this.radLabel1.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.radLabel1.Location = new System.Drawing.Point(259, 164);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(37, 21);
            this.radLabel1.TabIndex = 4;
            this.radLabel1.Text = "帐号:";
            // 
            // radLabel2
            // 
            this.radLabel2.BackColor = System.Drawing.Color.Transparent;
            this.radLabel2.Font = new System.Drawing.Font("微软雅黑", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.radLabel2.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.radLabel2.Location = new System.Drawing.Point(259, 195);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(37, 21);
            this.radLabel2.TabIndex = 5;
            this.radLabel2.Text = "密码:";
            // 
            // btnExit
            // 
            this.btnExit.AutoSize = true;
            this.btnExit.BackColor = System.Drawing.Color.Transparent;
            this.btnExit.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExit.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.btnExit.LinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.btnExit.Location = new System.Drawing.Point(481, 226);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(49, 19);
            this.btnExit.TabIndex = 6;
            this.btnExit.TabStop = true;
            this.btnExit.Text = "退   出";
            // 
            // password
            // 
            this.password.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.password.Location = new System.Drawing.Point(302, 191);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(145, 26);
            this.password.TabIndex = 7;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Menu;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.BorderWidth = 0;
            this.ClientSize = new System.Drawing.Size(560, 290);
            this.Controls.Add(this.password);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.radLabel2);
            this.Controls.Add(this.radLabel1);
            this.Controls.Add(this.serverlist);
            this.Controls.Add(this.savepassword);
            this.Controls.Add(this.lbLoginStatus);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.account);
            this.DoubleBuffered = true;
            this.Name = "LoginForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginForm";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.account)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnLogin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lbLoginStatus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.savepassword)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.serverlist)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radLabel2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Telerik.WinControls.UI.RadTextBox account;
        private Telerik.WinControls.UI.RadButton btnLogin;
        private Telerik.WinControls.UI.RadLabel lbLoginStatus;
        private Telerik.WinControls.UI.RadCheckBox savepassword;
        private Telerik.WinControls.UI.RadListControl serverlist;
        private Telerik.WinControls.UI.RadLabel radLabel1;
        private Telerik.WinControls.UI.RadLabel radLabel2;
        private System.Windows.Forms.LinkLabel btnExit;
        private System.Windows.Forms.MaskedTextBox password;
        private Telerik.WinControls.Themes.Windows8Theme windows8Theme1;
    }
}
