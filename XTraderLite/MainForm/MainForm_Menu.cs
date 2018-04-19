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
using TradingLib.XTrader.Control;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
namespace XTraderLite
{
    public partial class MainForm
    {


        void WireMenu()
        {

            //工具
            menuConnect.Click += new EventHandler(menuConnect_Click);
            menuDisconnect.Click += new EventHandler(menuDisconnect_Click);
            menuScreen.Click += new EventHandler(menuScreen_Click);
            menuPrint.Click += new EventHandler(menuPrint_Click);
            printDocument.PrintPage += new PrintPageEventHandler(printDocument1_PrintPage);
           
            menuExit.Click += new EventHandler(menuExit_Click);
            menuDataFarmSiteList.Click += new EventHandler(menuDataFarmSiteList_Click);

            //分析
            menuIntraView.Click += new EventHandler(menuIntraView_Click);
            menuBarView.Click += new EventHandler(menuBarView_Click);
            menuTradeSplit.Click += new EventHandler(menuTradeSplit_Click);
            menuPriceVol.Click += new EventHandler(menuPriceVol_Click);
            menuSwitchKchart.Click += new EventHandler(menuSwitchKchart_Click);


            //交易
            menuTrading.Click += new EventHandler(menuTrading_Click);
            menuPay.Click += new EventHandler(menuPay_Click);

            //帮助
            menuRelief.Click += new EventHandler(menuRelief_Click);
            menuAbout.Click += new EventHandler(menuAbout_Click);
            menuShortCutKey.Click += new EventHandler(menuShortCutKey_Click);

            menuWatchMgr.Click += new EventHandler(menuWatchMgr_Click);
            if (!Global.ShowMDIP)
            {
                menuDataFarmSiteList.Visible = false;
            }
        }

        void menuWatchMgr_Click(object sender, EventArgs e)
        {
            var fm = new fmWatchMgr(watchList);
            fm.ShowDialog();
        }

        //打开网页 在线出入金
        void menuPay_Click(object sender, EventArgs e)
        {
            try
            {
                //System.Diagnostics.Process.Start("iexplore.exe", Global.PayUrl);
            }
            catch (Exception ex)
            {
                //MessageBox.Show(string.Format("请手工打开网页:{0} 进行出入金操作", Global.PayUrl));
            }
                
        }

        void menuDataFarmSiteList_Click(object sender, EventArgs e)
        {
            frmDataFarmList fm = new frmDataFarmList();
            fm.ShowDialog();
            fm.Close();
        }

        void menuShortCutKey_Click(object sender, EventArgs e)
        {
            frmShortCutKey fm = new frmShortCutKey();
            fm.ShowDialog();
        }

        void menuAbout_Click(object sender, EventArgs e)
        {
            frmAbout fm = new frmAbout();
            fm.ShowDialog();
        }

        void menuRelief_Click(object sender, EventArgs e)
        {
            frmRelief fm = new frmRelief();
            fm.ShowDialog();
        }

        void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            e.Graphics.DrawImage(memoryImage, 0, 0);
        }

        void menuExit_Click(object sender, EventArgs e)
        {
            Application.ExitThread();
            Environment.Exit(0);
        }


        #region 打印屏幕

        Bitmap memoryImage;
        PrintDocument printDocument = new PrintDocument();

        private void CaptureScreen()
        {
            Graphics myGraphics = this.CreateGraphics();
            Size s = this.Size;
            memoryImage = new Bitmap(s.Width, s.Height, myGraphics);
            Graphics memoryGraphics = Graphics.FromImage(memoryImage);
            memoryGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, s);
        }

        void menuPrint_Click(object sender, EventArgs e)
        {
            DialogResult dr = MessageBox.Show("确认打印屏幕?", "打印", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (dr == System.Windows.Forms.DialogResult.OK)
            {
                CaptureScreen();
                printDocument.Print();
            }
        }
        #endregion


        void menuScreen_Click(object sender, EventArgs e)
        {

            Bitmap bit = new Bitmap(Width, Height);//实例化一个和窗体一样大的bitmap
            Graphics g = Graphics.FromImage(bit);
            g.CompositingQuality = CompositingQuality.HighQuality;//质量设为最高
            g.CopyFromScreen(PointToScreen(Point.Empty), Point.Empty, Size);//只保存某个控件（这里是panel游戏区）
            //bit.Save("weiboTemp.png");//默认保存格式为PNG，保存成jpg格式质量不是很好
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.DefaultExt = "*.png";
            sfd.Filter = "png|*.png";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                bit.Save(sfd.FileName);
            }
            bit.Dispose();
        }

        void menuDisconnect_Click(object sender, EventArgs e)
        {
            System.Threading.ThreadPool.QueueUserWorkItem(o => MDService.DataAPI.Disconnect());
        }

        void menuConnect_Click(object sender, EventArgs e)
        {
            List<string> serverList = new List<string>();
            int port = 0;
            foreach (var v in (new ServerConfig("market.cfg")).GetServerNodes())
            {
                if (port == 0) port = v.Port;
                serverList.Add(v.Address);
            }
            System.Threading.ThreadPool.QueueUserWorkItem(o => MDService.DataAPI.Connect(serverList.ToArray(), port));
        }



        void menuPriceVol_Click(object sender, EventArgs e)
        {
            ViewPriceVolList();
        }

        void menuTradeSplit_Click(object sender, EventArgs e)
        {
            ViewTickList();
        }

        void menuBarView_Click(object sender, EventArgs e)
        {
            ctrlKChart.KChartViewType = CStock.KChartViewType.KView;
            ViewKChart();
        }

        void menuIntraView_Click(object sender, EventArgs e)
        {
            ctrlKChart.KChartViewType = CStock.KChartViewType.TimeView;
            ViewKChart();
        }
        
        void menuTrading_Click(object sender, EventArgs e)
        {
            SwitchTradingBox();
        }

        void menuSwitchKchart_Click(object sender, EventArgs e)
        {
            SwitchMainView();
        }
    }
}
