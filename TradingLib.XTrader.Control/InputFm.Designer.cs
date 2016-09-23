partial class InputFm
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
        this.button1 = new System.Windows.Forms.Button();
        this.button2 = new System.Windows.Forms.Button();
        this.min1 = new System.Windows.Forms.NumericUpDown();
        this.label1 = new System.Windows.Forms.Label();
        this.name = new System.Windows.Forms.TextBox();
        this.max1 = new System.Windows.Forms.NumericUpDown();
        this.def = new System.Windows.Forms.NumericUpDown();
        this.cur = new System.Windows.Forms.NumericUpDown();
        this.label2 = new System.Windows.Forms.Label();
        this.label3 = new System.Windows.Forms.Label();
        this.label4 = new System.Windows.Forms.Label();
        this.label5 = new System.Windows.Forms.Label();
        ((System.ComponentModel.ISupportInitialize)(this.min1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.max1)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.def)).BeginInit();
        ((System.ComponentModel.ISupportInitialize)(this.cur)).BeginInit();
        this.SuspendLayout();
        // 
        // button1
        // 
        this.button1.DialogResult = System.Windows.Forms.DialogResult.OK;
        this.button1.Location = new System.Drawing.Point(251, 72);
        this.button1.Name = "button1";
        this.button1.Size = new System.Drawing.Size(84, 29);
        this.button1.TabIndex = 0;
        this.button1.Text = "确认";
        this.button1.UseVisualStyleBackColor = true;
        // 
        // button2
        // 
        this.button2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        this.button2.Location = new System.Drawing.Point(157, 72);
        this.button2.Name = "button2";
        this.button2.Size = new System.Drawing.Size(84, 29);
        this.button2.TabIndex = 1;
        this.button2.Text = "取消";
        this.button2.UseVisualStyleBackColor = true;
        // 
        // min1
        // 
        this.min1.Location = new System.Drawing.Point(93, 30);
        this.min1.Name = "min1";
        this.min1.Size = new System.Drawing.Size(56, 21);
        this.min1.TabIndex = 3;
        // 
        // label1
        // 
        this.label1.AutoSize = true;
        this.label1.Location = new System.Drawing.Point(10, 17);
        this.label1.Name = "label1";
        this.label1.Size = new System.Drawing.Size(47, 12);
        this.label1.TabIndex = 4;
        this.label1.Text = "参数名:";
        // 
        // name
        // 
        this.name.Location = new System.Drawing.Point(10, 30);
        this.name.Name = "name";
        this.name.Size = new System.Drawing.Size(59, 21);
        this.name.TabIndex = 5;
        // 
        // max1
        // 
        this.max1.Location = new System.Drawing.Point(155, 30);
        this.max1.Name = "max1";
        this.max1.Size = new System.Drawing.Size(56, 21);
        this.max1.TabIndex = 6;
        this.max1.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
        // 
        // def
        // 
        this.def.Location = new System.Drawing.Point(217, 31);
        this.def.Name = "def";
        this.def.Size = new System.Drawing.Size(56, 21);
        this.def.TabIndex = 7;
        this.def.ValueChanged += new System.EventHandler(this.def_ValueChanged);
        // 
        // cur
        // 
        this.cur.Enabled = false;
        this.cur.Location = new System.Drawing.Point(279, 31);
        this.cur.Name = "cur";
        this.cur.Size = new System.Drawing.Size(56, 21);
        this.cur.TabIndex = 8;
        // 
        // label2
        // 
        this.label2.AutoSize = true;
        this.label2.Location = new System.Drawing.Point(91, 17);
        this.label2.Name = "label2";
        this.label2.Size = new System.Drawing.Size(47, 12);
        this.label2.TabIndex = 9;
        this.label2.Text = "最小值:";
        // 
        // label3
        // 
        this.label3.AutoSize = true;
        this.label3.Location = new System.Drawing.Point(155, 17);
        this.label3.Name = "label3";
        this.label3.Size = new System.Drawing.Size(47, 12);
        this.label3.TabIndex = 10;
        this.label3.Text = "最大值:";
        // 
        // label4
        // 
        this.label4.AutoSize = true;
        this.label4.Location = new System.Drawing.Point(217, 17);
        this.label4.Name = "label4";
        this.label4.Size = new System.Drawing.Size(47, 12);
        this.label4.TabIndex = 11;
        this.label4.Text = "缺省值:";
        // 
        // label5
        // 
        this.label5.AutoSize = true;
        this.label5.Location = new System.Drawing.Point(277, 17);
        this.label5.Name = "label5";
        this.label5.Size = new System.Drawing.Size(47, 12);
        this.label5.TabIndex = 12;
        this.label5.Text = "当前值:";
        // 
        // InputFm
        // 
        this.AcceptButton = this.button1;
        this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
        this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        this.ClientSize = new System.Drawing.Size(345, 108);
        this.Controls.Add(this.label5);
        this.Controls.Add(this.label4);
        this.Controls.Add(this.label3);
        this.Controls.Add(this.label2);
        this.Controls.Add(this.cur);
        this.Controls.Add(this.def);
        this.Controls.Add(this.max1);
        this.Controls.Add(this.name);
        this.Controls.Add(this.label1);
        this.Controls.Add(this.min1);
        this.Controls.Add(this.button2);
        this.Controls.Add(this.button1);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.Name = "InputFm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
        this.Text = "插入可调变量";
        this.Shown += new System.EventHandler(this.InputFm_Shown);
        ((System.ComponentModel.ISupportInitialize)(this.min1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.max1)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.def)).EndInit();
        ((System.ComponentModel.ISupportInitialize)(this.cur)).EndInit();
        this.ResumeLayout(false);
        this.PerformLayout();

    }

    #endregion

    private System.Windows.Forms.Button button1;
    private System.Windows.Forms.Button button2;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label label5;
    public System.Windows.Forms.NumericUpDown min1;
    public System.Windows.Forms.TextBox name;
    public System.Windows.Forms.NumericUpDown max1;
    public System.Windows.Forms.NumericUpDown def;
    public System.Windows.Forms.NumericUpDown cur;
}
