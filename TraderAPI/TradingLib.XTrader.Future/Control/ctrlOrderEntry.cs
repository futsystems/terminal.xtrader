using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.TraderCore;
using Common.Logging;

namespace TradingLib.XTrader.Future.Control
{
    public partial class ctrlOrderEntry : UserControl,TradingLib.API.IEventBinder
    {
        ctrlListBox priceBox;
        ctrlNumBox sizeBox;

        QSEnumOffsetFlag _currentOffsetFlag = QSEnumOffsetFlag.UNKNOWN;

        ILog logger = LogManager.GetLogger("OrderEntry");
        public ctrlOrderEntry()
        {
            InitializeComponent();

            InitControl();

            WireEvent();
            
        }

        void WireEvent()
        {
            
            tabControl1.Selecting += new TabControlCancelEventHandler(tabControl1_Selecting);

            //合约选择控件
            inputSymbol.SymbolSelected += new Action<Symbol>(inputSymbol_SymbolSelected);//下拉选择
            inputSymbol.TextChanged += new EventHandler(inputSymbol_TextChanged);//合约输入

            inputFlagClose.Click += new EventHandler(inputFlagClose_Click);
            inputFlagCloseToday.Click += new EventHandler(inputFlagCloseToday_Click);
            inputFlagOpen.Click += new EventHandler(inputFlagOpen_Click);
            inputFlagAuto.CheckedChanged += new EventHandler(inputFlagAuto_CheckedChanged);

            CoreService.EventCore.RegIEventHandler(this);
        }

        void inputSymbol_TextChanged(object sender, EventArgs e)
        {
            if (inputSymbol.Text.Length >= 4)
            {
                Symbol symbol = CoreService.BasicInfoTracker.Symbols.Where(sym => sym.Symbol == inputSymbol.Text).FirstOrDefault();
                if (symbol != null)
                {
                    logger.Info(string.Format("Symbol:{0} Selected", symbol.Symbol));
                }
            }
        }

        void inputSymbol_SymbolSelected(Symbol obj)
        {
            logger.Info(string.Format("Symbol:{0} Selected", obj.Symbol));
        }


        #region OrderOffsetFlag
        void inputFlagAuto_CheckedChanged(object sender, EventArgs e)
        {
            if (inputFlagAuto.Checked)
            {
                _currentOffsetFlag = QSEnumOffsetFlag.UNKNOWN;//服务端自动判定
                inputFlagClose.Enabled = false;
                inputFlagOpen.Enabled = false;
                inputFlagCloseToday.Enabled = false;
                inputFlagClose.Checked = false;
                inputFlagOpen.Checked = false;
                inputFlagCloseToday.Checked = false;

            }
            else
            {
                _currentOffsetFlag = QSEnumOffsetFlag.OPEN;
                inputFlagClose.Enabled = true;
                inputFlagOpen.Enabled = true;
                inputFlagCloseToday.Enabled = true;
                inputFlagClose.Checked = false;
                inputFlagOpen.Checked = true;
                inputFlagCloseToday.Checked = false;
            }
        }
        void inputFlagOpen_Click(object sender, EventArgs e)
        {
            _currentOffsetFlag = QSEnumOffsetFlag.OPEN;
        }

        void inputFlagCloseToday_Click(object sender, EventArgs e)
        {
            _currentOffsetFlag = QSEnumOffsetFlag.CLOSETODAY;
        }

        void inputFlagClose_Click(object sender, EventArgs e)
        {
            _currentOffsetFlag = QSEnumOffsetFlag.CLOSE;
        }
        #endregion



        /// <summary>
        /// 禁止切换Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = true;
        }


        
        void InitControl()
        {
            inputArbFlag.SelectedIndex = 0;
            //inputArbFlag.DrawMode = DrawMode.OwnerDrawVariable;
            //inputArbFlag.ItemHeight = 16;
            //inputArbFlag.IntegralHeight = false;
            inputSymbol.DropDownSizeMode = SizeMode.UseControlSize;

            ListBox f = new ListBox();
            priceBox = new ctrlListBox();
            priceBox.ItemSelected += new Action<string>(box_ItemSelected);
            priceBox.Height = 80;
            priceBox.Width = 0;//默认使用DropDownContrl尺寸,设置Width为0后 则使用触发控件的Width

            priceBox.Items.Add("对手价");
            priceBox.Items.Add("对手价超一");
            priceBox.Items.Add("对手价超二");
            priceBox.Items.Add("挂单价");
            priceBox.Items.Add("最新价");
            priceBox.Items.Add("市价");
            priceBox.Items.Add("涨停价");
            priceBox.Items.Add("跌停价");

            inputPrice.DropDownSizeMode = SizeMode.UseControlSize;
            inputPrice.DropDownControl = priceBox;

            sizeBox = new ctrlNumBox();
            sizeBox.NumSelected += new Action<int>(sizeBox_NumSelected);
            inputSize.DropDownControl = sizeBox;
            inputSize.DropDownSizeMode = SizeMode.UseControlSize;

        }

        void sizeBox_NumSelected(int obj)
        {
            inputSize.SetValue(obj.ToString());
            inputSize.HideDropDown();
        }

        void box_ItemSelected(string obj)
        {
            inputPrice.SetTxtVal(obj);
            inputPrice.HideDropDown();
        }


        public void OnInit()
        {
            
            //初始化合约选择控件
            foreach (var g in CoreService.BasicInfoTracker.Symbols.GroupBy(sym => sym.SecurityFamily))
            {
                SymbolSet symbolset = new SymbolSet(string.Format("{0}   {1}", g.Key.Code, g.Key.Name));
                foreach (var symbol in g.OrderBy(sym=>sym.Month))
                {
                    symbolset.AddSymbol(symbol);
                }
                inputSymbol.AddSymbolSet(symbolset);
            }
            inputSymbol.InitListBox();

            
        }

        public void OnDisposed()
        { 
        
        }
    }
}
