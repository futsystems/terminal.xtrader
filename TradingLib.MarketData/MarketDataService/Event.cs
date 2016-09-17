using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Common.Logging;
using TradingLib.MarketData;
using System.Windows.Forms;

namespace TradingLib.MarketData
{
    public class EventHub
    {
        ILog logger = LogManager.GetLogger("EventHub");

        /// <summary>
        /// 基础数据与帐户列表数据初始化完成事件
        /// </summary>
        public event Action OnInitializedEvent;
        event Action _OnInitializedEvent;
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
        void RegisterInitializedCallBack(Action callback)
        {
            if (!MDService.Initialized)
            {
                _OnInitializedEvent += new Action(callback);
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
        public event Action OnConnectedEvent;
        internal void FireConnectedEvent()
        {
            logger.Debug("FireConnectedEvent ***");
            if (OnConnectedEvent != null)
                OnConnectedEvent();
        }

        /// <summary>
        /// 通讯连接断开事件
        /// </summary>
        public event Action OnDisconnectedEvent;
        internal void FireDisconnectedEvent()
        {
            logger.Debug("FireDisconnectedEvent");
            if (OnDisconnectedEvent != null)
                OnDisconnectedEvent();
        }

        public event Action<MDLoginResponse> OnLoginEvent;
        internal void FireLoginEvent(MDLoginResponse response)
        {
            logger.Debug("FireLoginEvent");
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


    }
}
