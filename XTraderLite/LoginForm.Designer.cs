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
            this.topImage = new System.Windows.Forms.PictureBox();
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
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.password = new System.Windows.Forms.MaskedTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.topImage)).BeginInit();
            this.holder.SuspendLayout();
            this.SuspendLayout();
            // 
            // topImage
            // 
            this.topImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.topImage.Image = global::XTraderLite.Properties.Resources.login;
            this.topImage.InitialImage = null;
            this.topImage.Location = new System.Drawing.Point(0, 0);
            this.topImage.Name = "topImage";
            this.topImage.Size = new System.Drawing.Size(560, 217);
            this.topImage.TabIndex = 0;
            this.topImage.TabStop = false;
            // 
            // _msg
            // 
            this._msg.AutoSize = true;
            this._msg.BackColor = System.Drawing.Color.Transparent;
            this._msg.Location = new System.Drawing.Point(235, 332);
            this._msg.Name = "_msg";
            this._msg.Size = new System.Drawing.Size(17, 12);
            this._msg.TabIndex = 21;
            this._msg.Text = "--";
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(460, 290);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "取 消";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(460, 236);
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
            this.checkBox1.Location = new System.Drawing.Point(382, 295);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(72, 16);
            this.checkBox1.TabIndex = 4;
            this.checkBox1.Text = "记住密码";
            this.checkBox1.UseVisualStyleBackColor = false;
            // 
            // username
            // 
            this.username.Location = new System.Drawing.Point(302, 263);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(155, 21);
            this.username.TabIndex = 2;
            this.username.Text = "88888888";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(233, 295);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 15;
            this.label3.Text = "登入密码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(231, 266);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "登入用户";
            // 
            // cbServer
            // 
            this.cbServer.FormattingEnabled = true;
            this.cbServer.Location = new System.Drawing.Point(302, 236);
            this.cbServer.Name = "cbServer";
            this.cbServer.Size = new System.Drawing.Size(155, 20);
            this.cbServer.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(231, 239);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 12;
            this.label1.Text = "行情服务器";
            // 
            // holder
            // 
            this.holder.BackColor = System.Drawing.Color.Transparent;
            this.holder.Controls.Add(this.label5);
            this.holder.Controls.Add(this.label4);
            this.holder.Controls.Add(this.password);
            this.holder.Controls.Add(this._msg);
            this.holder.Controls.Add(this.topImage);
            this.holder.Controls.Add(this.btnCancel);
            this.holder.Controls.Add(this.btnLogin);
            this.holder.Controls.Add(this.label1);
            this.holder.Controls.Add(this.checkBox1);
            this.holder.Controls.Add(this.cbServer);
            this.holder.Controls.Add(this.label2);
            this.holder.Controls.Add(this.username);
            this.holder.Controls.Add(this.label3);
            this.holder.Location = new System.Drawing.Point(1, 1);
            this.holder.Name = "holder";
            this.holder.Size = new System.Drawing.Size(560, 370);
            this.holder.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(23, 325);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(143, 21);
            this.label5.TabIndex = 23;
            this.label5.Text = "交易大师 荣誉出品";
            // 
            // label4
            // 
            this.label4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label4.Location = new System.Drawing.Point(223, 236);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(2, 110);
            this.label4.TabIndex = 22;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(302, 290);
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
            this.ClientSize = new System.Drawing.Size(562, 372);
            this.Controls.Add(this.holder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "登入交易大师";
            ((System.ComponentModel.ISupportInitialize)(this.topImage)).EndInit();
            this.holder.ResumeLayout(false);
            this.holder.PerformLayout();
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
    }
}