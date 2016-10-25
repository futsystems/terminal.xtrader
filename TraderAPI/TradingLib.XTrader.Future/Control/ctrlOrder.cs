using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using Common.Logging;

namespace TradingLib.XTrader.Future
{
    public partial class ctrlOrder : UserControl
    {
        ILog logger = LogManager.GetLogger("ctrlPosition");
        FGrid orderGrid = null;
        public ctrlOrder()
        {
            InitializeComponent();

            //checkButton.Paint += new PaintEventHandler(button1_Paint);

            this.orderGrid = new FGrid();
            this.orderGrid.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(orderGrid);


            InitTable();
            BindToTable();

            WireEvent();
        }

        void WireEvent()
        {
            this.SizeChanged += new EventHandler(ctrlOrder_SizeChanged);
            btnAll.Click += new EventHandler(btnAll_Click);
            btnCancel.Click += new EventHandler(btnCancel_Click);
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            btnCancel.Checked = true;
            btnAll.Checked = false;
        }

        void btnAll_Click(object sender, EventArgs e)
        {
            btnCancel.Checked = false;
            btnAll.Checked = true;
        }
        void ctrlOrder_SizeChanged(object sender, EventArgs e)
        {

            this.btnAll.Height = (this.Height-2) / 2;
            this.btnCancel.Location = new Point(0, this.btnAll.Height+2);
            this.btnCancel.Height = this.Height - this.btnAll.Height - 2;
        }

        void button1_Paint(object sender, PaintEventArgs e)
        {
            Color f = Color.FromArgb(127, 157, 185);
            ControlPaint.DrawBorder(e.Graphics, orderGrid.ClientRectangle,
                 f, 1, ButtonBorderStyle.Solid,
                 f, 1, ButtonBorderStyle.Solid,
                 f, 1, ButtonBorderStyle.Solid,
                 f, 1, ButtonBorderStyle.Solid);
        }



        const string ID = "委托键";
        const string TIME = "委托时间";
        const string SYMBOL = "合约";

        const string SIDE = "买卖";
        const string FLAG = "开平";

        const string ORDERPRICE = "委托价格";
        const string TOTALSIZE = "委手";
        const string FILLSIZE = "成手";

        const string STATUS = "状态";
        const string COMMENT = "备注";
        const string SPECULATE = "投保";
        const string ORDERID = "委托号";
        const string NAME = "名称";




        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();
        
        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            tb.Columns.Add(ID);
            tb.Columns.Add(TIME);
            tb.Columns.Add(SYMBOL);

            tb.Columns.Add(SIDE);
            tb.Columns.Add(FLAG);
            tb.Columns.Add(ORDERPRICE);
            tb.Columns.Add(TOTALSIZE);
            tb.Columns.Add(FILLSIZE);

            tb.Columns.Add(STATUS);
            tb.Columns.Add(COMMENT);
            tb.Columns.Add(SPECULATE);
            tb.Columns.Add(ORDERID);
            tb.Columns.Add(NAME);


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
            DataGridView grid = orderGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            //datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            grid.Columns[ID].Visible = false;
            
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            ///ResetColumeSize();
        }
    }
}
