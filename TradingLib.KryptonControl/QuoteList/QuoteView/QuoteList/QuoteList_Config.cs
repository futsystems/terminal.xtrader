using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using Common.Logging;

namespace TradingLib.KryptonControl
{
    public partial class ViewQuoteList
    {

        Dictionary<EnumQuoteListType, QuoteColumnConfigs> configMap = new Dictionary<EnumQuoteListType, QuoteColumnConfigs>();
        void InitConfig()
        {
            QuoteColumnConfigs tmp = null;

            tmp = new QuoteColumnConfigs(EnumQuoteListType.STOCK_CN);
            tmp.AddColumn(new ColumnConfig(EnumFileldType.INDEX, 2));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOL, 2));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOLNAME, 3));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.CHANGEPECT, 3));
            configMap.Add(EnumQuoteListType.STOCK_CN, tmp);


            tmp = new QuoteColumnConfigs(EnumQuoteListType.FUTURE_CN);
            tmp.AddColumn(new ColumnConfig(EnumFileldType.INDEX, 2));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.SYMBOL, 2));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LAST, 3));
            tmp.AddColumn(new ColumnConfig(EnumFileldType.LASTSIZE, 3));
            configMap.Add(EnumQuoteListType.FUTURE_CN, tmp);

        }

        void ApplyConfig(EnumQuoteListType type)
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
                        column.Width = cfg.Width * 50;
                    }
                }

                //重新获得可视列 集合
                visibleColumns = totalColumns.Where(c => c.Visible).OrderBy(c => c.Index).ToList();

            }
        }
    }
}
