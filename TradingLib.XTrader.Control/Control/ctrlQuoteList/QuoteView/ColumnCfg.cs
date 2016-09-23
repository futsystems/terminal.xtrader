using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.XTrader.Control
{
 
    /// <summary>
    /// 列配置信息
    /// 用于设置某个列信息
    /// </summary>
    public class ColumnConfig
    {

        public ColumnConfig(EnumFileldType field, int width)
        {
            this.Index = -1;
            this.Field = field;
            this.Width = width;
        }
        /// <summary>
        /// 序号
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public int Width { get; set; }

        /// <summary>
        /// 列
        /// </summary>
        public EnumFileldType Field { get; set; }
    }

    /// <summary>
    /// 某个Quote类别对应的列配置信息
    /// 用于指定列的顺序 宽度 等
    /// 1.序列化配置到文本文件
    /// 2.从文本文件反序列化到对象
    /// </summary>
    public class QuoteColumnConfigs
    {
        SortedDictionary<int, ColumnConfig> columnsConfig = new SortedDictionary<int, ColumnConfig>();

        public QuoteColumnConfigs(EnumQuoteListType type)
        {
            this.QuoteType = type;

        }

        /// <summary>
        /// 返回某个列的配置
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public ColumnConfig this[EnumFileldType type]
        {
            get
            {
                return columnsConfig.Values.FirstOrDefault(cfg => cfg.Field == type);
            }
        }
        /// <summary>
        /// 报价类别
        /// </summary>
        public EnumQuoteListType QuoteType
        {
            get;
            private set;
        }

        int _count = 0;
        public int Count
        {
            get { return _count; }
        }
        /// <summary>
        /// 添加某列
        /// 列序号自动维护
        /// </summary>
        /// <param name="cfg"></param>
        public void AddColumn(ColumnConfig cfg)
        {
            if(columnsConfig.Values.Any(c=>c.Field==cfg.Field))
            {
                throw new Exception("duplicate column");
            }
            cfg.Index = _count;
            columnsConfig.Add(cfg.Index,cfg);
            _count++;
        }

        /// <summary>
        /// 所有列配置
        /// </summary>
        IEnumerable<ColumnConfig> ColumnConfigs
        {
            get
            {
                return columnsConfig.Values;
            }
        }
    }
}
