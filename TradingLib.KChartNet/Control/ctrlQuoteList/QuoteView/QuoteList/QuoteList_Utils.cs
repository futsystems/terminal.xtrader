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

namespace TradingLib.KryptonControl
{
    public partial class ViewQuoteList
    {

        /// <summary>
        /// 与前值比较火的对应的颜色
        /// </summary>
        /// <param name="A"></param>
        /// <param name="preA"></param>
        /// <returns></returns>
        internal Color GetUpDownColor(double A, double preA)
        {
            if (A > preA)
            {
                return this.DefaultQuoteStyle.UPColor;
            }
            if (A < preA)
            {
                return this.DefaultQuoteStyle.DNColor;
            }
            return this.DefaultQuoteStyle.EQColor;
        }

    }
}
