using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace XTraderLite
{
    public partial class MainForm
    {

        void InitDataAPI()
        {
            _dataAPI = new DataAPI.TDX.TDXDataAPI();
            _dataAPI.OnConnected += new Action(_dataAPI_OnConnected);
            _dataAPI.OnDisconnectd += new Action(_dataAPI_OnDisconnectd);
            _dataAPI.OnLoginSuccess += new Action(_dataAPI_OnLoginSuccess);
            _dataAPI.OnLoginFail += new Action(_dataAPI_OnLoginFail);
            
        }

        void _dataAPI_OnLoginFail()
        {
            logger.Info("DataAPI LoginFail");
        }

        void _dataAPI_OnLoginSuccess()
        {
            logger.Info("DataAPI LoginSuccess");
        }

        void _dataAPI_OnDisconnectd()
        {
            logger.Info("DataAPI Disconnected");
        }

        void _dataAPI_OnConnected()
        {
            logger.Info("DataAPI Connected");
            _dataAPI.Login();
        }

    }
}
