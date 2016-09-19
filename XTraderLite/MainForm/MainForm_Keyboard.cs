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
        void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            logger.Info("Key Press:" + e.KeyChar);
           
        }

        void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            logger.Info("Key Down:" + e.KeyCode.ToString());
            switch (e.KeyCode)
            { 

                case Keys.Escape:
                    ViewQuoteList();
                    break;
                case Keys.F12:
                    SwitchTradingBox();
                    break;
                case Keys.Enter:
                case Keys.F5:
                    {
                        MDSymbol tmp = null;
                        if (ctrlQuoteList.Visible)
                        {
                            tmp = ctrlQuoteList.SymbolSelected;
                            if (tmp == null) return;
                        }

                        if (tmp == null)
                        {
                            tmp = this.CurrentKChartSymbol;
                            if (tmp == null)
                                return;
                        }

                        bool needset=SwitchMainView(e.KeyCode== Keys.Enter?true:false);
                        //if (needset) SetKChartSymbol(tmp);
                    }
                    break;
                default:
                    break;

            }
        }

        //public override bool PreProcessMessage(ref Message msg)
        //{
        //    if (msg.Msg == 0x100)//WM_KEYDOWN
        //    {
        //        Keys key = (Keys)msg.WParam.ToInt32();
        //        logger.Info("key message:" + key.ToString());
        //        switch (key)
        //        {
        //            case Keys.F12:
        //                SwitchTradingBox();
        //                break;
        //            case Keys.Enter:
        //                //ProcessEnter();
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    return base.PreProcessMessage(ref msg);
        //}
    }
}
