namespace TradingLib.TraderAPI.Future
{
    partial class ctrlPosition
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button6 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.positionGrid = new TradingLib.TraderAPI.FPosition();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.positionGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Dock = System.Windows.Forms.DockStyle.Left;
            this.button1.Location = new System.Drawing.Point(0, 0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(27, 233);
            this.button1.TabIndex = 1;
            this.button1.Text = "持仓";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.button6);
            this.panel1.Controls.Add(this.button5);
            this.panel1.Controls.Add(this.button4);
            this.panel1.Controls.Add(this.button3);
            this.panel1.Controls.Add(this.button2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(27, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(775, 25);
            this.panel1.TabIndex = 2;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(291, 0);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(66, 20);
            this.button6.TabIndex = 4;
            this.button6.Text = "止损止盈";
            this.button6.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(219, 0);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(66, 20);
            this.button5.TabIndex = 3;
            this.button5.Text = "快捷锁仓";
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(147, 0);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(66, 20);
            this.button4.TabIndex = 2;
            this.button4.Text = "快捷反手";
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(75, 0);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(66, 20);
            this.button3.TabIndex = 1;
            this.button3.Text = "快捷平仓";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(3, 0);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(66, 20);
            this.button2.TabIndex = 0;
            this.button2.Text = "全部平仓";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // positionGrid
            // 
            this.positionGrid.AllowUserToAddRows = false;
            this.positionGrid.AllowUserToDeleteRows = false;
            this.positionGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.positionGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.positionGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.positionGrid.BackgroundColor = System.Drawing.Color.White;
            this.positionGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.positionGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.positionGrid.Location = new System.Drawing.Point(27, 25);
            this.positionGrid.Margin = new System.Windows.Forms.Padding(0);
            this.positionGrid.Name = "positionGrid";
            this.positionGrid.ReadOnly = true;
            this.positionGrid.RowHeadersVisible = false;
            this.positionGrid.RowTemplate.Height = 23;
            this.positionGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.positionGrid.Size = new System.Drawing.Size(775, 208);
            this.positionGrid.TabIndex = 3;
            // 
            // ctrlPosition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.positionGrid);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Name = "ctrlPosition";
            this.Size = new System.Drawing.Size(802, 233);
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.positionGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button6;
        private FPosition positionGrid;
    }
}
