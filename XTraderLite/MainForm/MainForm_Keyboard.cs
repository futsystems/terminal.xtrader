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


        bool FocusInMarket
        {
            get { return viewMap.Values.Any(view => view.Focused); }
        }

        void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            int key = e.KeyChar;
            string ks = e.KeyChar.ToString();
            ks = ks.ToUpper();
            if (((key >= '0') && (key <= '9')) || ((key >= 'A') && (key <= 'Z')) || ((key >= 'a') && (key <= 'z')))
            {
                if (FocusInMarket)
                {
                    this.KeyPreview = false;
                    SearchBox.BringToFront();

                    SearchBox.SetBounds(this.Width - SearchBox.Width - 15, this.Height - SearchBox.Height - 70, SearchBox.Width, SearchBox.Height);
                    SearchBox.Visible = true;

                    KeyCode.Text = ks;
                    KeyCode.Focus();
                    KeyCode.SelectionStart = 1;
                    KeyCode.SelectionLength = 0;
                }
            }
           
        }

        void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            { 
                case Keys.Escape:
                    RollBackView();
                    break;
                case Keys.F1:
                    ViewTickList();
                    break;
                case Keys.F2:
                    ViewPriceVolList();
                    break;
                case Keys.F3:
                    {
                        MDSymbol sym = MDService.DataAPI.GetSymbol(ConstsExchange.EXCH_SSE, "999999");
                        ViewKChart(sym);
                        break;
                    }
                case Keys.F4:
                    {
                        MDSymbol sym = MDService.DataAPI.GetSymbol(ConstsExchange.EXCH_SZE, "399001");
                        ViewKChart(sym);
                        break;
                    }
                case Keys.F8:
                    {
                        if (ctrlKChart.Visible && ctrlKChart.IsBarView)
                        {
                            SwitchFreq();
                        }
                        break;
                    }
                case Keys.F12:
                    SwitchTradingBox();
                    break;
                case Keys.Enter:
                case Keys.F5:
                    SwitchMainView();
                    break;
                case Keys.F10:
                    ViewSymbolInfo();
                    break;
                case Keys.F11:
                    Control c = GetFocusedControl();
                    MessageBox.Show(c.ToString());
                    break;
                default:
                    break;

            }
        }
    }
}
