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
            this.components = new System.ComponentModel.Container();
            this.account = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.savepassword = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.password = new ComponentFactory.Krypton.Toolkit.KryptonMaskedTextBox();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.kryptonComboBox1 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel5 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.btnExit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnLogin = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonComboBox2 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.serverlist = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel4 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lbLoginStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.kryptonPalette1 = new ComponentFactory.Krypton.Toolkit.KryptonPalette(this.components);
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.serverlist)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // account
            // 
            this.account.Location = new System.Drawing.Point(87, 63);
            this.account.Name = "account";
            this.account.Size = new System.Drawing.Size(120, 20);
            this.account.TabIndex = 2;
            // 
            // savepassword
            // 
            this.savepassword.Location = new System.Drawing.Point(229, 78);
            this.savepassword.Name = "savepassword";
            this.savepassword.Size = new System.Drawing.Size(73, 20);
            this.savepassword.TabIndex = 8;
            this.savepassword.Values.Text = "保存密码";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(87, 89);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(120, 20);
            this.password.TabIndex = 9;
            this.password.Text = "kryptonMaskedTextBox1";
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.kryptonComboBox1);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel5);
            this.kryptonPanel1.Controls.Add(this.btnExit);
            this.kryptonPanel1.Controls.Add(this.btnLogin);
            this.kryptonPanel1.Controls.Add(this.kryptonComboBox2);
            this.kryptonPanel1.Controls.Add(this.serverlist);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel4);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.savepassword);
            this.kryptonPanel1.Controls.Add(this.password);
            this.kryptonPanel1.Controls.Add(this.account);
            this.kryptonPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(326, 206);
            this.kryptonPanel1.TabIndex = 10;
            // 
            // kryptonComboBox1
            // 
            this.kryptonComboBox1.DropDownWidth = 119;
            this.kryptonComboBox1.Enabled = false;
            this.kryptonComboBox1.Location = new System.Drawing.Point(88, 126);
            this.kryptonComboBox1.Name = "kryptonComboBox1";
            this.kryptonComboBox1.Size = new System.Drawing.Size(119, 21);
            this.kryptonComboBox1.TabIndex = 19;
            this.kryptonComboBox1.Text = "验证码";
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(14, 126);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(63, 20);
            this.kryptonLabel5.TabIndex = 18;
            this.kryptonLabel5.Values.Text = "加密协议:";
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(229, 38);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(73, 25);
            this.btnExit.TabIndex = 17;
            this.btnExit.Values.Text = "取 消";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(229, 7);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(73, 25);
            this.btnLogin.TabIndex = 16;
            this.btnLogin.Values.Text = "确 定";
            // 
            // kryptonComboBox2
            // 
            this.kryptonComboBox2.DropDownWidth = 119;
            this.kryptonComboBox2.Enabled = false;
            this.kryptonComboBox2.Location = new System.Drawing.Point(88, 37);
            this.kryptonComboBox2.Name = "kryptonComboBox2";
            this.kryptonComboBox2.Size = new System.Drawing.Size(119, 21);
            this.kryptonComboBox2.TabIndex = 15;
            this.kryptonComboBox2.Text = "资金帐号";
            // 
            // serverlist
            // 
            this.serverlist.DropDownWidth = 119;
            this.serverlist.Location = new System.Drawing.Point(88, 11);
            this.serverlist.Name = "serverlist";
            this.serverlist.Size = new System.Drawing.Size(119, 21);
            this.serverlist.TabIndex = 14;
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(14, 89);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(63, 20);
            this.kryptonLabel2.TabIndex = 13;
            this.kryptonLabel2.Values.Text = "交易密码:";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(32, 63);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(45, 20);
            this.kryptonLabel1.TabIndex = 12;
            this.kryptonLabel1.Values.Text = "帐  号:";
            // 
            // kryptonLabel4
            // 
            this.kryptonLabel4.Location = new System.Drawing.Point(14, 37);
            this.kryptonLabel4.Name = "kryptonLabel4";
            this.kryptonLabel4.Size = new System.Drawing.Size(63, 20);
            this.kryptonLabel4.TabIndex = 11;
            this.kryptonLabel4.Values.Text = "帐号类型:";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(26, 11);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(51, 20);
            this.kryptonLabel3.TabIndex = 10;
            this.kryptonLabel3.Values.Text = "营业部:";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lbLoginStatus});
            this.statusStrip1.Location = new System.Drawing.Point(0, 184);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.ManagerRenderMode;
            this.statusStrip1.Size = new System.Drawing.Size(326, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lbLoginStatus
            // 
            this.lbLoginStatus.Name = "lbLoginStatus";
            this.lbLoginStatus.Size = new System.Drawing.Size(17, 17);
            this.lbLoginStatus.Text = "--";
            // 
            // kryptonPalette1
            // 
            this.kryptonPalette1.BasePaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.Office2007Blue;
            // 
            // kryptonManager1
            // 
            this.kryptonManager1.GlobalPalette = this.kryptonPalette1;
            this.kryptonManager1.GlobalPaletteMode = ComponentFactory.Krypton.Toolkit.PaletteModeManager.Custom;
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(326, 206);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.kryptonPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "LoginForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "用户登入";
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.serverlist)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonTextBox account;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox savepassword;
        private ComponentFactory.Krypton.Toolkit.KryptonMaskedTextBox password;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel4;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kryptonComboBox2;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox serverlist;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnLogin;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnExit;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lbLoginStatus;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kryptonComboBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private ComponentFactory.Krypton.Toolkit.KryptonPalette kryptonPalette1;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
    }
}