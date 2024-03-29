﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;

namespace FutsTrader
{
    static class Program
    {
        

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            //处理UI线程异常
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //处理非UI线程异
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);


            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                Application.Run(new Starter());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            //TradeLink.AppKit.CrashReport.Report(SrvForm.PROGRAM, (Exception)e.ExceptionObject); 
            Exception ex = (Exception)e.ExceptionObject;
            Globals.logger.GotDebug(ex.ToString());
            Globals.logger.GotDebug(ex.StackTrace.ToString());

        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            Globals.logger.GotDebug(ex.ToString());
            Globals.logger.GotDebug(ex.StackTrace.ToString());
        }
    }
}
