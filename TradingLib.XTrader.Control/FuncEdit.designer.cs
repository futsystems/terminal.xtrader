namespace CStock
{
    partial class FuncEdit
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
            this.GSView = new System.Windows.Forms.TreeView();
            this.bt1 = new System.Windows.Forms.Button();
            this.bt2 = new System.Windows.Forms.Button();
            this.bt3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.add = new System.Windows.Forms.Button();
            this.delete = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // GSView
            // 
            this.GSView.HideSelection = false;
            this.GSView.Location = new System.Drawing.Point(12, 12);
            this.GSView.Name = "GSView";
            this.GSView.Size = new System.Drawing.Size(272, 373);
            this.GSView.TabIndex = 0;
            this.GSView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.GSView_AfterSelect);
            // 
            // bt1
            // 
            this.bt1.Location = new System.Drawing.Point(305, 12);
            this.bt1.Name = "bt1";
            this.bt1.Size = new System.Drawing.Size(99, 31);
            this.bt1.TabIndex = 2;
            this.bt1.Text = "新建";
            this.bt1.UseVisualStyleBackColor = true;
            this.bt1.Click += new System.EventHandler(this.bt1_Click);
            // 
            // bt2
            // 
            this.bt2.Enabled = false;
            this.bt2.Location = new System.Drawing.Point(305, 49);
            this.bt2.Name = "bt2";
            this.bt2.Size = new System.Drawing.Size(99, 31);
            this.bt2.TabIndex = 1;
            this.bt2.Text = "修改";
            this.bt2.UseVisualStyleBackColor = true;
            this.bt2.Click += new System.EventHandler(this.bt2_Click);
            // 
            // bt3
            // 
            this.bt3.Enabled = false;
            this.bt3.Location = new System.Drawing.Point(305, 86);
            this.bt3.Name = "bt3";
            this.bt3.Size = new System.Drawing.Size(99, 31);
            this.bt3.TabIndex = 3;
            this.bt3.Text = "删除";
            this.bt3.UseVisualStyleBackColor = true;
            this.bt3.Click += new System.EventHandler(this.bt3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(305, 342);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(99, 31);
            this.button4.TabIndex = 4;
            this.button4.Text = "关闭";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // add
            // 
            this.add.Location = new System.Drawing.Point(305, 123);
            this.add.Name = "add";
            this.add.Size = new System.Drawing.Size(97, 27);
            this.add.TabIndex = 5;
            this.add.Text = "增加分类";
            this.add.UseVisualStyleBackColor = true;
            this.add.Click += new System.EventHandler(this.add_Click);
            // 
            // delete
            // 
            this.delete.Location = new System.Drawing.Point(305, 156);
            this.delete.Name = "delete";
            this.delete.Size = new System.Drawing.Size(96, 27);
            this.delete.TabIndex = 6;
            this.delete.Text = "删除分类";
            this.delete.UseVisualStyleBackColor = true;
            this.delete.Click += new System.EventHandler(this.delete_Click);
            // 
            // FuncEdit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 395);
            this.Controls.Add(this.delete);
            this.Controls.Add(this.add);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.bt3);
            this.Controls.Add(this.bt1);
            this.Controls.Add(this.bt2);
            this.Controls.Add(this.GSView);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FuncEdit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "公式管理器";
            this.Load += new System.EventHandler(this.FuncEdit_Load);
            this.Shown += new System.EventHandler(this.FuncEdit_Shown);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView GSView;
        private System.Windows.Forms.Button bt1;
        private System.Windows.Forms.Button bt2;
        private System.Windows.Forms.Button bt3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button add;
        private System.Windows.Forms.Button delete;
    }
}