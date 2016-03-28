using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.MDClient;
using Easychart.Finance.DataProvider;
using Easychart.Finance.DataClient;
using Common.Logging;

namespace WindowsDemo
{
    public class TLDataClient: DataClientBase
    {
        ILog logger = LogManager.GetLogger("TLDataClient");

        public override event StreamingDataChanged OnStreamingData;

        public override bool NeedLogin
        {
            get
            {
                return false;
            }
        }

        public override bool SupportEod
        {
            get
            {
                return true;
            }
        }

        public override bool SupportIntraday
        {
            get
            {
                return true;
            }
        }

        public override bool SupportStreaming
        {
            get
            {
                return true;
            }
        }


        MDClient _client = null;
        MDHandler handler = null;
        public TLDataClient()
        {
            handler = new MDHandler();
            handler.BarsRspEvent += new Action<List<BarImpl>, RspInfo, int, bool>(handler_BarsRspEvent);
            _client = new TradingLib.MDClient.MDClient("114.55.72.206", 5060, 5060);
            _client.RegisterHandler(handler);
            _client.Start();
            while (!_client.Inited)
            {
                Util.sleep(100);
            }
        }

        void handler_BarsRspEvent(List<BarImpl> arg1, RspInfo arg2, int arg3, bool arg4)
        {
            logger.Info("got bars form server");
            //if (this.OnStreamingData != null)
            //{ 
            //    foreach(var bar in arg1)
            //    {
            //        DataPacket pd = new DataPacket(bar.Symbol, bar.BarStartTime, bar.Open, bar.High, bar.Low, bar.Close, bar.Volume, bar.Close);
            //        this.OnStreamingData(this, pd);
            //    }
            //}
        }

        public override void DownloadStreaming()
        {
            logger.Info("start qry bras");
            _client.QryBar("IF1604", 60, DateTime.MinValue, DateTime.MaxValue, 10000);
        }

        public void QryBar()
        {
            logger.Info("start qry bras");
            _client.QryBar("IF1604", 60, DateTime.MinValue, DateTime.MaxValue, 10000);
        }


        
    }
}
