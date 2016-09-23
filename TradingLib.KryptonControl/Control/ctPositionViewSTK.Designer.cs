namespace TradingLib.KryptonControl
{
    partial class ctPositionViewSTK
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
            this.positionGrid = new TradingLib.KryptonControl.FGrid();
            ((System.ComponentModel.ISupportInitialize)(this.positionGrid)).BeginInit();
            this.SuspendLayout();
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
            this.positionGrid.Location = new System.Drawing.Point(0, 0);
            this.positionGrid.Margin = new System.Windows.Forms.Padding(0);
            this.positionGrid.Name = "positionGrid";
            this.positionGrid.ReadOnly = true;
            this.positionGrid.RowHeadersVisible = false;
            this.positionGrid.RowTemplate.Height = 23;
            this.positionGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.positionGrid.Size = new System.Drawing.Size(707, 316);
            this.positionGrid.TabIndex = 0;
            // 
            // ctPositionViewSTK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.positionGrid);
            this.Name = "ctPositionViewSTK";
            this.Size = new System.Drawing.Size(707, 316);
            ((System.ComponentModel.ISupportInitialize)(this.positionGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private FGrid positionGrid;


    }
}
