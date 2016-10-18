using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;
using System.Text;
using TradingLib.MarketData;

namespace DataAPI.TDX
{
    public partial class TDXDataAPI
    {
        

        #region 后台维护线程


        Thread _bwthread = null;
        bool _bwgo = false;
        void StartWatchDog()
        {
            if (_bwgo) return;

            _bwgo = true;
            _bwthread = new Thread(_bw_DoWork);
            _bwthread.IsBackground = true;
            _bwthread.Start();
            logger.Info("Watcher backend threade started");
        }

        void StopWatchDog()
        {
            if (!_bwgo) return;

            _bwgo = false;
            _bwthread.Abort();
            int _wait = 0;
            while (_bwthread.IsAlive && _wait < 5)
            {
                logger.Info("Waiting Watchdog thread stop....");
                _wait++;
                Thread.Sleep(200);

            }
            _bwthread = null;
            logger.Info("Watcher backend threade stopped");
        }

        DateTime _lastHeartbeatSent = DateTime.MinValue;
        DateTime _lastheartbeat = DateTime.Now;
        bool _reconnectreq = false;
        /// <summary>
        /// 心跳维护线程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void _bw_DoWork()
        {
            while (_bwgo)
            {
                // 获得当前时间
                //long now = DateTime.Now.Ticks;
                //计算上次heartbeat以来的时间间隔
                double diff = (DateTime.Now- _lastheartbeat).TotalSeconds;// / 10000;//(ticks/10000得到MS)
                //logger.Info("连接:" + _connect.ToString() + " 请求重新连接:" + (_reconnectreq).ToString() + "心跳间隔"+(diff < _sendheartbeat).ToString()+" 上次心跳时间:" + _lastheartbeat.ToString() + " Diff:" + diff.ToString() + " 发送心跳间隔:" + _sendheartbeat.ToString());
                //服务端处于连接状态 服务度不处重连状态 服务端心跳间隔小于设定间隔
                if (!(_connected &&  (!_reconnectreq) && (diff < 3)))//任何一个条件不满足将进行下面的操作
                {
                    //如果心跳当前状态正常,则请求一个心跳 请求后心跳状态处于非正常状态 不会再重复发送请求
                    if (IsHeartbeatOk)
                    {
                        //logger.Info("heartbeat request at: " + DateTime.Now.ToString()+" _heartbeatdeadat:"+_heartbeatdeadat.ToString() + " _diff:"+diff.ToString());
                        //当得到响应请求后,_recvheartbeat = !_recvheartbeat; 因此在发送了一个hearbeatrequest后 在没有得到服务器反馈前不会再次重新发送
                        //logger.Info("???");
                        RequestHeartBeatRequest();
                    }
                    else if (diff > 9)//心跳间隔超时后,我们请求服务端的心跳回报,如果服务端的心跳响应超过心跳死亡时间,则我们尝试 重新建立连接
                    {
                        //logger.Info("HeartBeat Dead try to reconnect");
                        //logger.Info("xxxxxxxxxxxxxxx diff:" + diff.ToString() + " dead:" + _heartbeatdeadat.ToString());
                        StartReconnect();
                    }
                }

                Thread.Sleep(250);
            }
        }
        #endregion 

        void RequestHeartBeatRequest()
        {
            //logger.Info("HeartBeat Request");

            //设置请求状态与接收状态相反 当收到心跳回报后将请求状态设置成接收状态
            _requestheartbeat = !_recvheartbeat;

            QrySeurityBars("SSE", "999999", ConstFreq.Freq_Day, 0, 1, 1000);
        }

        void OnHeartBeatResponse()
        {
            _lastheartbeat = DateTime.Now;
            //logger.Info("HeartBeat Response");
            _recvheartbeat = !_recvheartbeat;  
        }

        Thread _reconnectThread = null;
        void StartReconnect()
        {
            if (_reconnectreq) return;
            logger.Info("Start reconnect thread");
            _reconnectreq = true;

            _reconnectThread = new Thread(Reconnect);
            _reconnectThread.IsBackground = true;
            _reconnectThread.Start();
        }

        void StopReconnect(bool wait = false)
        {
            if (!_reconnectreq) return;
            logger.Info("Stop reconnect thread");
            _reconnectreq = false;
            if (wait)
            {
                _reconnectThread.Join();
            }
            else
            {
                _reconnectThread.Abort();
                _reconnectThread = null;
            }
        }

        void Reconnect()
        {
            Disconnect();
            Connect(_hosts, _port);
        }
    }
}
