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
    public partial class fmSecurityEdit : Form
    {
        public fmSecurityEdit()
        {
            InitializeComponent();


            ManagerHelper.AdapterToIDataSource(cbSecurityType).BindDataSource(ManagerHelper.GetEnumValueObjects<SecurityType>());
            ManagerHelper.AdapterToIDataSource(cbExchange).BindDataSource(ManagerHelper.GetExchangeCombList());

            ManagerHelper.AdapterToIDataSource(cbMarketTime).BindDataSource(ManagerHelper.GetMarketTimeCombList());
            ManagerHelper.AdapterToIDataSource(cbCurrency).BindDataSource(ManagerHelper.GetEnumValueObjects<CurrencyType>());

            btnSubmit.Click += new EventHandler(btnSubmit_Click);
           
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_sec != null)
            {
                //
                _sec.Code = code.Text;
                _sec.Name = name.Text;
                _sec.Currency = (CurrencyType)cbCurrency.SelectedValue;
                _sec.Type =(SecurityType)cbSecurityType.SelectedValue;

                _sec.Multiple = (int)multiple.Value;
                _sec.PriceTick = pricetick.Value;
              
                _sec.exchange_fk = (int)cbExchange.SelectedValue;
               
                _sec.mkttime_fk = (int)cbMarketTime.SelectedValue;

                DataCoreService.DataClient.ReqUpdateSecurity(_sec);
            }
            else
            {
                SecurityFamilyImpl target = new SecurityFamilyImpl();

                target.ID = 0;//0标识新增 数据库ID非0
                target.Code = code.Text;
                target.Name = name.Text;
                target.Currency = (CurrencyType)cbCurrency.SelectedValue;
                target.Type = (SecurityType)cbSecurityType.SelectedValue;

                target.Multiple = (int)multiple.Value;
                target.PriceTick = pricetick.Value;


                target.exchange_fk = (int)cbExchange.SelectedValue;

                target.mkttime_fk = (int)cbMarketTime.SelectedValue;

                DataCoreService.DataClient.ReqUpdateSecurity(target);
            }
            this.Close();
        
        }

        SecurityFamilyImpl _sec = null;
        //当前编辑的合约
        public SecurityFamilyImpl Security
        {
            get
            {
                return _sec;
            }
            set
            {
                this.Text = "编辑品种";
                _sec = value;
                code.Text = _sec.Code;
                name.Text = _sec.Name;
                cbCurrency.SelectedValue = _sec.Currency;
                multiple.Value = _sec.Multiple;
                pricetick.Value = _sec.PriceTick;
                cbExchange.SelectedValue = _sec.exchange_fk;
                cbMarketTime.SelectedValue = _sec.mkttime_fk;
                cbSecurityType.SelectedValue = _sec.Type;
            }
        }
    }
}
