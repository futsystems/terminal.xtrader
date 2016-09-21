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

            SymbolListBox.DoubleClick += new EventHandler(SearchSelect);
            SearchBox.MouseMove += new MouseEventHandler(Search_MouseMove);
            SearchBox.MouseUp += new MouseEventHandler(Search_MouseUp);
            SearchBox.MouseDown += new MouseEventHandler(Search_MouseDown);
        }

        private bool m_isSearchMouseDown = false;
        private Point m_SearchMousePos = new Point();
        void Search_MouseMove(object sender, MouseEventArgs e)
        {
            if (m_isSearchMouseDown)
            {
                Point tempPos = Cursor.Position;
                SearchBox.Location = new Point(SearchBox.Location.X + (tempPos.X - m_SearchMousePos.X), SearchBox.Location.Y + (tempPos.Y - m_SearchMousePos.Y));
                m_SearchMousePos = Cursor.Position;
            }
        }

        void Search_MouseUp(object sender, MouseEventArgs e)
        {
            m_isSearchMouseDown = false;
        }

        void Search_MouseDown(object sender, MouseEventArgs e)
        {
            m_SearchMousePos = Cursor.Position;
            m_isSearchMouseDown = true;
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
                SearchBox.Visible = false;
                return;
            }
            KeyCode.Text = KeyCode.Text.ToUpper();
            KeyCode.SelectionStart = KeyCode.Text.Length;
            KeyCode.SelectionLength = 0;
            SymbolListBox.Items.Clear();
            if ((s1[0] >= '0') && (s1[0] <= '9'))
            {
                string[] array = MDService.DataAPI.Symbols.Where(sym => sym.Symbol.Contains(s1)).Select(sym=>sym.KeyTitle).ToArray();
                SymbolListBox.Items.AddRange(array);
            }
            if ((s1[0] >= 'A') && (s1[0] <= 'Z'))
            {
                string[] array = MDService.DataAPI.Symbols.Where(sym => sym.Key.Contains(s1)).Select(sym => sym.KeyTitle).ToArray();
                SymbolListBox.Items.AddRange(array);

            }
            if (SymbolListBox.Items.Count > 0)
            {
                SymbolListBox.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 响应Escap以及Enter键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void KeyCode_KeyDown(object sender, KeyEventArgs e)
        {
            Keys kv = e.KeyCode;// e.KeyValue;

            if (kv == Keys.Escape)
            {
                this.KeyPreview = true;
                SearchBox.Visible = false;
            }
            if (kv == Keys.Enter)
            {
                SearchBox.Visible = false;
                this.KeyPreview = true;
                SearchSelect(sender, null);
                logger.Info("SearchBox enter symbol");
            }
        }

        //搜索输入文本框中的上下按键 同步ListBox中中选中行上下移动
        void KeyCode_MyKeyDown(object sender, EventArgs e)
        {
            if (SymbolListBox.SelectedIndex < SymbolListBox.Items.Count - 1) SymbolListBox.SelectedIndex++;
            return;
        }

        void KeyCode_MyKeyUp(object sender, EventArgs e)
        {
            if (SymbolListBox.SelectedIndex > 0) SymbolListBox.SelectedIndex--;
            return;
        }


        private void SearchSelect(object sender, EventArgs e)
        {
            int fh = SymbolListBox.SelectedIndex;
            if (fh == -1)
                return;
            String s = (string)SymbolListBox.SelectedItem;
            String[] s1 = s.Split(' ');
            foreach(var stk in MDService.DataAPI.Symbols)
            {
                if (stk.Symbol.Contains(s1[0]))
                {
                    SearchBox.Visible = false;
                    this.KeyPreview = true;
                    ViewKChart(stk);
                    break;
                }
            }
        }
    }
}
