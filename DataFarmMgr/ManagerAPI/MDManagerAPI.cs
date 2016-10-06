using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.DataCore;

namespace TradingLib.DataFarmManager
{
    public static class MDManagerAPI
    {

        public static int DemoRequest(this MDClient client, string function)
        {
            return client.ReqContribRequest("DataFarm", function, "");
        }
 
    }
}
