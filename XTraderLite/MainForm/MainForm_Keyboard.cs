using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

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
                case Keys.Enter:
                    logger.Info("active:" + this.ActiveControl.GetType().Name);
                    SwitchMainView(true);
                    break;
                case Keys.Escape:
                    ViewQuoteList();
                    break;
                case Keys.F12:
                    SwitchTradingBox();
                    break;
                case Keys.F5:
                    SwitchMainView(false);
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
