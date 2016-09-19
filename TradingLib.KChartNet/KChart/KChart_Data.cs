using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStock
{
    /// <summary>
    /// 执行数据添加与清空
    /// </summary>
    public partial class TStock
    {
        //K线  加载数据--会清除已有数据
        //public void AddAll(string name, double[] f1, int len, Boolean repaint, bool front = false)
        //{
        //    int ww2, leftw, rightw, i, cc, widths;
        //    if (!front)
        //    {
        //        GS[0].BInsert(name, f1, len);
        //    }
        //    else
        //    {
        //        GS[0].FInsert(name, f1, len);
        //    }
        //    if (repaint)
        //    {
        //        leftw = 0;
        //        if (GS[0].ShowLeft)
        //            leftw = 40;
        //        rightw = 0;
        //        if (GS[0].ShowRight)
        //            rightw = 40;
        //        cc = GS[0].RecordCount;
        //        ww2 = 0;
        //        if (this.ShowDetailPanel)
        //            ww2 = Board.Width + SP1.Width;
        //        widths = (int)(Math.Floor((Width - ww2 - leftw - rightw) / GS[0].FScale));
        //        this.StartIndex = cc - widths + 1;
        //        for (i = 0; i < techwindows; i++)
        //            GS[i].run();
        //        if (ShowFs == false)
        //            this.Invalidate();
        //    }
        //}

        public void AddKViewData(string name, double[] f1, int len,bool fInsert=false)
        {
            if (fInsert)
            {
                GS[0].FInsert(name, f1, len);
            }
            else
            {
                GS[0].BInsert(name, f1, len);
            }
        }

        public void ResetIndex(int index)
        {
            foreach (var gs in GS)
            {
                gs.ResetIndex(index);
            }
        }

        /// <summary>
        /// 设置某个数据集的值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="index"></param>
        public void SetValue(string name, int index, double val)
        {
            GS[0].EditValue(name, index, val);
        }

        /// <summary>
        /// 追加一个值
        /// </summary>
        /// <param name="name"></param>
        /// <param name="val"></param>
        public void AppendValue(string name, double val)
        {
            GS[0].AppendValue(name, val);
        }


        /// <summary>
        /// 获得某个时间对应的序号
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public int GetIndex(long dt)
        {
            return GS[0].GetIndex(dt);
        }


        //K线  追加数据--append:true 追加  false修改最后一个数据
        //public void AddOne(string name, double f1, Boolean append, Boolean repaint)
        //{
        //    int ww2, leftw, rightw, i, cc, widths;
        //    GS[0].AppendValue(name, f1);//将数据加入到公式数据集
        //    if (repaint)
        //    {
        //        leftw = 0;
        //        if (GS[0].ShowLeft)
        //            leftw = 40;
        //        rightw = 0;
        //        if (GS[0].ShowRight)
        //            rightw = 40;
        //        cc = GS[0].RecordCount;
        //        ww2 = 0;
        //        if (ShowBoard)
        //            ww2 = Board.Width + SP1.Width;
        //        widths = (int)(Math.Floor((Width - ww2 - leftw - rightw) / GS[0].FScale));
        //        LeftBar = cc - widths;
        //        for (i = 0; i < techwindows; i++)
        //            GS[i].run();
        //        if (ShowFs == false)
        //            this.Invalidate();
        //    }
        //}



        public void FS_AddAll(string name, float[] f1, int len, Boolean repaint)
        {
            double[] td = new double[f1.Length];
            for (int i = 0; i < f1.Length; i++)
                td[i] = (double)f1[i];
            FS_AddAll(name, td, len, repaint);
        }
        //分时图  加载数据--会清除已有数据 
        public void FS_AddAll(string name, double[] f1, int len, Boolean repaint)
        {
            FSGS[0].Add(name, f1, len);
            if (repaint)
            {
                for (int i = 0; i < fswindows; i++)
                    FSGS[i].run();
                if (this.IsIntraView)
                    Invalidate();
            }

        }

        //分时图  追加数据--append:true 追加  false修改最后一个数据
        //public void FS_AddOne(string name, double f1, Boolean append, Boolean repaint)
        //{
        //    FSGS[0].AppendValue(name, f1);
        //    if (repaint)
        //    {
        //        for (int i = 0; i < fswindows; i++)
        //            FSGS[i].run();
        //        if (ShowFs)
        //            this.Invalidate();
        //    }

        //}

        /// <summary>
        /// 增加一条分笔数据
        /// </summary>
        /// <param name="time"></param>
        /// <param name="value"></param>
        /// <param name="vol"></param>
        /// <param name="tick"></param>
        public void AddTxnData(int time, double value, int vol, int tick, int tickcount,bool update=false)
        {
            ctDetailsBoard1.AddTick(time, value, vol, tick, tickcount,update);
        }

        /// <summary>
        /// 添加一条价量分布数据
        /// </summary>
        /// <param name="value"></param>
        /// <param name="vol"></param>
        public void AddPriceVol(double value, int vol,bool update=false)
        {
            //调用控件执行操作
            ctDetailsBoard1.AddJia(value, vol,update);
        }


        /// <summary>
        /// 清空分笔数据
        /// </summary>
        public void ClearTxnData()
        {
            ctDetailsBoard1.ClearFenbi();
        }

        /// <summary>
        /// 清空价量分布数据
        /// </summary>
        public void ClearPriceVol()
        {
            ctDetailsBoard1.ClearJia();
        }

        


        /// <summary>
        /// 清空所有数据
        /// </summary>
        public void ClearData()
        {
            //设置服务端数据请求完备标示
            noMoreData = false;
            DL = null;
            FCurStock = null;
            this.ClearIntraViewData();
            this.ClearKViewData();
            this.ClearQuan();
            //if (SellValue[0] != null)
            //{
            //    for (int i = 0; i < 5; i++)
            //    {
            //        SellValue[i].Text = "";
            //        SellVol[i].Text = "";
            //        BuyValue[i].Text = "";
            //        BuyVol[i].Text = "";

            //    }
            //}

            if (DL != null)
            {
                DL.Clear();
                DL = null;
            }
            //FenBiList.Clear();
            //JiaList.Clear();
            ctDetailsBoard1.ClearData();
            zhilist.Clear();
        }

        /// <summary>
        /// 清空K线数据
        /// </summary>
        public void ClearKViewData()
        {
            for (int i = 0; i < 10; i++)
                GS[i].cleardata();
            //this.Invalidate();
        }

        /// <summary>
        /// 清空分时数据
        /// </summary>
        public void ClearIntraViewData()
        {
            for (int i = 0; i < 10; i++)
                FSGS[i].cleardata();
            //this.Invalidate();
        }


        /// <summary>
        /// 重新计算数据
        /// 设定StartIndex
        /// </summary>
        public void ReCalculate(object obj,bool calcInd = true)
        {
            logger.Info("ReCalculate:"+obj.ToString());
            int dataLength, detailBoardWidth, leftW, rightW, showCount;
            dataLength = GS[0].RecordCount;
            if(!this.StartFix)
            {
                leftW = 0;
                if (GS[0].ShowLeft)
                    leftW = 40;
                rightW = 0;
                if (GS[0].ShowRight)
                    rightW = 40;
                detailBoardWidth = 0;
                if (this.ShowDetailPanel)
                    detailBoardWidth = Board.Width + SP1.Width;
                showCount = (int)(Math.Floor((Width - detailBoardWidth - leftW - rightW - (!this.StartFix?this.ExtendedRightSpace:0)) / GS[0].FScale));
                this.StartIndex = dataLength - showCount;
            }
            if (calcInd)
            {
                //执行计算
                for (int i = 0; i < techwindows; i++)//只计算显示的窗口指标数据
                    GS[i].run();
            }
        }

        /// <summary>
        /// 获得最小显示K线数量
        /// </summary>
        /// <returns></returns>
        int GetMinShowBarCount(double barScale=0)
        {
            int detailBoardWidth, leftW, rightW;
            double scale;
            scale = GS[0].FScale;
            if (barScale > 0)
                scale = barScale;

            leftW = 0;
            if (GS[0].ShowLeft)
                leftW = 40;
            rightW = 0;
            if (GS[0].ShowRight)
                rightW = 40;
            detailBoardWidth = 0;
            if (this.ShowDetailPanel)
                detailBoardWidth = Board.Width + SP1.Width;
            return (int)(Math.Floor((Width - detailBoardWidth - leftW - rightW - this.ExtendedRightSpace) /scale ));

        }

    }
}
