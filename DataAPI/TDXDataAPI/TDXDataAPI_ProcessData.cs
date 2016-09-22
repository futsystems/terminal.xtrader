using System;
using System.Collections.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
using Common.Logging;

using TradingLib.MarketData;

namespace DataAPI.TDX
{
    public partial class TDXDataAPI
    {

        Thread mainthread = null;
        bool _recvgo = false;
        bool busying = false;


        void StartRecv()
        {
            if (_recvgo) return;
            logger.Info("Start Recv Thread");
            _recvgo = true;
            mainthread = new Thread(ThreadWork);
            //mainthread.IsBackground = true;
            mainthread.Start();
        }

        void StopRecv()
        {
            if (!_recvgo) return;
            logger.Info("Stop Recv Thread");
            _recvgo = false;
            mainthread.Join();
            logger.Info(" mainthread join after");
        }
        private void ThreadWork()
        {
            logger.Info("ThreadWork Loop");
            byte[] DataHeader = new byte[16];
            SendBuf sb = null;
            RecvDataHeader head = new RecvDataHeader();
            IAsyncResult sr = null;

            while (_recvgo)
            {
                try
                {

                    //logger.Info("xxxxx");
                    if (m_hSocket == null)
                    {
                        break;
                    }
                    //if (busying)
                    //{
                    //    //logger.Info("busying sleep");
                    //    Thread.Sleep(20);
                    //    continue;
                    //}

                    sb = null;
                    while (SendList.Count > 0)
                    {
                        _profiler.EnterSection("数据处理/含网络");
                        sb = (SendBuf)SendList.Dequeue();
                        //logger.Info("       process send request:" + sb.RequestId.ToString());
                        m_hSocket.Send(sb.Send, sb.Send.Length, SocketFlags.None);
                        //获取消息头
                        int Len = m_hSocket.Receive(DataHeader, 16, SocketFlags.None);
                        //读取数据失败
                        if (Len != 16)
                        {
                            m_hSocket.Close();
                            m_hSocket = null;
                            break;
                        }
                        head = (RecvDataHeader)TDX.TDXDecoder.BytesToStuct(DataHeader, 0, head.GetType());
                        //数据检查
                        if (head.CheckSum != 7654321)
                            continue;

                        //根据消息头中的size获得对应长度的服务端传输过来的消息
                        byte[] buf = new byte[head.Size];
                        int elen = head.Size;
                        int fcur = 0;
                        int Len1, min1 = 1024;
                        while (fcur < elen)
                        {
                            min1 = Math.Min(1024, elen - fcur);
                            Len1 = m_hSocket.Receive(buf, fcur, min1, SocketFlags.None);
                            if (Len1 > 0)
                                fcur += Len1;
                        }
                        //数据长度异常
                        if (fcur != elen)
                        {
                            m_hSocket.Close();
                            m_hSocket = null;
                            break;
                        }
                        byte[] recvbuf = new byte[head.DePackSize + 1];
                        if ((head.EncodeMode & 0x10) == 0x10)
                        {
                            int LL = TDX.TDXDecoder.Decompress(buf, head.DePackSize, ref recvbuf);
                            if (LL != head.DePackSize)
                            {
                                logger.Error("解压出错:长度不同=depacksize=" + head.DePackSize.ToString() + " 解压长度:=" + LL.ToString());
                            }
                        }
                        else
                            Array.Copy(buf, recvbuf, head.DePackSize);
                        int t = 0;
                        switch (head.msgid)
                        {
                            case 0x526:
                            case 0x527: t = 0x39; break;
                            case 0x551: t = 0x49; break;
                            case 0x556: t = 0x69; break;
                            case 0x56e:
                            case 0x573: t = 0x77; break;
                        }
                        if (t > 0)
                        {
                            for (int i = 0; i < head.DePackSize; i++)
                                recvbuf[i] = (byte)(recvbuf[i] ^ t);
                        }
                        sb.hd = head;
                        sb.Buffer = recvbuf;
                        //_profiler.EnterSection("数据处理");
                        ProcessData(sb);
                        //_profiler.LeaveSection();
                        //logger.Info("       process request done:" + sb.RequestId.ToString());
                        //_profiler.LeaveSection();
                    }
                    //sr = this.BeginInvoke(DataCuLi, sb);//处理数据
                    //this.EndInvoke(sr);
                    //String data = myDelegate.EndInvoke(result, null, null);
                    //while (doing)
                    //{
                    //    Thread.Sleep(30); 
                    //}
                    //this.BeginInvoke(DataCuLi, sb);

                    //logger.Info("thread wait new request----------");
                    // clear current flag signal
                    _processWaiting.Reset();

                    // wait for a new signal to continue reading
                    _processWaiting.WaitOne(5000);

                    //foreach(var msg in _profiler.GetStatsStringList())
                    //{
                    //    logger.Info(msg);
                    //}

                }
                catch (Exception ex)
                {
                    //_recvgo = false;
                    logger.Info("Process Loop Error:" + ex.ToString());
                    break;

                }
            }
            logger.Info(" main loop stopped");
        }

        


