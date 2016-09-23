using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Reflection;
using System.Windows.Forms;
namespace CStock
{
    public partial class EditFunc : Form
    {
        public EditFunc()
        {
            InitializeComponent();

        }
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetKeyboardState")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        //大小写状态
        public static bool CapsLockStatus
        {
            get
            {
                byte[] bs = new byte[256];
                GetKeyboardState(bs);
                return (bs[0x14] == 1);
            }
        }

        //插入键状态
        public static bool InsertStatus
        {
            get
            {
                byte[] bs = new byte[256];
                GetKeyboardState(bs);
                return (bs[0x2D] == 1);
            }
        }

        //小键盘上的数字键状态
        public static bool NumLockStatus
        {
            get
            {
                byte[] bs = new byte[256];
                GetKeyboardState(bs);
                return (bs[0x90] == 1);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string ErrorString = "";
            string gs = Gongsi.Text;
            if (gs.Length == 0)
            {
                MessageBox.Show("请输入公式内容!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (CheckGongSi(gs, ref ErrorString))
            {
                Log.Text = "测试通过";
            }
            else
            {
                Log.Text = ErrorString + "\r\n" + "编译错误";
            }
        }

        private void EditFunc_Load(object sender, EventArgs e)
        {
            panel4.Dock = DockStyle.Fill;
            TGongSi gs = new TGongSi();
            for (int i = 0; i < gs.func.Count; i++)
            {
                SubList.Items.Add(gs.func.GetName(i));
            }
            SubList.Sorted = true;
            this.Gongsi.TextChanged += new System.EventHandler(this.TextChange);

            TStringList sl = new TStringList();
            sl.Text = Gongsi.Text;
            string s1 = sl.values("drawk:").ToLower();
            if (s1.IndexOf("true") > -1)
                drawk.Checked = true;

        }


        private void EditFunc_Shown(object sender, EventArgs e)
        {
            name.Focus();
            TextChange(null, null);
        }

        private void TextChange(object sender, EventArgs e)
        {
            TStringList sl = new TStringList();
            sl.Text = Gongsi.Text;
            string s1 = sl.values("drawk:").ToLower();

            this.drawk.CheckedChanged -= new System.EventHandler(this.drawk_CheckedChanged);
            this.title.TextChanged -= new System.EventHandler(this.title_TextChanged);
            this.name.TextChanged -= new System.EventHandler(this.name_TextChanged);
            this.password.TextChanged -= new System.EventHandler(this.password_TextChanged);

            if (s1.IndexOf("true") > -1)
                drawk.Checked = true;
            if (s1.IndexOf("false") > -1)
                drawk.Checked = false;
            name.Text = sl.values("techname:").Replace(";", "");
            password.Text = sl.values("password:").Replace(";", "");
            title.Text = sl.values("techtitle:").Replace(";", "");

            this.drawk.CheckedChanged += new System.EventHandler(this.drawk_CheckedChanged);
            this.title.TextChanged += new System.EventHandler(this.title_TextChanged);
            this.name.TextChanged += new System.EventHandler(this.name_TextChanged);
            this.password.TextChanged += new System.EventHandler(this.password_TextChanged);

            sb3.Text = InsertStatus ? "插入" : "覆盖";
            sb4.Text = CapsLockStatus ? "大写" : "小写";
            sb5.Text = NumLockStatus ? "数字" : "";
        }

        private void Gongsi_Click(object sender, EventArgs e)
        {
            int row, col = 1;
            string text = Gongsi.Text.Substring(0, Gongsi.SelectionStart);
            string[] lines = text.Split('\n');
            row = lines.Length;
            if (lines.Length - 1 >= 0)
                col = lines[lines.Length - 1].Length + 1;
            xx.Text = col.ToString();
            yy.Text = row.ToString();

        }

        private void Gongsi_KeyDown(object sender, KeyEventArgs e)
        {
            Gongsi_Click(null, null);
        }

        private void input_Click(object sender, EventArgs e)
        {
            InputFm fm = new InputFm();
            if (fm.ShowDialog() == DialogResult.OK)
            {
                string name = fm.name.Text.Trim();
                if (name.Length > 0)
                {
                    string s = name + ":=INPUT(" + fm.min1.Value.ToString() + "," + fm.max1.Value.ToString() + "," + fm.def.Value.ToString() + "," + fm.def.Value.ToString() + ");\r\n" + Gongsi.Text;
                    Gongsi.Text = s;
                }
            }
            fm.Dispose();
        }



        private void color_Click(object sender, EventArgs e)
        {
            if (cd1.ShowDialog() == DialogResult.OK)
            {

                String ss = String.Format("COLOR{0:X2}{1:X2}{2:X2}", cd1.Color.R, cd1.Color.G, cd1.Color.B);
                string s = Gongsi.Text;
                int idx = Gongsi.SelectionStart;
                s = s.Insert(idx, ss);
                Gongsi.Text = s;
                Gongsi.SelectionStart = idx;
                Gongsi.Focus();
            }

        }

        private void SubList_DoubleClick(object sender, EventArgs e)
        {
            int i = SubList.SelectedIndex;
            if (i == -1)
                return;
            string ss = SubList.Items[i].ToString();// SubList.SelectedValue.ToString();
            string s = Gongsi.Text;
            int idx = Gongsi.SelectionStart;
            s = s.Insert(idx, ss);
            Gongsi.Text = s;
            Gongsi.SelectionStart = idx;
            Gongsi.Focus();
        }
        private void drawk_CheckedChanged(object sender, EventArgs e)
        {
            string ss = drawk.Checked ? "DRAWK:=TRUE;" : "DRAWK:=FALSE;";
            int old = Gongsi.SelectionStart;
            TStringList sl = new TStringList();
            sl.Text = Gongsi.Text;
            int row = -1;
            string s1 = sl.values("drawk:", out row).ToLower();
            this.Gongsi.TextChanged -= new System.EventHandler(this.TextChange);

            if (row == -1)
            {
                ss = Gongsi.Text + "\r\n" + ss;
                Gongsi.Text = ss;
            }
            else
            {
                sl[row] = ss;
                Gongsi.Text = sl.Text.Trim();
            }
            Gongsi.SelectionStart = old;
            this.Gongsi.TextChanged += new System.EventHandler(this.TextChange);

        }
        private void name_TextChanged(object sender, EventArgs e)
        {
            string ss = "TECHNAME:=" + name.Text + ";";
            int old = Gongsi.SelectionStart;
            TStringList sl = new TStringList();
            sl.Text = Gongsi.Text;
            int row = -1;
            string s1 = sl.values("techname:", out row).ToLower();
            this.Gongsi.TextChanged -= new System.EventHandler(this.TextChange);
            if (row == -1)
            {
                ss = Gongsi.Text + "\r\n" + ss;
                Gongsi.Text = ss;
            }
            else
            {
                sl[row] = ss;
                Gongsi.Text = sl.Text.Trim();
            }
            Gongsi.SelectionStart = old;
            this.Gongsi.TextChanged += new System.EventHandler(this.TextChange);
        }

        private void title_TextChanged(object sender, EventArgs e)
        {
            string ss = "TECHTITLE:=" + title.Text + ";";
            int old = Gongsi.SelectionStart;
            TStringList sl = new TStringList();
            sl.Text = Gongsi.Text;
            int row = -1;
            string s1 = sl.values("techtitle:", out row).ToLower();
            this.Gongsi.TextChanged -= new System.EventHandler(this.TextChange);

            if (row == -1)
            {
                ss = Gongsi.Text + "\r\n" + ss;
                Gongsi.Text = ss;
            }
            else
            {
                sl[row] = ss;
                Gongsi.Text = sl.Text.Trim();
            }
            Gongsi.SelectionStart = old;
            this.Gongsi.TextChanged += new System.EventHandler(this.TextChange);

        }

        bool CheckGongSi(string str, ref string Error)
        {
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
                Error = cm.ErrorString;
                return false;
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
            Error = gs.ErrorString;
            return rt;
        }

        private void EditFunc_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.OK)
            {
                if (name.Text.Length == 0)
                {
                    MessageBox.Show("请输入公式名称!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }

                string gs = Gongsi.Text.Trim();
                if (gs.Length == 0)
                {
                    MessageBox.Show("请输入公式内容!", "提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                    return;
                }

                string ErrorString = "";
                if (!CheckGongSi(Gongsi.Text, ref ErrorString))
                {
                    Log.Text = ErrorString + "\r\n" + "编译错误";
                    e.Cancel = true;
                    return;
                }
            }
        }

        private void password_TextChanged(object sender, EventArgs e)
        {
            string ss = "PASSWORD:=" + password.Text + ";";
            int old = Gongsi.SelectionStart;
            TStringList sl = new TStringList();
            sl.Text = Gongsi.Text;
            int row = -1;
            string s1 = sl.values("password:", out row).ToLower();
            this.Gongsi.TextChanged -= new System.EventHandler(this.TextChange);
            if (row == -1)
            {
                ss = Gongsi.Text + "\r\n" + ss;
                Gongsi.Text = ss;
            }
            else
            {
                sl[row] = ss;
                Gongsi.Text = sl.Text.Trim();
            }
            Gongsi.SelectionStart = old;
            this.Gongsi.TextChanged += new System.EventHandler(this.TextChange);
        }

        private void sb6_Click(object sender, EventArgs e)
        {
            TStringList src = new TStringList();
            TStringList dst = new TStringList();
            src.Text = Gongsi.Text;
            Compile cm = new Compile();
            cm.S_Compile(src, dst);
            Log.Text = dst.Text;
        }

        private void SubList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SubList.SelectedIndex == -1)
                return;
            string fname = SubList.Text;
            for (int i = 0; i < Notes.Items.Count; i++)
            {
                string s1 = ((string)Notes.Items[i]).ToLower();
                if (s1.IndexOf(fname) == 0)
                {
                    Log.Text = "";
                    string s2 = "";
                    for (int j = i; j < Notes.Items.Count; j++)
                    {
                      s1= ((string)Notes.Items[j]).Trim();
                      if (s1.Length == 0)
                          break;
                      s2 += s1 + "\r\n";
                    }
                    Log.Text = s2;
                    break;
                }

            }


        }


    }

}