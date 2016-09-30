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

namespace TradingLib.DataFarmManager
{
    public partial class fmSymbolEdit : Form
    {
        bool _loaded = false;
        SymbolImpl _symbol = null;
        public fmSymbolEdit()
        {
            InitializeComponent();
            
            ManagerHelper.AdapterToIDataSource(cbExchange).BindDataSource(ManagerHelper.GetExchangeCombList());
            ManagerHelper.AdapterToIDataSource(cbMonth).BindDataSource(ManagerHelper.GetExpireMonth());
            ManagerHelper.AdapterToIDataSource(cbSymbolType).BindDataSource(ManagerHelper.GetEnumValueObjects<QSEnumSymbolType>());



            _loaded = true;
            WireEvent();
        }

        void WireEvent()
        {
            cbExchange.SelectedIndexChanged += new EventHandler(cbExchange_SelectedIndexChanged);
            cbSecurity.SelectedIndexChanged += new EventHandler(cbSecurity_SelectedIndexChanged);
            cbMonth.SelectedIndexChanged += new EventHandler(cbMonth_SelectedIndexChanged);
            cbSymbolType.SelectedIndexChanged += new EventHandler(cbSymbolType_SelectedIndexChanged);

            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            this.Load += new EventHandler(fmSymbolEdit_Load);
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_symbol != null)
            {
                switch (_symbol.SecurityFamily.Type)
                {
                        //设置到期日期,合约类别
                    case SecurityType.FUT:
                        _symbol.ExpireDate = Util.ToTLDate(this.expiredate.Value);
                        _symbol.SymbolType = (QSEnumSymbolType)cbSymbolType.SelectedValue;
                        break;
                    //case SecurityType.STK:
                    //    _symbol.Name = symName.Text;
                    //    break;
                    default:
                        break;
                }

                CoreService.MDClient.ReqUpdateSymbol(_symbol);
            }
            else
            {
                SecurityFamilyImpl sec = CurrentSecurity;
                if (sec == null)
                {
                    MessageBox.Show("请选择品种");
                    return;
                }


                SymbolImpl target = new SymbolImpl();

                target.Symbol = lbSymbol.Text;
                target.SymbolType = (QSEnumSymbolType)cbSymbolType.SelectedValue;

                target.security_fk = sec.ID;

                //按照品种设定对应的信息
                switch (sec.Type)
                {
                    case SecurityType.STK:
                        target.Strike = 0;
                        target.OptionSide = QSEnumOptionSide.NULL;

                        target.ExpireDate = 0;
                        target.Month = "";

                        //target.Symbol = symbol_input.Text;//股票不自动生成合约编码 通过手工输入
                        //target.Name = symName.Text;
                        break;
                    case SecurityType.FUT:
                        target.Strike = 0;
                        target.OptionSide = QSEnumOptionSide.NULL;

                        //标准合约设定到期日
                        if (target.SymbolType == QSEnumSymbolType.Standard)
                        {
                            int fullmonth = (int)cbMonth.SelectedValue;
                            DateTime dt = GetExpireDateTime(fullmonth);

                            target.Month = ManagerHelper.GetMonth(fullmonth);
                            if (this.expiredate.Value < dt.FirstDayOfMonth() || this.expiredate.Value > dt.LastDayOfMonth())
                            {
                                MessageBox.Show("到期日选择不正确");
                                return;
                            }
                            target.ExpireDate = Util.ToTLDate(this.expiredate.Value);
                        }
                        else
                        {
                            target.Month = (string)cbMonth.SelectedValue;
                            target.ExpireDate = 0;
                        }

                        break;
                   
                    default:
                        target.Strike = 0;
                        target.OptionSide = QSEnumOptionSide.NULL;
                        //target.ExpireMonth = 0;
                        target.ExpireDate = 0;
                        break;
                }
                CoreService.MDClient.ReqUpdateSymbol(target);
            }
        }

        void fmSymbolEdit_Load(object sender, EventArgs e)
        {
            cbExchange_SelectedIndexChanged(null, null);//触发第一次数据绑定
        }


