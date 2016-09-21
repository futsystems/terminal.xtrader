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

        Dictionary<EnumQuoteListType, QuoteColumnConfigs> configMap = new Dictionary<EnumQuoteListType, QuoteColumnConfigs>();
        void InitConfig()
        {
            QuoteColumnConfigs tmp = null;

            tmp = new QuoteColumnConfigs(EnumQuoteListType.ALL);
            tmp.AddColumn(new ColumnConfig(EnumFileldType.INDEX, 4));
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
            tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOL,5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOLNAME,7));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.CHANGEPECT, 4));

            tmp.AddColumn(new ColumnConfig(EnumFileldType.LAST, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.CHANGE, 4));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.BID, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.ASK, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.VOL, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LASTSIZE, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.OPEN, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.HIGH, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LOW, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.PRECLOSE, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.AVGPRICE, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.BSIDE,7));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.SSIDE, 7));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.BIDSIZE, 5));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.ASKSIZE, 5));
            configMap.Add(EnumQuoteListType.STOCK_CN, tmp);


            tmp = new QuoteColumnConfigs(EnumQuoteListType.FUTURE_CN);
            tmp.AddColumn(new ColumnConfig(EnumFileldType.INDEX, 2));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOL, 2));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LAST, 3));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LASTSIZE, 3));
            configMap.Add(EnumQuoteListType.FUTURE_CN, tmp);

        }

        public void ApplyConfig(EnumQuoteListType type)
        {
            QuoteColumnConfigs configs = null;
            if (configMap.TryGetValue(type, out configs))
            {
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
    }
}
