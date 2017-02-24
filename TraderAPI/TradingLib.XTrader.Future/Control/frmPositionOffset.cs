﻿using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;

namespace TradingLib.XTrader.Future
{
    public partial class frmPositionOffset : System.Windows.Forms.Form
    {
        ILog logger = LogManager.GetLogger("FormPositionOffset");
        public frmPositionOffset()
        {
            InitializeComponent();
            InitOffsetType();
            
        }
        void InitOffsetType()
        {
            ArrayList l = new ArrayList();
            ValueObject<QSEnumPositionOffsetType> vo1 = new ValueObject<QSEnumPositionOffsetType>();
            vo1.Name = "价格";
            vo1.Value = QSEnumPositionOffsetType.PRICE;
            l.Add(vo1);

            ValueObject<QSEnumPositionOffsetType> vo2 = new ValueObject<QSEnumPositionOffsetType>();
            vo2.Name = "点数";
            vo2.Value = QSEnumPositionOffsetType.POINTS;
            l.Add(vo2);

            //ValueObject<QSEnumPositionOffsetType> vo3 = new ValueObject<QSEnumPositionOffsetType>();
            //vo3.Name = "跟踪";
            //vo3.Value = QSEnumPositionOffsetType.TRAILING;
            //l.Add(vo3);

            typelist.DisplayMember = "Name";
            typelist.ValueMember = "Value";

            Factory.IDataSourceFactory(typelist).BindDataSource(l);
        }

        QSEnumPositionOffsetType OffsetType { get { return (QSEnumPositionOffsetType)typelist.SelectedValue; }}
        QSEnumPositionOffsetDirection Direction { get { return _posOffset.Direction; } }

        PositionOffsetArg _posOffset = null;
        Position _position = null;

        //加载止盈止损参数
        /*系统弹出对话框,从当前映射中找到当前止盈止损设置,并加载到窗体
         * 交易员进行相关修改后,选择启用或者禁止。或者直接提交参数修改同时更新到本地
         *服务端接受参数后并通知客户端
         * 这里的args是映射中的对象没有进行复制,因此确认参数修改会直接修改内存中的数据
         * */
        public void ParseOffset(Position pos,PositionOffsetArg args)
        {
            _posOffset = args;
            _position = pos;

            symbol.Text = string.Format("{0}({1})", _posOffset.Symbol, _position.DirectionType == QSEnumPositionDirectionType.Long ? "多" : "空");
            directionLabel.Text = Util.GetEnumDescription(_posOffset.Direction);
            if (_posOffset.Direction == QSEnumPositionOffsetDirection.LOSS)
            {
                typelabel.Text = "止损方式";
                sizelabel.Text = "止损手数";
            }
            else
            {
                typelabel.Text = "止盈方式";
                sizelabel.Text = "止盈手数";
            }
            switchLabel.Text = _posOffset.Enable ? "运行" : "停止";
            btnSwitchOffset.Text = _posOffset.Enable ? "禁止" : "启用";

            typelist.SelectedValue = _posOffset.OffsetType;
            size.Maximum = _position.UnsignedSize;//设定size最大值

             //更新ui文字信息
            UpdateUISetting();

            //如果止损止盈参数生效 则将参数填充到控件 否则使用uisetting()默认值
            FillOffsetArgs();
            logger.Info(string.Format("value:{0} size:{1} type:{2}", _posOffset.Value, _posOffset.Size, _posOffset.OffsetType));
           
        }

        private void frmPositionOffset_Deactivate(object sender, EventArgs e)
        {
            Close();
        }

        private void frmPositionOffset_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        void FillOffsetArgs()
        {
            if (_posOffset.Enable)
            {
                value.Value = _posOffset.Value;
                start.Value = _posOffset.Start;
                size.Value = _posOffset.Size<=size.Maximum?_posOffset.Size:size.Maximum;
            }
        }
        /// <summary>
        /// 根据止盈止损参数设定修改界面
        /// </summary>
        void UpdateUISetting()
        {
            switch (OffsetType)
            {
                case QSEnumPositionOffsetType.PRICE:
                    arglabel.Text = Direction == QSEnumPositionOffsetDirection.LOSS ? "止损价格" : "止盈价格";
                    start.Enabled = false;
                    value.Increment = _position.oSymbol.SecurityFamily.PriceTick;
                    value.DecimalPlaces = _position.oSymbol.SecurityFamily.PriceTick.GetDecimalPlaces();
                    value.Value = _position.AvgPrice;//成本价

                    break;
                case QSEnumPositionOffsetType.POINTS:
                    arglabel.Text = Direction == QSEnumPositionOffsetDirection.LOSS ? "止损点数" : "止盈点数";
                    start.Enabled = false;
                    value.Increment = _position.oSymbol.SecurityFamily.PriceTick;
                    value.DecimalPlaces = _position.oSymbol.SecurityFamily.PriceTick.GetDecimalPlaces();
                    value.Value =  _position.oSymbol.SecurityFamily.PriceTick * 100;//100跳
                    break;
                case QSEnumPositionOffsetType.TRAILING:
                    arglabel.Text = Direction == QSEnumPositionOffsetDirection.LOSS ? "回吐点数" : "回吐点数";
                    start.Enabled = true;
                    start.Increment = _position.oSymbol.SecurityFamily.PriceTick;
                    start.DecimalPlaces = _position.oSymbol.SecurityFamily.PriceTick.GetDecimalPlaces();
                    break;
                default:
                    break;

            }
        }


        //止盈止损类别改变
        private void typelist_SelectedValueChanged(object sender, EventArgs e)
        {
            if (_posOffset == null) return;
            UpdateUISetting();
            //如果切换回原来的模式则把原来模式的数据填充
            if (_posOffset.OffsetType == this.OffsetType)
            {
                FillOffsetArgs();
            }
            
        }

        //将设置窗口上的参数传递到positionoffset中去
        void UpdateOffsetArgs()
        {
            _posOffset.OffsetType = OffsetType;
            _posOffset.Value = value.Value;
            _posOffset.Size = (int)size.Value;
            _posOffset.Start = start.Value;
        }

        private void btnUpdateOffset_Click(object sender, EventArgs e)
        {
            UpdateOffsetArgs();
        }

        private void btnSwitchOffset_Click(object sender, EventArgs e)
        {
            _posOffset.Enable = !_posOffset.Enable;
            switchLabel.Text = _posOffset.Enable?"运行":"停止";
            btnSwitchOffset.Text = _posOffset.Enable ? "禁止" : "启用";
            UpdateOffsetArgs();
           
        }


    }
}
