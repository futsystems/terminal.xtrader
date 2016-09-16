using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Runtime.InteropServices;

namespace DataAPI.TDX
{
    public class TDXDecoder
    {

        /// <summary>
        /// 估算某个时间点到目前有多少个Bar数据
        /// </summary>
        /// <param name="date"></param>
        /// <param name="time"></param>
        /// <param name="freq">0->5分钟K线    1->15分钟K线    2->30分钟K线  3->1小时K线    4->日K线  5->周K线  6->月K线  7->1分钟    10->季K线  11->年K线</param>
        /// <returns></returns>
        public static int RequestCount(DateTime lasttime, int freq)
        {
            if (freq == 7)
            {
                int minute = (int)DateTime.Now.Subtract(lasttime).TotalMinutes;
                return minute + 1;
            }

            if (freq == 0)
            {
                int minute = (int)DateTime.Now.Subtract(lasttime).TotalMinutes;
                return minute / 5 + 1;
            }
            if (freq == 1)
            {
                int minute = (int)DateTime.Now.Subtract(lasttime).TotalMinutes;
                return minute / 15 + 1;
            }
            if (freq == 2)
            {
                int minute = (int)DateTime.Now.Subtract(lasttime).TotalMinutes;
                return minute / 30 + 1;
            }
            if (freq == 3)
            {
                int minute = (int)DateTime.Now.Subtract(lasttime).TotalMinutes;
                return minute / 60 + 1;
            }

            if (freq == 4)
            {
                int day = (int)DateTime.Now.Subtract(lasttime).TotalDays;
                return day + 1;
            }
            if (freq == 5)
            {
                int day = (int)DateTime.Now.Subtract(lasttime).TotalDays;
                return day/7 + 1;
            }
            if (freq == 6)
            {
                int day = (int)DateTime.Now.Subtract(lasttime).TotalDays;
                return day / 31 + 1;
            }

            if (freq == 10)
            {
                int day = (int)DateTime.Now.Subtract(lasttime).TotalDays;
                return day / 93 + 1;
            }
            if (freq == 11)
            {
                int day = (int)DateTime.Now.Subtract(lasttime).TotalDays;
                return day / 365 + 1;
            }
            return 800;

        }
        //1:=A股 2:=B股 5:=中小 6:=创业 3:=债券  7:=指数 10=权证 4:=基金 8:三板
        /// <summary>
        /// 根据代码以及市场 判断板块类别
        /// </summary>
        /// <param name="stkcode"></param>
        /// <param name="stkmark"></param>
        /// <returns></returns>
        public static int GetStockType(int market,string stkcode)
        {
            int k = 10;
            int val = Convert.ToInt32(stkcode);
            if (market == 0)//深圳
            {
                k = 10;
                if ((val >= 000001) && (val < 002000))
                    k = 1;
                if ((val >= 002000) && (val < 003000))
                    k = 5;
                if ((val >= 070000) && (val < 140000))
                    k = 3;
                if ((val >= 150000) && (val < 190000))
                    k = 4;
                if ((val >= 200000) && (val < 300000))
                    k = 2;
                if ((val >= 300000) && (val < 390000))
                    k = 6;
                if ((val >= 360000) && (val < 370000))
                    k = 3;
                if ((val >= 390000) && (val < 400000))
                    k = 7;
                if ((val >= 400000) && (val < 500000))
                    k = 8;
                if ((val >= 800000) && (val < 900000))
                    k = 9;
            }
            if (market == 1) //上海
            {
                k = 10;
                if ((val >= 000001) && (val <= 001000))
                    k = 7;
                if ((val >= 010000) && (val < 021000))
                    k = 3;
                if ((val >= 090000) && (val < 205000))
                    k = 3;
                if ((val >= 500000) && (val < 600000))
                    k = 4;
                if ((val >= 600000) && (val < 604000))
                    k = 1;
                if ((val >= 700000) && (val < 800000))
                    k = 3;
                if ((val >= 800000) && (val < 900000))
                    k = 9;
                if ((val >= 900901) && (val < 901000))
                    k = 2;
                if ((val >= 930000) && (val <= 999999))
                    k = 7;
            }
            return k;
        }


        /// <summary>
        /// byte数组转结构体
        /// </summary>
        /// <param name="bytes">byte数组</param>
        /// <param name="offset">开始转换的位置</param>
        /// <param name="type">结构体类型</param>
        /// <returns>转换后的结构体</returns>
        public static object BytesToStuct(byte[] bytes, int offset, Type type)
        {


