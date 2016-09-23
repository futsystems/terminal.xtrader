using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
namespace CStock
{
    public partial class tiao : Form
    {
        public tiao()
        {
            InitializeComponent();
        }

        public TGongSi gs = null;
        public TStock sk = null;

        Label[] LL = new Label[10];
        NumericUpDown[] num = new NumericUpDown[10];
        int[] def = new int[10];

        private void tiao_Load(object sender, EventArgs e)
        {
            LL[0] = label2;
            LL[1] = label3;
            LL[2] = label4;
            LL[3] = label5;
            LL[4] = label6;
            LL[5] = label7;
            LL[6] = label8;
            LL[7] = label9;
            LL[8] = label10;
            LL[9] = label11;

            num[0] = num1;
            num[1] = num2;
            num[2] = num3;
            num[3] = num4;
            num[4] = num5;
            num[5] = num6;
            num[6] = num7;
            num[7] = num8;
            num[8] = num9;
            num[9] = num10;
            for (int i = 0; i < 10; i++)
            {
                LL[i].Visible = false;
                num[i].Visible = false;
            }
        }

        private void tiao_Shown(object sender, EventArgs e)
        {
            if (gs == null)
                return;
            for (int t = 0; t < gs.CurTech.Input.Count; t++)
            {
                Tinput pt = gs.TechList[0].Input[t];
                LL[t].Visible = true;
                LL[t].Text = pt.name;
                def[t] = pt.def1;
                num[t].Visible = true;
                num[t].Minimum = pt.min1;
                num[t].Maximum = pt.max1;
                num[t].Value = pt.val1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            for (int t = 0; t < gs.CurTech.Input.Count; t++)
            {
                Tinput pt = gs.TechList[0].Input[t];
                num[t].Value = pt.def1;
            }

        }

        private void num1_ValueChanged(object sender, EventArgs e)
        {
            string inputstr = "";
            for (int i = 0; i < 10; i++)
            {
                if (num[i].Visible)
                    inputstr += num[i].Value.ToString() + ",";
            }
            if (inputstr.Length > 0)
            {
                inputstr = inputstr.Substring(0, inputstr.Length - 1);
                gs.run(inputstr);
                if (sk != null)
                    sk.Invalidate();
            }



            /*

            TStringList pg = gs.TechList[0].pg1;
            int row = 0;
            for (int i = 0; i < 10; i++)
            {
                if (num[i].Visible)
                {
                    string ss = pg.values(LL[i].Text + ":", out row);
                    if (row > -1)
                    {
                        pg[row] = LL[i].Text + ":=INPUT(" + num[i].Minimum.ToString() + "," +
                            num[i].Maximum.ToString() + "," +
                            def[i].ToString() + "," +
                            num[i].Value.ToString() + ");";
                    }
                }
            }
            gs.setprogtext(pg.Text);
            if (sk != null)
                sk.Invalidate();
            */
        }

    }

}