using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace TradingLib.XLProtocol
{
    /// <summary>
    /// 业务结构体数据包
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class XLFieldData<T>
        where T:IXLField
    {
        /// <summary>
        /// 业务数据域头
        /// </summary>
        public XLFieldHeader FieldHeader;

        /// <summary>
        /// 业务数据域
        /// </summary>
        public T FieldData;
    }


    public class XLPacketData
    {

        public XLPacketData(XLMessageType msgType)
        {
            _messageType = msgType;
        }

        public XLPacketData(XLMessageType msgType, List<XLFieldData<IXLField>> fields)
        {
            _messageType = msgType;
            _fieldList.AddRange(fields);
        }

        XLMessageType _messageType = XLMessageType.T_HEARTBEEAT;
        /// <summary>
        /// 消息类别
        /// </summary>
        public XLMessageType MessageType { get { return _messageType; } }



        List<XLFieldData<IXLField>> _fieldList = new List<XLFieldData<IXLField>>();
        /// <summary>
        /// 业务数据结
        /// </summary>
        public List<XLFieldData<IXLField>> FieldList { get { return _fieldList; } }


        /// <summary>
        /// 添加一个业务数据域
        /// </summary>
        /// <param name="field"></param>
        public void AddField(IXLField field)
        {
            XLFieldHeader header = new XLFieldHeader();
            int fieldLen = Marshal.SizeOf(field);
            FillFieldHeader(ref header, field.FieldType, (ushort)fieldLen);

            _fieldList.Add(new XLFieldData<IXLField>() { FieldHeader = header,FieldData = field});
        }



        /// <summary>
        /// 将业务数据包打包成byte数组
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public static byte[] PackToBytes(XLPacketData packet)
        {
            XLProtocolHeader protoHeader = new XLProtocolHeader();
            XLDataHeader dataHeader = new XLDataHeader();

            ushort fieldLength = (ushort)packet.FieldList.Sum(d => d.FieldHeader.FieldLength);
            ushort pktLen = (ushort)(XLConstants.PROTO_HEADER_LEN + XLConstants.DATA_HEADER_LEN + fieldLength);
            ushort fieldCount = (ushort)packet.FieldList.Count;
            FillProtoHeader(ref protoHeader, packet.MessageType, pktLen);

            FillDataHeader(ref dataHeader, XLEnumSeqType.SeqQry, (uint)0, fieldCount, pktLen, true, 0);

            Byte[] data = new Byte[pktLen];

            int offset = 0;
            Array.Copy(XLStructHelp.StructToBytes<XLProtocolHeader>(protoHeader), 0, data, 0, XLConstants.PROTO_HEADER_LEN);
            offset += XLConstants.PROTO_HEADER_LEN;
            Array.Copy(XLStructHelp.StructToBytes<XLDataHeader>(dataHeader), 0, data, offset, XLConstants.DATA_HEADER_LEN);
            offset += XLConstants.DATA_HEADER_LEN;

            //遍历所有业务数据域 转换成byte数组
            foreach (var field in packet.FieldList)
            {
                Array.Copy(XLStructHelp.StructToBytes<XLFieldHeader>(field.FieldHeader), 0, data, offset, XLConstants.FIELD_HEADER_LEN);
                Array.Copy(XLStructHelp.StructToBytes(field.FieldData), 0, data, offset + XLConstants.FIELD_HEADER_LEN, field.FieldHeader.FieldLength);
                offset += (XLConstants.FIELD_HEADER_LEN + field.FieldHeader.FieldLength);
            }

            return data;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="data"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static XLPacketData Deserialize(XLMessageType type,byte[] data,int offset)
        { 
            int _offset = offset;
            XLDataHeader dataHeader = XLStructHelp.BytesToStruct<XLDataHeader>(data, _offset);

            List<XLFieldData<IXLField>> fieldList = ParsePktDataV12(data, _offset, dataHeader.FieldCount);

            return new XLPacketData(type, fieldList);
        }

        /// <summary>
        /// 从数据包中解析 业务数据
        /// </summary>
        /// <param name="data"></param>
        /// <param name="len"></param>
        /// <param name="cnt"></param>
        /// <returns></returns>
        static List<XLFieldData<IXLField>> ParsePktDataV12(byte[] data, int offset,ushort cnt)
        {
            List<XLFieldData<IXLField>> list = new List<XLFieldData<IXLField>>();
            int _offset = offset;
            for (int i = 0; i < cnt; i++)
            {
                XLFieldHeader fieldHeader = XLStructHelp.BytesToStruct<XLFieldHeader>(data, _offset);
                XLFieldType fieldType = (XLFieldType)fieldHeader.FieldID;
                IXLField fieldData = XLStructHelp.BytesToStruct(data, _offset + XLConstants.FIELD_HEADER_LEN,fieldType);

                list.Add(new XLFieldData<IXLField> { FieldHeader = fieldHeader, FieldData = fieldData});

                offset += XLConstants.FIELD_HEADER_LEN + fieldHeader.FieldLength;
            }
            return list;
        }


        /// <summary>
        /// 将业务数据包打包成Json字符串
        /// </summary>
        /// <param name="packet"></param>
        /// <returns></returns>
        public static string PackToJson(XLPacketData packet)
        {
            return string.Empty;
        }

        /// <summary>
        /// 填充协议头
        /// </summary>
        /// <param name="header"></param>
        /// <param name="messageType"></param>
        /// <param name="pktLen"></param>
        static void FillProtoHeader(ref XLProtocolHeader header, XLMessageType messageType, ushort pktLen)
        {
            header.XLMessageType = (short)messageType;
            header.XLMessageLength = (ushort)(pktLen - XLConstants.PROTO_HEADER_LEN);//数据长度 = 总长度 - 协议头长度
        }

        /// <summary>
        /// 填充数据头
        /// </summary>
        /// <param name="header"></param>
        /// <param name="isLast"></param>
        /// <param name="seqType"></param>
        /// <param name="seqNo"></param>
        /// <param name="requestId"></param>
        /// <param name="fieldCount"></param>
        /// <param name="pktlen"></param>
        static void FillDataHeader(ref XLDataHeader header, XLEnumSeqType seqType, uint seqNo, ushort fieldCount, ushort pktlen, bool isLast, uint requestId)
        {
            header.Enctype = XLConstants.XL_VER_1;
            header.Version = XLConstants.XL_ENC_NONE;
            header.IsLast = (byte)(isLast?1:0);
            header.SeqType = (byte)seqType;
            header.SeqNo = seqNo;
            header.RequestID = requestId;
            header.FieldCount = fieldCount;
            header.FieldLength = (ushort)(pktlen - XLConstants.PROTO_HEADER_LEN - XLConstants.DATA_HEADER_LEN);
            
        }
        /// <summary>
        /// 填充业务域头
        /// </summary>
        /// <param name="header"></param>
        /// <param name="fieldType"></param>
        /// <param name="fieldLen"></param>
        static void FillFieldHeader(ref XLFieldHeader header,XLFieldType fieldType,ushort fieldLen)
        {
            header.FieldID = (ushort)fieldType;
            header.FieldLength = fieldLen;
        }
    }
}
