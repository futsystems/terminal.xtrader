using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Common.Logging;

namespace TradingLib.XTrader.Control
{
    public partial class ViewQuoteList
    {
        EnumQuoteListType _blockType = EnumQuoteListType.ALL;
        public EnumQuoteListType BlockType { get { return _blockType; } }
        Dictionary<EnumQuoteListType, QuoteColumnConfigs> configMap = new Dictionary<EnumQuoteListType, QuoteColumnConfigs>();
        void InitConfig()
        {
            QuoteColumnConfigs tmp = null;

            tmp = new QuoteColumnConfigs(EnumQuoteListType.ALL);
            tmp.AddColumn(new ColumnConfig(EnumFileldType.INDEX, 4));
            if (!UIConstant.QuoteViewStdSumbolHidden)
                tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOL, 6));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOLNAME, 8));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LAST, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LASTSIZE, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.BID, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.BIDSIZE, 6));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.ASK, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.ASKSIZE, 6));

            configMap.Add(EnumQuoteListType.ALL, tmp);


            tmp = new QuoteColumnConfigs(EnumQuoteListType.STOCK_CN);
            tmp.AddColumn(new ColumnConfig(EnumFileldType.INDEX, 4));
            if (!UIConstant.QuoteViewStdSumbolHidden)
                tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOL,5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOLNAME,7));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.CHANGEPECT, 4));

            tmp.AddColumn(new ColumnConfig(EnumFileldType.LAST, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.CHANGE, 4));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.BID, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.ASK, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.VOL, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LASTSIZE, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.TURNOVERRATE, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.OPEN, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.HIGH, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LOW, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.PRECLOSE, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.PE, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.AMOUNT, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.AVGPRICE, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.BSIDE,7));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.SSIDE, 7));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.BIDSIZE, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.ASKSIZE, 5));
            configMap.Add(EnumQuoteListType.STOCK_CN, tmp);


            tmp = new QuoteColumnConfigs(EnumQuoteListType.FUTURE_CN);
            tmp.AddColumn(new ColumnConfig(EnumFileldType.INDEX, 2));
            if (!UIConstant.QuoteViewStdSumbolHidden)
                tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOL, 2));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LAST, 3));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LASTSIZE, 3));
            configMap.Add(EnumQuoteListType.FUTURE_CN, tmp);

            tmp = new QuoteColumnConfigs(EnumQuoteListType.FUTURE_IQFeed);
            tmp.AddColumn(new ColumnConfig(EnumFileldType.INDEX, 4));
            if (!UIConstant.QuoteViewStdSumbolHidden)
                tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOL, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOLNAME, 7));

            tmp.AddColumn(new ColumnConfig(EnumFileldType.LAST, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LASTSIZE, 3));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.BID, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.ASK, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.BIDSIZE, 7));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.ASKSIZE, 7));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.VOL, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.PREOI, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.CHANGE, 4));
            //tmp.AddColumn(new ColumnConfig(EnumFileldType.PRECLOSE, 4));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.OPEN, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.HIGH, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LOW, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.CHANGEPECT, 4));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.PRESETTLEMENT, 4));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.TIME, 6));

            configMap.Add(EnumQuoteListType.FUTURE_IQFeed, tmp);


        }

         
        public void ApplyConfig(EnumQuoteListType type)
        {
            
            QuoteColumnConfigs configs = null;
            if (configMap.TryGetValue(type, out configs))
            {
                _blockType = type;
                ColumnConfig cfg = null;
                foreach (var column in totalColumns)
                {
                    column.Visible = false;
                    column.Index = -1;

                    cfg = configs[column.FieldType];
                    if (cfg != null)
                    {
                        column.Visible = true;
                        column.Index = cfg.Index;
                        column.Width = (int)Math.Ceiling(cfg.Width * this.DefaultQuoteStyle.FontWidth);
                        column.Title = GetColumnTitle(type,column);
                        
                    }

                }

                //重新获得可视列 集合
                visibleColumns = totalColumns.Where(c => c.Visible).OrderBy(c => c.Index).ToList();
            }


            //计算列起点 总宽等参数
            CalcColunmStartX();
            //重置所有绘图区域
            this.ResetRect();
        }


        /// <summary>
        /// 根据不同类型的面板显示类型 调整列头数据
        /// </summary>
        /// <param name="type"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        string GetColumnTitle(EnumQuoteListType type,QuoteColumn column)
        {
            switch (type)
            {
                case EnumQuoteListType.STOCK_CN:
                    {
                        if (column.FieldType == EnumFileldType.LAST) return "现价";
                        if (column.FieldType == EnumFileldType.LASTSIZE) return "现量";
                        if (column.FieldType == EnumFileldType.VOL) return "总量";
                        if (column.FieldType == EnumFileldType.OPEN) return "今开";
                        if (column.FieldType == EnumFileldType.SYMBOL) return "代码";

                        break;
                    }
                case EnumQuoteListType.ALL:
                    {
                        if (column.FieldType == EnumFileldType.SYMBOL) return "代码";

                        break;
                    }
                default:
                    break;
            }
            //使用默认Title
            return QuoteColumn.GetEnumDescription(column.FieldType);
        }


    }
}
