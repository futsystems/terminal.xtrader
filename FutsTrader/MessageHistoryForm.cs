using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Telerik.WinControls;

namespace FutsTrader
{
    public partial class MessageHistoryForm : Telerik.WinControls.UI.RadForm
    {
        public MessageHistoryForm()
        {
            InitializeComponent();

            this.FormClosing += new FormClosingEventHandler(MessageHistoryForm_FormClosing);
        }

        void MessageHistoryForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        public void GotMessage(string message)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(GotMessage), new object[] { message });
            }
            else
            {
                Telerik.WinControls.UI.RadListDataItem item = new Telerik.WinControls.UI.RadListDataItem(string.Format("{0} | {1}", DateTime.Now.ToString("HH:mm:ss"), message));
                messagelist.Items.Insert(0,item);
                messagelist.SelectedIndex = 0;
            }
        }


    }
}
