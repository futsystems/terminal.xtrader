using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TradingLib.API;
using TradingLib.Common;
using Common.Logging;

namespace TradingLib.TraderCore
{
    /// <summary>
    /// 核心事件
    /// 
    /// </summary>
    public class EventCore
    {
        static ILog logger = LogManager.GetLogger("EventCore");

        /// <summary>
        /// 基础数据与帐户列表数据初始化完成事件
        /// </summary>
        public event VoidDelegate OnInitializedEvent;
        event VoidDelegate _OnInitializedEvent;
        internal void FireInitializedEvent()
        {
            LogService.Debug("FireInitializedEvent");
            //先调用本地初始化完成依赖回调
            if (_OnInitializedEvent != null)
            {
                _OnInitializedEvent();
            }
            //调用其他初始化完成事件订阅回调
            if (OnInitializedEvent != null)
                OnInitializedEvent();
        }


        /// <summary>
        /// 注册初始化完成依赖调用
        /// 在相关界面创建过程中 有些界面在初始化完成之前创建，有些在初始化完成之后创建
        /// 初始化完成之前创建的需要在初始化完成之后获得基础数据填充界面
        /// </summary>
        /// <param name="callback"></param>
        void RegisterInitializedCallBack(VoidDelegate callback)
        {
            if (!CoreService.Initialized)
            {
                _OnInitializedEvent += new VoidDelegate(callback);
            }
            else
            {
                callback();
            }
        }

        /// <summary>
        /// 注册EventHandler用于执行事件注册与延迟加载
        /// 如果内部核心数据初始化完毕则直接调用否则延迟到数据加载完毕进行调用
        /// </summary>
        /// <param name="control"></param>
        public void RegIEventHandler(object control)
        {
            if (control is UserControl)
            {
                if (control is IEventBinder)
                {
                    IEventBinder h = control as IEventBinder;
                    //注册初始化完成事件响应函数 用于响应初始化完成事件 当对象在初始化完成之前创建 需要在完成初始化后 加载基础数据
                    RegisterInitializedCallBack(h.OnInit);
                    //将组件销毁的事件与对应的注销函数进行绑定
                    (control as UserControl).Disposed += (s, e) => { h.OnDisposed(); };
                }
            }

            if (control is Form)
            {
                if (control is IEventBinder)
                {
                    IEventBinder h = control as IEventBinder;
                    //注册初始化完成事件响应函数 用于响应初始化完成事件 当对象在初始化完成之前创建 需要在完成初始化后 加载基础数据
                    LogService.Debug("EventCore Register EventHandler:" + control.ToString());
                    RegisterInitializedCallBack(h.OnInit);
                    //将组件销毁的事件与对应的注销函数进行绑定
                    (control as Form).Disposed += (s, e) => { h.OnDisposed(); };
                }
            }
        }




        /// <summary>
        /// 通讯连接建立事件
        /// </summary>
        public event VoidDelegate OnConnectedEvent;
        internal void FireConnectedEvent()
        {
            //LogService.Debug("FireConnectedEvent ***");
            if (OnConnectedEvent != null)
                OnConnectedEvent();
        }

        /// <summary>
        /// 通讯连接断开事件
        /// </summary>
        public event VoidDelegate OnDisconnectedEvent;
        internal void FireDisconnectedEvent()
        {
            //LogService.Debug("FireDisconnectedEvent");
            if (OnDisconnectedEvent != null)
                OnDisconnectedEvent();
        }


        public event Action<LoginResponse> OnLoginEvent;
        internal void FireLoginEvent(LoginResponse response)
        {
            //LogService.Debug("FireLoginEvent");
            if (OnLoginEvent != null)
                OnLoginEvent(response);
        }


        /// <summary>
        /// 登入状态变化事件
        /// </summary>
        public event Action<string> OnInitializeStatusEvent;
        internal void FireInitializeStatusEvent(string msg)
        {
            if (OnInitializeStatusEvent != null)
                OnInitializeStatusEvent(msg);
        }

        public event Action<RspInfo> OnRspInfoEvent;
        internal void FireRspInfoEvent(RspInfo info)
        {
            LogService.Debug("FireRspInfoEvent");
            if (OnRspInfoEvent != null)
                OnRspInfoEvent(info);
        }