            //得到结构体的大小
            int size = Marshal.SizeOf(type);
            //byte数组长度小于结构体的大小
            if (size > (bytes.Length - offset))
            {
                //返回空
                return null;
            }
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将byte数组拷到分配好的内存空间
            try
            {
                // 将字节数组复制到结构体指针
                Marshal.Copy(bytes, offset, structPtr, size);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ByteArrayToStructure FAIL: error " + ex.ToString());
            }
            //将内存空间转换为目标结构体
            object obj1 = Marshal.PtrToStructure(structPtr, type);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回结构体
            return obj1;
        }
        /// <summary>
        /// 结构体转byte数组
        /// </summary>
        /// <param name="structObj">要转换的结构体</param>
        /// <returns>转换后的byte数组</returns>
        public static byte[] StructToBytes(object structObj)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(structObj);
            //创建byte数组
            byte[] bytes = new byte[size];
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structObj, structPtr, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;
        }


        public static Int64 EnCodeMark(string stkcode, int stkmark)
        {
            Int64 k = 0;
            string s = stkmark.ToString() + stkcode;
            k = Convert.ToInt64(s);
            return k;
        }



        //解包数据
        public static int TDXDecode(byte[] buf, int start, ref int next)
        {
            int num, num3, num2, num4, num5, num6, num7, num8;
            byte cc;
            num = 0;
            num3 = 0;
            num2 = 0;
            while (num2 < 0x20)
            {
                cc = buf[start + num2];
                num4 = cc;
                num5 = (num4 & 0x80) / 0x80;
                if (num2 == 0)
                {
                    num3 = 1 - (((num4 & 0x40) / 0x40) * 2);
                    num6 = num4 & 0x3F;
                    num = num + num6;
                }
                else if (num2 == 1)
                {
                    num7 = (num4 & 0x7F) * (1 << (num2 * 6));
                    num = num + num7;
                }
                else
                {
                    num8 = (num4 & 0x7F) * (1 << (num2 * 7 - 1));
                    num = num + num8;
                }
                if (num5 == 0)
                {
                    num = num * num3;
                    break;
                }
                num2++;
            }
            next = start + num2 + 1;
            return num;
        }
        //读取16位数据
        public static ushort TDXGetInt16(byte[] buf, int start, ref int next)
        {

            ushort Num = BitConverter.ToUInt16(buf, start);//buf[start+1]*256+buf[start];// (short *)&buf[start];
            next = start + 2;
            return Num;
        }
        //读取32位数据
        public static int TDXGetInt32(byte[] buf, int start, ref int next)
        {
            int Num = BitConverter.ToInt32(buf, start);//(long int *)&buf[start];
            next = start + 4;
            return Num;
        }
        //读取浮点数据float
        public static float TDXGetDouble(byte[] buf, int start, ref int next)
        {
            float d1 = BitConverter.ToSingle(buf, start);// (float*)&buf[start];
            next = start + 4;
            return d1;
        }
        //读取时间：HHMM
        public static int TDXGetTime(byte[] buf, int start, ref int next)
        {
            int i = (int)BitConverter.ToInt16(buf, start);
            next = start + 2;
            int ri, mm, ss;
            mm = (i / 60);
            ss = (i % 60);
            if (ss > 59)
            {
                ss = ss - 60;
                mm++;
            }
            ri = mm * 100 + ss;
            return ri;
        }
        //v 解包成年月日时分
        public static void TDXGetDate(int v, ref int yy, ref int mm, ref int dd, ref int hhh, ref int mmm)
        {
            yy = 2012;
            mm = 1;
            dd = 1;
            hhh = 9;
            mmm = 30;
            if (v > 21000000)
            {
                yy = 2004 + ((v & 0xF800) >> 11);
                int d1 = v & 0x7FF;
                mm = d1 / 100;
                dd = d1 % 100;
                int d2 = v >> 16;
                hhh = d2 / 60;
                mmm = d2 % 60;
            }
            else
            {
                yy = v / 10000;
                mm = (v - yy * 10000) / 100;
                dd = v % 100;
                hhh = 9;
                mmm = 30;
            }
        }


        public static int Decompress(byte[] source, int Len, ref byte[] rv)
        {
            byte[] buffer = new byte[0x800];
            MemoryStream ms = new MemoryStream(source, 2, source.Length - 6);
            DeflateStream zipStream = new DeflateStream(ms, CompressionMode.Decompress);
            MemoryStream stream3 = new MemoryStream();
            while (true)
            {
                int count = zipStream.Read(buffer, 0, buffer.Length);
                if (count <= 0)
                {
                    break;
                }
                stream3.Write(buffer, 0, count);
            }
            zipStream.Close();
            rv = stream3.ToArray();
            stream3.Close();
            return rv.Length;
        }
    }
}
