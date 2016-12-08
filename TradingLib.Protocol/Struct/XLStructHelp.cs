using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TradingLib.XLProtocol
{
    public static class XLStructHelp
    {

        /// <summary>
        /// 将业务数据结构体转换成Byte数组
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static byte[] StructToBytes(IXLField field)
        {
            switch (field.FieldType)
            { 
                case XLFieldType.F_REQ_LOGIN:
                    return StructToBytes<XLReqLoginField>((XLReqLoginField)field);
                default:
                    return null;
            }
        }

        public static Byte[] StructToBytes<T>(T obj)// where T : IByteSwap
        {
            T structure = (T)obj;
            //structure.Swap();
            Int32 size = Marshal.SizeOf(structure);
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(structure, buffer, false);
                Byte[] bytes = new Byte[size];
                Marshal.Copy(buffer, bytes, 0, size);
                return bytes;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }
        public static T BytesToStruct<T>(Byte[] bytes, int offset = 0) //where T : struct,IByteSwap
        {
            Int32 size = Marshal.SizeOf(typeof(T));
            IntPtr buffer = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.Copy(bytes, offset, buffer, size);
                T obj = (T)Marshal.PtrToStructure(buffer, typeof(T));
                //obj.Swap();
                return obj;
            }
            finally
            {
                Marshal.FreeHGlobal(buffer);
            }
        }



        public static UInt16 ReverseBytes(UInt16 value)
        {
            return (UInt16)((value & 0xFFU) << 8 | (value & 0xFF00U) >> 8);
        }
        public static Int16 ReverseBytes(Int16 value)
        {
            return (Int16)ReverseBytes((UInt16)value);
        }
        // 翻转字节顺序 (32-bit)
        public static UInt32 ReverseBytes(UInt32 value)
        {
            return (value & 0x000000FFU) << 24 | (value & 0x0000FF00U) << 8 |
                   (value & 0x00FF0000U) >> 8 | (value & 0xFF000000U) >> 24;
        }
        public static Int32 ReverseBytes(Int32 value)
        {
            return (Int32)ReverseBytes((UInt32)value);
        }
        // 翻转字节顺序 (64-bit)
        public static UInt64 ReverseBytes(UInt64 value)
        {
            return (value & 0x00000000000000FFUL) << 56 | (value & 0x000000000000FF00UL) << 40 |
                   (value & 0x0000000000FF0000UL) << 24 | (value & 0x00000000FF000000UL) << 8 |
                   (value & 0x000000FF00000000UL) >> 8 | (value & 0x0000FF0000000000UL) >> 24 |
                   (value & 0x00FF000000000000UL) >> 40 | (value & 0xFF00000000000000UL) >> 56;
        }

        public static double ReverseBytes(double value)
        {
            long bit = BitConverter.DoubleToInt64Bits(value);
            long rbit = (long)ReverseBytes((ulong)bit);
            double rdouble = BitConverter.Int64BitsToDouble(rbit);
            return rdouble;
        }
    }
}
