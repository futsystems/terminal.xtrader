using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CStock
{
    public partial class TStock
    {
        #region 加载指标公式
        /// <summary>
        /// 加载资源里的公式
        /// </summary>
        /// <param name="resourecname">资源名称</param>
        /// <param name="type">1-10对应K线10个窗口 100---均线上图 101--均线下图</param> 
        public void LoadWfc(String ResourecName, int type)
        {
            TGongSi gs1 = null;
            if ((type > 0) && (type < 11))
                gs1 = GS[type - 1];
            if (type > 100)
                gs1 = FSGS[type - 100];
            if (gs1 != null)
                gs1.LoadWfc(ResourecName);
        }

        /// <summary>
        /// 加载外部公式文件
        /// </summary>
        /// <param name="resourecname">文件名称 绝对路径</param>//
        /// <param name="type">1-10对应K线10个窗口 100---均线上图 101--均线下图</param> 
        public void LoadWfcFileName(String WfcFileName, int type)
        {
            TGongSi gs1 = null;
            if ((type > 0) && (type < 11))
                gs1 = GS[type - 1];
            if (type > 100)
                gs1 = FSGS[type - 100];
            if (gs1 != null)
                gs1.loadprogram(WfcFileName);
        }

        #endregion

    }
}
