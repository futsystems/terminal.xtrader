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


namespace TradingLib.XTrader
{
    public partial class frmConfig : Form
    {
        public frmConfig()
        {
            InitializeComponent();
            btnSubmit.Click += new EventHandler(btnSubmit_Click);
            this.Load += new EventHandler(frmConfig_Load);
        }

        void btnSubmit_Click(object sender, EventArgs e)
        {
            TraderConfig.ExDoubleOrderCancelIfNotFilled = cbExDoubleOrderCancelIfNotFilled.Checked;
            TraderConfig.ExDoubleOrderFilledEntryClosePosition = cbExDoubleOrderFilledEntryClosePosition.Checked;
            TraderConfig.ExSendOrderDirect = cbExSendOrderDirect.Checked;
            TraderConfig.ExSwitchSymbolOfMarketDataView = cbExSwitchSymbolOfMarketDataView.Checked;
            TraderConfig.ExSwitchToOpenWhenCloseOrderSubmit = cbExSwitchToOpenWhenCloseOrderSubmit.Checked;
            TraderConfig.ExPositionLine = cbExPositionLine.Checked;
            TraderConfig.Save();
            this.Close();
        }

        void frmConfig_Load(object sender, EventArgs e)
        {
            cbExDoubleOrderCancelIfNotFilled.Checked = TraderConfig.ExDoubleOrderCancelIfNotFilled;
            cbExDoubleOrderFilledEntryClosePosition.Checked = TraderConfig.ExDoubleOrderFilledEntryClosePosition;
            cbExSwitchSymbolOfMarketDataView.Checked = TraderConfig.ExSwitchSymbolOfMarketDataView;
            cbExSwitchToOpenWhenCloseOrderSubmit.Checked = TraderConfig.ExSwitchToOpenWhenCloseOrderSubmit;
            cbExSendOrderDirect.Checked = TraderConfig.ExSendOrderDirect;
            cbExPositionLine.Checked = TraderConfig.ExPositionLine;
        }


    }
}
