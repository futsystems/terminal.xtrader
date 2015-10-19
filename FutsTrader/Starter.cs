using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Ant.Component;
using System.Windows.Forms;
namespace FutsTrader
{
    public class Starter : SplashScreenApplicationContext
    {
        LoginForm _loginform;
        //用于调用升级逻辑,然后再显示启动窗口与主窗口
        protected override bool OnUpdate()
        {
            //没有更新我们返回false 程序正常运行
            Updater update = new Updater();
            //MessageBox.Show("start to here");
            if (update.Detect())
            {
                //MessageBox.Show("detected");
                update.Update("XTrader.exe", true);
                return true;
            }
            else
            {
                return false;
            }

        }
        protected override void OnActiveMainForm()
        {

        }
        /// <summary>
        /// 初始化登入窗体
        /// </summary>
        protected override void OnCreateSplashScreenForm()
        {
            _loginform = new LoginForm(this);
            this.SplashScreenForm = _loginform;//启动窗体
        }


        

        protected override void OnCreateMainForm()
        {
            //在线程中创建主窗体,防止登入界面卡顿
            new Thread(delegate()
            {
                this.PrimaryForm = new TradeLite();

                //主窗体初始化完毕后 开启登入按钮
                _loginform.EnableLogin();
            }).Start();
        }

        protected override void SetSeconds()
        {
            this.SecondsShow = 60 * 60;//启动窗体显示的时间(秒)
        }
    }
}
