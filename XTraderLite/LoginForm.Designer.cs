﻿namespace XTraderLite
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
            this._msg = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnLogin = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.username = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbServer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.holder = new System.Windows.Forms.Panel();
            this.panel_XGJ = new System.Windows.Forms.Panel();
            this.@__msg2 = new System.Windows.Forms.Label();
            this.password2 = new System.Windows.Forms.MaskedTextBox();
            this.username2 = new System.Windows.Forms.TextBox();
            this.panel_controlbox = new System.Windows.Forms.Panel();
            this.btnClose2 = new System.Windows.Forms.PictureBox();
            this.btnMin2 = new System.Windows.Forms.PictureBox();
            this.btnLogin2 = new System.Windows.Forms.Button();
            this.panel_Classic = new System.Windows.Forms.Panel();
            this.topImage = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.MaskedTextBox();
            this.holder.SuspendLayout();
            this.panel_XGJ.SuspendLayout();
            this.panel_controlbox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMin2)).BeginInit();
            this.panel_Classic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topImage)).BeginInit();
            this.SuspendLayout();
            // 
            // _msg
            // 
            this._msg.AutoSize = true;
            this._msg.BackColor = System.Drawing.Color.Transparent;
            this._msg.Location = new System.Drawing.Point(230, 340);
            this._msg.Name = "_msg";
            this._msg.Size = new System.Drawing.Size(17, 12);
            this._msg.TabIndex = 21;
            this._msg.Text = "--";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(455, 298);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取 消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(455, 244);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 48);
            this.btnLogin.TabIndex = 5;
            this.btnLogin.Text = "登 入";
            this.btnLogin.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.BackColor = System.Drawing.Color.Transparent;
            this.checkBox1.Location = new System.Drawing.Point(377, 303);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "记住密码";
            this.checkBox1.UseVisualStyleBackColor = false;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(297, 271);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(155, 21);
            this.username.TabIndex = 2;
            this.username.Text = "88888888";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(228, 303);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "登入密码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(226, 274);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "登入用户";
            // 
            // cbServer
            // 
            this.cbServer.FormattingEnabled = true;
            this.cbServer.Location = new System.Drawing.Point(297, 244);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(155, 20);
            this.cbServer.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(226, 247);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "行情服务器";
            // 
            // holder
            // 
            this.holder.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.holder.BackColor = System.Drawing.Color.Transparent;
            this.holder.Controls.Add(this.panel_XGJ);
            this.holder.Controls.Add(this.panel_Classic);
            this.holder.Location = new System.Drawing.Point(1, 1);
            this.holder.Name = "holder";
            this.holder.Size = new System.Drawing.Size(1020, 707);
            this.holder.TabIndex = 2;
            // 
            // panel_XGJ
            // 
            this.panel_XGJ.BackgroundImage = global::XTraderLite.Properties.Resources.bg;
            this.panel_XGJ.Controls.Add(this.@__msg2);
            this.panel_XGJ.Controls.Add(this.password2);
            this.panel_XGJ.Controls.Add(this.username2);
            this.panel_XGJ.Controls.Add(this.panel_controlbox);
            this.panel_XGJ.Controls.Add(this.btnLogin2);
            this.panel_XGJ.Location = new System.Drawing.Point(131, 166);
            this.panel_XGJ.Name = "panel_XGJ";
            this.panel_XGJ.Size = new System.Drawing.Size(730, 410);
            this.panel_XGJ.TabIndex = 25;
            // 
            // __msg2
            // 
            this.@__msg2.AutoSize = true;
            this.@__msg2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(214)))), ((int)(((byte)(241)))));
            this.@__msg2.Location = new System.Drawing.Point(37, 371);
            this.@__msg2.Name = "__msg2";
            this.@__msg2.Size = new System.Drawing.Size(17, 12);
            this.@__msg2.TabIndex = 4;
            this.@__msg2.Text = "--";
            // 
            // password2
            // 
            this.password2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(214)))), ((int)(((byte)(241)))));
            this.password2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.password2.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.password2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(110)))), ((int)(((byte)(165)))));
            this.password2.Location = new System.Drawing.Point(510, 157);
            this.password2.Name = "password2";
            this.password2.PasswordChar = '*';
            this.password2.Size = new System.Drawing.Size(135, 16);
            this.password2.TabIndex = 3;
            // 
            // username2
            // 
            this.username2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(144)))), ((int)(((byte)(214)))), ((int)(((byte)(241)))));
            this.username2.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.username2.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.username2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(58)))), ((int)(((byte)(110)))), ((int)(((byte)(165)))));
            this.username2.Location = new System.Drawing.Point(510, 117);
            this.username2.Name = "username2";
            this.username2.Size = new System.Drawing.Size(135, 16);
            this.username2.TabIndex = 2;
            // 
            // panel_controlbox
            // 
            this.panel_controlbox.Controls.Add(this.btnClose2);
            this.panel_controlbox.Controls.Add(this.btnMin2);
            this.panel_controlbox.Location = new System.Drawing.Point(648, 0);
            this.panel_controlbox.Name = "panel_controlbox";
            this.panel_controlbox.Size = new System.Drawing.Size(76, 19);
            this.panel_controlbox.TabIndex = 1;
            // 
            // btnClose2
            // 
            this.btnClose2.Image = global::XTraderLite.Properties.Resources.close_normal;
            this.btnClose2.Location = new System.Drawing.Point(32, 0);
            this.btnClose2.Name = "btnClose2";
            this.btnClose2.Size = new System.Drawing.Size(44, 19);
            this.btnClose2.TabIndex = 3;
            this.btnClose2.TabStop = false;
            // 
            // btnMin2
            // 
            this.btnMin2.Image = global::XTraderLite.Properties.Resources.min_normal;
            this.btnMin2.Location = new System.Drawing.Point(0, 0);
            this.btnMin2.Name = "btnMin2";
            this.btnMin2.Size = new System.Drawing.Size(32, 19);
            this.btnMin2.TabIndex = 2;
            this.btnMin2.TabStop = false;
            // 
            // btnLogin2
            // 
            this.btnLogin2.BackgroundImage = global::XTraderLite.Properties.Resources.login_normal;
            this.btnLogin2.Location = new System.Drawing.Point(482, 280);
            this.btnLogin2.Name = "btnLogin2";
            this.btnLogin2.Size = new System.Drawing.Size(168, 46);
            this.btnLogin2.TabIndex = 0;
            this.btnLogin2.UseVisualStyleBackColor = true;
            // 
            // panel_Classic
            // 
            this.panel_Classic.Controls.Add(this.topImage);
            this.panel_Classic.Controls.Add(this.label4);
            this.panel_Classic.Controls.Add(this.label5);
            this.panel_Classic.Controls.Add(this.password);
            this.panel_Classic.Controls.Add(this.btnLogin);
            this.panel_Classic.Controls.Add(this._msg);
            this.panel_Classic.Controls.Add(this.label3);
            this.panel_Classic.Controls.Add(this.btnCancel);
            this.panel_Classic.Controls.Add(this.username);
            this.panel_Classic.Controls.Add(this.label2);
            this.panel_Classic.Controls.Add(this.label1);
            this.panel_Classic.Controls.Add(this.cbServer);
            this.panel_Classic.Controls.Add(this.checkBox1);
            this.panel_Classic.Location = new System.Drawing.Point(3, 3);
            this.panel_Classic.Name = "panel_Classic";
            this.panel_Classic.Size = new System.Drawing.Size(574, 115);
            this.panel_Classic.TabIndex = 24;
            // 
            // topImage
            // 
            this.topImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.topImage.Image = global::XTraderLite.Properties.Resources.login;
            this.topImage.InitialImage = null;
            this.topImage.Location = new System.Drawing.Point(0, 0);
            this.topImage.Name = "topImage";
            this.topImage.Size = new System.Drawing.Size(574, 224);
            this.topImage.TabIndex = 0;
            this.topImage.TabStop = false;
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(218, 244);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(2, 110);
            this.label4.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(20, 333);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 21);
            this.label5.TabIndex = 23;
            this.label5.Text = "交易大师 荣誉出品";
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(297, 298);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(74, 21);
            this.password.TabIndex = 3;
            this.password.Text = "888888";
            // 
            // LoginForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1022, 709);
            this.Controls.Add(this.holder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登入交易大师";
            this.holder.ResumeLayout(false);
            this.panel_XGJ.ResumeLayout(false);
            this.panel_XGJ.PerformLayout();
            this.panel_controlbox.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.btnClose2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnMin2)).EndInit();
            this.panel_Classic.ResumeLayout(false);
            this.panel_Classic.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.topImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox topImage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label _msg;
        private System.Windows.Forms.Panel holder;
        private System.Windows.Forms.MaskedTextBox password;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel_Classic;
        private System.Windows.Forms.Panel panel_XGJ;
        private System.Windows.Forms.Button btnLogin2;
        private System.Windows.Forms.Panel panel_controlbox;
        private System.Windows.Forms.PictureBox btnMin2;
        private System.Windows.Forms.PictureBox btnClose2;
        private System.Windows.Forms.TextBox username2;
        private System.Windows.Forms.MaskedTextBox password2;
        private System.Windows.Forms.Label __msg2;
    }
}