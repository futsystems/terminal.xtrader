namespace DataCoreTest
{
    partial class MainForm
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.ipaddress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.port = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.clientNum = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.debugControl1 = new DataCoreTest.DebugControl();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "IP:";
            // 
            // ipaddress
            // 
            this.ipaddress.Location = new System.Drawing.Point(43, 20);
            this.ipaddress.Name = "ipaddress";
            this.ipaddress.Size = new System.Drawing.Size(131, 21);
            this.ipaddress.TabIndex = 2;
            this.ipaddress.Text = "116.62.187.140";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(180, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "Port:";
            // 
            // port
            // 
            this.port.Location = new System.Drawing.Point(221, 20);
            this.port.Name = "port";
            this.port.Size = new System.Drawing.Size(54, 21);
            this.port.TabIndex = 4;
            this.port.Text = "55622";
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(268, 61);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 5;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            // 
            // clientNum
            // 
            this.clientNum.Location = new System.Drawing.Point(91, 63);
            this.clientNum.Name = "clientNum";
            this.clientNum.Size = new System.Drawing.Size(61, 21);
            this.clientNum.TabIndex = 6;
            this.clientNum.Text = "1";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "Client Num:";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.ipaddress);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.clientNum);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.btnStart);
            this.groupBox1.Controls.Add(this.port);
            this.groupBox1.Location = new System.Drawing.Point(886, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(357, 550);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "groupBox1";
            // 
            // debugControl1
            // 
            this.debugControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.debugControl1.EnableSearching = true;
            this.debugControl1.ExternalTimeStamp = 0;
            this.debugControl1.Location = new System.Drawing.Point(0, 0);
            this.debugControl1.Margin = new System.Windows.Forms.Padding(2);
            this.debugControl1.Name = "debugControl1";
            this.debugControl1.Size = new System.Drawing.Size(878, 550);
            this.debugControl1.TabIndex = 0;
            this.debugControl1.TimeStamps = true;
            this.debugControl1.UseExternalTimeStamp = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1241, 550);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.debugControl1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DataFarm Test";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private DebugControl debugControl1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox ipaddress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox port;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.TextBox clientNum;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}

