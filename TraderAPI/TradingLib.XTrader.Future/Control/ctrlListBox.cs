using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Common.Logging;

namespace TradingLib.XTrader.Future
{
    public partial class ctrlListBox : UserControl
    {

        ILog logger = LogManager.GetLogger("ctrlListBox");
        /// <summary>
        /// 对象选择事件
        /// </summary>
        public event Action<string> ItemSelected;

        public ctrlListBox()
        {
            InitializeComponent();

            fListBox1.ItemSelected +=new Action<string>(fListBox1_ItemSelected);
            vScrollBar1.ValueChanged += new EventHandler(vScrollBar1_ValueChanged);
            this.Load += new EventHandler(ctrlListBox_Load);
        }

        void vScrollBar1_ValueChanged(object sender, EventArgs e)
        {
            logger.Info("scroll Value:" + vScrollBar1.Value);
            int start = vScrollBar1.Value - fListBox1.ShowCount;
            fListBox1.StartIndex = start;
        }

        void ctrlListBox_Load(object sender, EventArgs e)
        {
            if (fListBox1.ShowCount < fListBox1.Items.Count)
            {
                vScrollBar1.Visible = true;
                vScrollBar1.LargeChange = 1;
                
                //最后一条显示的index
                vScrollBar1.Minimum = fListBox1.ShowCount;
                vScrollBar1.Maximum = fListBox1.Items.Count;
            }
            else
            {
                vScrollBar1.Visible = false;
            }
        }

        void  fListBox1_ItemSelected(string obj)
        {
            if (ItemSelected != null)
            {
                ItemSelected(obj);
            }
        }

        
        /// <summary>
        /// 对象集合
        /// </summary>
        public List<string> Items { get { return fListBox1.Items; } }

        /// <summary>
        /// 当前选中
        /// </summary>
        public string SelectedItem { get { return fListBox1.SelectedItem; } }

        public int ItemHeight { get { return fListBox1.ItemHeight; } }

    }
}
