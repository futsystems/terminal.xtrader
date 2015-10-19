namespace TradingLib.TraderControl
{
    partial class ctPositionView
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
            this.positiongrid = new Telerik.WinControls.UI.RadGridView();
            this.radPanel1 = new Telerik.WinControls.UI.RadPanel();
            this.isDoubleFlat = new Telerik.WinControls.UI.RadCheckBox();
            this.btnCancel = new Telerik.WinControls.UI.RadButton();
            this.btnFlatAll = new Telerik.WinControls.UI.RadButton();
            this.btnFlat = new Telerik.WinControls.UI.RadButton();
            this.btnShowAll = new Telerik.WinControls.UI.RadRadioButton();
            this.btnShowHold = new Telerik.WinControls.UI.RadRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.positiongrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.positiongrid.MasterTemplate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).BeginInit();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.isDoubleFlat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFlatAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFlat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnShowAll)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnShowHold)).BeginInit();
            this.SuspendLayout();
            // 
            // positiongrid
            // 
            this.positiongrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.positiongrid.Location = new System.Drawing.Point(0, 0);
            this.positiongrid.Name = "positiongrid";
            this.positiongrid.Size = new System.Drawing.Size(694, 170);
            this.positiongrid.TabIndex = 0;
            this.positiongrid.Text = "radGridView1";
            this.positiongrid.ValueChanged += new System.EventHandler(this.positiongrid_ValueChanged);
            // 
            // radPanel1
            // 
            this.radPanel1.Controls.Add(this.isDoubleFlat);
            this.radPanel1.Controls.Add(this.btnCancel);
            this.radPanel1.Controls.Add(this.btnFlatAll);
            this.radPanel1.Controls.Add(this.btnFlat);
            this.radPanel1.Controls.Add(this.btnShowAll);
            this.radPanel1.Controls.Add(this.btnShowHold);
            this.radPanel1.Controls.Add(this.positiongrid);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(694, 195);
            this.radPanel1.TabIndex = 1;
            this.radPanel1.Text = "radPanel1";
            // 
            // isDoubleFlat
            // 
            this.isDoubleFlat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.isDoubleFlat.Location = new System.Drawing.Point(328, 172);
            this.isDoubleFlat.Name = "isDoubleFlat";
            this.isDoubleFlat.Size = new System.Drawing.Size(66, 18);
            this.isDoubleFlat.TabIndex = 11;
            this.isDoubleFlat.Text = "双击平仓";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(262, 171);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(60, 22);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "撤 单";
            // 
            // btnFlatAll
            // 
            this.btnFlatAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFlatAll.Location = new System.Drawing.Point(197, 171);
            this.btnFlatAll.Name = "btnFlatAll";
            this.btnFlatAll.Size = new System.Drawing.Size(60, 22);
            this.btnFlatAll.TabIndex = 8;
            this.btnFlatAll.Text = "全 平";
            // 
            // btnFlat
            // 
            this.btnFlat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnFlat.Location = new System.Drawing.Point(131, 171);
            this.btnFlat.Name = "btnFlat";
            this.btnFlat.Size = new System.Drawing.Size(60, 22);
            this.btnFlat.TabIndex = 7;
            this.btnFlat.Text = "平 仓";
            // 
            // btnShowAll
            // 
            this.btnShowAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowAll.Location = new System.Drawing.Point(55, 172);
            this.btnShowAll.Name = "btnShowAll";
            this.btnShowAll.Size = new System.Drawing.Size(66, 18);
            this.btnShowAll.TabIndex = 2;
            this.btnShowAll.Text = "当日明细";
            this.btnShowAll.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.btnShowAll_ToggleStateChanged);
            // 
            // btnShowHold
            // 
            this.btnShowHold.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnShowHold.Location = new System.Drawing.Point(4, 172);
            this.btnShowHold.Name = "btnShowHold";
            this.btnShowHold.Size = new System.Drawing.Size(43, 18);
            this.btnShowHold.TabIndex = 1;
            this.btnShowHold.Text = "持仓";
            this.btnShowHold.ToggleStateChanged += new Telerik.WinControls.UI.StateChangedEventHandler(this.btnShowHold_ToggleStateChanged);
            // 
            // ctPositionView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.radPanel1);
            this.Name = "ctPositionView";
            this.Size = new System.Drawing.Size(694, 195);
            this.Load += new System.EventHandler(this.ctPositionView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.positiongrid.MasterTemplate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.positiongrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.radPanel1)).EndInit();
            this.radPanel1.ResumeLayout(false);
            this.radPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.isDoubleFlat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFlatAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnFlat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnShowAll)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnShowHold)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Telerik.WinControls.UI.RadGridView positiongrid;
        private Telerik.WinControls.UI.RadPanel radPanel1;
        private Telerik.WinControls.UI.RadRadioButton btnShowAll;
        private Telerik.WinControls.UI.RadRadioButton btnShowHold;
        private Telerik.WinControls.UI.RadButton btnFlatAll;
        private Telerik.WinControls.UI.RadButton btnFlat;
        private Telerik.WinControls.UI.RadButton btnCancel;
        private Telerik.WinControls.UI.RadCheckBox isDoubleFlat;
    }
}
