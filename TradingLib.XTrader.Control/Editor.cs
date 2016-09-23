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
    public partial class Editor : Form
    {
        string[] notes = new string[50];

        string[] outstr = new string[4];
        public string GongText = "";
        bool  init =false;

        public Editor()
        {
            InitializeComponent();
            if (techtype.Items.Count > 0)
                techtype.SelectedIndex = 0;
            drawk.SelectedIndex = 0;
            digit.SelectedIndex = 0;          
       }

        private void textBox2_KeyDown(object sender, KeyEventArgs e)
        {
            if ((int)e.KeyCode == 13)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            gongsi.Dock = DockStyle.Fill;
            //gongsi.Document.HighlightingStrategy = HighlightingStrategyFactory.CreateHighlightingStrategy("VBNET");

        }


        private void button5_Click(object sender, EventArgs e)
        {
            if (cd1.ShowDialog() == DialogResult.OK)
            {
                Color cc = cd1.Color;
                string s1 = ",COLOR" + cc.R.ToString("X2") + cc.G.ToString("X2") + cc.B.ToString("X2");
                gongsi.SelectedText = s1;
                //gongsi.ActiveTextAreaControl.TextArea.InsertString(s1);
                gongsi.Focus();
            }
        }

        private void button4_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Point p = button4.PointToScreen(new Point(0, 0));
                string s1 = gongsi.SelectedText;// "";// gongsi.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText;
                if (s1.Length > 0)
                {
                    Cut.Enabled = true;
                    Copy.Enabled = true;
                    Del.Enabled = true;
                }
                else
                {
                    Cut.Enabled = false;
                    Copy.Enabled = false;
                    Del.Enabled = false;
                }

                this.pm1.Width = 113;
                this.pm1.Show(p.X, p.Y+button4.Height+4);
            }
        }

        private void selectall_Click(object sender, EventArgs e)
        {
            //gongsi.ActiveTextAreaControl.TextArea.ClipboardHandler.SelectAll(null, null);
            //string s1 = txtContent.ActiveTextAreaControl.TextArea.SelectionManager.SelectedText;
        }

        private void empty_Click(object sender, EventArgs e)
        {
            gongsi.Text = "";
        }

        private void 只读ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            gongsi.Enabled = false;
        }

        private void LineHide_Click(object sender, EventArgs e)
        {
            //gongsi.ShowLineNumbers ^= true;
            //if (gongsi.ShowLineNumbers)
            //    LineHide.Text = "隐藏沟槽";
            //else
            //    LineHide.Text = "显示沟槽";
        }

        private void Cut_Click(object sender, EventArgs e)
        {
            gongsi.Cut();
            //gongsi.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(null,null);
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            gongsi.Copy();
           // gongsi.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(null, null);
        }

        private void paste_Click(object sender, EventArgs e)
        {
            gongsi.Paste();
           // gongsi.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(null, null);
        }

        private void Dell_Click(object sender, EventArgs e)
        {
            if (gongsi.SelectedText.Length>0) 
                gongsi.SelectedText = "";  
           // gongsi.ActiveTextAreaControl.TextArea.ClipboardHandler.Delete(null, null);
        }

        private void NowSave_Click(object sender, EventArgs e)
        {
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                TStringList sl = new TStringList();
                sl.Text = gongsi.Text;
                sl.SaveToFile(sfd.FileName);
            }
        }

        private void LoadFile_Click(object sender, EventArgs e)
        {
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                TStringList sl = new TStringList();
                sl.LoadFromFile(ofd.FileName);
                gongsi.Text = sl.Text;
            }
        }

        public bool TryStrToInt(string str, ref int outv)
        {
            outv = 0;
            try
            {
                outv = int.Parse(str);
                return true;
            }
            catch { }
            return false;
        }
        public bool TryStrToFloat(string str, ref double outv)
        {
            outv = 0;
            try
            {
                outv =Double.Parse(str);
                return true;
            }
            catch { }
            return false;
        }
        public void SetText(String str)
        {
            int i;
            init = false;
            TStringList sl = new TStringList();
            sl.Text = str;

            string s1 = sl.values("drawk:").ToLower().Replace(";", ""); ;
            if (s1 == "true")
                drawk.SelectedIndex = 0;
            if (s1 == "false")
                drawk.SelectedIndex = 1;
            wfcname.Text = sl.values("techname:").Replace(";", "").ToUpper();
            title.Text = sl.values("techtitle:").Replace(";", "").ToUpper();
            s1 = sl.values("digit").Replace(";", "");
            if (s1.Length > 0)
            {
                double d1 = 0;
                if (TryStrToFloat(s1, ref d1))
                {
                    i = Convert.ToInt32(d1);
                    if ((i > -1) && (i < 7))
                        digit.SelectedIndex = i;
                    else
                        digit.SelectedIndex = 0;
                }
                else
                    digit.SelectedIndex = 0;
            }

            string pass = sl.values("password:").Replace(";", "");
            if (pass.Length == 0)
            {
                passck.Checked = false;
                password.Enabled = false;
            }
            else
            {
                passck.Checked = true;
                password.Enabled = true;
                password.Text = pass;
            }
            s1 = sl.values("ystr:").Replace(";", "");
            if (s1.Length > 0)
                yStr.Text = s1.Replace('_', ';');

            s1 = sl.values("yvalue:").Replace(";", "");
            if (s1.Length > 0)
            {
                string[] ss = s1.Split('_');
                double d1 = 0;
                TextBox[] tb ={ yv1, yv2, yv3, yv4 };
                for (i = 0; i < ss.Length; i++)
                {
                    if (TryStrToFloat(ss[i], ref d1))
                    {
                        tb[i].Text = ss[i];
                    }
                }
            }



            double[] fopen ={ 22.59, 22.17, 21, 20.02, 20.20, 20.38 };
            double[] fhigh = { 22.59, 22.67, 21, 20.37, 20.6, 20.9 };
            double[] flow = { 22.08, 21.92, 19.94, 19.64, 20.10, 20.19 };
            double[] fclose = { 22.14, 22.15, 20.02, 20.12, 20.39, 20.44 };
            double[] fvol = { 55977300, 38599693, 117504326, 56984842, 38554183, 46305400 };
            double[] famount = { 1248606080, 859815744, 2385878272, 1139477760, 786200256, 952838144 };

            double[] fdate = { 20130326, 20130327, 20130328, 20130329, 20130401, 20130402 };
            double[] ftime = { 930, 930, 930, 930, 930, 930 };

            Compile cm = new Compile();
            TStringList src = new TStringList();
            src.Text = str;
            TStringList dst = new TStringList();
            if (!cm.S_Compile(src, dst))
            {
                return;
            }

            TGongSi gs = new TGongSi();
            gs.Add("open", fopen, fopen.Length);
            gs.Add("high", fhigh, fhigh.Length);
            gs.Add("low", flow, flow.Length);
            gs.Add("close", fclose, fclose.Length);
            gs.Add("vol", fvol, fvol.Length);
            gs.Add("amount", famount, famount.Length);
            gs.Add("data", fdate, fdate.Length);
            gs.Add("time", ftime, ftime.Length);
            bool rt = gs.setprogtext(str);
            TextBox[] bn ={ pn1, pn2, pn3, pn4, pn5, pn6, pn7, pn8, pn9, pn10, pn11, pn12, pn13, pn14, pn15, pn16 };
            TextBox[] bi ={ pi1, pi2, pi3, pi4, pi5, pi6, pi7, pi8, pi9, pi10, pi11, pi12, pi13, pi14, pi15, pi16 };
            TextBox[] ba ={ pa1, pa2, pa3, pa4, pa5, pa6, pa7, pa8, pa9, pa10, pa11, pa12, pa13, pa14, pa15, pa16 };
            TextBox[] bv ={ pv1, pv2, pv3, pv4, pv5, pv6, pv7, pv8, pv9, pv10, pv11, pv12, pv13, pv14, pv15, pv16 };

            for (int t = 0; t < gs.CurTech.Input.Count; t++)
            {
                Tinput pt = gs.TechList[0].Input[t];
                bn[t].Text = pt.name;
                bi[t].Text = pt.min1.ToString("d0");
                ba[t].Text = pt.max1.ToString("d0");
                bv[t].Text = pt.def1.ToString("d0");
                notes[t] = pt.notes;
            }

            gongsi.Text = str;

            string s2 = "";
            for (i = 0; i < sl.Count; i++)
            {
                s1 = sl[i].Trim();
                if (s1.IndexOf('*') == 0)
                    s2 = s2 + s1 + "\r\n";
            }
            if (s2.Length > 0)
            {
                outstr[3] = s2;
                OutText.Text = s2;
            }
            /*
            string[] s2 ={ "techname:=", "techtitle:=","password:=", "drawk:=","ystr:=","yvalue:=","digit:=","input(" };
            i = 0;
            while (i < sl.Count)
            {
                s1 = sl[i].ToLower();
                for (j = 0; j < s2.Length; j++)
                {
                 
                }

            }
            */
            //init = true;
        }



        private void bk1_Click(object sender, EventArgs e)
        {
            Button b1=(Button)sender ;
            int id = Convert.ToInt32(b1.Tag);

            OutText.Text = outstr[id-1];
            Button[] bb ={ bk1, bk2, bk3, bk4 };
            for (int i = 0; i < 4; i++)
            {
                bb[i].BackColor = SystemColors.Control;
            }
            bb[id-1].BackColor = Color.Gray;
        }

        private void SaveAs_Click(object sender, EventArgs e)
        {
            if (wfcname.Text.Length==0)
            {
                MessageBox.Show("请输入公式名称");
                wfcname.Focus();
             return;
            }


            OleDbConnection con = new OleDbConnection("Data Source=stock.mdb;Provider=Microsoft.Jet.OLEDB.4.0;");
            con.Open();
            OleDbCommand com1 = new OleDbCommand("select * from [公式库] where 名称=\'" + wfcname.Text + "\'", con);
            OleDbDataReader qu1 = com1.ExecuteReader();
            if (qu1.Read())
            {
                MessageBox.Show("您新建的公式，名称与已有的公式名称重名,请重新命名!");
                wfcname.Focus();
                return;
            }
            qu1.Close();
            com1.Dispose();
            con.Close();
            con.Dispose();
            DialogResult = DialogResult.Yes; 
        }

        void SetValue(string str, string val)
        {
            TStringList sl = new TStringList();
           
            sl.Text = gongsi.Text;
            string s1 = sl.values(str + ":").ToLower().Replace(";", ""); ;
            string s2 = val.ToLower();
            if (s1.Length == 0)
            {
                if (s2.Length > 0)
                    sl.InsertText(0, str.ToUpper() + ":=" + val + ";");
            }
            else
            {
                int d = 0;
                string s3 = sl.values(str + ":", out d);
                if (s2.Length > 0)
                    sl[d] = str.ToUpper() + ":=" + val + ";";
                else
                    sl.Delete(d);
            }
            gongsi.Text = sl.Text;
        }

        private void wfcname_TextChanged(object sender, EventArgs e)
        {
            if (!init) return;
           SetValue("techname",wfcname.Text);
        }

        private void title_TextChanged(object sender, EventArgs e)
        {
            if (!init) return;
            SetValue("techtitle", title.Text);
        }

        private void passck_CheckedChanged(object sender, EventArgs e)
        {
            if (!init) return;
            password.Enabled = passck.Checked;
            if (!password.Enabled)
                SetValue("password","");
            else
                SetValue("password",password.Text);
        }
        private void password_TextChanged(object sender, EventArgs e)
        {
            if (!init) return;
            SetValue("password", password.Text);
        }

        private void Version_TextChanged(object sender, EventArgs e)
        {
            if (!init) return;
            SetValue("version", Version.Text);
        }

        private void drawk_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init) return;
            string[] ss = { "TRUE", "FALSE" };
            SetValue("drawk", ss[drawk.SelectedIndex]);
        }

        private void digit_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!init) return;
            SetValue("digit", digit.SelectedIndex.ToString());
        }

        private void pn1_TextChanged(object sender, EventArgs e)
        {
            TextBox b1 = (TextBox)sender;
            int id = Convert.ToInt32(b1.Tag);

        }

        private void button7_Click(object sender, EventArgs e)
        {
            string ErrorString = "";
            string gs = gongsi.Text;
            if (gs.Length == 0)
            {
                MessageBox.Show("请输入公式内容!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            bk1_Click(bk2, null);
            if (Compile.CheckGongSi(gs, ref ErrorString))
            {
                outstr[1] = "测试通过";
            }
            else
            {
                outstr[1] = ErrorString + "\r\n" + "编译错误";
            }
            OutText.Text = outstr[1];

        }

        private void yStr_TextChanged(object sender, EventArgs e)
        {
            if (!init) return;

            string ss = yStr.Text;
            if (ss.Length > 0)
                ss = ss.Replace(';', '_'); 
            SetValue("YSTR", ss);
        }

        private void yv1_TextChanged(object sender, EventArgs e)
        {
            if (!init) return;

            TextBox[] b1 ={ yv1, yv2, yv3, yv4 };
            string s1 =yv1.Text+"_"+yv2.Text+"_"+yv3.Text+"_"+yv4.Text;
            if (s1.Length > 3)
                SetValue("YVALUE", s1);
            if (s1.Length==0)
                SetValue("YVALUE", "");
        }

        private void yStr_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox b1=(TextBox)sender;
              if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 13 && e.KeyChar != 45 && e.KeyChar != 46)
            {
                e.Handled = true;
            }
             //输入为负号时，只能输入一次且只能输入一次
            if (e.KeyChar == 45 && (b1.SelectionStart != 0 || ((TextBox)sender).Text.IndexOf("-") >= 0)) 
                e.Handled = true;
            if (e.KeyChar == 46 && (b1.Text.IndexOf(".")>-1))
                e.Handled = true;
        }

        private void pn1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 13 && e.KeyChar != 45)
            {
                e.Handled = true;
            }

            //输入为负号时，只能输入一次且只能输入一次
            if (e.KeyChar == 45 && (((TextBox)sender).SelectionStart != 0 || ((TextBox)sender).Text.IndexOf("-") >= 0))
                e.Handled = true;
        }

        private void inputchange(object sender, EventArgs e)
        {
            if (!init) return;

            TextBox b1 = (TextBox)sender;
            int id = Convert.ToInt32(b1.Tag)-1;

            TextBox[] bn ={ pn1, pn2, pn3, pn4, pn5, pn6, pn7, pn8, pn9, pn10, pn11, pn12, pn13, pn14, pn15, pn16 };
            TextBox[] bi ={ pi1, pi2, pi3, pi4, pi5, pi6, pi7, pi8, pi9, pi10, pi11, pi12, pi13, pi14, pi15, pi16 };
            TextBox[] ba ={ pa1, pa2, pa3, pa4, pa5, pa6, pa7, pa8, pa9, pa10, pa11, pa12, pa13, pa14, pa15, pa16 };
            TextBox[] bv ={ pv1, pv2, pv3, pv4, pv5, pv6, pv7, pv8, pv9, pv10, pv11, pv12, pv13, pv14, pv15, pv16 };

            if ((bn[id].Text.Length > 0) && (bi[id].Text.Length > 0) && (ba[id].Text.Length > 0) && (bv[id].Text.Length > 0))
            {
                string ss = "INPUT(" + bi[id].Text + "," + ba[id].Text + "," + bv[id].Text + "," + bv[id].Text;
                if (notes[id].Length > 0)
                    ss = ss + "," + notes[id];
                ss=ss+")";
                SetValue(bn[id].Text, ss);
            }
        }

        private void YStrKey(object sender, KeyPressEventArgs e)
        {
            TextBox b1 = (TextBox)sender;
            if ((e.KeyChar < 48 || e.KeyChar > 57) && e.KeyChar != 8 && e.KeyChar != 13 && e.KeyChar != '-' && e.KeyChar != '.' && e.KeyChar!=';')
            {
                e.Handled = true;
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            TechList tl = new TechList();
            if (tl.ShowDialog()==DialogResult.OK)
            {
                string s1 = tl.SelectFunc;
                string s2 = "";
                if (tl.paramcount==1)
                    s2="()";
                if (tl.paramcount>1)
                {
                    s2="(";
                    for (int i = 0; i < tl.paramcount - 1; i++)
                        s2 = s2 + ",";
                    s2 = s2 + ")";
                }
                gongsi.SelectedText = s1 + s2;
                //gongsi.ActiveTextAreaControl.TextArea.InsertString(s1+s2);
                gongsi.Focus();
            }
            tl.Dispose(); 
        }

        private void Editor_Shown(object sender, EventArgs e)
        {
            init = true;
        }

    }
}