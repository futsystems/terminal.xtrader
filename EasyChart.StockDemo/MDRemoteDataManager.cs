using System;
using System.Collections.Generic;
using System.Text;
using Easychart.Finance.DataProvider;

using TradingLib.API;
using TradingLib.Common;
using TradingLib.MDClient;


namespace WindowsDemo
{
    public class MDRemoteDataManager:DataManagerBase
    {
        MDClient client = null;
        MDHandler handler = null;
        public MDRemoteDataManager()
        {
            handler = new MDHandler();
            handler.BarsRspEvent += new Action<List<BarImpl>, RspInfo, int, bool>(handler_BarsRspEvent);
            client = new TradingLib.MDClient.MDClient("114.55.72.206", 5060, 5060);
            client.RegisterHandler(handler);
            client.Start();
        }

        void handler_BarsRspEvent(List<BarImpl> arg1, RspInfo arg2, int arg3, bool arg4)
        {
            throw new NotImplementedException();
        }
        public override IDataProvider GetData(string Code, int Count)
        {
            return base.GetData(Code, Count);
        }
    }
}
