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
            DataCoreService.EventContrib.RegisterCallback("DataFarm", "QryCalendarList", OnQryCalendarItems);

            DataCoreService.MDClient.ReqContribRequest("DataFarm", "QryCalendarList", "");
        }

        List<CalendarItem> calenarlist = new List<CalendarItem>();
        void OnQryCalendarItems(string json, bool islast)
        {

            CalendarItem item = ManagerHelper.ParseJsonResponse<CalendarItem>(json);
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



        Exchange _exchange;
        public void SetExchange(Exchange ex)
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
