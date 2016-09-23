using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CStock
{
    public partial class TechList : Form
    {
        public string SelectFunc;
        public int paramcount = 0;
        bool init = false;
        public TechList()
        {
            InitializeComponent();
        }

        private void TechList_Load(object sender, EventArgs e)
        {
            OutStr.Dock = DockStyle.Fill;
        }

        private void TechList_Shown(object sender, EventArgs e)
        {
            TV.SelectedNode = TV.Nodes[0];  
        }

        private void TV_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode td = TV.SelectedNode;
            if (td == null)
                return;
            string s1;
            string[] ss;
            init = false;
            if (td.Text == "全部函数")
            {
                OutStr.Text = "";
                LV1.Items.Clear();
                LV1.BeginUpdate(); 
                for (int i = 0; i < TL1.Items.Count; i++)
                {
                    s1 = (string)TL1.Items[i];
                    ss = s1.Split(',');
                    if (ss.Length == 4)
                    {
                        ListViewItem lv = LV1.Items.Add(ss[2]);
                        lv.ImageIndex = 1;
                        lv.SubItems.Add(ss[3]);
                    }
                }
                LV1.EndUpdate();
                init = true;
                return;
            }
            string s2 =","+td.Text+",";
            OutStr.Text = "";
            LV1.Items.Clear();
            LV1.BeginUpdate(); 
            for (int i = 0; i < TL1.Items.Count; i++)
            {
                s1 = (string)TL1.Items[i];
                if (s1.IndexOf(s2) > -1)
                {
                    ss = s1.Split(',');
                    ListViewItem lv = new ListViewItem();
                    lv.ImageIndex = 1;
                    lv.Text = ss[2];
                    lv.SubItems.Add(ss[3]);
                    LV1.Items.Add(lv);
                }
            }
            LV1.EndUpdate();
            init = true;
        }

        private void LV1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init) return;
            if (LV1.SelectedItems.Count == 0)
                return;
            string fname = LV1.SelectedItems[0].Text;
            SelectFunc = fname;
            string ss1 = "," + fname + ",";
            for (int i = 0; i < TL1.Items.Count; i++)
            {
                string ss2 = (string)TL1.Items[i];
                if (ss2.IndexOf(ss1) > -1)
                {
                    string [] ss = ss2.Split(',');
                    paramcount = Convert.ToInt32(ss[0]); 
                    break;
                }
            }
            for (int i = 0; i < Notes.Items.Count; i++)
            {
                string s1 =(string)Notes.Items[i];
                if (s1.IndexOf(fname) == 0)
                {
                    string s2 = "";
                    for (int j = i; j < Notes.Items.Count; j++)
                    {
                        s1 = ((string)Notes.Items[j]).Trim();
                        if (s1.Length == 0)
                            break;
                        s2 += s1 + "\r\n";
                    }
                    OutStr.Text = s2;
                    break;
                }

            }

        }
    }
}