        public event Action<PromptMessage> OnPromptMessageEvent;
        internal void FirePromptMessageEvent(PromptMessage msg)
        {
            if (OnPromptMessageEvent != null)
            {
                OnPromptMessageEvent(msg);
            }
        }


        #region 扩展命令处理
        /// <summary>
        /// 注册Request回调函数
        /// </summary>
        /// <param name="module"></param>
        /// <param name="cmd"></param>
        /// <param name="del"></param>
        public void RegisterCallback(string module, string cmd, Action<RspInfo, string, bool> del)
        {
            string key = module.ToUpper() + "-" + cmd.ToUpper();

            if (!callbackmap.Keys.Contains(key))
            {
                callbackmap.TryAdd(key, new List<Action<RspInfo,string, bool>>());
            }
            callbackmap[key].Add(del);

        }

        /// <summary>
        /// 注册Notify回调函数
        /// </summary>
        /// <param name="module"></param>
        /// <param name="cmd"></param>
        /// <param name="del"></param>
        public void RegisterNotifyCallback(string module, string cmd, Action<string> del)
        {
            string key = module.ToUpper() + "-" + cmd.ToUpper();

            if (!notifycallbackmap.Keys.Contains(key))
            {
                notifycallbackmap.TryAdd(key, new List<Action<string>>());
            }

            notifycallbackmap[key].Add(del);

        }



        /// <summary>
        /// 注销Request回调函数
        /// </summary>
        /// <param name="module"></param>
        /// <param name="cmd"></param>
        /// <param name="del"></param>
        public void UnRegisterCallback(string module, string cmd, Action<RspInfo,string, bool> del)
        {
            string key = module.ToUpper() + "-" + cmd.ToUpper();

            if (!callbackmap.Keys.Contains(key))
            {
                callbackmap.TryAdd(key, new List<Action<RspInfo,string, bool>>());
            }

            if (callbackmap[key].Contains(del))
            {
                callbackmap[key].Remove(del);
            }
        }

        /// <summary>
        /// 注销Notify回调函数
        /// </summary>
        /// <param name="module"></param>
        /// <param name="cmd"></param>
        /// <param name="del"></param>
        public void UnRegisterNotifyCallback(string module, string cmd, Action<string> del)
        {
            string key = module.ToUpper() + "-" + cmd.ToUpper();

            if (!notifycallbackmap.Keys.Contains(key))
            {
                notifycallbackmap.TryAdd(key, new List<Action<string>>());
            }
            if (notifycallbackmap[key].Contains(del))
            {
                notifycallbackmap[key].Remove(del);
            }
        }


        ConcurrentDictionary<string, List<Action<string>>> notifycallbackmap = new ConcurrentDictionary<string, List<Action<string>>>();
        ConcurrentDictionary<string, List<Action<RspInfo, string, bool>>> callbackmap = new ConcurrentDictionary<string, List<Action<RspInfo,string, bool>>>();
        /// <summary>
        /// 响应服务端的扩展回报 通过扩展模块ID 操作码 以及具体的json回报内容
        /// </summary>
        /// <param name="module"></param>
        /// <param name="cmd"></param>
        /// <param name="result"></param>
        internal void GotContribResponse(string module, string cmd,RspInfo info, string result, bool islast)
        {
            string key = module.ToUpper() + "-" + cmd.ToUpper();
            if (callbackmap.Keys.Contains(key))
            {
                foreach (Action<RspInfo,string, bool> del in callbackmap[key])
                {
                    try
                    {
                        del(info,result, islast);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("ContribResponse Callback Error", ex);
                    }
                }
            }
            else
            {
                logger.Warn("do not have any callback for " + key + " registed!");
            }
        }

        /// <summary>
        /// 响应服务端的通知回报
        /// </summary>
        /// <param name="module"></param>
        /// <param name="cmd"></param>
        /// <param name="result"></param>
        internal void GotContribNotifyResponse(string module, string cmd, string result)
        {
            string key = module.ToUpper() + "-" + cmd.ToUpper();
            if (notifycallbackmap.Keys.Contains(key))
            {
                foreach (Action<string> del in notifycallbackmap[key])
                {
                    try
                    {
                        del(result);
                    }
                    catch (Exception ex)
                    {
                        logger.Error("ContribNotifyResponse Callback Error", ex);
                    }
                }
            }
            else
            {
                logger.Warn("do not have any callback for " + key + " registed!");
            }
        }
        #endregion
    }
}
