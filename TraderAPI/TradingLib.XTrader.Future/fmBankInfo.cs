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
using TradingLib.TraderCore;

namespace TradingLib.XTrader.Future
{
    public partial class fmBankInfo : Form, IEventBinder
    {
        public fmBankInfo()
        {
            InitializeComponent();

            btnSubmit.Click += new EventHandler(btnSubmit_Click);

            this.Load += new EventHandler(fmBankInfo_Load);
        }

        void fmBankInfo_Load(object sender, EventArgs e)
        {
            CoreService.EventCore.RegIEventHandler(this);
            CoreService.TLClient.ReqQryContractBank();
        }

        public void OnInit()
        {
            CoreService.EventCore.RegisterCallback("MsgExch", "QryContractBank", OnQryBank);
            CoreService.EventCore.RegisterCallback("AccountManager", "QryAccountProfile", OnQryAccountProfile);
        }

        public void OnDisposed()
        { 
        
        }

        void OnQryBank(RspInfo info,string json, bool islast)
        {
            ContractBank[] splist = json.DeserializeObject<ContractBank[]>();

            if (splist != null && islast)
            {
                ArrayList list = new ArrayList();

                foreach (var item in splist)
                {
                    ValueObject<int> vo = new ValueObject<int>();
                    vo.Name = item.Name;
                    vo.Value = item.ID;
                    list.Add(vo);
                }

                cbbank.DataSource = list;
                cbbank.ValueMember = "Value";
                cbbank.DisplayMember = "Name";
            }
            if (islast)
            {
                CoreService.TLClient.ReqQryAccountProfile();
                
            }
        }



        AccountProfile _profile = null;
        void OnQryAccountProfile(RspInfo info, string json, bool islast)
        {
            var profile = json.DeserializeObject<AccountProfile>();
            if (profile != null)
            {
                _profile = profile;
                name.Text = profile.Name;
                branch.Text = profile.Branch;
                acno.Text = profile.BankAC;
                idcard.Text = profile.IDCard;
                cbbank.SelectedValue = profile.Bank_ID;

            }
        }
        void btnSubmit_Click(object sender, EventArgs e)
        {
            if (_profile != null)
            {
                
                _profile.Name = name.Text;
                _profile.Branch = branch.Text;
                _profile.BankAC = acno.Text;
                _profile.IDCard = idcard.Text;
                _profile.Bank_ID = (int)cbbank.SelectedValue;
                if (_profile.Bank_ID <= 0)
                { 
                    MessageBox.Show("请选择签约银行");
                    return;
                }
                if (string.IsNullOrEmpty(_profile.Name))
                {
                    MessageBox.Show("请填写姓名");
                    return;
                }
                if (string.IsNullOrEmpty(_profile.BankAC))
                {
                    MessageBox.Show("请填写银行账户");
                    return;
                }
                CoreService.TLClient.ReqUpdateAccountProfile(_profile);
            }
        }
    }
}
