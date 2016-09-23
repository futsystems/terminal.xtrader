using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.OleDb;
namespace CStock
{
    public partial class SelectFunc : Form
    {
        Label[] LL = new Label[10];
        NumericUpDown[] num = new NumericUpDown[10];
        int[] def = new int[10];

        public string techname, techtitle,input;
        public string GetStr = "";
        public TStringList pg = new TStringList();

        public SelectFunc()
        {
            InitializeComponent();
        }

        private void SelectFunc_Shown(object sender, EventArgs e)
        {
            OleDbConnection con = new OleDbConnection("Data Source=stock.mdb;Provider=Microsoft.Jet.OLEDB.4.0;");
            con.Open();


            //OleDbCommand command = new OleDbCommand("Select * FROM [公式分类] where 分类=1 order by 序号", con);
            OleDbCommand command = new OleDbCommand("Select * FROM [公式分类]", con);
            OleDbDataReader datareader = command.ExecuteReader();


            while (datareader.Read())
            {
                string name = datareader["名称"].ToString();
                if (name.Length > 0)
                {
                    TreeNode td = GSView.Nodes.Add(name);

                    OleDbCommand com1 = new OleDbCommand("Select * FROM [公式库] where 分类名称=\'" + name + "\'", con);
                    OleDbDataReader qu1 = com1.ExecuteReader();
                    while (qu1.Read())
                    {
                        string name1 = qu1["名称"].ToString();
                        string title1 = qu1["描述"].ToString();
                        if (name1.Length > 0)
                            td.Nodes.Add(name1 + " " + title1);
                    }
                    qu1.Close();
                    com1.Dispose();
                }

            }


            con.Close();
            con.Dispose();

            if (GSView.Nodes.Count > 0)
                GSView.SelectedNode = GSView.Nodes[0];
        }

        private void GSView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            double[] fopen ={ 22.59, 22.17, 21, 20.02, 20.20, 20.38 };
            double[] fhigh = { 22.59, 22.67, 21, 20.37, 20.6, 20.9 };
            double[] flow = { 22.08, 21.92, 19.94, 19.64, 20.10, 20.19 };
            double[] fclose = { 22.14, 22.15, 20.02, 20.12, 20.39, 20.44 };
            double[] fvol = { 55977300, 38599693, 117504326, 56984842, 38554183, 46305400 };
            double[] famount = { 1248606080, 859815744, 2385878272, 1139477760, 786200256, 952838144 };

            double[] fdate = { 20130326, 20130327, 20130328, 20130329, 20130401, 20130402 };
            double[] ftime = { 930, 930, 930, 930, 930, 930 };

            TreeNode td = GSView.SelectedNode;
            GetStr = "";
            if (td == null)
                return;
            for (int i = 0; i < 10; i++)
            {
                LL[i].Visible = false;
                num[i].Visible = false;
            }
            if (td.Level != 1)
                return;
            string str = td.Text;
            string[] ss = str.Split(' ');
            OleDbConnection con = new OleDbConnection("Data Source=stock.mdb;Provider=Microsoft.Jet.OLEDB.4.0;");
            con.Open();
            OleDbCommand com1 = new OleDbCommand("Select * FROM [公式库] where 名称=\'" + ss[0] + "\'", con);
            OleDbDataReader qu1 = com1.ExecuteReader();
            if (qu1.Read())
            {
                string id = qu1["编号"].ToString();
                string name1 = qu1["名称"].ToString();
                string title1 = qu1["描述"].ToString();
                string content = qu1["内容"].ToString();
                techname=name1;
                techtitle = title1;
                TGongSi gs = new TGongSi();
                gs.Add("open", fopen, fopen.Length);
                gs.Add("high", fhigh, fhigh.Length);
                gs.Add("low", flow, flow.Length);
                gs.Add("close", fclose, fclose.Length);
                gs.Add("vol", fvol, fvol.Length);
                gs.Add("amount", famount, famount.Length);
                gs.Add("data", fdate, fdate.Length);
                gs.Add("time", ftime, ftime.Length);
                if (gs.setprogtext(content))
                {
                    for (int t = 0; t < gs.CurTech.Input.Count; t++)
                    {
                        Tinput pt = gs.CurTech.Input[t];
                        LL[t].Visible = true;
                        LL[t].Text = pt.name;
                        def[t] = pt.def1;
                        num[t].Visible = true;
                        num[t].Minimum = pt.min1;
                        num[t].Maximum = pt.max1;
                        num[t].Value = pt.val1;
                    }
                    pg.Text = content;
                }
                //ed.Text = gs.CurTech.pg1.Text;
            }
            qu1.Close();
            com1.Dispose();
            con.Close();
            con.Dispose();
        }
        private void SelectFunc_Load(object sender, EventArgs e)
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

        private void button2_Click(object sender, EventArgs e)
        {
            int row = 0;
            input = "";
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
                        input += num[i].Value.ToString()+",";
                    }
                }
            }
            if (input.Length>0)
                input = input.Substring(0, input.Length - 1);
            GetStr = pg.Text;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            TreeNode td = GSView.SelectedNode;
            if (td == null)
                return;
            if (td.Level == 0)
                return;
            string str = td.Text;
            string[] ss = str.Split(' ');

            if (MessageBox.Show("是否删除公式:[" + ss[0] + " " + ss[1] + "]?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                OleDbConnection con = new OleDbConnection("Data Source=stock.mdb;Provider=Microsoft.Jet.OLEDB.4.0;");
                con.Open();
                OleDbCommand com1 = new OleDbCommand("delete from [公式库] where 名称=\'" + ss[0] + "\'", con);
                OleDbDataReader qu1 = com1.ExecuteReader();
                qu1.Close();
                com1.Dispose();
                con.Close();
                con.Dispose();
                int index = td.Index;
                GSView.Nodes.Remove(td);
            }
        }


    }
}