        void cbExchange_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded) return;
            int exid = (int)cbExchange.SelectedValue;
            ManagerHelper.AdapterToIDataSource(cbSecurity).BindDataSource(ManagerHelper.GetSecurityCombListViaExchange(exid));

            GenSymbolName();
        }

        void cbSecurity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded) return;
            try
            {
                SecurityFamilyImpl sec = CurrentSecurity;

                //symbol.Visible = false;
                //symbol_input.Visible = false;
                //lbSymbolName.Visible = false;
                //symName.Visible = false;

                if (sec == null) return;
                switch (sec.Type)
                {
                    case SecurityType.IDX:
                        cbMonth.Enabled = false;
                        expiredate.Enabled = false;
                        //symbol.Visible = true;
                        break;


                    //case SecurityType.STK:
                    //    cbexpiremonth.Enabled = false;
                    //    expiredate.Enabled = false;
                    //    cbSymbolType.Enabled = false;

                    //    //股票手工输入Symbol
                    //    symbol_input.Visible = true;
                    //    lbSymbolName.Visible = true;
                    //    symName.Visible = true;
                    //    break;
                    case SecurityType.FUT:
                        //symbol.Visible = true;
                        cbMonth.Enabled = true;
                        expiredate.Enabled = true;
                        cbSymbolType.Enabled = true;
                        break;
                    default:
                        break;

                }
            }
            catch (Exception ex)
            {
                //Globals.Debug("error securitychanged:" + ex.ToString());
            }

            GenSymbolName();
        }

        void cbMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loaded) return;
            try
            {
                int expire = (int)cbMonth.SelectedValue;
                SecurityFamilyImpl sec = CurrentSecurity;
                if (sec == null) return;
                //如果是标准合约则更新过期日期
                QSEnumSymbolType symboltype = (QSEnumSymbolType)cbSymbolType.SelectedValue;

                //MessageBox.Show("year:" + year.ToString() + " month:" + m.ToString());
                if (symboltype == QSEnumSymbolType.Standard)
                {
                    this.expiredate.Value = GetExpireDateTime(expire).LastDayOfMonth();
                }

            }
            catch (Exception ex)
            {
                //Globals.Debug("error expiremonth:" + ex.ToString());
            }

            GenSymbolName();
        }


        void cbSymbolType_SelectedIndexChanged(object sender, EventArgs e)
        {
            QSEnumSymbolType symboltype = (QSEnumSymbolType)cbSymbolType.SelectedValue;
            switch (symboltype)
            {
                case QSEnumSymbolType.Standard:
                    expiredate.Enabled = true;
                    ManagerHelper.AdapterToIDataSource(cbMonth).BindDataSource(ManagerHelper.GetExpireMonth());

                    break;
                case QSEnumSymbolType.MonthContinuous:
                    expiredate.Enabled = false;
                    ManagerHelper.AdapterToIDataSource(cbMonth).BindDataSource(ManagerHelper.GenExpireMonthWithOutYear());

                    break;
                default:
                    break;
            }

            GenSymbolName();
        }



        

        /// <summary>
        /// 通过201501来获得到期月初时间
        /// </summary>
        /// <param name="expire"></param>
        /// <returns></returns>
        DateTime GetExpireDateTime(int expire)
        {
            //month 201602
            int year = expire / 100;
            int month = expire - year * 100;
            //获得某个月最后一天
            DateTime tmp = new DateTime(year, month, 1, 0, 0, 0);
            return tmp;
        }


        SecurityFamilyImpl CurrentSecurity
        {
            get
            {
                int id = (int)cbSecurity.SelectedValue;
                return CoreService.MDClient.GetSecurity(id);//获得当前选中的security
            }
        }

        


        void GenSymbolName()
        {
            if (!_loaded) return;
            try
            {
                SecurityFamilyImpl sec = CurrentSecurity;
                if (sec == null) return;

                switch (sec.Type)
                {
                    case SecurityType.FUT:
                        QSEnumSymbolType symboltype = (QSEnumSymbolType)cbSymbolType.SelectedValue;
                        switch (symboltype)
                        {
                            case QSEnumSymbolType.Standard:
                                {
                                    int month = (int)cbMonth.SelectedValue;
                                    lbSymbol.Text = ManagerHelper.GenFutSymbol(sec, month);
                                    break;
                                }
                            case QSEnumSymbolType.MonthContinuous:
                                {
                                    string month = (string)cbMonth.SelectedValue;
                                    lbSymbol.Text = ManagerHelper.GenSymbolMonthContinuous(sec.Code, month);
                                    this.expiredate.Value = DateTime.MaxValue;
                                    break;
                                }
                            default:
                                break;
                        }
                        break;

                    default:
                        break;
                }



            }
            catch (Exception ex)
            {
                //Globals.Debug("error expiremonth:" + ex.ToString());
            }
        }
        
        public SymbolImpl Symbol
        {
            get
            {
                return _symbol;
            }

            set
            {
                _symbol = value;

                this.Text = "编辑合约[" + _symbol.Symbol + "]";

                int exid = _symbol.SecurityFamily != null ? (_symbol.SecurityFamily.Exchange as Exchange).ID : 0;
                cbExchange.SelectedValue = exid;
                cbExchange.Enabled = false;

                ManagerHelper.AdapterToIDataSource(cbSecurity).BindDataSource(ManagerHelper.GetSecurityCombListViaExchange(exid));
                cbSecurity.SelectedValue = _symbol.SecurityFamily != null ? (_symbol.SecurityFamily as SecurityFamilyImpl).ID : 0;
                cbSecurity.Enabled = false;

                lbSymbol.Text = _symbol.Symbol;
                lbSymbol.Enabled = false;

                cbMonth.Enabled = false;
                expiredate.Enabled = false;
                cbSymbolType.Enabled = false;

                if (_symbol.SecurityFamily.Type == SecurityType.FUT)
                {
                    //绑定月份
                    ManagerHelper.AdapterToIDataSource(cbMonth).BindDataSource(ManagerHelper.GetExpireMonth());
                    cbMonth.SelectedValue = (int)_symbol.ExpireDate / 100;
                    cbMonth.Enabled = false;

                    //设定过期日
                    this.expiredate.Value = (_symbol.ExpireDate == 0 ? DateTime.Now : Util.ToDateTime(_symbol.ExpireDate, 0));
                    cbSymbolType.Enabled = false;
                    if (_symbol.SymbolType == QSEnumSymbolType.MonthContinuous)
                    {
                        this.expiredate.Enabled = false;
                    }
                    else
                    {
                        expiredate.Enabled = true;
                    }
                }

            }
        }
    }
}
