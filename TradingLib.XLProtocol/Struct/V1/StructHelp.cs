﻿using System;
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

        public static byte[] StructToBytes(IXLField field)
        {
            XLFieldType fieldType = (XLFieldType)field.FieldID;
            switch (fieldType)
            {
                case XLFieldType.F_ERROR:
                    return XLStructHelp.StructToBytes<ErrorField>((ErrorField)field);
                case XLFieldType.F_REQ_LOGIN:
                    return XLStructHelp.StructToBytes<XLReqLoginField>((XLReqLoginField)field);
                case XLFieldType.F_RSP_LOGIN:
                    return XLStructHelp.StructToBytes<XLRspLoginField>((XLRspLoginField)field);
                case XLFieldType.F_REQ_UPDATEPASS:
                    return XLStructHelp.StructToBytes<XLReqUserPasswordUpdateField>((XLReqUserPasswordUpdateField)field);
                case XLFieldType.F_RSP_UPDATEPASS:
                    return XLStructHelp.StructToBytes<XLRspUserPasswordUpdateField>((XLRspUserPasswordUpdateField)field);
                default:
                    throw new Exception(string.Format("FieldType:{0} not supported", fieldType));
            }
        }

        public static IXLField BytesToStruct(byte[] data, int offset, XLFieldType type)
        {
            switch (type)
            {
                case XLFieldType.F_ERROR:
                    return XLStructHelp.BytesToStruct<ErrorField>(data, offset);
                case XLFieldType.F_REQ_LOGIN:
                    return XLStructHelp.BytesToStruct<XLReqLoginField>(data, offset);
                case XLFieldType.F_RSP_LOGIN:
                    return XLStructHelp.BytesToStruct<XLRspLoginField>(data, offset);
                case XLFieldType.F_REQ_UPDATEPASS:
                    return XLStructHelp.BytesToStruct<XLReqUserPasswordUpdateField>(data, offset);
                case XLFieldType.F_RSP_UPDATEPASS:
                    return XLStructHelp.BytesToStruct<XLRspUserPasswordUpdateField>(data, offset);
                default:
                    throw new Exception(string.Format("FieldType:{0} not supported", type));
            }
        }
    }
}
