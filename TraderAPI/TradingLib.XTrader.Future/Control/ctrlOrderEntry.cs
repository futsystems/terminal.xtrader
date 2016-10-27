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
    public partial class ctrlOrderEntry : UserControl, TradingLib.API.IEventBinder
    {
        ILog logger = LogManager.GetLogger("OrderEntry");

        ctrlListBox priceBox;
        ctrlNumBox sizeBox;

        QSEnumOffsetFlag _currentOffsetFlag = QSEnumOffsetFlag.UNKNOWN;

        Symbol _symbol = null;
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


            btnBuy.Click += new EventHandler(btnBuy_Click);
            btnSell.Click += new EventHandler(btnSell_Click);


            CoreService.EventIndicator.GotOrderEvent += new Action<Order>(EventIndicator_GotOrderEvent);
            CoreService.EventIndicator.GotErrorOrderEvent += new Action<Order, RspInfo>(EventIndicator_GotErrorOrderEvent);
            CoreService.EventCore.RegIEventHandler(this);
        }

        void EventIndicator_GotErrorOrderEvent(Order arg1, RspInfo arg2)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<Order, RspInfo>(EventIndicator_GotErrorOrderEvent), new object[] { arg1, arg2 });
            }
            else
            {
                if (arg1.RequestID == _orderInesertId)
                {
                    btnBuy.Enabled = true;
                    btnSell.Enabled = true;
                }
            }
        }

        void EventIndicator_GotOrderEvent(Order obj)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<Order>(EventIndicator_GotOrderEvent), new object[] { obj });
            }
            else
            {
                if (obj.RequestID == _orderInesertId)
                {
                    btnBuy.Enabled = true;
                    btnSell.Enabled = true;
                }

                if (obj.Status == QSEnumOrderStatus.Opened)
                {
                    //QryMaxOrderVol();
                    //QryAccountFinance();
                }
            }
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

        void inputSymbol_TextChanged(object sender, EventArgs e)
        {
            if (inputSymbol.Text.Length >= 4)
            {
                Symbol symbol = CoreService.BasicInfoTracker.Symbols.Where(sym => sym.Symbol == inputSymbol.Text).FirstOrDefault();
                if (symbol != null)
                {
                    logger.Info(string.Format("Symbol:{0} Selected", symbol.Symbol));
                    _symbol = symbol;
                }
            }
        }

        void inputSymbol_SymbolSelected(Symbol obj)
        {
            logger.Info(string.Format("Symbol:{0} Selected", obj.Symbol));
            _symbol = obj;
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
        /// <summary>
        /// 禁止切换Tab
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            e.Cancel = true;
        }



        #region 提交委托
        int _orderInesertId = 0;

        void btnSell_Click(object sender, EventArgs e)
        {
            SendOrder(false);
        }

        void btnBuy_Click(object sender, EventArgs e)
        {
            SendOrder(true);
        }

        /// <summary>
        /// 从加入输入控件获得价格
        /// </summary>
        /// <param name="side"></param>
        /// <param name="price"></param>
        /// <returns></returns>
        bool GetInputPrice(bool side,out decimal price)
        {
            price = 0;
            
            if (inputPrice.IsTxtMode)
            {
                Tick snapshot = CoreService.TradingInfoTracker.TickTracker[_symbol.Exchange, _symbol.Symbol];
                if (snapshot == null) return false;
                switch (inputPrice.TxtValue)
                {
                    case "对手价":
                        {
                            price = side ? snapshot.AskPrice : snapshot.BidPrice;
                            return true;
                        }
                    case "对手价超一":
                        {
                            price = side ? (snapshot.AskPrice + _symbol.SecurityFamily.PriceTick) : (snapshot.BidPrice - _symbol.SecurityFamily.PriceTick);
                            return true;
                        }
                    case "对手价超二":
                        {
                            price = side ? (snapshot.AskPrice + 2 * _symbol.SecurityFamily.PriceTick) : (snapshot.BidPrice - 2 * _symbol.SecurityFamily.PriceTick);
                            return true;
                        }
                    case "挂单价":
                        {
                            price = side ? snapshot.BidPrice : snapshot.AskPrice;
                            return true;
                        }
                    case "最新价":
                        {
                            price = snapshot.Trade;
                            return true;
                        }
                    case "市价":
                        {
                            price = 0;
                            return true;
                        }
                    case "涨停价":
                        {
                            price = snapshot.UpperLimit;
                            return true;
                        }
                    case "跌停价":
                        {
                            price = snapshot.LowerLimit;
                            return true;
                        }
                    default:
                        return false;
                }
            }
            else
            {
                //根据输入的价格返回
                if (!decimal.TryParse(inputPrice.TxtValue, out price)) return false;
                return true;
            }
        }

        string GetOffsetString()
        {
            switch (_currentOffsetFlag)
            {
                case QSEnumOffsetFlag.OPEN: return "开仓";
                case QSEnumOffsetFlag.CLOSE: return "平仓";
                case QSEnumOffsetFlag.CLOSETODAY: return "平今";
                default:
                    return "";
            }
        }
        string GetPriceString(decimal price)
        {
            if (price == 0) return "市价";
            return "限价:" + price.ToFormatStr(_symbol.SecurityFamily.GetPriceFormat());
        }
        void SendOrder(bool side)
        {
            if (_symbol == null)
            {
                MessageBox.Show("请选择交易合约", "委托参数错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int size = 0;
            if (!int.TryParse(inputSize.TxtValue, out size)) size = 0;
            if (size == 0)
            {
                MessageBox.Show("委托数量需大于零", "委托参数错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            

            decimal price=0;
            bool priceok = GetInputPrice(side, out price);
            if (!priceok)
            {
                MessageBox.Show("委托价格异常", "委托参数错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Order order = new OrderImpl();
            order.Symbol = _symbol.Symbol;
            order.Exchange = _symbol.Exchange;

            order.Size = size;
            order.Side = side;
            order.LimitPrice = price;

            //以价格:00 买入1手 
            string msg = "以价格:{0} {1}{4}{2}手 {3}".Put(GetPriceString(order.LimitPrice), order.Side ? "买入" : "卖出", order.UnsignedSize, _symbol.GetName(), GetOffsetString());
            if (MessageBox.Show(msg, "确认提交委托?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                _orderInesertId = CoreService.TLClient.ReqOrderInsert(order);
                btnBuy.Enabled = false;
                btnSell.Enabled = false;
            }
        }
        #endregion


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











        #region IEventBinder
        public void OnInit()
        {

            //初始化合约选择控件
            foreach (var g in CoreService.BasicInfoTracker.Symbols.GroupBy(sym => sym.SecurityFamily))
            {
                SymbolSet symbolset = new SymbolSet(string.Format("{0}   {1}", g.Key.Code, g.Key.Name));
                foreach (var symbol in g.OrderBy(sym => sym.Month))
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

        #endregion

    }
}
