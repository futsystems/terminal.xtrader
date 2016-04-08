using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.API;
using TradingLib.Common;
using TradingLib.KryptonControl;


namespace DataFarmMgr
{
    public class PageHolder
    {
        List<IPage> _pagelist = new List<IPage>();

        Dictionary<string, IPage> _pageMap = new Dictionary<string, IPage>();
        public PageHolder()
        { 
        
        }

        /// <summary>
        /// 获得某个页面
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IPage GetPage(string name)
        {
            IPage target = null;
            if (_pageMap.TryGetValue(name.ToUpper(), out target))
                return target;
            return null;
        }

        /// <summary>
        /// 添加一个页面
        /// </summary>
        /// <param name="page"></param>
        public void AddPage(IPage page)
        {
            _pageMap.Add(page.PageName.ToUpper(), page);
        }

        
        /// <summary>
        /// 显示某个页面
        /// </summary>
        /// <param name="name"></param>
        public void ShowPage(string name)
        {
            IPage page = GetPage(name);
            if (page != null)
            {
                foreach (var item in _pageMap.Values)
                {
                    item.Hide();
                }
                page.Show();
                page.Focus();
            }
        }

    }
}
