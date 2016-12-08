using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

/*
 *  XLProtocol 为头部定长协议
 * 
 *  
 * |---XLProtocolHeader----|---XLDataHeader---------------|---XLFieldHeader--XLField---1|---XLFieldHeader--XLField---2|......
 *  
 * 
 * XLProtocolHeader 4个字节
 * XLDataHeader 16个字节
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * 
 * */
namespace TradingLib.XLProtocol
{
    /// <summary>
    /// 协议报头 4字节
    /// </summary>
    public struct XLProtocolHeader
    {
        /// <summary>
        /// 数据包类型
        /// </summary>
        public short XLMessageType;

        /// <summary>
        /// 数据包长度
        /// </summary>
        public ushort XLMessageLength;

    }


    /// <summary>
    /// 协议数据头 16个字节
    /// </summary>
    public struct XLDataHeader
    {
        /// <summary>
        /// 压缩类别
        /// </summary>
        public byte Enctype;

        /// <summary>
        /// 版本号
        /// </summary>
        public byte Version;

        /// <summary>
        /// 是否是最后一条回报
        /// </summary>
        public byte IsLast;

        /// <summary>
        /// 响应序列号类别
        /// 1:Req 2:Qry 3:RTN
        /// </summary>
        public byte SeqType;

        /// <summary>
        /// 回报序列号
        /// </summary>
        public uint SeqNo;

        /// <summary>
        /// 请求编号
        /// </summary>
        public uint RequestID;

        /// <summary>
        /// 数据域个数
        /// </summary>
        public ushort FieldCount;

        /// <summary>
        /// 数据长度 未压缩长度 包含 FieldHeader, FieldData
        /// </summary>
        public ushort FieldLength;

        
    }

    /// <summary>
    /// 业务数据头
    /// </summary>
    public struct XLFieldHeader
    {
        /// <summary>
        /// 业务结构体ID
        /// </summary>
        public ushort FieldID;

        /// <summary>
        /// 业务结构体长度
        /// </summary>
        public ushort FieldLength;
    }
}
