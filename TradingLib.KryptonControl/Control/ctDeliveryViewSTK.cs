using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Common.Logging;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;


namespace TradingLib.KryptonControl
{
    public partial class ctDeliveryViewSTK : UserControl
    {
        ILog logger = LogManager.GetLogger("ctDeliveryViewSTK");
        public ctDeliveryViewSTK()
        {
            InitializeComponent();
            InitTable();
            BindToTable();

        }

        /// <summary>
        /// 获得合约名称
        /// </summary>
        /// <param name="fill"></param>
        /// <returns></returns>
        string GetSymbolName(Trade fill)
        {
            if (fill.oSymbol != null) return fill.oSymbol.GetName();
            return fill.Symbol;
        }

        string FormatPrice(Trade fill, decimal val)
        {
            if (fill.oSymbol != null) return val.ToFormatStr(fill.oSymbol);
            SecurityFamily sec = CoreService.BasicInfoTracker.GetSecurity(fill.SecurityCode);
            if (sec == null) return val.ToFormatStr();
            return val.ToFormatStr(sec);
        }

        decimal GetAmount(Trade fill)
        {
            if (fill.oSymbol != null) return fill.GetAmount();
            SecurityFamily sec = CoreService.BasicInfoTracker.GetSecurity(fill.SecurityCode);
            if (sec == null) return Math.Abs(fill.xSize) * fill.xPrice;
            return Math.Abs(fill.xSize) * fill.xPrice*sec.Multiple;
        }

        public void GotFill(Trade fill)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<Trade>(GotFill), new object[] { fill });
            }
            else
            {

                DataRow r = tb.Rows.Add(fill.xDate);
                int i = tb.Rows.Count - 1;//得到新建的Row号
                tb.Rows[i][FILLDATE] = fill.xDate;
                tb.Rows[i][SYMBOL] = fill.Symbol;
                tb.Rows[i][SYMBOLNAME] = GetSymbolName(fill);
                tb.Rows[i][REMARK] = "";
                tb.Rows[i][SIZE] = Math.Abs(fill.xSize);
                tb.Rows[i][PRICE] = FormatPrice(fill, fill.xPrice);
                tb.Rows[i][AMOUNT] = GetAmount(fill).ToFormatStr();
                tb.Rows[i][AMOUNTEX] = 0;
                tb.Rows[i][COMMISSION] = fill.Commission.ToFormatStr();
                tb.Rows[i][STAMPTAX] = fill.StampTax.ToFormatStr();
                tb.Rows[i][TRANSFERFEE] = fill.TransferFee.ToFormatStr();
                tb.Rows[i][CONTRACTID] = fill.id;
            }
        }


        const string FILLDATE = "成交日期";
        const string SYMBOL = "证券代码";
        const string SYMBOLNAME = "证券名称";
        const string REMARK = "摘要";
        const string SIZE = "成交数量";
        const string PRICE = "成交均价";
        const string AMOUNT = "成交金额";
        const string AMOUNTEX = "发生金额";
        const string COMMISSION = "手续费";
        const string STAMPTAX = "印花税";
        const string TRANSFERFEE = "过户费";
        const string CONTRACTID = "合同编号";



        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();

        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            tb.Columns.Add(FILLDATE);
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(SYMBOLNAME);
            tb.Columns.Add(REMARK);
            tb.Columns.Add(SIZE);
            tb.Columns.Add(PRICE);

            tb.Columns.Add(AMOUNT);
            tb.Columns.Add(AMOUNTEX);
            tb.Columns.Add(COMMISSION);
            tb.Columns.Add(STAMPTAX);
            tb.Columns.Add(TRANSFERFEE);
            tb.Columns.Add(CONTRACTID);

        }

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            DataGridView grid = deliveryGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            //datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            //grid.Columns[ORDERID].Visible = false;
            //grid.Columns[TRADEID].Visible = false;
            //grid.Columns[DATETIME].Visible = false;

            //set width
            //grid.Columns[SYMBOL].Width = 80;
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        /// <summary>
        /// 清空表格内容
        /// </summary>
        public void Clear()
        {

            deliveryGrid.DataSource = null;
            tb.Rows.Clear();
            BindToTable();
        }

    }
}
