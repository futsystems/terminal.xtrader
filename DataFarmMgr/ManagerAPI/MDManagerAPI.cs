using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.MDClient;

namespace TradingLib.DataFarmManager
{
    public static class MDManagerAPI
    {

        public static int DemoRequest(this MDClient.MDClient client, string function)
        {
            return client.ReqMGRContribRequest("DataFarm", function, "");
        }
 
    }
}
