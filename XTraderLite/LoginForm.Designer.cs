namespace XTraderLite
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
            this.topImage = new System.Windows.Forms.PictureBox();
            this.holder = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.btnCancel = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this._msg = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.btnLogin = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonCheckBox1 = new ComponentFactory.Krypton.Toolkit.KryptonCheckBox();
            this.kryptonMaskedTextBox1 = new ComponentFactory.Krypton.Toolkit.KryptonMaskedTextBox();
            this.kryptonTextBox1 = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonComboBox1 = new ComponentFactory.Krypton.Toolkit.KryptonComboBox();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonPalette1 = new ComponentFactory.Krypton.Toolkit.KryptonPalette(this.components);
            this.kryptonManager1 = new ComponentFactory.Krypton.Toolkit.KryptonManager(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.topImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.holder)).BeginInit();
            this.holder.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // topImage
            // 
            this.topImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.topImage.Image = global::XTraderLite.Properties.Resources.login;
            this.topImage.InitialImage = null;
            this.topImage.Location = new System.Drawing.Point(0, 0);
            this.topImage.Name = "topImage";
            this.topImage.Size = new System.Drawing.Size(560, 215);
            this.topImage.TabIndex = 0;
            this.topImage.TabStop = false;
            // 
            // holder
            // 
            this.holder.Controls.Add(this.btnCancel);
            this.holder.Controls.Add(this._msg);
            this.holder.Controls.Add(this.btnLogin);
            this.holder.Controls.Add(this.kryptonCheckBox1);
            this.holder.Controls.Add(this.kryptonMaskedTextBox1);
            this.holder.Controls.Add(this.kryptonTextBox1);
            this.holder.Controls.Add(this.kryptonLabel3);
            this.holder.Controls.Add(this.kryptonLabel2);
            this.holder.Controls.Add(this.kryptonComboBox1);
            this.holder.Controls.Add(this.kryptonLabel1);
            this.holder.Controls.Add(this.topImage);
            this.holder.Location = new System.Drawing.Point(1, 1);
            this.holder.Name = "holder";
            this.holder.Size = new System.Drawing.Size(560, 368);
            this.holder.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(480, 293);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(68, 25);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Values.Text = "退 出";
            // 
            // _msg
            // 
            this._msg.Location = new System.Drawing.Point(221, 335);
            this._msg.Name = "_msg";
            this._msg.Size = new System.Drawing.Size(172, 20);
            this._msg.TabIndex = 10;
            this._msg.Values.Text = "请联系业务人员购买正版软件";
            // 
            // btnLogin
            // 
            this.btnLogin.Location = new System.Drawing.Point(481, 234);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(68, 53);
            this.btnLogin.TabIndex = 1;
            this.btnLogin.Values.Text = "登 入";
            // 
            // kryptonCheckBox1
            // 
            this.kryptonCheckBox1.Location = new System.Drawing.Point(401, 294);
            this.kryptonCheckBox1.Name = "kryptonCheckBox1";
            this.kryptonCheckBox1.Size = new System.Drawing.Size(73, 20);
            this.kryptonCheckBox1.TabIndex = 7;
            this.kryptonCheckBox1.Values.Text = "记住密码";
            // 
            // kryptonMaskedTextBox1
            // 
            this.kryptonMaskedTextBox1.Location = new System.Drawing.Point(287, 296);
            this.kryptonMaskedTextBox1.Name = "kryptonMaskedTextBox1";
            this.kryptonMaskedTextBox1.Size = new System.Drawing.Size(107, 20);
            this.kryptonMaskedTextBox1.TabIndex = 6;
            // 
            // kryptonTextBox1
            // 
            this.kryptonTextBox1.Location = new System.Drawing.Point(287, 268);
            this.kryptonTextBox1.Name = "kryptonTextBox1";
            this.kryptonTextBox1.Size = new System.Drawing.Size(172, 20);
            this.kryptonTextBox1.TabIndex = 5;
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(221, 293);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(60, 20);
            this.kryptonLabel3.TabIndex = 4;
            this.kryptonLabel3.Values.Text = "登入密码";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(221, 265);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(60, 20);
            this.kryptonLabel2.TabIndex = 3;
            this.kryptonLabel2.Values.Text = "登入用户";
            // 
            // kryptonComboBox1
            // 
            this.kryptonComboBox1.DropDownWidth = 172;
            this.kryptonComboBox1.Location = new System.Drawing.Point(287, 237);
            this.kryptonComboBox1.Name = "kryptonComboBox1";
            this.kryptonComboBox1.Size = new System.Drawing.Size(172, 21);
            this.kryptonComboBox1.TabIndex = 2;
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(221, 237);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(60, 20);
            this.kryptonLabel1.TabIndex = 1;
            this.kryptonLabel1.Values.Text = "行情站点";
            // 
            // kryptonPalette1
            // 
            this.kryptonPalette1.BasePaletteMode = ComponentFactory.Krypton.Toolkit.PaletteMode.ProfessionalSystem;
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
            this.ClientSize = new System.Drawing.Size(562, 370);
            this.Controls.Add(this.holder);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "LoginForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "LoginForm";
            ((System.ComponentModel.ISupportInitialize)(this.topImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.holder)).EndInit();
            this.holder.ResumeLayout(false);
            this.holder.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonComboBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox topImage;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel holder;
        private ComponentFactory.Krypton.Toolkit.KryptonComboBox kryptonComboBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonCheckBox kryptonCheckBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonMaskedTextBox kryptonMaskedTextBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonTextBox kryptonTextBox1;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnLogin;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel _msg;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnCancel;
        private ComponentFactory.Krypton.Toolkit.KryptonPalette kryptonPalette1;
        private ComponentFactory.Krypton.Toolkit.KryptonManager kryptonManager1;
    }
}