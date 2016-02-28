namespace StockTrader
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
            this.serverlist = new ComponentFactory.Krypton.Toolkit.KryptonListBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.account = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.btnLogin = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnExit = new ComponentFactory.Krypton.Toolkit.KryptonLinkLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lbLoginStatus = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.savepassword = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.password = new ComponentFactory.Krypton.Toolkit.KryptonMaskedTextBox();
            this.SuspendLayout();
            // 
            // serverlist
            // 
            this.serverlist.Location = new System.Drawing.Point(12, 160);
            this.serverlist.Name = "serverlist";
            this.serverlist.Size = new System.Drawing.Size(194, 86);
            this.serverlist.TabIndex = 0;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(258, 164);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(38, 20);
            this.kryptonLabel1.StateCommon.ShortText.Color1 = System.Drawing.Color.White;
            this.kryptonLabel1.TabIndex = 1;
            this.kryptonLabel1.Values.Text = "帐号:";
            // 
            // account
            // 
            this.account.Location = new System.Drawing.Point(302, 164);
            this.account.Name = "account";
            this.account.Size = new System.Drawing.Size(145, 20);
            this.account.TabIndex = 2;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(460, 160);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(87, 58);
            this.btnLogin.StateCommon.Content.ShortText.Font = new System.Drawing.Font("微软雅黑", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnLogin.TabIndex = 3;
            this.btnLogin.Values.Text = "登 入";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(481, 226);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(39, 20);
            this.btnExit.TabIndex = 4;
            this.btnExit.Values.Text = "退 出";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(258, 198);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(38, 20);
            this.kryptonLabel2.StateCommon.ShortText.Color1 = System.Drawing.Color.White;
            this.kryptonLabel2.TabIndex = 5;
            this.kryptonLabel2.Values.Text = "密码:";
            // 
            // lbLoginStatus
            // 
            this.lbLoginStatus.Location = new System.Drawing.Point(35, 262);
            this.lbLoginStatus.Name = "lbLoginStatus";
            this.lbLoginStatus.Size = new System.Drawing.Size(20, 20);
            this.lbLoginStatus.StateCommon.ShortText.Color1 = System.Drawing.Color.White;
            this.lbLoginStatus.TabIndex = 7;
            this.lbLoginStatus.Values.Text = "--";
            // 
            // savepassword
            // 
            this.savepassword.Location = new System.Drawing.Point(317, 225);
            this.savepassword.Name = "savepassword";
            this.savepassword.Size = new System.Drawing.Size(73, 20);
            this.savepassword.StateCommon.ShortText.Color1 = System.Drawing.Color.White;
            this.savepassword.TabIndex = 8;
            this.savepassword.Values.Text = "记住密码";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(302, 198);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(145, 20);
            this.password.TabIndex = 9;
            this.password.Text = "kryptonMaskedTextBox1";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(560, 290);
            this.Controls.Add(this.password);
            this.Controls.Add(this.savepassword);
            this.Controls.Add(this.lbLoginStatus);
            this.Controls.Add(this.kryptonLabel2);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnLogin);
            this.Controls.Add(this.account);
            this.Controls.Add(this.kryptonLabel1);
            this.Controls.Add(this.serverlist);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonListBox serverlist;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox account;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnLogin;
        private ComponentFactory.Krypton.Toolkit.KryptonLinkLabel btnExit;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lbLoginStatus;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox savepassword;
        private ComponentFactory.Krypton.Toolkit.KryptonMaskedTextBox password;
    }
}