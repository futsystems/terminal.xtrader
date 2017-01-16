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
using TradingLib.DataCore;


namespace TradingLib.DataFarmManager
{
    public partial class fmExchangeEdit : Form
    {
        public fmExchangeEdit()
        {
            InitializeComponent();

            this.Text = "添加交易所";

            ManagerHelper.AdapterToIDataSource(cbCountry).BindDataSource(ManagerHelper.GetEnumValueObjects<Country>());
            ManagerHelper.AdapterToIDataSource(cbTimeZone).BindDataSource(ManagerHelper.GetTimeZoneList());

            this.Load += new EventHandler(fmExchangeEdit_Load);
            this.FormClosing += new FormClosingEventHandler(fmExchangeEdit_FormClosing);
            
        }

        void fmExchangeEdit_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataCoreService.EventContrib.UnRegisterCallback("DataFarm", "QryCalendarList", OnQryCalendarItems);
        }

        void fmExchangeEdit_Load(object sender, EventArgs e)
        {
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            DataCoreService.EventContrib.RegisterCallback("DataFarm", "QryCalendarList", OnQryCalendarItems);

            DataCoreService.DataClient.ReqContribRequest("DataFarm", "QryCalendarList", "");
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_exchange != null)
            {
                if (MessageBox.Show("确认更新交易所?","更新",MessageBoxButtons.YesNo,MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    _exchange.Name = this.name.Text;
                    _exchange.Title = this.title.Text;
                    _exchange.Country = (Country)this.cbCountry.SelectedValue;
                    _exchange.Calendar = this.calendar.SelectedValue.ToString();
                    _exchange.TimeZoneID = this.cbTimeZone.SelectedValue.ToString();
                    _exchange.CloseTime = Util.ToTLTime(this.closeTime.Value);
                    DataCoreService.DataClient.ReqUpdateExchange(_exchange);
                    this.Close();
                }
            }
            else
            {
                if (MessageBox.Show("确认添加交易所?", "添加", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    ExchangeImpl ex = new ExchangeImpl();
                    ex.Name = this.name.Text;
                    ex.Title = this.title.Text;
                    ex.Country = (Country)this.cbCountry.SelectedValue;
                    ex.EXCode = this.code.Text;
                    ex.Calendar = this.calendar.SelectedValue.ToString();
                    ex.TimeZoneID = this.cbTimeZone.SelectedValue.ToString();
                    ex.CloseTime = Util.ToTLTime(this.closeTime.Value);

                    DataCoreService.DataClient.ReqUpdateExchange(ex);
                    this.Close();
                }
            }
        }

        List<CalendarItem> calenarlist = new List<CalendarItem>();
        void OnQryCalendarItems(string json, bool islast)
        {

            CalendarItem item = json.DeserializeObject<CalendarItem>();
            if (item != null)
            {
                calenarlist.Add(item);
            }
            if (islast)
            {
                UpdateCalendarList(calenarlist);
                if (_exchange != null)
                {
                    this.calendar.SelectedValue = _exchange.Calendar;
                }
            }
        }


        ArrayList GetCalendarCBList(List<CalendarItem> items)
        {
            ArrayList list = new ArrayList();
            ValueObject<string> v = new ValueObject<string>();
            v.Name = "默认";
            v.Value = "";
            list.Add(v);
            foreach (var item in items)
            {
                ValueObject<string> vo = new ValueObject<string>();
                vo.Name = item.Name;
                vo.Value = item.Code;
                list.Add(vo);
            }
            return list;
        }
        void UpdateCalendarList(List<CalendarItem> items)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<List<CalendarItem>>(UpdateCalendarList), new object[] { items });
            }
            else
            {
                ManagerHelper.AdapterToIDataSource(calendar).BindDataSource(GetCalendarCBList(items));
            }
        }



        ExchangeImpl _exchange;
        public void SetExchange(ExchangeImpl ex)
        {
            _exchange = ex;
            this.Text = "编辑交易所:" + ex.EXCode;
            this.code.Text = _exchange.EXCode;
            this.code.Enabled = false;
            this.name.Text = _exchange.Name;
            this.title.Text = _exchange.Title;
            this.cbCountry.SelectedValue = _exchange.Country;
            this.cbTimeZone.SelectedValue = _exchange.TimeZoneID;
            this.closeTime.Value = Util.ToDateTime(Util.ToTLDate(), _exchange.CloseTime);
        }

    }
}
