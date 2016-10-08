using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.DataCore;


namespace TradingLib.DataFarmManager
{
    public partial class fmBarDataEdit : Form
    {
        public fmBarDataEdit()
        {
            InitializeComponent();
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            btnDelete.Click += new EventHandler(btnDelete_Click);
        }

        void btnDelete_Click(object sender, EventArgs e)
        {
            if (_bar != null)
            {

                if (MessageBox.Show("确认删除Bar数据", "删除Bar数据", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    string cmd = TradingLib.Mixins.Json.JsonMapper.ToJson(
                    new
                    {
                        Exchange=_symbol.Exchange,
                        Symbol=_symbol.Symbol,
                        IntervalType = BarInterval.CustomTime,
                        Interval=60,
                        ID=new int[]{_bar.ID},
                        
                    });
                    DataCoreService.DataClient.ReqContribRequest("DataFarm", "DeleteBar", cmd);
                }
            }
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_bar != null)
            {
                if (MessageBox.Show("确认更新Bar数据", "更新Bar数据", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    _bar.Exchange = _symbol.Exchange;
                    _bar.Symbol = _symbol.Symbol;
                    _bar.Open = (double)open.Value;
                    _bar.High = (double)high.Value;
                    _bar.Low = (double)low.Value;
                    _bar.Close = (double)close.Value;
                    _bar.Volume = (int)vol.Value;
                    _bar.OpenInterest = (int)oi.Value;
                    _bar.TradeCount = (int)xCount.Value;
                    _bar.TradingDay = (int)tradingday.Value;
                    string barstr = TradingLib.Mixins.Json.JsonMapper.ToJson(_bar);
                    DataCoreService.DataClient.ReqContribRequest("DataFarm", "UpdateBar", barstr);
                }
            }
            else
            { 
                
            }
            
        }


        BarImpl _bar = null;
        public void SetBar(BarImpl bar)
        {
            this.Text = "修改Bar数据";
            _bar = bar;

            barId.Text = bar.ID.ToString();
            symbol.Text = bar.Symbol;
            freq.Text = string.Format("{0}-{1}", bar.Interval, bar.IntervalType);
            datetime.Text = bar.EndTime.ToString();

            open.Value = (decimal)bar.Open;
            high.Value = (decimal)bar.High;
            low.Value = (decimal)bar.Low;
            close.Value = (decimal)bar.Close;
            vol.Value = bar.Volume;
            oi.Value = bar.OpenInterest;
            xCount.Value = bar.TradeCount;
            tradingday.Value = bar.TradingDay;

        }

        Symbol _symbol = null;
        public void SetSymbol(Symbol symbol)
        {
            _symbol = symbol;
        }
    }
}
