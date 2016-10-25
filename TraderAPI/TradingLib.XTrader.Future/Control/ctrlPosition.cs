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
    public partial class ctrlPosition : UserControl
    {
        ILog logger = LogManager.GetLogger("ctrlPosition");
        FGrid positionGrid = null;
        public ctrlPosition()
        {
            InitializeComponent();

            button1.Paint += new PaintEventHandler(button1_Paint);

            this.positionGrid = new FGrid();
            this.positionGrid.Dock = DockStyle.Fill;
            this.panel2.Controls.Add(positionGrid);

            InitTable();
            BindToTable();
        }

        void button1_Paint(object sender, PaintEventArgs e)
        {
            Color f = Color.FromArgb(127, 157, 185);
            ControlPaint.DrawBorder(e.Graphics, positionGrid.ClientRectangle,
                 f, 1, ButtonBorderStyle.Solid,
                 f, 1, ButtonBorderStyle.Solid,
                 f, 1, ButtonBorderStyle.Solid,
                 f, 1, ButtonBorderStyle.Solid);
        }



        const string POSKEY = "持仓键";
        const string SYMBOL = "合约";
        const string SIDE = "方向";

        const string PROPERTY = "属性";
        const string SIZE = "持仓";

        const string SIZECANFLAT = "可用";
        const string AVGPRICE = "开仓均价";
        const string LOSSTARGET = "止损/数量";

        const string PROFITTARGET = "止盈/数量";
        const string FLAG = "投保";
        const string NAME = "名称";




        DataTable tb = new DataTable();
        BindingSource datasource = new BindingSource();
        
        /// <summary>
        /// 初始化数据表格
        /// </summary>
        private void InitTable()
        {
            tb.Columns.Add(POSKEY);
            tb.Columns.Add(SYMBOL);
            tb.Columns.Add(SIDE);

            tb.Columns.Add(PROPERTY);
            tb.Columns.Add(SIZE);
            tb.Columns.Add(SIZECANFLAT);
            tb.Columns.Add(AVGPRICE);
            tb.Columns.Add(LOSSTARGET);

            tb.Columns.Add(PROFITTARGET);
            tb.Columns.Add(FLAG);
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
            DataGridView grid  = positionGrid;
            //grid.TableElement.BeginUpdate();             
            //grid.MasterTemplate.Columns.Clear(); 
            datasource.DataSource = tb;
            //datasource.Sort = DATETIME + " DESC";
            grid.DataSource = datasource;

            grid.Columns[POSKEY].Visible = false;
            
            for (int i = 0; i < tb.Columns.Count; i++)
            {
                grid.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            ///ResetColumeSize();
        }
    }
}
