using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

namespace TradingLib.XTrader.Future
{
    public class ctVerify: UserControl
    {
        private string _yz = "";
        private IContainer components = null;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Panel panel1;
        List<Label> lblist = new List<Label>();
        List<Color> colorlist = new List<Color>();
        public ctVerify()
        {
            this.InitializeComponent();
            colorlist.Add(Color.Blue);
            colorlist.Add(Color.DeepSkyBlue);
            colorlist.Add(Color.Blue);
            colorlist.Add(Color.Red);
            colorlist.Add(Color.SaddleBrown);
            colorlist.Add(Color.Maroon);
            colorlist.Add(Color.Firebrick);
            colorlist.Add(Color.DarkRed);
            colorlist.Add(Color.Indigo);
            colorlist.Add(Color.Yellow);

            lblist.Add(this.label1);
            lblist.Add(this.label2);
            lblist.Add(this.label3);
            lblist.Add(this.label4);

            ResetVerify();
        }

        public void ResetVerify()
        {
            this._yz = string.Empty;
            Random ran = new Random();
            foreach (Label labs in lblist)
            {
                labs.Text = ran.Next(0, 9).ToString();
                this._yz = this._yz + labs.Text;
                labs.ForeColor = colorlist[ran.Next(0, 9)];
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panel1 = new Panel();
            this.label4 = new Label();
            this.label3 = new Label();
            this.label2 = new Label();
            this.label1 = new Label();
            this.panel1.SuspendLayout();
            base.SuspendLayout();
            this.panel1.BackColor = SystemColors.ScrollBar;
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = DockStyle.Fill;
            this.panel1.Location = new Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new Size(0x3f, 0x15);
            this.panel1.TabIndex = 0;
            this.label4.AutoSize = true;
            this.label4.BackColor = Color.Transparent;
            this.label4.Font = new Font("黑体", 12f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.label4.ForeColor = Color.Magenta;
            this.label4.Location = new Point(0x2c, 1);
            this.label4.Name = "label4";
            this.label4.Size = new Size(0x11, 0x10);
            this.label4.TabIndex = 0;
            this.label4.Text = "0";
            this.label3.AutoSize = true;
            this.label3.BackColor = Color.Transparent;
            this.label3.Font = new Font("黑体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.label3.ForeColor = Color.FromArgb(0xff, 0x80, 0);
            this.label3.Location = new Point(0x1d, 3);
            this.label3.Name = "label3";
            this.label3.Size = new Size(15, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "4";
            this.label2.AutoSize = true;
            this.label2.Font = new Font("黑体", 12f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.label2.ForeColor = Color.Blue;
            this.label2.Location = new Point(14, 2);
            this.label2.Name = "label2";
            this.label2.Size = new Size(0x11, 0x10);
            this.label2.TabIndex = 0;
            this.label2.Text = "7";
            this.label1.AutoSize = true;
            this.label1.BackColor = Color.Transparent;
            this.label1.Font = new Font("黑体", 10.5f, FontStyle.Bold, GraphicsUnit.Point, 0x86);
            this.label1.ForeColor = Color.Red;
            this.label1.Location = new Point(1, 4);
            this.label1.Name = "label1";
            this.label1.Size = new Size(15, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "4";
            base.AutoScaleDimensions = new SizeF(6f, 12f);
            
            base.Controls.Add(this.panel1);
            base.Name = "Yanzhenma";
            base.Size = new Size(0x3f, 0x15);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            base.ResumeLayout(false);
        }

        public string VerifyCode
        {
            get
            {
                return this._yz;
            }
        }
    }
}


