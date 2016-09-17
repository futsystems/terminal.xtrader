using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CStock
{
    public partial class TStock
    {

        public void SwitchViewType()
        {
            switch (this.ViewType)
            {
                case KChartViewType.TimeView:
                    this.ViewType = KChartViewType.KView;
                    break;
                case KChartViewType.KView:
                    this.ViewType = KChartViewType.TimeView;
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// 处理键盘消息
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public override bool PreProcessMessage(ref   Message msg)
        {
            int ww;
            if (msg.Msg == 0x100)//WM_KEYDOWN
            {
                /*
                 * if (((Keys)msg.WParam.ToInt32()) == Keys.F5)
                {
                    ShowFs ^= true;
                    return true;
                }
                */

                if ((Keys)msg.WParam.ToInt32() == Keys.Enter)
                {
                    logger.Info("Enter");
                    //当处于十字光标时 回车取历史分笔数据
                    //if (this.ShowCrossCursor && this.IsIntraView)
                    //{
                    //    TGongSi gs = GS[0];
                    //    int j = gs.FCurBar;//当前光标所有周期
                    //    TBian b1 = gs.check("date");
                    //    TBian c1 = gs.check("close");
                    //    if ((b1 != null) && (j < b1.len))
                    //    {
                    //        //if (StockClick != null)
                    //        {
                    //            double close = c1.value[j];
                    //            if (j > 1)
                    //                close = c1.value[j - 1];//上一天的收盘等于今天的昨收价
                    //            StockEventArgs ee = new StockEventArgs(b1.value[j], close);
                    //            //StockClick(this, ee);
                    //        }
                    //    }
                    //}
                    //else //其余状态切换显示模式
                    //{
                    //    this.SwitchViewType();
                    //    //ShowFs ^= true;
                    //}
                    return false;//直接返回true 窗体就无法捕捉到Enter事件了返回false 则窗体还会捕捉到该按键
                }
                //上下键
                if ((((Keys)msg.WParam.ToInt32()) == Keys.Up) || ((Keys)msg.WParam.ToInt32() == Keys.Down))
                {

                    if (!this.IsIntraView)
                    {
                        //放大
                        if (((Keys)msg.WParam.ToInt32()) == Keys.Up)
                        {
                            this.ZoomIn();
                        }

                        //缩小
                        if (((Keys)msg.WParam.ToInt32()) == Keys.Down)
                        {
                            this.ZoomOut();
                        }

                    }
                    return true;
                }

                #region 左右键
                //左键
                if ((Keys)msg.WParam.ToInt32() == Keys.Left)
                {
                    //左右键 如果信息面板没有显示则显示该面板
                    if (!DataHint.Visible)
                        DataHint.Visible = true;

                    TGongSi gs = GS[0];
                    if (this.IsIntraView)
                        gs = FSGS[0];
                    int i = gs.FCurBar;
                    if (!ShowCrossCursor)
                        i = gs.RecordCount - 1;
                    if (this.IsIntraView)
                    {
                        int j = 240;
                        if (i > j)
                            i = j - 1;
                        if (i > 0)
                            i = i - 1;
                        if (!ShowCrossCursor)
                            i = gs.RecordCount - 1;
                        curx = i;
                    }
                    else
                    {
                        /* Bar数据用数组储存 因此最早的数据序号为1 最新的数据序号
                         * 比如从服务端请求500个数据则序号从1到500
                         * gs.FCurBar 为当前光标所在位置
                         * gs.StartIndex为当前显示区域中左侧第一个可见Bar对应的序号
                         * gs.EndIndex为当前显示区域中右侧第一个可见Bar对应的需要
                         * 
                         * 
                         * */
                        int j = gs.RecordCount;
                        if (i > j)
                            i = j - 1;
                        //如果光标所在当前位置大于0 i递减
                        if (i > 0)
                            i--;
                        //如果当前位置大于左侧位置则不修改左侧位置
                        if (i < gs.StartIndex)
                        {
                            //如果当前
                            if (!this.StartFix)
                            {
                                D("key move left,startindex not fix");
                                this.StartFix = true;
                            }
                            if (gs.StartIndex > 0)//并且左边可见位置大于0 则左边位置减1,这样重新绘图时 图像就会向右侧移动位
                            {
                                this.StartIndex--;
                            }
                            else
                                i = 0;
                        }
                        D(string.Format("Data Length:{0} gs.StartIndex:{1} gs.EndIndex:{2} gs.FCur:{3} i:{4}", gs.RecordCount, gs.StartIndex, gs.EndIndex, gs.FCurBar,i));
                        curx = i;
                    }
                    cury = -2;
                    ShowCrossCursor = true;
                    return true;
                }
                if ((Keys)msg.WParam.ToInt32() == Keys.Right)
                {
                    if (!DataHint.Visible)
                        DataHint.Visible = true;
                    TGongSi gs = GS[0];
                    if (this.IsIntraView)
                        gs = FSGS[0];
                    int i = gs.FCurBar;
                    if (!ShowCrossCursor)
                    {
                        i = gs.StartIndex;
                        if (this.IsIntraView)
                            i = 0;
                    }
                    if (this.IsIntraView)
                    {
                        int j = Math.Min(gs.RecordCount, 240);
                        if (i < j - 1)
                        {
                            i++;
                            curx = i;
                        }
                    }
                    if(this.IsBarView)
                    {
                        int j = gs.RecordCount;
                        //当前位置小于数据总长度则位置递增
                        if (i < j - 1)
                            i++;
                        if (i == j - 1)
                        {
                            D("last bar, change firstFix false");
                            this.StartFix = false;

                        }
                        //数据右侧位置小于数据总长度(右侧有数据未显示) 且当前光标位置大于右侧位置 则修改LeftBar左移图像
                        if ((i > gs.EndIndex - 1) && (gs.EndIndex < gs.RecordCount))
                        {
                            this.StartIndex++;
                        }
                        
                        curx = i;

                    }
                    D(string.Format("Data Length:{0} gs.StartIndex:{1} gs.EndIndex:{2} gs.FCur:{3} i:{4}", gs.RecordCount, gs.StartIndex, gs.EndIndex, gs.FCurBar, i));
                    cury = -2;
                    ShowCrossCursor = true;
                    return true;
                }
                #endregion

            }
            return base.PreProcessMessage(ref  msg);
        }

    }
}
