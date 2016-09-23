using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TradingLib.XTrader.Control
{
    /// <summary>
    /// 报价表列信息
    /// </summary>
    public class QuoteColumn
    {
        /// <summary>
        /// 获得某个Enum的描述
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        static string GetEnumDescription(object e)
        {
            //获取字段信息 
            System.Reflection.FieldInfo[] ms = e.GetType().GetFields();
            Type t = e.GetType();
            foreach (System.Reflection.FieldInfo f in ms)
            {
                //判断名称是否相等 
                if (f.Name != e.ToString()) continue;
                //反射出自定义属性 
                foreach (Attribute attr in f.GetCustomAttributes(true))
                {
                    //类型转换找到一个Description，用Description作为成员名称 
                    System.ComponentModel.DescriptionAttribute dscript = attr as System.ComponentModel.DescriptionAttribute;
                    if (dscript != null)
                        return dscript.Description;
                }
            }
            //如果没有检测到合适的注释，则用默认名称 
            return e.ToString();
        }

        int GetDefaultWidth(EnumFileldType type)
        {
            switch (type)
            { 
                case EnumFileldType.INDEX:
                    return 60;
                case EnumFileldType.SYMBOL:
                    return 80;
                case EnumFileldType.SYMBOLNAME:
                    return 100;
                case EnumFileldType.LAST:
                case EnumFileldType.BID:
                case EnumFileldType.ASK:
                    return 70;
                case EnumFileldType.LASTSIZE:
                case EnumFileldType.BIDSIZE:
                case EnumFileldType.ASKSIZE:
                    return 40;
                case EnumFileldType.VOL:
                case EnumFileldType.OI:
                    return 80;
                default:
                    return 70;

            }
        }

        public QuoteColumn(EnumFileldType type)
        {
            this.FieldType = type;
            this.Title = GetEnumDescription(this.FieldType);
            this.StartX = 0;
            this.Width = GetDefaultWidth(this.FieldType);
            this.Visible = true;
            this.Index = -1;

        }

        public int Index { get; set; }
        /// <summary>
        /// 标体头
        /// </summary>
        public string Title { get; private set;}

        /// <summary>
        /// 字段类别
        /// </summary>
        public EnumFileldType FieldType { get; private set; } 

        /// <summary>
        /// 起点X坐标
        /// </summary>
        public int StartX { get; set; }

        /// <summary>
        /// 列宽度
        /// </summary>
        public int Width { get; set; }


        /// <summary>
        /// 是否可见
        /// </summary>
        public bool Visible { get; set; }
    }
}
