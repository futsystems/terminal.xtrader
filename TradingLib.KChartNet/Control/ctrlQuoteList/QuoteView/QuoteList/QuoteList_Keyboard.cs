using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Common.Logging;
using TradingLib.MarketData;

namespace TradingLib.KryptonControl
{
    public partial class ViewQuoteList
    {
        public override bool PreProcessMessage(ref System.Windows.Forms.Message msg)
        {
            
            if (msg.Msg == 0x100)//WM_KEYDOWN
            {
                Keys key = (Keys)msg.WParam.ToInt32();
                //logger.Info("key message:"+key.ToString());
                if (key == Keys.Up)
                {
                    this.RowUp();
                    return true;
                }
                if (key == Keys.Down)
                {
                    this.RowDown();
                    return true;
                }
                if (key == Keys.Left || key == Keys.Right)
                {
                    return true;
                }

            }
            return base.PreProcessMessage(ref msg);
        }


        void ViewQuoteList_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {

            try
            {
                //logger.Info("KeyDown:{0}".Put(e.KeyCode));

                if (e.KeyCode == Keys.Return)//Q打开K线图//回车打开k线
                {
                    MDSymbol symbol = SelectedSymbol;

                    //logger.Info("Open Chart Symbol:{0}".Put(symbol != null ? symbol.Symbol : "null"));
                    menuOpenKChart(null, null);
                }

                if (e.KeyCode == Keys.W)//W打开盘口
                    openTimeSales();
                if (e.KeyCode == Keys.E)//E打开小下单面板
                    openTicket();


            }
            catch (Exception ex)
            {

            }

        }

        void ViewQuoteList_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            //MessageBox.Show("xx");
            logger.Info("KeyPress:" + e.KeyChar);
        }
    }
}
