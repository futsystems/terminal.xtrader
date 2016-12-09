using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TradingLib.XLProtocol;

namespace TradingLib.XLProtocol.V1
{
    /// <summary>
    /// V1协议版本StructHelp
    /// </summary>
    public class StructHelp
    {
        /// <summary>
        /// 将业务数据结构体转换成Byte数组
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public static byte[] StructToBytes(IXLField field)
        {
            XLFieldType fieldType = (XLFieldType)field.FieldID;
            switch (fieldType)
            {
                case XLFieldType.F_REQ_LOGIN:
                    return XLStructHelp.StructToBytes<XLReqLoginField>((XLReqLoginField)field);
                default:
                    return null;
            }
        }

        public static IXLField BytesToStruct(byte[] data, int offset, XLFieldType type)
        {
            switch (type)
            {
                case XLFieldType.F_REQ_LOGIN:
                    return XLStructHelp.BytesToStruct<XLReqLoginField>(data, offset);
                default:
                    return null;
            }
        }
    }
}
