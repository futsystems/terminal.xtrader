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

    public partial class FuncEdit : Form
    {
        public FuncEdit()
        {
            InitializeComponent();
        }

        private void FuncEdit_Load(object sender, EventArgs e)
        {


        }

        private void FuncEdit_Shown(object sender, EventArgs e)
        {
            OleDbConnection con = new OleDbConnection("Data Source=stock.mdb;Provider=Microsoft.Jet.OLEDB.4.0;");
            con.Open();


            OleDbCommand command = new OleDbCommand("Select * FROM [公式分类]", con);
            //OleDbCommand command = new OleDbCommand("Select * FROM [公式分类] where 分类=1 order by 序号", con);
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

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void GSView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode td = GSView.SelectedNode;
            if (td == null)
            {
                bt1.Enabled = false;
                bt2.Enabled = false;
                bt3.Enabled = false;
                delete.Enabled = false;
                return;
            }
            if (td.Level == 0)
            {
                bt1.Enabled = true;
                delete.Enabled = true;
                bt2.Enabled = false;
                bt3.Enabled = false;
            }
            if (td.Level == 1)
            {
                delete.Enabled = false;
                bt1.Enabled = false;
                bt2.Enabled = true;
                bt3.Enabled = true;
            }
        }

        private void bt2_Click(object sender, EventArgs e)
        {
            TreeNode td = GSView.SelectedNode;
            if (td == null)
                return;
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
                TStringList src = new TStringList();
                TStringList dst = new TStringList();
                src.Text = content;
                Compile cm = new Compile();
                cm.S_Compile(src, dst);
                string pass = dst.values("password:");
                bool pw = true;
                if (pass.Length > 0)
                {
                    pw = false;
                    CheckPass cp = new CheckPass();
                    if (cp.ShowDialog() == DialogResult.OK)
                    {
                        pw = cp.password.Text == pass;
                        if (!pw)
                            MessageBox.Show("密码错误,不能修改", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                if (pw)
                {

                    Editor ef = new Editor();
                    ef.SetText(content);
                    DialogResult d1=ef.ShowDialog();
                    if (d1== DialogResult.OK)
                    {
                        string cmd = "update 公式库 Set 名称='" + ef.wfcname.Text.Trim() + "',描述='" + ef.title.Text.Trim() + "',内容='" + ef.gongsi.Text.Trim() + "' where 编号=" + id;
                        OleDbCommand cmd2 = new OleDbCommand(cmd, con);//修改密码
                        cmd2.ExecuteNonQuery();
                    }
                    if (d1 == DialogResult.Yes)
                    {

                        string txt = ef.gongsi.Text.Replace("\'", "\'\'");
                        string cmd = "INSERT INTO [公式库] (分类名称,名称,描述,内容,类型) Values('" + td.Text + "','" + ef.wfcname.Text + "','" + ef.title.Text + "','" + txt + "',1);";
                        OleDbCommand com = new OleDbCommand(cmd, con);
                        com.ExecuteNonQuery();
                        TreeNode td1 = td.Nodes.Add(ef.wfcname.Text + " " + ef.title.Text);
                    }
                    ef.Dispose();
                    //EditFunc ef = new EditFunc();
                    //ef.name.Text = name1;
                    //ef.title.Text = title1;
                    //ef.Gongsi.Text = content;
                    //if (ef.ShowDialog() == DialogResult.OK)
                    //{
                    //    string cmd = "update 公式库 Set 名称='" + ef.name.Text.Trim() + "',描述='" + ef.title.Text.Trim() + "',内容='" + ef.Gongsi.Text.Trim() + "' where 编号=" + id;
                    //    OleDbCommand cmd2 = new OleDbCommand(cmd, con);//修改密码
                    //    cmd2.ExecuteNonQuery();
                    //}
                    //ef.Dispose();
                }
            }
            qu1.Close();
            com1.Dispose();


            con.Close();
            con.Dispose();
        }

        private void bt3_Click(object sender, EventArgs e)
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

        private void bt1_Click(object sender, EventArgs e)
        {
            TreeNode td = GSView.SelectedNode;
            if (td == null)
                return;
            if (td.Level != 0)
                return;
            EditFunc ef = new EditFunc();
            DialogResult d1=ef.ShowDialog();
            if (d1== DialogResult.OK)
            {
                OleDbConnection con = new OleDbConnection("Data Source=stock.mdb;Provider=Microsoft.Jet.OLEDB.4.0;");
                con.Open();
                OleDbCommand com1 = new OleDbCommand("select * from [公式库] where 名称=\'" + ef.name.Text + "\'", con);
                OleDbDataReader qu1 = com1.ExecuteReader();
                if (!qu1.Read())//不存在
                {

                    string txt = ef.Gongsi.Text.Replace("\'", "\'\'");
                    string cmd = "INSERT INTO [公式库] (分类名称,名称,描述,内容,类型) Values('" + td.Text + "','" + ef.name.Text + "','" + ef.title.Text + "','" + txt + "',1);";
                    OleDbCommand com = new OleDbCommand(cmd, con);
                    com.ExecuteNonQuery();
                    TreeNode td1 = td.Nodes.Add(ef.name.Text + " " + ef.title.Text);
                }
                qu1.Close();
                com1.Dispose();
                con.Close();
                con.Dispose();
            }
            ef.Dispose();
        }

        private void add_Click(object sender, EventArgs e)
        {
            AddFenLei fl = new AddFenLei();
            if (fl.ShowDialog() == DialogResult.OK)
            {
                OleDbConnection con = new OleDbConnection("Data Source=stock.mdb;Provider=Microsoft.Jet.OLEDB.4.0;");
                con.Open();

                OleDbCommand command = new OleDbCommand("Select * FROM [公式分类] where 名称=\'"+fl.leiname.Text+"\'", con);
                OleDbDataReader qu1 = command.ExecuteReader();
                if (qu1.Read())
                {
                    MessageBox.Show("已存在分类: " + fl.leiname.Text);
                }
                else
                {
                    string cmd = "INSERT INTO [公式分类] (名称,序号,分类) Values('" + fl.leiname.Text + "',1,1);";
                    OleDbCommand com = new OleDbCommand(cmd, con);
                    com.ExecuteNonQuery();
                    TreeNode td =GSView.Nodes.Add(fl.leiname.Text);
                }
                qu1.Close();
                command.Dispose();
                con.Close();
                con.Dispose();
            }



        }

        private void delete_Click(object sender, EventArgs e)
        {
            TreeNode td = GSView.SelectedNode;
            if (td == null)
                return;
            if (td.Level != 0)
                return;
            string str = td.Text;
            if (MessageBox.Show("是否删除分类:[" + str + "]?", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                OleDbConnection con = new OleDbConnection("Data Source=stock.mdb;Provider=Microsoft.Jet.OLEDB.4.0;");
                con.Open();
                OleDbCommand com1 = new OleDbCommand("delete from [公式分类] where 名称=\'" +str + "\'", con);
                OleDbDataReader qu1 = com1.ExecuteReader();
                qu1.Close();
                com1.Dispose();
                con.Close();
                con.Dispose();
                GSView.Nodes.Remove(td);
            }

        }



    }
}