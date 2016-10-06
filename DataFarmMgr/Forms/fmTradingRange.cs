using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;

namespace TradingLib.DataFarmManager
{
    public partial class fmTradingRange : Form
    {
        public fmTradingRange()
        {
            InitializeComponent();

            ManagerHelper.AdapterToIDataSource(startDay).BindDataSource(GetWeekDayArray());
            ManagerHelper.AdapterToIDataSource(endDay).BindDataSource(GetWeekDayArray());
            ManagerHelper.AdapterToIDataSource(settleFlag).BindDataSource(ManagerHelper.GetEnumValueObjects<QSEnumRangeSettleFlag>());
            startTime.Value = Util.ToDateTime(Util.ToTLDate(), 0);
            endTime.Value = Util.ToDateTime(Util.ToTLDate(), 0);

            btnSubmit.Click += new EventHandler(btnSubmit_Click);

        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.Close();
        }

        TradingRangeImpl _range = null;
        public void SetTradingRange(TradingRangeImpl range)
        {
            _range = range;
            startDay.SelectedValue = range.StartDay;
            startTime.Value = Util.ToDateTime(Util.ToTLDate(), range.StartTime);
            endDay.SelectedValue = range.EndDay;
            endTime.Value = Util.ToDateTime(Util.ToTLDate(), range.EndTime);
            settleFlag.SelectedValue = range.SettleFlag;
            marketClose.Checked = range.MarketClose;

        }

        public TradingRangeImpl CurrentRange
        {
            get
            {
                if (_range != null)
                {
                    _range.StartDay = (DayOfWeek)startDay.SelectedValue;
                    _range.StartTime = Util.ToTLTime(startTime.Value);
                    _range.EndDay = (DayOfWeek)endDay.SelectedValue;
                    _range.EndTime = Util.ToTLTime(endTime.Value);
                    _range.SettleFlag = (QSEnumRangeSettleFlag)settleFlag.SelectedValue;
                    _range.MarketClose = marketClose.Checked;
                    return _range;
                }
                else
                {
                    TradingRangeImpl range = new TradingRangeImpl();
                    range.StartDay = (DayOfWeek)startDay.SelectedValue;
                    range.StartTime = Util.ToTLTime(startTime.Value);
                    range.EndDay = (DayOfWeek)endDay.SelectedValue;
                    range.EndTime = Util.ToTLTime(endTime.Value);
                    range.SettleFlag = (QSEnumRangeSettleFlag)settleFlag.SelectedValue;
                    range.MarketClose = marketClose.Checked;
                    return range;
                }
            }
        }

        ArrayList GetWeekDayArray()
        {
            ArrayList list = new ArrayList();
            ValueObject<DayOfWeek> t1 = new ValueObject<DayOfWeek>();
            t1.Name = "星期一";
            t1.Value = DayOfWeek.Monday;
            list.Add(t1);

            ValueObject<DayOfWeek> t2 = new ValueObject<DayOfWeek>();
            t2.Name = "星期二";
            t2.Value = DayOfWeek.Tuesday;
            list.Add(t2);

            ValueObject<DayOfWeek> t3 = new ValueObject<DayOfWeek>();
            t3.Name = "星期三";
            t3.Value = DayOfWeek.Wednesday;
            list.Add(t3);

            ValueObject<DayOfWeek> t4 = new ValueObject<DayOfWeek>();
            t4.Name = "星期四";
            t4.Value = DayOfWeek.Thursday;
            list.Add(t4);

            ValueObject<DayOfWeek> t5 = new ValueObject<DayOfWeek>();
            t5.Name = "星期五";
            t5.Value = DayOfWeek.Friday;
            list.Add(t5);

            ValueObject<DayOfWeek> t6 = new ValueObject<DayOfWeek>();
            t6.Name = "星期六";
            t6.Value = DayOfWeek.Saturday;
            list.Add(t6);

            ValueObject<DayOfWeek> t7 = new ValueObject<DayOfWeek>();
            t7.Name = "星期日";
            t7.Value = DayOfWeek.Sunday;
            list.Add(t7);

            return list;

        }
    }
}
