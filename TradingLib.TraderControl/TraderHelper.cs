using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
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



    }
}
