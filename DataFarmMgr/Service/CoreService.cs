//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace TradingLib.DataFarmManager
//{
//    public class CoreService
//    {


//        static CoreService defaultinstance = null;

//        static CoreService()
//        {
//            defaultinstance = new CoreService();
//        }

//        bool _isinited = false;
//        public static bool Initialized
//        {
//            get
//            {
//                return defaultinstance._isinited;
//            }
//        }


//        MDClient.MDClient _client = null;

//        public static MDClient.MDClient MDClient
//        {
//            get
//            {
//                return defaultinstance._client;
//            }
//        }

//        public static void InitClient(string address, int port)
//        {
//            if (defaultinstance._client == null)
//            {
//                MDClient.MDClient tlclient = new MDClient.MDClient(address, port ,port);

//                defaultinstance._client = tlclient;
//            }
//        }

//    }
//}
