using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using Common.Logging;

namespace TradingLib.XTrader.Future
{
    public partial class ctrlTrade : UserControl
    {
        ILog logger = LogManager.GetLogger("ctrlPosition");
        FGrid tradeGrid = null;
        public ctrlTrade()
        {
            InitializeComponent();

            //checkButton.Paint += new PaintEventHandler(button1_Paint);

            this.tradeGrid = new FGrid();
            this.tradeGrid.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(tradeGrid);


            InitTable();
            BindToTable();
        }

        //void button1_Paint(object sender, PaintEventArgs e)
        //{
        //    Color f = Color.FromArgb(127, 157, 185);
        //    ControlPaint.DrawBorder(e.Graphics, positionGrid.ClientRectangle,
        //         f, 1, ButtonBorderStyle.Solid,
        //         f, 1, ButtonBorderStyle.Solid,
        //         f, 1, ButtonBorderStyle.Solid,
        //         f, 1, ButtonBorderStyle.Solid);
        //}




        const string TIME = "成交时间";
        const string SYMBOL = "合约";

        const string SIDE = "买卖";
        const string FLAG = "开平";

        const string PRICE = "成交价格";
        const string SIZE = "手数";
        const string ORDERID = "委托号";

        const string NAME = "名称";
        const string SYSID = "系统号";
        const string TRADERID = "成交号";




        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();
        
        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            tb.Columns.Add(TIME);
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(SIDE);

            tb.Columns.Add(FLAG);
            tb.Columns.Add(PRICE);
            tb.Columns.Add(SIZE);
            tb.Columns.Add(ORDERID);
            tb.Columns.Add(NAME);

            tb.Columns.Add(SYSID);
            tb.Columns.Add(TRADERID);
        }

        //void ResetColumeSize()
        //{
        //    ComponentFactory.Krypton.Toolkit.KryptonDataGridView grid = positionGrid;

        //    grid.Columns[SYMBOL].Width = 100;
        //    grid.Columns[SYMBOLNAME].Width = 100;
        //    grid.Columns[TOTALSIZE].Width = 60;
        //    grid.Columns[AVABILESIZE].Width = 60;
        //    grid.Columns[FRONZENSIZE].Width = 60;

        //    grid.Columns[POSITIONPROFIT].Width = 80;
        //    grid.Columns[AVGPRICE].Width = 80;
        //    grid.Columns[LASTPRICE].Width = 80;
        //    grid.Columns[POSITIONPROFITPECT].Width = 80;
        //    grid.Columns[MARKETVALUE].Width = 120;




        //}

        /// <summary>
        /// 绑定数据表格到grid
        /// </summary>
        private void BindToTable()
        {
            DataGridView grid = tradeGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            //datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            for (int i = 0; i < tb.Columns.Count; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            ///ResetColumeSize();
        }
    }
}
