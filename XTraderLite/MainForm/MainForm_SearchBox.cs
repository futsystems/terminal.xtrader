using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using TradingLib.MarketData;

namespace XTraderLite
{
    public partial class MainForm
    {
        void InitSearchBox()
        {
            KeyCode.MyKeyUp += new EventHandler(KeyCode_MyKeyUp);
            KeyCode.MyKeyDown += new EventHandler(KeyCode_MyKeyDown);
            KeyCode.KeyDown += new KeyEventHandler(KeyCode_KeyDown);
            KeyCode.TextChanged += new EventHandler(KeyCode_TextChanged);

            GpListBox.DoubleClick += new EventHandler(GPDouble);
        }


        /// <summary>
        /// 输入框文字变化后 动态更新GroupList显示的内容
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KeyCode_TextChanged(object sender, EventArgs e)
        {
            String s1 = KeyCode.Text.ToUpper();
            if (KeyCode.Text.Length == 0)
            {
                this.KeyPreview = true;
                GpKey.Visible = false;
                return;
            }
            KeyCode.Text = KeyCode.Text.ToUpper();
            KeyCode.SelectionStart = KeyCode.Text.Length;
            KeyCode.SelectionLength = 0;
            GpListBox.Items.Clear();
            if ((s1[0] >= '0') && (s1[0] <= '9'))
            {
                string[] array = MDService.DataAPI.Symbols.Where(sym => sym.Symbol.Contains(s1)).Select(sym=>sym.KeyTitle).ToArray();
                GpListBox.Items.AddRange(array);
            }
            if ((s1[0] >= 'A') && (s1[0] <= 'Z'))
            {
                string[] array = MDService.DataAPI.Symbols.Where(sym => sym.Key.Contains(s1)).Select(sym => sym.KeyTitle).ToArray();
                GpListBox.Items.AddRange(array);

            }
            if (GpListBox.Items.Count > 0)
            {
                GpListBox.SelectedIndex = 0;
            }
        }

        void KeyCode_KeyDown(object sender, KeyEventArgs e)
        {
            Keys kv = e.KeyCode;// e.KeyValue;

            if (kv == Keys.Escape)
            {
                this.KeyPreview = true;
                GpKey.Visible = false;
            }
            if (kv == Keys.Enter)
            {
                GpKey.Visible = false;
                this.KeyPreview = true;
                GPDouble(sender, null);
                logger.Info("SearchBox enter symbol");
            }
        }

        void KeyCode_MyKeyDown(object sender, EventArgs e)
        {
            //logger.Info("SearchBox key down");
            int fh = GpListBox.Items.Count;
            if (fh - 1 > GpListBox.SelectedIndex)
            {
                GpListBox.SelectedIndex = GpListBox.SelectedIndex + 1;
            }
            KeyCode.SelectionStart = KeyCode.Text.Length;
            return;
        }

        void KeyCode_MyKeyUp(object sender, EventArgs e)
        {
            //logger.Info("SearchBox key up");
            int fh = GpListBox.SelectedIndex;
            if (fh > 0)
                GpListBox.SelectedIndex = GpListBox.SelectedIndex - 1;
            KeyCode.SelectionStart = KeyCode.Text.Length + 1;
            return;
        }


        private void GPDouble(object sender, EventArgs e)
        {
            int fh = GpListBox.SelectedIndex;
            if (fh == -1)
                return;
            String s = (string)GpListBox.SelectedItem;
            String[] s1 = s.Split(' ');
            foreach(var stk in MDService.DataAPI.Symbols)
            {
                if (stk.Symbol.Contains(s1[0]))
                {
                    GpKey.Visible = false;
                    this.KeyPreview = true;

                    ViewBarChart();
                    SetKChartSymbol(stk);
                    break;
                }
            }
        }
    }
}
