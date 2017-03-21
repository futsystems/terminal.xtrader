using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAPI.Futs
{
    public class DataAPIConstants
    {
        static DataAPIConstants()
        {
            IsLongSymbolName = false;
        }
        public static bool IsLongSymbolName { get; set; }
    }
}
