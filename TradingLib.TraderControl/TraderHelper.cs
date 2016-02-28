using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using Telerik.WinControls.UI;
using System.Windows.Forms;


namespace TradingLib.TraderControl
{
    public class TraderHelper
    {
        public static System.Windows.Forms.DialogResult WindowMessage(string message, string title = "提示")
        {
            MessageForm fm = new MessageForm(message, title);
            return fm.ShowDialog();
        }

        public static System.Windows.Forms.DialogResult ConfirmWindow(string message, string title = "提示")
        {
            return fmConfirm.Show(message, title);
        }

        /// <summary>
        /// 将控件适配到IDataSource用于数据的统一绑定
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static IDataSource AdapterToIDataSource(object obj)
        {
            if (obj is RadDropDownList)
                return new RadDropDownList2IDataSource(obj as RadDropDownList);
            else if (obj is RadListControl)
                return new RadListControl2IDataSource(obj as RadListControl);
            else if (obj is ListBox)
                return new ListBox2IDataSource(obj as ListBox);
            else if (obj is ComboBox)
                return new ComboBox2IDataSource(obj as ComboBox);
            return new Invalid2IDataSource(); ;
        }


        /// <summary>
        /// 获得某个合约
        /// </summary>
        /// <param name="symbol"></param>
        /// <returns></returns>
        public static Symbol GetSymbol(string symbol)
        {
            return CoreService.BasicInfoTracker.GetSymbol(symbol);
        }


 


        /// <summary>
        /// 获得某个合约的数字显示方式
        /// </summary>
        /// <param name="sym"></param>
        /// <returns></returns>
        public static string GetDisplayFormat(Symbol sym)
        {
            if (sym == null) return "{0:F2}";
            if (sym.SecurityFamily == null) return "{0:F2}";
            return GetDisplayFormat(sym.SecurityFamily.PriceTick);
        }
        /// <summary>
        /// 通过PriceTick得到数字显示格式
        /// </summary>
        /// <param name="pricetick"></param>
        /// <returns></returns>
        public static string GetDisplayFormat(decimal pricetick)
        {
            //1 0.2
            string[] p = pricetick.ToString().Split('.');
            if (p.Length <= 1)
                return "{0:F0}";
            else
                return "{0:F" + p[1].ToCharArray().Length.ToString() + "}";

        }

        public static int GetDecimalPlace(decimal pricetick)
        {

            //1 0.2
            string[] p = pricetick.ToString().Split('.');
            if (p.Length <= 1)
                return 0;
            else
                return p[1].ToCharArray().Length;

        }

        static string GetDefaultWebBrowserFilePath()
        {
            string _BrowserKey1 = @"Software\Clients\StartmenuInternet\";
            string _BrowserKey2 = @"\shell\open\command";

            RegistryKey _RegistryKey = Registry.CurrentUser.OpenSubKey(_BrowserKey1, false);
            if (_RegistryKey == null)
                _RegistryKey = Registry.LocalMachine.OpenSubKey(_BrowserKey1, false);
            String _Result = _RegistryKey.GetValue("").ToString();
            _RegistryKey.Close();

            _RegistryKey = Registry.LocalMachine.OpenSubKey(_BrowserKey1 + _Result + _BrowserKey2);
            _Result = _RegistryKey.GetValue("").ToString();
            _RegistryKey.Close();

            if (_Result.Contains("\""))
            {
                _Result = _Result.TrimStart('"');
                _Result = _Result.Substring(0, _Result.IndexOf('"'));
            }
            return _Result;
        }

        public static void OpenURL(string url = "http://www.baidu.com")
        {
            //string BrowserPath = TraderHelper.GetDefaultWebBrowserFilePath();//.GetDefaultWebBrowserFilePath();
            string gotoUrl = url;
            if (!gotoUrl.StartsWith("http://"))
            {
                gotoUrl = "http://" + gotoUrl;
            }
            //如果输入的url地址为空，则清空url地址，浏览器默认跳转到默认页面
            if (gotoUrl == "http://")
            {
                gotoUrl = "";
            }
            System.Diagnostics.Process.Start(gotoUrl);
        }

        public static ArrayList GetOffsetCBList()
        {
            ArrayList list = new ArrayList();
            ValueObject<QSEnumOffsetFlag> vo0 = new ValueObject<QSEnumOffsetFlag>();
            vo0.Name = Util.GetEnumDescription(QSEnumOffsetFlag.UNKNOWN);
            vo0.Value = QSEnumOffsetFlag.UNKNOWN;
            list.Add(vo0);

            ValueObject<QSEnumOffsetFlag> vo1 = new ValueObject<QSEnumOffsetFlag>();
            vo1.Name = Util.GetEnumDescription(QSEnumOffsetFlag.OPEN);
            vo1.Value = QSEnumOffsetFlag.OPEN;
            list.Add(vo1);

            ValueObject<QSEnumOffsetFlag> vo2 = new ValueObject<QSEnumOffsetFlag>();
            vo2.Name = Util.GetEnumDescription(QSEnumOffsetFlag.CLOSE);
            vo2.Value = QSEnumOffsetFlag.CLOSE;
            list.Add(vo2);

            ValueObject<QSEnumOffsetFlag> vo3 = new ValueObject<QSEnumOffsetFlag>();
            vo3.Name = Util.GetEnumDescription(QSEnumOffsetFlag.CLOSETODAY);
            vo3.Value = QSEnumOffsetFlag.CLOSETODAY;
            list.Add(vo3);
            return list;

        }
    }
}