        public void ProcessData(DataAPI.TDX.SendBuf sb)
        {
            if (sb == null)
                return;
            try
            {
                byte[] RecvBuffer = sb.Buffer;
                string codes;
                byte[] code = new byte[6];
                byte[] name = new byte[8];
                string names;
                int i, t, n;
                int num4, num5, num6, num7, num8, num9 = 0, num10;
                //if (List1.Visible)
                //{
                //    if (List1.Items.Count > 30)
                //        List1.Items.Clear();
                //    List1.Items.Add(sb.hd.msgid.ToString("X04"));
                //}
                int time1;
                //CStock.Stock sk = null;
                switch (sb.hd.msgid)
                {

                    case 0x51a:
                        #region 当日价格分布数据处理
                        i = 0;
                        n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        logger.Info(string.Format("QryPriceVol Response Count:{0}", n));

                        if (n == 0)
                            return;
                        //GP.ClearJia();
                        i = 11;
                        for (int j = 0; j < 5; j++)
                            TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        i += 4;
                        for (int j = 0; j < 3; j++)
                            TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        i += 4;
                        for (int j = 0; j < 16; j++)
                            TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        i += 2;
                        num4 = 0;
                        List<PriceVolPair> pvlist = new List<PriceVolPair>();
                        for (int j = 0; j < n; j++)
                        {
                            num4 = num4 + TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                            num5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                            num6 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                            num7 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                            double v1 = (double)num4 / 100.0;
                            //logger.Info("PV {0} {1}".Put(v1, num5));
                            //GP.AddJia(v1, num5);
                            //if (OnRspQryPriceVolPair != null)
                            //{
                            //    OnRspQryPriceVolPair(new PriceVolPair(v1, num5), null, sb.RequestId, j == n - 1);
                            //}
                            //
                            pvlist.Add(new PriceVolPair(v1, num5));
                        }
                        if (OnRspQryPriceVolPair != null)
                        {
                            OnRspQryPriceVolPair(pvlist, null, n, sb.RequestId);
                        }
                        #endregion


                        break;
                    case 0xfb5://历史分笔
                        #region 历史分笔处理
                        i = 0;
                        n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        logger.Info(string.Format("QryHistTransactionData Response Count:{0}", n));
                        if (n > 0)
                        {
                            i = 6;
                            num4 = 0;
                            double close;
                            int vol, dealcount, sellorbuy;

                            //List<CStock.Tick> lt1 = new List<CStock.Tick>();
                            for (int j = 0; j < n; j++)
                            {
                                //CStock.Tick tk = new CStock.Tick();
                                time1 = TDX.TDXDecoder.TDXGetTime(RecvBuffer, i, ref i);
                                num5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                close = (double)((num4 + num5) / 100.0);
                                vol = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                dealcount = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);

                                //lt1.Add(tk);
                                num4 = num5 + num4;
                                i = i + 1;

                                TradeSplit split = new TradeSplit(time1, close, vol, 0, dealcount);
                                //logger.Info(split.ToString());
                            }
                            //HisTicks.InsertRange(0, lt1);
                        }
                        //if (n == 2000)
                        //    GetHisFenBiLine(sb.sk, HisDate, HisTicks.Count, 2000);
                        //if (n < 2000)
                        //{
                        //    HisList.Items.Clear();
                        //    HisList.BeginUpdate();
                        //    for (i = 0; i < HisTicks.Count; i++)
                        //    {
                        //        CStock.Tick tk = HisTicks[i];
                        //        string ss = string.Format("{0:D2}:{1:D2}  ", tk.time / 100, tk.time % 100);
                        //        ss += string.Format("{0:F2}  ", tk.value);
                        //        ss += tk.vol.ToString("D5") + "  ";// string.Format("{0:D}  ", tk.vol);
                        //        if (tk.tick == 1)
                        //            ss += "B ";
                        //        else if (tk.tick == 0)
                        //            ss += "S ";
                        //        else
                        //            ss += "  ";
                        //        HisList.Items.Add(ss);
                        //    }
                        //    HisList.EndUpdate();
                        //    HisList.SelectedIndex = HisTicks.Count - 1;
                        //    HisTicks.Clear();
                        //}
                        #endregion

                        break;
                    case 0xfc5://分笔
                        {
                            _profiler.EnterSection("分笔处理");
                            #region 当日分笔数据处理
                            i = 0;
                            n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                            logger.Info(string.Format("QryTransactionData Response Count:{0}", n));
                            List<TradeSplit> tslist = new List<TradeSplit>();
                            //不存在分笔数据
                            if (n == 0)
                            {
                                //Ticks.Clear();
                                if (OnRspQryTradeSplit != null)
                                {
                                    OnRspQryTradeSplit(tslist, null, 0, sb.RequestId);
                                }
                                return;
                            }

                            double close;
                            int vol, dealcount, sellorbuy;
                            num4 = 0;
                            i = 2;
                            for (int j = 0; j < n; j++)
                            {
                                time1 = TDX.TDXDecoder.TDXGetTime(RecvBuffer, i, ref i);
                                num5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                close = (num4 + num5) / 100.0;
                                vol = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                dealcount = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                sellorbuy = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                num4 = num5 + num4;
                                i = i + 1;
                                tslist.Add(new TradeSplit(time1, close, vol, sellorbuy, dealcount));
                            }
                            if (OnRspQryTradeSplit != null)
                            {
                                OnRspQryTradeSplit(tslist, null, n, sb.RequestId);
                            }


                            #endregion
                            _profiler.LeaveSection();
                        }
                        break;
                    case 0x526://查询某组合约行情快照回报

                        #region 合约快照处理
                        i = 0;
                        n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        if (n == 0)
                            return;
                        i = 2;
                        string uniqueKey = string.Empty;
                        MDSymbol target = null;
                        List<MDSymbol> list = new List<MDSymbol>();
                        for (int j = 0; j < n; j++)
                        {
                            byte m = RecvBuffer[i];
                            Array.Copy(RecvBuffer, i + 1, code, 0, 6);
                            codes = System.Text.Encoding.GetEncoding("GB2312").GetString(code);
                            uniqueKey = string.Format("{0}-{1}", GetMarketString((int)m), codes);
                            if (symbolMap.TryGetValue(uniqueKey, out target))
                            {
                                //sk = (CStock.Stock)FSH[EnCodeMark(codes, m)];
                                //if (sk == null)
                                //    break;
                                i = i + 9;
                                double prize = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i) / 100.0;
                                target.TickSnapshot.Price = prize;
                                target.TickSnapshot.last = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100.0);
                                if (target.PreClose == 0.0)
                                    target.PreClose = (float)target.TickSnapshot.last;
                                target.TickSnapshot.Open = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.High = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.Low = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.Time = TDX.TDXDecoder.TDXGetInt32(RecvBuffer, i, ref i);
                                TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                target.TickSnapshot.Volume = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                target.TickSnapshot.Size = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);// '现量;
                                target.TickSnapshot.Amount = TDX.TDXDecoder.TDXGetDouble(RecvBuffer, i, ref i);
                                target.TickSnapshot.B = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                target.TickSnapshot.S = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);

