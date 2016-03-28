using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Common.Logging;


using STOCKCHARTXLib;
using TradingLib.API;
using TradingLib.Common;


namespace TradingLib.Chart
{
    public partial class ctlChart
    {

        Symbol _symbol = null;
        /// <summary>
        /// Chart对应的合约
        /// </summary>
        public Symbol Symbol
        {
            get { return _symbol; }
            set { _symbol = null; }
        }

        const string OPEN = "Open";
        const string HIGH = "High";
        const string LOW = "Low";
        const string ClOSE = "Close";
        const string VOLUME = "Volume";
        const string OPENINTEREST = "Oi";

        string GetSerieseName(string name)
        {
            return string.Format("{0}{1}{2}", _symbol.Symbol, ".", name);
        }

        private string NAME_OPEN { get { return GetSerieseName(OPEN); } }
        private string NAME_HIGH { get { return GetSerieseName(HIGH); } }
        private string NAME_LOW { get { return GetSerieseName(LOW); } }
        private string NAME_CLOSE { get { return GetSerieseName(ClOSE); } }
        private string NAME_VOLUME { get { return GetSerieseName(VOLUME); } }
        private string NAME_OI { get { return GetSerieseName(OPENINTEREST); } }


    }
}
