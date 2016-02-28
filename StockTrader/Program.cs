using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;

namespace StockTrader
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
                WinConsole.Initialize();

                // To make Debug or Trace output to the console, do the following.
                //Debug.Listeners.Remove("default");
                //Debug.Listeners.Add(new TextWriterTraceListener(new ConsoleWriter(...)));

                Console.SetError(new ConsoleWriter(Console.Error, TradingLib.Common.ConsoleColor.Red | TradingLib.Common.ConsoleColor.Intensified | TradingLib.Common.ConsoleColor.WhiteBG, ConsoleFlashMode.FlashUntilResponse, true));
                WinConsole.Color = TradingLib.Common.ConsoleColor.Blue | TradingLib.Common.ConsoleColor.Intensified | TradingLib.Common.ConsoleColor.BlueBG;
                WinConsole.Flash(true);

                Application.Run(new Starter());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = (Exception)e.ExceptionObject;
            MessageBox.Show(ex.ToString());


        }

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            Exception ex = e.Exception;
            MessageBox.Show(ex.ToString());
        }
    }
}
