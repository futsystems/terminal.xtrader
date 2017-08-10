using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Runtime.Serialization;
using System.Text;
using Common.Logging;

namespace TradingLib.MarketData
{
    public class NetHelper
    {
        static ILog logger = LogManager.GetLogger("NetHelper");

        /// <summary>
        /// 从http地址获得json数据并解析成数据对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public static T GetHttpJsonResponse<T>(string url)
        {
            try
            {
                System.Net.WebRequest wReq = System.Net.WebRequest.Create(url);
                System.Net.WebResponse wResp = wReq.GetResponse();
                using (System.IO.Stream respStream = wResp.GetResponseStream())
                {
                    using (System.IO.StreamReader receiveStream = new System.IO.StreamReader(respStream))
                    {
                        string receiveString = receiveStream.ReadToEnd();
                        var serializer = new DataContractJsonSerializer(typeof(T));
                        using (var stream = new MemoryStream(Encoding.UTF8.GetBytes(receiveString)))
                        {
                            return (T)serializer.ReadObject(stream);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error("GetHttpJsonResponse Error:" + ex.ToString());
                return default(T);
            }
        }
    }
}