                                TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);

                                target.TickSnapshot.Buy1 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.Sell1 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.BuyQTY1 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.SellQTY1 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                                target.TickSnapshot.Buy2 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.Sell2 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.BuyQTY2 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.SellQTY2 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                                target.TickSnapshot.Buy3 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.Sell3 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.BuyQTY3 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.SellQTY3 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.Buy4 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.Sell4 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.BuyQTY4 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.SellQTY4 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.Buy5 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.Sell5 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.BuyQTY5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.SellQTY5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                                target.TickSnapshot.BiCount = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);//逐笔 笔数
                                TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);


                                target.TickSnapshot.Buy6 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.Sell6 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.BuyQTY6 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.SellQTY6 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                                target.TickSnapshot.Buy7 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.Sell7 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.BuyQTY7 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.SellQTY7 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                                target.TickSnapshot.Buy8 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.Sell8 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.BuyQTY8 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.SellQTY8 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.Buy9 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.Sell9 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.BuyQTY9 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.SellQTY9 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.Buy10 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.Sell10 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                                target.TickSnapshot.BuyQTY10 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                                target.TickSnapshot.SellQTY10 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                                target.TickSnapshot.buyall = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);//买均
                                target.TickSnapshot.sellall = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100); ;//卖均
                                target.TickSnapshot.buyQTYall = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);//总买
                                target.TickSnapshot.sellQTYall = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);//总卖

                                list.Add(target);
                                //if (sk == FCurStock)
                                //    GP.SetStock(sk);

                                //StockJS(sk);
                            }
                        }
                        if (OnRspQryTickSnapshot != null)
                        {
                            OnRspQryTickSnapshot(list, null, n, sb.RequestId);
                        }
                        //if (ListBoard.Visible)
                        //{
                        //    if (Block1.Visible)
                        //    {
                        //        Stklist.Refresh();

                        //        if ((FCurPage == -1) && (fsorttype > 0))
                        //        {
                        //            if (fsorttype == 1)
                        //            {
                        //                Stklist.BeginUpdate();
                        //                QuickSortHightoLow(Stklist.Items, 0, Stklist.Items.Count - 1, fsort - 3);
                        //                Stklist.EndUpdate();
                        //                Stklist.Invalidate();
                        //            }
                        //            if (fsorttype == 2)
                        //            {
                        //                Stklist.BeginUpdate();
                        //                QuickSortLowToHigh(Stklist.Items, 0, Stklist.Items.Count - 1, fsort - 3);
                        //                Stklist.EndUpdate();
                        //                Stklist.Invalidate();
                        //            }
                        //        }
                        //    }

                        //}
                        break;
                        #endregion

                    case 0x53e:
                    case 0x53d:

                        i = 2;
                        n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        logger.Info("TickSnaphost response:" + n.ToString());
                        if (n == 0)
                            return;
                        i = 4;
                        int dd;
                        t = 0;
                        //for (int j = 0; j < n; j++)
                        //{
                        //    byte m = RecvBuffer[i];
                        //    Array.Copy(RecvBuffer, i + 1, code, 0, 6);
                        //    codes = System.Text.Encoding.GetEncoding("GB2312").GetString(code);
                        //    sk = (CStock.Stock)FSH[EnCodeMark(codes, m)];
                        //    if (sk == null)
                        //        break;
                        //    i = i + 7;
                        //    dd = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        //    //List1.Items.Add("d1:" + dd.ToString());
                        //    double prize = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i) / 100.0;
                        //    sk.now.prize = prize;
                        //    sk.now.last = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100.0);
                        //    sk.now.open = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.high = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.low = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.Time = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    //List1.Items.Add("d0:" + dd.ToString()); ;
                        //    sk.now.volume = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    sk.now.tradeQTY = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);// '现量;
                        //    sk.now.amount = TDX.TDXDecoder.TDXGetDouble(RecvBuffer, i, ref i);
                        //    sk.now.b = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    sk.now.s = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);

                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    //List1.Items.Add("d1:" + dd.ToString()); ;
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    //List1.Items.Add("d2:" + dd.ToString()); ;

                        //    sk.now.buy1 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell1 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY1 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY1 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                        //    sk.now.buy2 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell2 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY2 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY2 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);

                        //    sk.now.buy3 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell3 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY3 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY3 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.buy4 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell4 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY4 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY4 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.buy5 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.sell5 = prize + (((double)TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i)) / 100);
                        //    sk.now.buyQTY5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    sk.now.sellQTY5 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref  i);
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref t);
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, t, ref t);
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, t, ref t);
                        //    i = i + 3;
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    dd = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    double speed = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i) / 100;
                        //    TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);

                        //    if ((sb.hd.msgid == 0x53d) && (ListBoard.Visible) && (fsorttype > 0))
                        //    {
                        //        if (sb.type == 100)
                        //        {

                        //            Stklist.Items[Stklist.TopIndex + j] = sk;
                        //        }
                        //    }

                        //    if (sk == SHZS)
                        //    {
                        //        if (sk.now.Time > 14700000) //收盘
                        //        {
                        //            working = false;

                        //        }


                        //        shzs.Text = string.Format("{0:F2}  {1:F2}  {2:F2}亿", sk.now.prize, sk.now.prize - sk.now.last, sk.now.amount / 100000000);
                        //        if (sk.now.prize > sk.now.last)
                        //            shzs.ForeColor = Color.Red;
                        //        else
                        //            shzs.ForeColor = Color.Green;
                        //    }
                        //    if (sk == SZZS)
                        //    {
                        //        szzs.Text = string.Format("{0:F2}  {1:F2}  {2:F2}亿", sk.now.prize, sk.now.prize - sk.now.last, sk.now.amount / 100000000);
                        //        if (sk.now.prize > sk.now.last)
                        //            szzs.ForeColor = Color.Red;
                        //        else
                        //            szzs.ForeColor = Color.Green;
                        //    }

                        //    StockJS(sk);
                        //}
                        //if ((sb.hd.msgid == 0x53d) && (ListBoard.Visible))
                        //{
                        //    Stklist.Refresh();
                        //}
                        break;
                    case 0x51d://当日分时
                        _profiler.EnterSection("分时处理");
                        #region 当日分时数据处理
                        n = RecvBuffer[1] * 256 + RecvBuffer[0];
                        logger.Info(string.Format("QryMinuteDate Response Count:{0}", n));
                        if (n == 0)
                        {
                            if (OnRspQryMinuteData != null)
                            {
                                Dictionary<string, double[]> tmp = new Dictionary<string, double[]>();
                                OnRspQryMinuteData(tmp, null, n, sb.RequestId);
                            }
                        }
                        if (n > 0)
                        {
                            i = 4;
                            num4 = 0;
                            double res;
                            double[] date = new double[n + 1];
                            double[] time = new double[n + 1];
                            double[] close = new double[n + 1];
                            double[] amount = new double[n + 1];
                            double[] vol = new double[n + 1];
                            for (int j = 0; j < n; j++)
                            {
                                //date[j] = StkDate;
                                time[j] = gptime(sb.Market, j);
                                num4 = num4 + TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                close[j] = (float)(num4 / 100.0);
                                res = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                vol[j] = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);

                                //logger.Info("{0} {1} {2}".Put(time[j], close[j], vol[j]));
                            }


                            if (OnRspQryMinuteData != null)
                            {
                                Dictionary<string, double[]> tmp = new Dictionary<string, double[]>();
                                tmp["date"] = date;
                                tmp["time"] = time;
                                tmp["close"] = close;
                                tmp["vol"] = vol;
                                OnRspQryMinuteData(tmp, null, n, sb.RequestId);
                            }
                        }
                        //if (n > 0)
                        //{
                        //    i = 4;
                        //    num4 = 0;
                        //    double res;
                        //    date = new double[n + 1];
                        //    time = new double[n + 1];
                        //    close = new double[n + 1];
                        //    amount = new double[n + 1];
                        //    vol = new double[n + 1];
                        //    for (int j = 0; j < n; j++)
                        //    {
                        //        Application.DoEvents();
                        //        date[j] = StkDate;
                        //        time[j] = gptime(sb.sk.mark, j);
                        //        num4 = num4 + TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        close[j] = (float)(num4 / 100.0);
                        //        res = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        vol[j] = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    }
                        //if (MutiBoard.Visible)
                        //{
                        //    sk = sb.sk;
                        //    for (i = 0; i < mgp.Length; i++)//遍历所有数据结构 如果stock对象相同则更新对应控件
                        //    {
                        //        if (msk[i] == sk)
                        //        {
                        //            mgp[i].BeginUpdate();
                        //            mgp[i].cleardata();
                        //            mgp[i].PreClose = sk.GP.YClose;
                        //            if (sk.now.last != 0)
                        //                mgp[i].PreClose = sk.now.last;
                        //            mgp[i].StkCode = sk.codes;
                        //            mgp[i].StkName = sk.names;
                        //            mgp[i].StkWeek = WeekString[zq];
                        //            mgp[i].EndUpdate();
                        //            mgp[i].FS_AddAll("date", date, n, false);
                        //            mgp[i].FS_AddAll("time", time, n, false);
                        //            mgp[i].FS_AddAll("vol", vol, n, false);
                        //            mgp[i].FS_AddAll("close", close, n, true);
                        //            break;
                        //        }
                        //    }
                        //}
                        //返回分时数据处理
                        //if ((sb.sk == FCurStock) && (StockBoard.Visible))
                        //{
                        //    GP.FS_cleardata();
                        //    sk = sb.sk;
                        //    if (sk != null)
                        //    {
                        //        GP.PreClose = sk.GP.YClose;
                        //        if (sk.now.last != 0)
                        //            GP.PreClose = sk.now.last;
                        //        GP.StkCode = sk.codes;
                        //        GP.StkName = sk.names;
                        //        GP.StkWeek = WeekString[zq];
                        //    }

                        //    if ((GP.Days > 1) && (DayTicks.Count > 0))
                        //    {

                        //        double[] date1 = new double[DayTicks.Count + n + 1];
                        //        double[] time11 = new double[DayTicks.Count + n + 1];
                        //        double[] close1 = new double[DayTicks.Count + n + 1];
                        //        double[] vol1 = new double[DayTicks.Count + n + 1];
                        //        for (int j = 0; j < DayTicks.Count; j++)
                        //        {
                        //            DayTick dt = DayTicks[j];
                        //            date1[j] = dt.date;
                        //            time11[j] = dt.time;
                        //            close1[j] = dt.close;
                        //            vol1[j] = dt.vol;
                        //        }
                        //        for (int j = 0; j < n; j++)
                        //        {
                        //            date1[DayTicks.Count + j] = date[j];
                        //            time11[DayTicks.Count + j] = time[j];
                        //            close1[DayTicks.Count + j] = close[j];
                        //            vol1[DayTicks.Count + j] = vol[j];
                        //        }
                        //        n = DayTicks.Count + n;
                        //        GP.FS_AddAll("date", date1, n, false);
                        //        GP.FS_AddAll("time", time11, n, false);
                        //        GP.FS_AddAll("vol", vol1, n, false);
                        //        GP.FS_AddAll("close", close1, n, true);
                        //    }
                        //    else
                        //    {
                        //        GP.FS_AddAll("date", date, n, false);
                        //        GP.FS_AddAll("time", time, n, false);
                        //        GP.FS_AddAll("vol", vol, n, false);
                        //        GP.FS_AddAll("close", close, n, true);
                        //    }
                        //}
                        //if ((StockBoard.Visible) && (GP.TabValue == 2))
                        //{
                        //    if ((sb.sk == SZZS) || (sb.sk == SHZS))
                        //    {
                        //        GP.zs_Clear();
                        //        GP.zs_SetName(sb.sk.names);
                        //        GP.zs_AddAll("date", date, n, false);
                        //        GP.zs_AddAll("time", time, n, false);
                        //        GP.zs_AddAll("vol", vol, n, false);
                        //        GP.zs_AddAll("close", close, n, true);
                        //    }
                        //}
                        //}
                        #endregion
                        _profiler.LeaveSection();
                        break;

                    case 0xfb4://历史分时
                        #region 历史分时数据处理
                        n = RecvBuffer[1] * 256 + RecvBuffer[0];
                        logger.Info(string.Format("QryHistMinuteDate Response Count:{0}", n));

                        if (n > 0)
                        {
                            i = 6;
                            num4 = 0;
                            double res;
                            double[] date = new double[n + 1];
                            double[] time = new double[n + 1];
                            double[] close = new double[n + 1];
                            double[] amount = new double[n + 1];
                            double[] vol = new double[n + 1];
                            for (int j = 0; j < n; j++)
                            {
                                //Application.DoEvents();
                                //date[j] = sb.type;
                                time[j] = gptime(sb.Market, j);
                                num4 = num4 + TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                close[j] = (double)(num4 / 100.0);
                                res = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                vol[j] = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);

                                //logger.Info("{0} {1} {2}".Put(time[j], close[j], vol[j]));
                            }

                            if (OnRspQryHistMinuteData != null)
                            {
                                double[][] tmp = new double[4][];
                                tmp[0] = date;
                                tmp[1] = time;
                                tmp[2] = close;
                                tmp[3] = vol;
                                OnRspQryHistMinuteData(tmp, null, n, sb.RequestId);
                            }

                        }

                        //if (n > 0)
                        //{
                        //    i = 6;
                        //    num4 = 0;
                        //    double res;
                        //    date = new double[n + 1];
                        //    time = new double[n + 1];
                        //    close = new double[n + 1];
                        //    amount = new double[n + 1];
                        //    vol = new double[n + 1];
                        //    for (int j = 0; j < n; j++)
                        //    {
                        //        Application.DoEvents();
                        //        date[j] = sb.type;
                        //        time[j] = gptime(sb.sk.mark, j);
                        //        num4 = num4 + TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        close[j] = (double)(num4 / 100.0);
                        //        res = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        vol[j] = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //    }
                        //    if (HisBoard.Visible)
                        //    {

                        //        tStock1.FS_cleardata();
                        //        sk = sb.sk;
                        //        if (sk != null)
                        //        {
                        //            tStock1.PreClose = hispreclose;
                        //            tStock1.StkCode = sk.codes;
                        //            tStock1.StkName = sk.names;
                        //            tStock1.StkWeek = WeekString[zq];
                        //        }
                        //        HisName.Text = sk.names + " " + HisDate.ToString() + " 历史分时图";
                        //        tStock1.FS_AddAll("date", date, n, false);
                        //        tStock1.FS_AddAll("time", time, n, false);
                        //        tStock1.FS_AddAll("vol", vol, n, false);
                        //        tStock1.FS_AddAll("close", close, n, true);
                        //    }
                        //    if (StockBoard.Visible) //多日分时
                        //    {
                        //        for (int j = 0; j < n; j++)
                        //        {
                        //            DayTick dt = new DayTick();
                        //            dt.date = (int)date[j];
                        //            dt.time = (int)time[j];
                        //            dt.vol = vol[j];
                        //            dt.close = close[j];
                        //            DayTicks.Add(dt);
                        //        }
                        //    }

                        //}
                        #endregion

                        break;
                    case 0x529://日线
                        _profiler.EnterSection("K线处理");
                        #region K线数据处理
                        n = RecvBuffer[1] * 256 + RecvBuffer[0];
                        logger.Info(string.Format("QrySecurityBars Response Count:{0}", n));
                        if (n == 0)
                        {
                            Dictionary<string, double[]> tmp = new Dictionary<string, double[]>();
                            tmp["date"] = new double[1];
                            tmp["time"] = new double[1];
                            tmp["open"] = new double[1];
                            tmp["high"] = new double[1];
                            tmp["low"] = new double[1];
                            tmp["close"] = new double[1];
                            tmp["vol"] = new double[1];
                            tmp["amount"] = new double[1];

                            if (OnRspQrySecurityBar != null)
                            {
                                OnRspQrySecurityBar(tmp, null, n, sb.RequestId);
                            }
                        }
                        if (n > 0)
                        {
                            int yy1 = 0, mm1 = 0, dd1 = 0, hhh = 0, mmm = 0;
                            double[] date = new double[n + 1];
                            double[] time = new double[n + 1];
                            double[] high = new double[n + 1];
                            double[] low = new double[n + 1];
                            double[] open = new double[n + 1];
                            double[] close = new double[n + 1];
                            double[] amount = new double[n + 1];
                            double[] vol = new double[n + 1];
                            double[] upcount = new double[n];
                            double[] downcount = new double[n];
                            bool isIndex = false;
                            //判断是否是指数Bars
                            if ((sb.StkType == 100) || (sb.StkType == 7))//指数
                                isIndex = true;
                            i = 2;
                            num9 = 0;
                            for (int j = 0; j < n; j++)
                            {
                                //Application.DoEvents();
                                TDX.TDXDecoder.TDXGetDate(TDX.TDXDecoder.TDXGetInt32(RecvBuffer, i, ref i), ref yy1, ref mm1, ref dd1, ref hhh, ref mmm);
                                date[j] = yy1 * 10000 + mm1 * 100 + dd1;
                                time[j] = hhh * 100 + mmm;
                                num10 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                open[j] = (float)((num9 + num10) / 1000.0);
                                num6 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                close[j] = (float)((num9 + num10 + num6) / 1000.0);
                                num7 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                high[j] = (float)((num9 + num10 + num7) / 1000.0);
                                num8 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                low[j] = (float)((num9 + num10 + num8) / 1000.0);

                                vol[j] = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                                amount[j] = TDX.TDXDecoder.TDXGetDouble(RecvBuffer, i, ref i);
                                num9 = num9 + num10 + num6;
                                //如果是指数 则还记录了上涨与下跌
                                if (isIndex)
                                {
                                    upcount[j] = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                                    downcount[j] = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                                }
                                //logger.Info("O:{0} H:{1} L:{2} C:{3}".Put(open[j], high[j], low[j], close[j]));
                            }

                            Dictionary<string, double[]> tmp = new Dictionary<string, double[]>();
                            tmp["date"] = date;
                            tmp["time"] = time;
                            tmp["open"] = open;
                            tmp["high"] = high;
                            tmp["low"] = low;
                            tmp["close"] = close;
                            tmp["vol"] = vol;
                            tmp["amount"] = amount;

                            if (OnRspQrySecurityBar != null)
                            {
                                OnRspQrySecurityBar(tmp, null, n, sb.RequestId);
                            }
                        }
                        //if (n > 0)
                        //{
                        //    int yy1 = 0, mm1 = 0, dd1 = 0, hhh = 0, mmm = 0;
                        //    date = new double[n + 1];
                        //    time = new double[n + 1];
                        //    high = new double[n + 1];
                        //    low = new double[n + 1];
                        //    open = new double[n + 1];
                        //    close = new double[n + 1];
                        //    amount = new double[n + 1];
                        //    vol = new double[n + 1];
                        //    double[] upcount = new double[n];
                        //    double[] downcount = new double[n];
                        //    bool zs = false;
                        //    if ((sb.sk.type == 100) || (sb.sk.type == 7))//指数
                        //        zs = true;
                        //    i = 2;
                        //    num9 = 0;
                        //    for (int j = 0; j < n; j++)
                        //    {
                        //        Application.DoEvents();
                        //        TDX.TDXDecoder.TDXGetDate(TDX.TDXDecoder.TDXGetInt32(RecvBuffer, i, ref i), ref yy1, ref mm1, ref dd1, ref hhh, ref mmm);
                        //        date[j] = yy1 * 10000 + mm1 * 100 + dd1;
                        //        time[j] = hhh * 100 + mmm;
                        //        num10 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        open[j] = (float)((num9 + num10) / 1000.0);
                        //        num6 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        close[j] = (float)((num9 + num10 + num6) / 1000.0);
                        //        num7 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        high[j] = (float)((num9 + num10 + num7) / 1000.0);
                        //        num8 = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        low[j] = (float)((num9 + num10 + num8) / 1000.0);

                        //        vol[j] = TDX.TDXDecoder.TDXDecode(RecvBuffer, i, ref i);
                        //        amount[j] = TDX.TDXDecoder.TDXGetDouble(RecvBuffer, i, ref i);
                        //        num9 = num9 + num10 + num6;
                        //        if (zs)
                        //        {
                        //            upcount[j] = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        //            downcount[j] = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                        //        }
                        //    }
                        //    if (sb.type == 1000)
                        //    {
                        //        if (GP.Days > 1)
                        //        {
                        //            List1.Items.Clear();
                        //            int t1 = Math.Max(0, n - GP.Days + 1);
                        //            for (int j = t1; j < n; j++)
                        //            {
                        //                List1.Items.Add(date[j].ToString());
                        //                byte[] aaa = { 0xC, 0x1, 0x30, 0x0, 0x1, 0x1, 0xD, 0x0, 0xD, 0x0, 0xB4, 0xF, 0xD1, 0xD2, 0xD3, 0xD4, 0xFF, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36 };
                        //                byte[] sb12 = BitConverter.GetBytes(Convert.ToInt32(date[j]));
                        //                sb12.CopyTo(aaa, 12);
                        //                aaa[16] = FCurStock.mark;
                        //                FCurStock.GP.code.CopyTo(aaa, 17);
                        //                SendBuf sb11 = new SendBuf();
                        //                sb11.sk = FCurStock;
                        //                sb11.Send = aaa;
                        //                sb11.type = 1;
                        //                SendList.Enqueue(sb11);
                        //            }

                        //            byte[] a = { 0xC, 0x1B, 0x8, 0x0, 0x1, 0x1, 0xE, 0x0, 0xE, 0x0, 0x1D, 0x5, 0xFF, 0x0, 0x31, 0x32, 0x33, 0x34, 0x35, 0x36, 0x0, 0x0, 0x0, 0x0 };
                        //            a[12] = FCurStock.mark;
                        //            FCurStock.GP.code.CopyTo(a, 14);
                        //            SendBuf sb21 = new SendBuf();
                        //            sb21.Send = a;
                        //            sb21.sk = FCurStock;
                        //            SendList.Enqueue(sb21);
                        //        }
                        //        return;
                        //    }
                        //    if (StockBoard.Visible)
                        //    {
                        //        int ll = GP.LeftBar;

                        //        GP.K_cleardata();
                        //        sk = sb.sk;
                        //        if (sk != null)
                        //        {
                        //            GP.PreClose = sk.GP.YClose;
                        //            if (sk.now.last != 0)
                        //                GP.PreClose = sk.now.last;
                        //            GP.StkCode = sk.codes;
                        //            GP.StkName = sk.names;
                        //            GP.StkWeek = WeekString[zq];
                        //        }

                        //        GP.AddAll("date", date, n, false);
                        //        GP.AddAll("time", time, n, false);
                        //        GP.AddAll("high", high, n, false);
                        //        GP.AddAll("low", low, n, false);
                        //        GP.AddAll("open", open, n, false);
                        //        GP.AddAll("close", close, n, false);
                        //        GP.AddAll("amount", amount, n, false);
                        //        if (zs)
                        //        {
                        //            GP.AddAll("upcount", upcount, n, false);
                        //            GP.AddAll("downcount", downcount, n, false);
                        //        }
                        //        GP.AddAll("vol", vol, n, true);
                        //        if (ll > 0)
                        //            GP.LeftBar = ll;
                        //    }
                        //    if (MutiBoard.Visible)
                        //    {
                        //        sk = sb.sk;
                        //        for (int j = 0; j < mgp.Length; j++)
                        //        {
                        //            if (msk[j] == sk)
                        //            {
                        //                mgp[j].BeginUpdate();
                        //                mgp[j].K_cleardata();
                        //                mgp[j].PreClose = sk.GP.YClose;
                        //                if (sk.now.last != 0)
                        //                    mgp[j].PreClose = sk.now.last;
                        //                mgp[j].StkCode = sk.codes;
                        //                mgp[j].StkName = sk.names;
                        //                mgp[j].StkWeek = WeekString[zq];
                        //                mgp[j].AddAll("date", date, n, false);
                        //                mgp[j].AddAll("time", time, n, false);
                        //                mgp[j].AddAll("high", high, n, false);
                        //                mgp[j].AddAll("low", low, n, false);
                        //                mgp[j].AddAll("open", open, n, false);
                        //                mgp[j].AddAll("close", close, n, false);
                        //                mgp[j].AddAll("amount", amount, n, false);
                        //                if (zs)
                        //                {
                        //                    mgp[j].AddAll("upcount", upcount, n, false);
                        //                    mgp[j].AddAll("downcount", downcount, n, false);
                        //                }
                        //                mgp[j].AddAll("vol", vol, n, true);
                        //                break;
                        //            }
                        //        }
                        //    }
                        //}
                        break;
                        #endregion
                        _profiler.LeaveSection();

                    case 0x2cf://F10资料类别
                        {
                            i = 0;
                            n = TDX.TDXDecoder.TDXGetInt16(RecvBuffer, i, ref i);
                            List<SymbolInfoType> typelist = new List<SymbolInfoType>();
                            if (n > 0)
                            {
                                i = 2;
                                for (int j = 0; j < n; j++)
                                {
                                    Array.Copy(RecvBuffer, i, name, 0, 8);
                                    i = i + 64;
                                    Array.Copy(RecvBuffer, i, code, 0, 6);
                                    i = i + 80;
                                    int start = TDX.TDXDecoder.TDXGetInt32(RecvBuffer, i, ref i);
                                    int len = TDX.TDXDecoder.TDXGetInt32(RecvBuffer, i, ref i);
                                    names = System.Text.Encoding.GetEncoding("GB2312").GetString(name);
                                    codes = System.Text.Encoding.GetEncoding("GB2312").GetString(code);
                                    //info[j].name = names;
                                    //info[j].code = codes;
                                    //info[j].start = start;
                                    //info[j].len = Len;
                                    //Binfo[j].Text = names;
                                    typelist.Add(new SymbolInfoType(codes, names, start, len, j));
                                }

                                if (OnRspQrySymbolInfoType != null)
                                {
                                    OnRspQrySymbolInfoType(typelist, null, n, sb.RequestId);
                                }

                                //bt1_Click(Binfo[0], null);
                            }
                        }
                        break;
                    case 0x2d0://F10资料
                        int k = RecvBuffer.Length - 24;
                        byte[] txt = new byte[k];
                        Array.Copy(RecvBuffer, 24, txt, 0, k);
                        string msg = System.Text.Encoding.GetEncoding("GB2312").GetString(txt);
                        if (OnRspQrySymbolInfo != null)
                        {
                            OnRspQrySymbolInfo(msg, null, 1, sb.RequestId);
                        }
                        break;
                }
            }
            catch
            {
            }
        }

    }
}
