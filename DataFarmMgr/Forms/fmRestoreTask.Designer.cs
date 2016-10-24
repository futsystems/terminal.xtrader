namespace TradingLib.DataFarmManager
{
    partial class fmRestoreTask
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnQryTaskStatus = new System.Windows.Forms.Button();
            this.btnResetTask = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnResetTask);
            this.panel1.Controls.Add(this.btnQryTaskStatus);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(616, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(254, 583);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(616, 583);
            this.panel2.TabIndex = 1;
            // 
            // btnQryTaskStatus
            // 
            this.btnQryTaskStatus.Location = new System.Drawing.Point(6, 3);
            this.btnQryTaskStatus.Name = "btnQryTaskStatus";
            this.btnQryTaskStatus.Size = new System.Drawing.Size(96, 43);
            this.btnQryTaskStatus.TabIndex = 0;
            this.btnQryTaskStatus.Text = "查询任务状态";
            this.btnQryTaskStatus.UseVisualStyleBackColor = true;
            // 
            // btnResetTask
            // 
            this.btnResetTask.Location = new System.Drawing.Point(126, 3);
            this.btnResetTask.Name = "btnResetTask";
            this.btnResetTask.Size = new System.Drawing.Size(96, 43);
            this.btnResetTask.TabIndex = 1;
            this.btnResetTask.Text = "重置任务状态";
            this.btnResetTask.UseVisualStyleBackColor = true;
            // 
            // fmRestoreTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(870, 583);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "fmRestoreTask";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "数据恢复任务";
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnQryTaskStatus;
        private System.Windows.Forms.Button btnResetTask;
    }
}