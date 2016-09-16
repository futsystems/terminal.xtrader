using System;
using System.Collections.Generic;
using System.Text;

namespace CStock
{
    [Serializable()]
    public class StockTech
    {
        public string techname;
        public string techtitle;
        public string techpass;
        public int version=1;
        public bool drawk=false;
        public string techtype;//公式分类
        public int digit=0;//缺省位数,品种小数位数,品种小数位数+1,固定0位,固定1位,固定2位,固定3位,固定4位
        public Tinput[] input = new Tinput[16];
        public string yStr;//Y轴自定义坐标
        public float[] yValue = new float[4];//Y轴额外分界
        public string content;//指标内容 
        public string note;//注释内容 
        public string parastr;//参数精灵
    }


}
