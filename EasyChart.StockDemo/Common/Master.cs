using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TradingLib.API;
using TradingLib.Common;


namespace Easychart.Finance.DataProvider
{
    public class DataMaster
    {
        
        /// <summary>
        /// 合约名称 20
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// 交易所
        /// 交易所-合约 形成唯一的键值对
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// 名称 50
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 合约类别 期货类储存连续合约,股票/外汇等没有到期日期的储存标准合约
        /// 期货合约按月份进行储存形成01-12 12个数据序列,同时会合并主力连续,当月连续 连三 等相关衍生数据
        /// </summary>
        public QSEnumSymbolType SymbolType { get; set; }

        /// <summary>
        /// 间隔类别
        /// </summary>
        public BarInterval IntervalType { get; set; }

        /// <summary>
        /// 间隔数
        /// </summary>
        public int Interval { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public long StartTime { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public long EndTime { get; set; }

        /// <summary>
        /// 上次修改时间
        /// </summary>
        public long ModifiedTime{ get; set; }

        /// <summary>
        /// 文件序号
        /// FXXX.DAT储存具体的数据
        /// 通过Master建立所有数据缩影
        /// </summary>
        public int FN { get; set; }


        public override string ToString()
        {
            return "{0}";
        }

        /// <summary>
        /// 键值
        /// </summary>
        public string Key
        { 
            get{
                return "{0}-{1}-{2}-{3}".Put(this.Exchange, this.Symbol, this.Interval, this.IntervalType);
            }
        }

        private static byte[] GetEmptyByteArray(int Count, byte Fill)
        {
            byte[] array = new byte[Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Fill;
            }
            return array;
        }

        private static byte[] StringToBytes(string s, int Count, byte Fill)
        {
            byte[] emptyByteArray = GetEmptyByteArray(Count, Fill);
            Encoding.UTF8.GetBytes(s, 0, s.Length, emptyByteArray, 0);
            return emptyByteArray;
        }

        private static string TrimToZero(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\0')
                {
                    return s.Substring(0, i);
                }
            }
            return s;
        }

        public static void Write(BinaryWriter binaryWriter, DataMaster master)
        {
            binaryWriter.Write(StringToBytes(master.Exchange, 20, 0));//20
            binaryWriter.Write(StringToBytes(master.Symbol, 20, 0));//20
            binaryWriter.Write(StringToBytes(master.Name,50, 0));//50
            binaryWriter.Write((int)master.SymbolType);//1
            //binaryWriter.Write((byte)master.TimeFrame);//1
            binaryWriter.Write((int)master.IntervalType);//1

            binaryWriter.Write(master.Interval);//4
            binaryWriter.Write(master.StartTime);//8
            binaryWriter.Write(master.EndTime);//8
            binaryWriter.Write(master.ModifiedTime);//8
            binaryWriter.Write(master.FN);//4
        }

        public static DataMaster Read(BinaryReader binaryReader)
        {
            DataMaster m = new DataMaster();
            m.Exchange = TrimToZero(Encoding.UTF8.GetString(binaryReader.ReadBytes(20)));
            m.Symbol = TrimToZero(Encoding.UTF8.GetString(binaryReader.ReadBytes(20)));
            m.Name = TrimToZero(Encoding.UTF8.GetString(binaryReader.ReadBytes(50)));

            m.SymbolType = (QSEnumSymbolType)binaryReader.ReadInt32();
            //m.TimeFrame = (EnumBarTimeFrame)binaryReader.ReadByte();
            m.IntervalType = (BarInterval)binaryReader.ReadInt32();

            m.Interval = binaryReader.ReadInt32();
            m.StartTime = binaryReader.ReadInt64();
            m.EndTime = binaryReader.ReadInt64();
            m.ModifiedTime = binaryReader.ReadInt64();
            m.FN = binaryReader.ReadInt32();

            return m;
        }
    }
}
