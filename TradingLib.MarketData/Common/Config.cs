using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TradingLib.MarketData
{
    public class ConfigFileBase
    {
        /// <summary>
        /// 获得配置文件
        /// </summary>
        /// <param name="cfgname"></param>
        /// <returns></returns>
        public static string GetConfigFile(string cfgname)
        {
            string filepath = Path.Combine(new string[] { AppDomain.CurrentDomain.BaseDirectory, "Config", cfgname });
            return filepath;
        }


        internal string fullName = string.Empty;
        public ConfigFileBase(string filename)
        {
            fullName = GetConfigFile(filename);
            bool hasCfgFile = File.Exists(fullName);
            if (hasCfgFile == false)
            {
                StreamWriter writer = new StreamWriter(File.Create(fullName), Encoding.Default);
                writer.Close();
            }
        }


    }

    public class ServerConfig:ConfigFileBase
    {
        public ServerConfig(string filename)
            : base(filename)
        { 
        
        }

        public List<ServerNode> GetServerNodes()
        {
            List<ServerNode> nodelist = new List<ServerNode>();
            using (StreamReader reader = new StreamReader(fullName, Encoding.Default))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] rec = line.Split('|');
                    if (rec.Length == 3)
                    {
                        try
                        {
                            ServerNode node = new ServerNode();
                            node.Title = rec[0];
                            node.Address = rec[1];
                            node.Port = int.Parse(rec[2]);
                            nodelist.Add(node);
                        }
                        catch (Exception ex)
                        {
                            continue;
                        }
                    }
                }
            }
            return nodelist;
        }
    }


}
