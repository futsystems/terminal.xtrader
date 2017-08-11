using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.DataCore;
using Common.Logging;


namespace DataCoreTest
{
    public class DataAPI : IDataCallback
    {
        ILog logger = LogManager.GetLogger("DataAPI");

        DataClient client = null;
        int _num;
        public DataAPI(int num,string ipaddress, int port)
        {
            client = new DataClient(new string[] { ipaddress }, port);
            client.RegisterCallback(this);
            
            _num = num;
        }

        public void Start()
        {
            client.Start();
        }

        public void OnConnected()
        {
            logger.Info(string.Format("#{0} Connected",_num));
            client.Login("", "");
        }
        public void OnDisconnected()
        { 
        
        }
        public void OnLogin(LoginResponse response)
        {
            client.QryMarketTime();

            //client.RegisterSymbol("HKEX", new string[] { "HSIQ7" });
        }

        public void OnInitialized()
        {
            logger.Info(string.Format("#{0} Inited", _num));
            foreach (var exch in client.Exchanges)
            {
                client.RegisterSymbol(exch.EXCode, client.Symbols.Where(sym => sym.Exchange == exch.EXCode).Select(sym => sym.Symbol).ToArray());
            }
        }

        public void OnRtnTick(Tick k)
        {
            //logger.Info("Tick:" + k.ToString());
        
        }
        public void OnRspBar(RspQryBarResponseBin response)
        { 
        
        }
        public void OnRspTradeSplit(RspXQryTradeSplitResponse response)
        { 
        
        }
        public void OnRspPriceVol(RspXQryPriceVolResponse response)
        { 
        
        }
        public void OnRspMinuteData(RspXQryMinuteDataResponse response)
        { 
        
        }
        

    }
}
