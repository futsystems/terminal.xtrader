using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;


namespace TradingLib.XTrader.Future
{
    public partial class PageBankAliPay2 : UserControl,IPage
    {
        string _pageName = PageTypes.PAGE_BANK;
        public string PageName { get { return _pageName; } }

        public PageBankAliPay2()
        {
            InitializeComponent();

            if (File.Exists("Config/qr.png"))
            {
                qrImage.Image = Image.FromFile("Config/qr.png");
            }
        }



    }
}
