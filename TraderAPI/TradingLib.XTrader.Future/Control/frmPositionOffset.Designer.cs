namespace TradingLib.XTrader.Future
{
    partial class frmPositionOffset
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
            this.radPanel1 = new System.Windows.Forms.Panel();
            this.directionLabel = new System.Windows.Forms.Label();
            this.btnUpdateOffset = new System.Windows.Forms.Button();
            this.btnSwitchOffset = new System.Windows.Forms.Button();
            this.switchLabel = new System.Windows.Forms.Label();
            this.radLabel2 = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.NumericUpDown();
            this.radLabel5 = new System.Windows.Forms.Label();
            this.size = new System.Windows.Forms.NumericUpDown();
            this.sizelabel = new System.Windows.Forms.Label();
            this.typelist = new System.Windows.Forms.ComboBox();
            this.value = new System.Windows.Forms.NumericUpDown();
            this.arglabel = new System.Windows.Forms.Label();
            this.typelabel = new System.Windows.Forms.Label();
            this.symbol = new System.Windows.Forms.Label();
            this.radLabel1 = new System.Windows.Forms.Label();
            this.radPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.start)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.size)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.value)).BeginInit();
            this.SuspendLayout();
            // 
            // radPanel1
            // 
            this.radPanel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.radPanel1.Controls.Add(this.directionLabel);
            this.radPanel1.Controls.Add(this.btnUpdateOffset);
            this.radPanel1.Controls.Add(this.btnSwitchOffset);
            this.radPanel1.Controls.Add(this.switchLabel);
            this.radPanel1.Controls.Add(this.radLabel2);
            this.radPanel1.Controls.Add(this.start);
            this.radPanel1.Controls.Add(this.radLabel5);
            this.radPanel1.Controls.Add(this.size);
            this.radPanel1.Controls.Add(this.sizelabel);
            this.radPanel1.Controls.Add(this.typelist);
            this.radPanel1.Controls.Add(this.value);
            this.radPanel1.Controls.Add(this.arglabel);
            this.radPanel1.Controls.Add(this.typelabel);
            this.radPanel1.Controls.Add(this.symbol);
            this.radPanel1.Controls.Add(this.radLabel1);
            this.radPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.radPanel1.Location = new System.Drawing.Point(0, 0);
            this.radPanel1.Name = "radPanel1";
            this.radPanel1.Size = new System.Drawing.Size(212, 159);
            this.radPanel1.TabIndex = 0;
            // 
            // directionLabel
            // 
            this.directionLabel.Location = new System.Drawing.Point(96, 3);
            this.directionLabel.Name = "directionLabel";
            this.directionLabel.Size = new System.Drawing.Size(78, 18);
            this.directionLabel.TabIndex = 22;
            this.directionLabel.Text = "--";
            // 
            // btnUpdateOffset
            // 
            this.btnUpdateOffset.Location = new System.Drawing.Point(127, 115);
            this.btnUpdateOffset.Name = "btnUpdateOffset";
            this.btnUpdateOffset.Size = new System.Drawing.Size(47, 24);
            this.btnUpdateOffset.TabIndex = 21;
            this.btnUpdateOffset.Text = "更新";
            this.btnUpdateOffset.Click += new System.EventHandler(this.btnUpdateOffset_Click);
            // 
            // btnSwitchOffset
            // 
            this.btnSwitchOffset.Location = new System.Drawing.Point(73, 115);
            this.btnSwitchOffset.Name = "btnSwitchOffset";
            this.btnSwitchOffset.Size = new System.Drawing.Size(48, 24);
            this.btnSwitchOffset.TabIndex = 20;
            this.btnSwitchOffset.Text = "启用";
            this.btnSwitchOffset.Click += new System.EventHandler(this.btnSwitchOffset_Click);
            // 
            // switchLabel
            // 
            this.switchLabel.Location = new System.Drawing.Point(51, 91);
            this.switchLabel.Name = "switchLabel";
            this.switchLabel.Size = new System.Drawing.Size(41, 18);
            this.switchLabel.TabIndex = 19;
            this.switchLabel.Text = "--";
            // 
            // radLabel2
            // 
            this.radLabel2.Location = new System.Drawing.Point(12, 91);
            this.radLabel2.Name = "radLabel2";
            this.radLabel2.Size = new System.Drawing.Size(46, 18);
            this.radLabel2.TabIndex = 18;
            this.radLabel2.Text = "状态:";
            // 
            // start
            // 
            this.start.Location = new System.Drawing.Point(98, 90);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(76, 21);
            this.start.TabIndex = 17;
            this.start.TabStop = false;
            this.start.Visible = false;
            // 
            // radLabel5
            // 
            this.radLabel5.Location = new System.Drawing.Point(12, 91);
            this.radLabel5.Name = "radLabel5";
            this.radLabel5.Size = new System.Drawing.Size(54, 18);
            this.radLabel5.TabIndex = 16;
            this.radLabel5.Text = "跟踪起点:";
            this.radLabel5.Visible = false;
            // 
            // size
            // 
            this.size.Location = new System.Drawing.Point(98, 68);
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(76, 21);
            this.size.TabIndex = 15;
            this.size.TabStop = false;
            // 
            // sizelabel
            // 
            this.sizelabel.Location = new System.Drawing.Point(12, 69);
            this.sizelabel.Name = "sizelabel";
            this.sizelabel.Size = new System.Drawing.Size(66, 18);
            this.sizelabel.TabIndex = 14;
            this.sizelabel.Text = "止损手数:";
            // 
            // typelist
            // 
            this.typelist.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.typelist.Location = new System.Drawing.Point(98, 22);
            this.typelist.Name = "typelist";
            this.typelist.Size = new System.Drawing.Size(76, 21);
            this.typelist.TabIndex = 13;
            this.typelist.Text = "--";
            this.typelist.SelectedValueChanged += new System.EventHandler(this.typelist_SelectedValueChanged);
            // 
            // value
            // 
            this.value.Location = new System.Drawing.Point(98, 46);
            this.value.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.value.Name = "value";
            this.value.Size = new System.Drawing.Size(76, 21);
            this.value.TabIndex = 10;
            this.value.TabStop = false;
            // 
            // arglabel
            // 
            this.arglabel.Location = new System.Drawing.Point(12, 47);
            this.arglabel.Name = "arglabel";
            this.arglabel.Size = new System.Drawing.Size(54, 18);
            this.arglabel.TabIndex = 3;
            this.arglabel.Text = "参数:";
            // 
            // typelabel
            // 
            this.typelabel.Location = new System.Drawing.Point(12, 25);
            this.typelabel.Name = "typelabel";
            this.typelabel.Size = new System.Drawing.Size(66, 18);
            this.typelabel.TabIndex = 2;
            this.typelabel.Text = "止损类别:";
            // 
            // symbol
            // 
            this.symbol.Location = new System.Drawing.Point(49, 3);
            this.symbol.Name = "symbol";
            this.symbol.Size = new System.Drawing.Size(29, 18);
            this.symbol.TabIndex = 1;
            this.symbol.Text = "--";
            // 
            // radLabel1
            // 
            this.radLabel1.Location = new System.Drawing.Point(12, 3);
            this.radLabel1.Name = "radLabel1";
            this.radLabel1.Size = new System.Drawing.Size(45, 18);
            this.radLabel1.TabIndex = 0;
            this.radLabel1.Text = "合约:";
            // 
            // frmPositionOffset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(212, 159);
            this.ControlBox = false;
            this.Controls.Add(this.radPanel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "frmPositionOffset";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "止盈止损";
            this.Deactivate += new System.EventHandler(this.frmPositionOffset_Deactivate);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmPositionOffset_FormClosing);
            this.radPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.start)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.size)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.value)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel roundRectShape1;
        private System.Windows.Forms.Panel radPanel1;
        private System.Windows.Forms.Label symbol;
        private System.Windows.Forms.Label radLabel1;
        private System.Windows.Forms.Label arglabel;
        private System.Windows.Forms.Label typelabel;
        private System.Windows.Forms.NumericUpDown value;
        private System.Windows.Forms.NumericUpDown start;
        private System.Windows.Forms.Label radLabel5;
        private System.Windows.Forms.NumericUpDown size;
        private System.Windows.Forms.Label sizelabel;
        private System.Windows.Forms.ComboBox typelist;
        private System.Windows.Forms.Label radLabel2;
        private System.Windows.Forms.Label switchLabel;
        private System.Windows.Forms.Button btnUpdateOffset;
        private System.Windows.Forms.Button btnSwitchOffset;
        private System.Windows.Forms.Label directionLabel;


    }
}
