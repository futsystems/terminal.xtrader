using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStock
{
    public partial class TStock
    {
        /// <summary>
        /// 缩小
        /// </summary>
        /// <param name="showall"></param>
        public void ZoomOut()
        {
            double oldScale = GS[0].FScale;
            if (oldScale < 0.02)
            {
                logger.Info("Scale too small,can not zoomout");
                return;
            }
            int rectWidth = GS[0].Bounds.Width;//绘图区域宽度
            int dataCount = GS[0].RecordCount;//数据集个数

            float scale = (float)(GS[0].FScale *0.8);
            int newMinCnt = GetMinShowBarCount(scale);//计算预计Scale可以显示的数据数量
            int minCnt = GetMinShowBarCount();

            //logger.Info(string.Format("startindex:{0} recordCount:{1} minCnt:{2} NewMinCnt:{3}", this.StartIndex, this.RecordCount, minCnt, newMinCnt));
            if (this.StartFix)//头部固定
            {
                //新数量大于当前显示数量
                if (newMinCnt > (this.RecordCount - this.StartIndex))
                {
                    logger.Info("minCnt > dataCount,make start do not fix");
                    this.StartFix = false;
                    this.StartIndex = Math.Max(0, this.RecordCount - minCnt);
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                        GS[i].FScale = scale;
                }
            }
            else
            {
                if (newMinCnt > dataCount)//需要显示的数据不够
                {
                    if (noMoreData)//无可加载数据则计算最小scale
                    {
                        scale = ((float)(rectWidth - ExtendedRightSpace) / (float)dataCount);
                        for (int i = 0; i < 10; i++)
                            GS[i].FScale = scale;
                    }
                    else
                    {
                        //对外出发加载数据请求
                        if (KViewLoadMoreData != null)
                        {
                            KViewLoadMoreData(this, new KViewLoadMoreDataEventArgs(dataCount));
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < 10; i++)
                        GS[i].FScale = scale;
                }
                        
            }

            

            // Scale发生变化则重新设置LeftBar
            if (oldScale != GS[0].FScale)
            {
                if (!this.StartFix)
                {
                    minCnt = GetMinShowBarCount();//计算最小显示Bar数量
                    this.StartIndex = Math.Max(0, this.RecordCount - minCnt);
                }
                this.Invalidate();
            }
        }

        /// <summary>
        /// 放大
        /// </summary>
        public void ZoomIn()
        {
            double oldScale = GS[0].FScale;
            //放大
            if (GS[0].FScale < 40)//BarScale宽度小于40则可以执行放大操作
            {
                for (int i = 0; i < 10; i++)
                {
                    GS[i].FScale = oldScale*1.2;
                }
            }
            
            if (oldScale != GS[0].FScale)
            {
                if (!this.StartFix)//StartIndex不固定
                {
                    int minCnt = GetMinShowBarCount();//计算最小显示Bar数量
                    this.StartIndex = Math.Max(0, this.RecordCount - minCnt);
                    
                }
                this.Invalidate();
            }

        }
    }
}
