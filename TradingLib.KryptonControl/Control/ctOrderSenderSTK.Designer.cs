namespace TradingLib.KryptonControl
{
    partial class ctOrderSenderSTK
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
            this.symbol = new ComponentFactory.Krypton.Toolkit.KryptonTextBox();
            this.price = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            this.size = new ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown();
            this.kryptonPanel1 = new ComponentFactory.Krypton.Toolkit.KryptonPanel();
            this.btnSubmit = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.btnReset = new ComponentFactory.Krypton.Toolkit.KryptonButton();
            this.kryptonLabel8 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lbMaxOrderVol = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel6 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel5 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.lbSymbolName = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel3 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel2 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            this.kryptonLabel1 = new ComponentFactory.Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).BeginInit();
            this.kryptonPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // symbol
            // 
            this.symbol.Location = new System.Drawing.Point(74, 34);
            this.symbol.Name = "symbol";
            this.symbol.Size = new System.Drawing.Size(97, 21);
            this.symbol.TabIndex = 22;
            this.symbol.Text = "600000";
            // 
            // price
            // 
            this.price.Location = new System.Drawing.Point(74, 86);
            this.price.Name = "price";
            this.price.Size = new System.Drawing.Size(97, 22);
            this.price.TabIndex = 23;
            // 
            // size
            // 
            this.size.Location = new System.Drawing.Point(74, 138);
            this.size.Name = "size";
            this.size.Size = new System.Drawing.Size(97, 22);
            this.size.TabIndex = 24;
            // 
            // kryptonPanel1
            // 
            this.kryptonPanel1.Controls.Add(this.btnSubmit);
            this.kryptonPanel1.Controls.Add(this.btnReset);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel8);
            this.kryptonPanel1.Controls.Add(this.size);
            this.kryptonPanel1.Controls.Add(this.lbMaxOrderVol);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel6);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel5);
            this.kryptonPanel1.Controls.Add(this.lbSymbolName);
            this.kryptonPanel1.Controls.Add(this.price);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel3);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel2);
            this.kryptonPanel1.Controls.Add(this.kryptonLabel1);
            this.kryptonPanel1.Controls.Add(this.symbol);
            this.kryptonPanel1.Location = new System.Drawing.Point(0, 0);
            this.kryptonPanel1.Name = "kryptonPanel1";
            this.kryptonPanel1.Size = new System.Drawing.Size(220, 212);
            this.kryptonPanel1.TabIndex = 25;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(123, 175);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(70, 25);
            this.btnSubmit.TabIndex = 30;
            this.btnSubmit.Values.Text = "买 入";
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(23, 175);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(70, 25);
            this.btnReset.TabIndex = 29;
            this.btnReset.Values.Text = "重 置";
            // 
            // kryptonLabel8
            // 
            this.kryptonLabel8.Location = new System.Drawing.Point(5, 138);
            this.kryptonLabel8.Name = "kryptonLabel8";
            this.kryptonLabel8.Size = new System.Drawing.Size(63, 22);
            this.kryptonLabel8.TabIndex = 28;
            this.kryptonLabel8.Values.Text = "买入数量:";
            // 
            // lbMaxOrderVol
            // 
            this.lbMaxOrderVol.Location = new System.Drawing.Point(74, 112);
            this.lbMaxOrderVol.Name = "lbMaxOrderVol";
            this.lbMaxOrderVol.Size = new System.Drawing.Size(21, 22);
            this.lbMaxOrderVol.TabIndex = 27;
            this.lbMaxOrderVol.Values.Text = "--";
            // 
            // kryptonLabel6
            // 
            this.kryptonLabel6.Location = new System.Drawing.Point(10, 112);
            this.kryptonLabel6.Name = "kryptonLabel6";
            this.kryptonLabel6.Size = new System.Drawing.Size(59, 22);
            this.kryptonLabel6.TabIndex = 26;
            this.kryptonLabel6.Values.Text = "可买(股):";
            // 
            // kryptonLabel5
            // 
            this.kryptonLabel5.Location = new System.Drawing.Point(30, 86);
            this.kryptonLabel5.Name = "kryptonLabel5";
            this.kryptonLabel5.Size = new System.Drawing.Size(38, 22);
            this.kryptonLabel5.TabIndex = 25;
            this.kryptonLabel5.Values.Text = "价格:";
            // 
            // lbSymbolName
            // 
            this.lbSymbolName.Location = new System.Drawing.Point(74, 60);
            this.lbSymbolName.Name = "lbSymbolName";
            this.lbSymbolName.Size = new System.Drawing.Size(21, 22);
            this.lbSymbolName.TabIndex = 24;
            this.lbSymbolName.Values.Text = "--";
            // 
            // kryptonLabel3
            // 
            this.kryptonLabel3.Location = new System.Drawing.Point(5, 60);
            this.kryptonLabel3.Name = "kryptonLabel3";
            this.kryptonLabel3.Size = new System.Drawing.Size(63, 22);
            this.kryptonLabel3.TabIndex = 23;
            this.kryptonLabel3.Values.Text = "证券名称:";
            // 
            // kryptonLabel2
            // 
            this.kryptonLabel2.Location = new System.Drawing.Point(5, 34);
            this.kryptonLabel2.Name = "kryptonLabel2";
            this.kryptonLabel2.Size = new System.Drawing.Size(63, 22);
            this.kryptonLabel2.TabIndex = 1;
            this.kryptonLabel2.Values.Text = "证券代码:";
            // 
            // kryptonLabel1
            // 
            this.kryptonLabel1.Location = new System.Drawing.Point(50, 5);
            this.kryptonLabel1.Name = "kryptonLabel1";
            this.kryptonLabel1.Size = new System.Drawing.Size(91, 29);
            this.kryptonLabel1.StateCommon.LongText.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.kryptonLabel1.StateCommon.ShortText.Font = new System.Drawing.Font("宋体", 14.25F);
            this.kryptonLabel1.TabIndex = 0;
            this.kryptonLabel1.Values.Text = "买入股票";
            // 
            // ctOrderSenderSTK
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.kryptonPanel1);
            this.Name = "ctOrderSenderSTK";
            this.Size = new System.Drawing.Size(220, 212);
            ((System.ComponentModel.ISupportInitialize)(this.kryptonPanel1)).EndInit();
            this.kryptonPanel1.ResumeLayout(false);
            this.kryptonPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ComponentFactory.Krypton.Toolkit.KryptonTextBox symbol;
        private ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown price;
        private ComponentFactory.Krypton.Toolkit.KryptonNumericUpDown size;
        private ComponentFactory.Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel2;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel3;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lbSymbolName;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel5;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel6;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel lbMaxOrderVol;
        private ComponentFactory.Krypton.Toolkit.KryptonLabel kryptonLabel8;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnReset;
        private ComponentFactory.Krypton.Toolkit.KryptonButton btnSubmit;
    }
}
