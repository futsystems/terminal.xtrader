namespace TradingLib.KryptonControl
{
    partial class PageSTKChangePass
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
            this.fPanel1 = new TradingLib.KryptonControl.FPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.newpass1 = new System.Windows.Forms.MaskedTextBox();
            this.pass = new System.Windows.Forms.MaskedTextBox();
            this.newpass2 = new System.Windows.Forms.MaskedTextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.fPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // fPanel1
            // 
            this.fPanel1.Controls.Add(this.btnSubmit);
            this.fPanel1.Controls.Add(this.newpass2);
            this.fPanel1.Controls.Add(this.pass);
            this.fPanel1.Controls.Add(this.newpass1);
            this.fPanel1.Controls.Add(this.label3);
            this.fPanel1.Controls.Add(this.label2);
            this.fPanel1.Controls.Add(this.label1);
            this.fPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.fPanel1.Location = new System.Drawing.Point(0, 0);
            this.fPanel1.Name = "fPanel1";
            this.fPanel1.Size = new System.Drawing.Size(813, 295);
            this.fPanel1.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Location = new System.Drawing.Point(171, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "当前交易密码:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Location = new System.Drawing.Point(183, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "新交易密码:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Location = new System.Drawing.Point(171, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "确认交易密码:";
            // 
            // newpass1
            // 
            this.newpass1.Location = new System.Drawing.Point(260, 81);
            this.newpass1.Name = "newpass1";
            this.newpass1.PasswordChar = '*';
            this.newpass1.Size = new System.Drawing.Size(152, 21);
            this.newpass1.TabIndex = 3;
            // 
            // pass
            // 
            this.pass.Location = new System.Drawing.Point(260, 54);
            this.pass.Name = "pass";
            this.pass.PasswordChar = '*';
            this.pass.Size = new System.Drawing.Size(152, 21);
            this.pass.TabIndex = 4;
            // 
            // newpass2
            // 
            this.newpass2.Location = new System.Drawing.Point(260, 108);
            this.newpass2.Name = "newpass2";
            this.newpass2.PasswordChar = '*';
            this.newpass2.Size = new System.Drawing.Size(152, 21);
            this.newpass2.TabIndex = 5;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(260, 135);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(94, 23);
            this.btnSubmit.TabIndex = 6;
            this.btnSubmit.Text = "确定更改";
            this.btnSubmit.UseVisualStyleBackColor = true;
            // 
            // PageSTKChangePass
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.fPanel1);
            this.Name = "PageSTKChangePass";
            this.Size = new System.Drawing.Size(813, 295);
            this.fPanel1.ResumeLayout(false);
            this.fPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private FPanel fPanel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.MaskedTextBox newpass1;
        private System.Windows.Forms.MaskedTextBox newpass2;
        private System.Windows.Forms.MaskedTextBox pass;
        private System.Windows.Forms.Button btnSubmit;
    }
}
