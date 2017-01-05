namespace TradingLib.XTrader.Future
{
    partial class ctrlTraderLogin
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.serverList = new System.Windows.Forms.ListBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.account = new System.Windows.Forms.TextBox();
            this.encrypt = new System.Windows.Forms.ComboBox();
            this.verify = new System.Windows.Forms.MaskedTextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.seat = new System.Windows.Forms.ComboBox();
            this._msg = new System.Windows.Forms.Label();
            this.holder = new System.Windows.Forms.Panel();
            this.password = new System.Windows.Forms.MaskedTextBox();
            this.ctVerify1 = new TradingLib.XTrader.Future.ctVerify();
            this.btnUnLock = new System.Windows.Forms.Button();
            this.PanelVerify = new System.Windows.Forms.Panel();
            this.holder.SuspendLayout();
            this.PanelVerify.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "交易站点:";
            // 
            // serverList
            // 
            this.serverList.FormattingEnabled = true;
            this.serverList.ItemHeight = 12;
            this.serverList.Location = new System.Drawing.Point(3, 31);
            this.serverList.Name = "serverList";
            this.serverList.Size = new System.Drawing.Size(160, 124);
            this.serverList.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(172, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "客 户 号:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(172, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "交易密码:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 3);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "安全方式:";
            // 
            // account
            // 
            this.account.Location = new System.Drawing.Point(238, 31);
            this.account.Name = "account";
            this.account.Size = new System.Drawing.Size(160, 21);
            this.account.TabIndex = 2;
            // 
            // encrypt
            // 
            this.encrypt.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.encrypt.FormattingEnabled = true;
            this.encrypt.Items.AddRange(new object[] {
            "验证码"});
            this.encrypt.Location = new System.Drawing.Point(69, 3);
            this.encrypt.Name = "encrypt";
            this.encrypt.Size = new System.Drawing.Size(70, 20);
            this.encrypt.TabIndex = 4;
            // 
            // verify
            // 
            this.verify.Location = new System.Drawing.Point(151, 3);
            this.verify.Name = "verify";
            this.verify.Size = new System.Drawing.Size(78, 21);
            this.verify.TabIndex = 5;
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(238, 126);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(75, 23);
            this.btnLogin.TabIndex = 6;
            this.btnLogin.Text = "登 录";
            this.btnLogin.UseVisualStyleBackColor = true;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(323, 126);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 7;
            this.btnExit.Text = "退 出";
            this.btnExit.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(172, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "交易席位:";
            // 
            // seat
            // 
            this.seat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.seat.FormattingEnabled = true;
            this.seat.Location = new System.Drawing.Point(237, 6);
            this.seat.Name = "seat";
            this.seat.Size = new System.Drawing.Size(161, 20);
            this.seat.TabIndex = 1;
            // 
            // _msg
            // 
            this._msg.AutoSize = true;
            this._msg.ForeColor = System.Drawing.Color.Blue;
            this._msg.Location = new System.Drawing.Point(3, 167);
            this._msg.Name = "_msg";
            this._msg.Size = new System.Drawing.Size(233, 12);
            this._msg.TabIndex = 13;
            this._msg.Text = "电信、联通用户请分别登入电信、联通站点";
            // 
            // holder
            // 
            this.holder.BackColor = System.Drawing.Color.Transparent;
            this.holder.Controls.Add(this.PanelVerify);
            this.holder.Controls.Add(this.btnUnLock);
            this.holder.Controls.Add(this.password);
            this.holder.Controls.Add(this.label1);
            this.holder.Controls.Add(this._msg);
            this.holder.Controls.Add(this.serverList);
            this.holder.Controls.Add(this.seat);
            this.holder.Controls.Add(this.label2);
            this.holder.Controls.Add(this.label5);
            this.holder.Controls.Add(this.label3);
            this.holder.Controls.Add(this.btnExit);
            this.holder.Controls.Add(this.btnLogin);
            this.holder.Controls.Add(this.account);
            this.holder.Location = new System.Drawing.Point(211, 0);
            this.holder.Name = "holder";
            this.holder.Size = new System.Drawing.Size(475, 187);
            this.holder.TabIndex = 14;
            // 
            // password
            // 
            this.password.Location = new System.Drawing.Point(238, 58);
            this.password.Name = "password";
            this.password.PasswordChar = '*';
            this.password.Size = new System.Drawing.Size(160, 21);
            this.password.TabIndex = 15;
            // 
            // ctVerify1
            // 
            this.ctVerify1.Location = new System.Drawing.Point(231, 3);
            this.ctVerify1.Name = "ctVerify1";
            this.ctVerify1.Size = new System.Drawing.Size(63, 21);
            this.ctVerify1.TabIndex = 14;
            this.ctVerify1.TabStop = false;
            // 
            // btnUnLock
            // 
            this.btnUnLock.Location = new System.Drawing.Point(238, 126);
            this.btnUnLock.Name = "btnUnLock";
            this.btnUnLock.Size = new System.Drawing.Size(75, 23);
            this.btnUnLock.TabIndex = 16;
            this.btnUnLock.Text = "解 锁";
            this.btnUnLock.UseVisualStyleBackColor = true;
            // 
            // PanelVerify
            // 
            this.PanelVerify.Controls.Add(this.label4);
            this.PanelVerify.Controls.Add(this.encrypt);
            this.PanelVerify.Controls.Add(this.verify);
            this.PanelVerify.Controls.Add(this.ctVerify1);
            this.PanelVerify.Location = new System.Drawing.Point(169, 85);
            this.PanelVerify.Name = "PanelVerify";
            this.PanelVerify.Size = new System.Drawing.Size(303, 26);
            this.PanelVerify.TabIndex = 17;
            // 
            // ctrlTraderLogin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.holder);
            this.Name = "ctrlTraderLogin";
            this.Size = new System.Drawing.Size(933, 235);
            this.holder.ResumeLayout(false);
            this.holder.PerformLayout();
            this.PanelVerify.ResumeLayout(false);
            this.PanelVerify.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox serverList;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox account;
        private System.Windows.Forms.ComboBox encrypt;
        private System.Windows.Forms.MaskedTextBox verify;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox seat;
        private System.Windows.Forms.Label _msg;
        private System.Windows.Forms.Panel holder;
        private ctVerify ctVerify1;
        private System.Windows.Forms.MaskedTextBox password;
        private System.Windows.Forms.Button btnUnLock;
        private System.Windows.Forms.Panel PanelVerify;
    }
}
