using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.TraderCore
{
    public enum EnumMessageLevel
    { 
        Error,
        Warn,
        Info,
    }

    /// <summary>
    /// 提示信息
    /// </summary>
    public class PromptMessage
    {
        public PromptMessage(string title, string message, EnumMessageLevel level= EnumMessageLevel.Error)
        {
            this.Title = title;
            this.Message = message;
            this.Level = level;

        }
        public EnumMessageLevel Level { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }
    }
}
